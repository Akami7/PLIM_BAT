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
using MyAgent;
using System.Windows.Threading;

namespace test
{
    public partial class MainPage : PhoneApplicationPage
    {
        // var
        private const string TASK_NAME = "MyAgent";
        public static int EasterEgg { get; set; }
        
        DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.timer.Tick += new EventHandler(timer_Tick);
            this.Loaded += new RoutedEventHandler(timer_Tick);
            EasterEgg = 0;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            
            //IsolatedStorageHelper.Clear();  // Uncomment this line if you want to clear the database


            
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            BatteryState bs = new BatteryState();
            bs.updateState();
            lvl.Value = bs.level;
        
        }

        private void lvl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/WeekClusteringPage.xaml", UriKind.Relative));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<BatteryState> batteryStateDB = IsolatedStorageHelper.GetObject<List<BatteryState>>(ScheduledAgent.BATTERY_STATES_DB);
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

        private void lvl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Debug.WriteLine("easterEgg!!!");
        }

        private void deb_Click(object sender, RoutedEventArgs e)
        {

            EasterEgg++;
            if(EasterEgg == 3)
            NavigationService.Navigate(new Uri("/DebugPage.xaml", UriKind.Relative));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ClusteringData cd = IsolatedStorageHelper.GetObject<ClusteringData>(ScheduledAgent.CLUSTERING_DATA_DB);

            if (cd == null)
            {
                // Nothing to do.
                Debug.WriteLine("No Clustering Data");
                return;
            }

            Debug.WriteLine("Monday : " + cd.getMonday());
            Debug.WriteLine("Tuesday : " + cd.getTuesday());
            Debug.WriteLine("Tursday : " + cd.getThursday());
            Debug.WriteLine("Wednesday : " + cd.getWednesday());
            Debug.WriteLine("Friday : " + cd.getFriday());
            Debug.WriteLine("Saturday : " + cd.getSaturday());
            Debug.WriteLine("Sunday : " + cd.getSunday());


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