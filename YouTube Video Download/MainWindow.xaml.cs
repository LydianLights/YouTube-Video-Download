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

using YouTubeApi;

using Microsoft.WindowsAPICodePack.Dialogs;

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
            // Parse input text for video ID
            string videoID = LinkParser.GetVideoIDFromURL(textBoxURL.Text);

            // If valid video ID, get video info
            YouTubeVideo video = null;

            if (videoID != null)
            {
                video = YouTubeApiService.GetVideo(videoID);
            }

            // DEBUG: Show the ID result
            labelTestURLParse.Content = video != null ? video.Title : "Failed to parse link!";
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


        // Set the initial download path text to the default setting
        private void textBoxDownloadPath_Loaded(object sender, RoutedEventArgs e)
        {
            string path = Properties.Settings.Default.downloadPath;
            if (path == "%default%")
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\YouTube Downloads";
            }

            textBoxDownloadPath.Text = path;
        }

        private void textBoxDownloadPath_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Properties.Settings.Default.downloadPath = textBoxDownloadPath.Text;
            Properties.Settings.Default.Save();
        }

        private void buttonDownloadPathBrowse_Click(object sender, RoutedEventArgs e)
        {
            string path = Properties.Settings.Default.downloadPath;

            // Initialize folder dialog
            var dlg = new CommonOpenFileDialog();
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = path;

            // Show dialog
            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                path = dlg.FileName;

                textBoxDownloadPath.Text = path;
                Properties.Settings.Default.downloadPath = path;
            }
        }
    }
}
