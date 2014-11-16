using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using System.Collections.Generic;


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
        protected override void OnInvoke(ScheduledTask task)
        {
            //TODO: Add code to perform your task in background
            LogBatteryState();
            NotifyComplete();
        }

        private void LogBatteryState()
        {
            List<BatteryState> batteryStateDB = IsolatedStorageHelper.GetObject<List<BatteryState>>("battery_states.db");

            if (batteryStateDB == null)
            {
                Debug.WriteLine("No entries");
                batteryStateDB = new List<BatteryState>();
            }

            batteryStateDB.Add(new BatteryState().updateState());
            IsolatedStorageHelper.SaveObject("battery_states.db", batteryStateDB);
            IsolatedStorageHelper.SaveObject("ref_count", batteryStateDB.Count);
        }
    }
}