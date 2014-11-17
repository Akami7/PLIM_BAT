using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MyAgent;
using System.Diagnostics;

namespace test
{
    public partial class DebugPage : PhoneApplicationPage
    {
        public DebugPage()
        {
            InitializeComponent();
            MainPage.EasterEgg = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void Rectangle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            // Generate the log database for 3 weeks of interval of time.
            // Will randomly drain the battery level each 30 minutes.
            // Battery level will be reset each day.

            List<BatteryState> bs_db = new List<BatteryState>();
            BatteryState bs = new BatteryState();
            
            DateTime startingDay = new DateTime(2014, 11, 1);

            bs.time = startingDay;
            bs.level = 100;
            
            for (int i = 0; i < 1200; i++)
            {

                BatteryState bs_tmp = new BatteryState();
                bs_tmp.level = bs.level;
                bs_tmp.time = bs.time;

                bs_db.Add(bs_tmp);
                
                bs.level -= new Random().Next(1, 3);  // pick a number between 1 and 3;
                bs.time = bs.time.AddMinutes(30); // The scheduled task agent will run each 30 minutes.

                Debug.WriteLine("lvl : " + bs.level + ", time : " + bs.time.ToString());
                // The next day we reset the battery level.
                if (bs.time.Hour == startingDay.Hour)
                {
                    bs.level = 100;
                }
                
            }

            IsolatedStorageHelper.SaveObject(ScheduledAgent.BATTERY_STATES_DB, bs_db);


        }

        private void Rectangle_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {

            // Generate the cluster database.

            ClusteringData cd = new ClusteringData();

            cd.setMonday(10);
            cd.setTuesday(35);
            cd.setThursday(30);
            cd.setWednesday(5);
            cd.setFriday(15);
            cd.setSaturday(80);
            cd.setSunday(99);


            IsolatedStorageHelper.SaveObject(ScheduledAgent.CLUSTERING_DATA_DB, cd);
        }

        private void Rectangle_Tap_2(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Debug.WriteLine("Deleteing Battery State DB");
            IsolatedStorageHelper.DeleteObject(ScheduledAgent.BATTERY_STATES_DB);
            
            Debug.WriteLine("Deleteing Clustering Database");
            IsolatedStorageHelper.DeleteObject(ScheduledAgent.CLUSTERING_DATA_DB);
        }
    }
}