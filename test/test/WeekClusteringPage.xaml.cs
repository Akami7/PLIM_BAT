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

namespace test
{
    public partial class WeekClusteringPage : PhoneApplicationPage
    {
        public WeekClusteringPage()
        {
            InitializeComponent();
            updateCharts();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            updateCharts();

        }

        private void updateCharts()
        {
            ClusteringData cd = IsolatedStorageHelper.GetObject<ClusteringData>(ScheduledAgent.CLUSTERING_DATA_DB);

            lundiSlide.Value = cd.getMonday();
            mardiSlide.Value = cd.getTuesday();
            mercrediSlide.Value = cd.getThursday();
            jeudiSlide.Value = cd.getWednesday();
            vendrediSlide.Value = cd.getFriday();
            samediSlide.Value = cd.getSaturday();
            dimancheSlide.Value = cd.getSunday();
        }
    }
}