using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using System.Collections.Generic;
using System;
using System.Threading;


namespace MyAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // The refCount is used to keep track of the last the database id when the app is closed.
            int refCount = IsolatedStorageHelper.GetObject<int>("ref_count");
            new BatteryState(refCount);


            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        public static string BATTERY_STATES_DB = "battery_states.db";
        public static string CLUSTERING_DATA_DB = "clustering_data.db";

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        /// 
        private bool firstRun = true;
        protected override void OnInvoke(ScheduledTask task)
        {
            //TODO: Add code to perform your task in background
            LogBatteryState();

            if(firstRun)
            {
                SetUpTimer(new TimeSpan(23, 50, 00));
                firstRun = false;
            }
            NotifyComplete();
        }


       
        private void LogBatteryState()
        {
            // Storing the battery state.
            List<BatteryState> batteryStateDB = IsolatedStorageHelper.GetObject<List<BatteryState>>(BATTERY_STATES_DB);

            if (batteryStateDB == null)
            {
                Debug.WriteLine("No entries");
                batteryStateDB = new List<BatteryState>();
            }

            BatteryState lastBatteryState = new BatteryState().updateState();

            batteryStateDB.Add(lastBatteryState);
            IsolatedStorageHelper.SaveObject(BATTERY_STATES_DB, batteryStateDB);
            IsolatedStorageHelper.SaveObject("ref_count", batteryStateDB.Count);



        }


        private void SetUpTimer(TimeSpan alertTime)
        {
            DateTime current = DateTime.Now;
            TimeSpan timeToGo = alertTime - current.TimeOfDay;
            if (timeToGo < TimeSpan.Zero)
            {
                return;//time already passed
            }
            System.Threading.Timer timer = new System.Threading.Timer(x =>
            {
                this.UpdateClusteringData();
            }, null, timeToGo, Timeout.InfiniteTimeSpan);
        }

        private void UpdateClusteringData()
        {
            ClusteringData cd = IsolatedStorageHelper.GetObject<ClusteringData>(CLUSTERING_DATA_DB);

            if (cd == null)
            {
                cd = new ClusteringData();
            }

            switch (DateTime.Today.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    cd.setMonday(0);
                    break;
                case DayOfWeek.Tuesday:
                    cd.setTuesday(0);
                    break;
                case DayOfWeek.Thursday:
                    cd.setThursday(0);
                    break;
                case DayOfWeek.Wednesday:
                    cd.setWednesday(0);
                    break;
                case DayOfWeek.Friday:
                    cd.setFriday(0);
                    break;
                case DayOfWeek.Saturday:
                    cd.setSaturday(0);
                    break;
                case DayOfWeek.Sunday:
                    cd.setSunday(0);
                    break;
            }


            Thread.Sleep(120000);
            SetUpTimer(new TimeSpan(23, 50, 00));
        }
    }
}