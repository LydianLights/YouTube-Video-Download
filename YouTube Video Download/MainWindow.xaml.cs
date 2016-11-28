using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using YouTubeVideoDownload.Services;

namespace YouTubeVideoDownload
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        private void buttonStartDownload_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonPauseDownload_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonStopDownload_Click(object sender, RoutedEventArgs e)
        {

        }



        private void buttonAddVideos_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonClearVideoList_Click(object sender, RoutedEventArgs e)
        {

        }


        // Resize the video list headers when the list is resized
        // Columns[0] = Video Title
        // Columns[1] = Progress
        private void listViewVideoList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double progressBarWidth = 100;
            double titleColumnWidth = listViewVideoList.ActualWidth - progressBarWidth;

            GridView view = (GridView)listViewVideoList.View;
            view.Columns[1].Width = progressBarWidth;
            view.Columns[0].Width = (titleColumnWidth > 0) ? titleColumnWidth : 0;
        }
    }
}
