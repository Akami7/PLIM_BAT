using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using test.Resources;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using Microsoft.Phone.Scheduler;

namespace test
{
    public partial class MainPage : PhoneApplicationPage
    {
        // var
        private const string TASK_NAME = "MyAgent";

        // Constructor
        public MainPage()
        {
            InitializeComponent();


            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            
            //IsolatedStorageHelper.Clear();  // Uncomment this line if you want to clear the database

            // The refCount is used to keep track of the last the database id when the app is closed.
            int refCount = IsolatedStorageHelper.GetObject<int>("ref_count");
            new BatteryState(refCount);

        }

        

        private void lvl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<BatteryState> batteryStateDB = IsolatedStorageHelper.GetObject<List<BatteryState>>("battery_states.db");

            if (batteryStateDB == null)
            {
                Debug.WriteLine("No entries");
                batteryStateDB = new List<BatteryState>();
            }

            batteryStateDB.Add(new BatteryState().updateState());
            IsolatedStorageHelper.SaveObject("battery_states.db", batteryStateDB);
            IsolatedStorageHelper.SaveObject("ref_count", batteryStateDB.Last().Id + 1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<BatteryState> batteryStateDB = IsolatedStorageHelper.GetObject<List<BatteryState>>("battery_states.db");
            if (batteryStateDB == null)
            {
                //Nothing to do.
                return;
            }

            foreach(BatteryState bs in batteryStateDB){
                Debug.WriteLine("Id : " + bs.Id
                    + ", Level : " + bs.level
                    + ", Time : " + bs.time);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

            StartAgent();


        }

        private void StartAgent()
        {
            StopAgentIfStarted();

            PeriodicTask task = new PeriodicTask(TASK_NAME);
            task.Description = "Our agent for battery lvl";
            ScheduledActionService.Add(task);
            #if DEBUG
            // If we’re debugging, attempt to start the task immediately 
            ScheduledActionService.LaunchForTest(TASK_NAME, new TimeSpan(0, 0, 1));
            #endif
        }

        private void StopAgentIfStarted()
        {
            if (ScheduledActionService.Find(TASK_NAME) != null)
            {
                ScheduledActionService.Remove(TASK_NAME);
            }
        } 
        

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}