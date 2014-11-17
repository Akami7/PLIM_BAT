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


        private void generateLogDB()
        {
            // Generate the log database for 3 weeks of interval of time.
            // Will randomly drain the battery level each 30 minutes.
            // Battery level will be reset each day.

            Debug.WriteLine("Generating Log DB");
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


        private void generateClusteringDB()
        {


            // Generate the cluster database.

            Debug.WriteLine("Generating Clustering Database");

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


        private void printLog()
        {
            List<BatteryState> batteryStateDB = IsolatedStorageHelper.GetObject<List<BatteryState>>(ScheduledAgent.BATTERY_STATES_DB);
            if (batteryStateDB == null)
            {
                //Nothing to do.
                return;
            }

            foreach (BatteryState bs in batteryStateDB)
            {
                Debug.WriteLine("Id : " + bs.Id
                    + ", Level : " + bs.level
                    + ", Time : " + bs.time);
            }
        }

        private void printCluster()
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
        private void clearAllDBs()
        {
            Debug.WriteLine("Deleteing Battery State DB");
            IsolatedStorageHelper.DeleteObject(ScheduledAgent.BATTERY_STATES_DB);

            Debug.WriteLine("Deleteing Clustering Database");
            IsolatedStorageHelper.DeleteObject(ScheduledAgent.CLUSTERING_DATA_DB);
        }

        private void GenAllButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Generating All the things ! \\o/");
            generateLogDB();
            generateClusteringDB();
        }

        private void GenLogButton_Click(object sender, RoutedEventArgs e)
        {
            generateLogDB();
        }

        private void GenClusterButton_Click(object sender, RoutedEventArgs e)
        {
            generateClusteringDB();
        }

        private void PrintLogButton_Click(object sender, RoutedEventArgs e)
        {
            printLog();
        }

        private void PrintClusterButton_Click(object sender, RoutedEventArgs e)
        {
            printCluster();
        }

        private void ClearAllButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Clear All the things \\o/ !");
            clearAllDBs();
        }

        private void BackMainPageButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

    }
}