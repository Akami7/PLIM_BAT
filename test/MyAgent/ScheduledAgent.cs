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
            List<BatteryState> batteryStateDB = IsolatedStorageHelper.GetObject<List<BatteryState>>("battery_states.db");

            if (batteryStateDB == null)
            {
                Debug.WriteLine("No entries");
                batteryStateDB = new List<BatteryState>();
            }

            BatteryState lastBatteryState = new BatteryState().updateState();

            batteryStateDB.Add(lastBatteryState);
            IsolatedStorageHelper.SaveObject("battery_states.db", batteryStateDB);
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
            ClusteringData cd = IsolatedStorageHelper.GetObject<ClusteringData>("classification_data.db");

            if (cd == null)
            {
                cd = new ClusteringData();
            }

            switch (DateTime.Today.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    cd.Monday = 0;
                    break;
                case DayOfWeek.Tuesday:
                    cd.Tuesday = 0;
                    break;
                case DayOfWeek.Thursday:
                    cd.Thursday = 0;
                    break;
                case DayOfWeek.Wednesday:
                    cd.Wednesday = 0;
                    break;
                case DayOfWeek.Friday:
                    cd.Friday = 0;
                    break;
                case DayOfWeek.Saturday:
                    cd.Saturday = 0;
                    break;
                case DayOfWeek.Sunday:
                    cd.Sunday = 0;
                    break;
            }


            Thread.Sleep(120000);
            SetUpTimer(new TimeSpan(23, 50, 00));
        }
    }
}