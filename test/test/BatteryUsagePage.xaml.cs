using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Diagnostics;
using System.Collections.ObjectModel;
using MyAgent;

namespace test
{
    public partial class BatteryUsagePage : PhoneApplicationPage
    {
        public BatteryUsagePage()
        {
            InitializeComponent();
            //DataContext = App.ViewModel;
            ContentPanel.DataContext = App.ViewModel;
            
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }

    public class Model
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Model(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    // Create a ViewModel  
    public class ViewModel
    {
        public ObservableCollection<Model> Collection { get; set; }
        public ViewModel()
        {
            Collection = new ObservableCollection<Model>();
            GenerateDatas();
        }
        private void GenerateDatas()
        {
           // List<BatteryState> bs_db = IsolatedStorageHelper.GetObject<List<BatteryState>>(ScheduledAgent.BATTERY_STATES_DB);
            List<BatteryState> bs_db = findTodayBatteryLog();

            if(bs_db == null)
            {
                return;
            }

            for (int i = 0; i < bs_db.Count; i++)
            {
                double y = bs_db[i].level;
                Debug.WriteLine("(" + i + "," + y + ")");

                this.Collection.Add(new Model(i, y));
            }

        }

        private object List<T1>(string p)
        {
            throw new NotImplementedException();
        }


        private List<BatteryState> findTodayBatteryLog()
        {
            List<BatteryState> bs_db = IsolatedStorageHelper.GetObject<List<BatteryState>>(ScheduledAgent.BATTERY_STATES_DB);

            if (bs_db == null)
            {
                return null;
            }

            List<BatteryState> bs_of_today = new List<BatteryState>();

            foreach (BatteryState bs in bs_db)
            {

                if (bs.time.Month == DateTime.Now.Month &&
                    bs.time.Day == DateTime.Now.Day)
                {
                    bs_of_today.Add(bs);
                }
            }

            return bs_of_today;
        }
    }
}