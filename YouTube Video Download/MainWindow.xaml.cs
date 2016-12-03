using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using YouTubeVideoDownload.Data;

using Microsoft.WindowsAPICodePack.Dialogs;

namespace YouTubeVideoDownload
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<VideoListEntry> VideoList;

        #region Initialization
        public MainWindow()
        {
            VideoList = new ObservableCollection<VideoListEntry>();
            InitializeComponent();
        }

        private void listViewVideoList_Loaded(object sender, RoutedEventArgs e)
        {
            listViewVideoList.ItemsSource = VideoList;
        }
        #endregion


        #region Download Buttons
        private void buttonStartDownload_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonPauseDownload_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonStopDownload_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion


        #region Video List
        // Checks URL text box for a video or playlist link and adds the videos to the queue
        private void buttonAddVideos_Click(object sender, RoutedEventArgs e)
        {
            string videoID = LinkParser.GetVideoIDFromURL(textBoxURL.Text);
            string playlistID = LinkParser.GetPlaylistIDFromURL(textBoxURL.Text);

            // Check which IDs were found
            if (videoID != null && playlistID != null)
            {
                labelUrlStatus.Content = $"video | {videoID} | playlist | {playlistID}";
            }
            else if (videoID != null)
            {
                labelUrlStatus.Content = $"video | {videoID}";
            }
            else if (playlistID != null)
            {
                labelUrlStatus.Content = $"playlist | {playlistID}";
            }
            else
            {
                labelUrlStatus.Content = "Invalid link!";
            }
        }

        // Clears all videos from the list
        private void buttonClearVideoList_Click(object sender, RoutedEventArgs e)
        {
            VideoList.Clear();
            labelUrlStatus.Content = "List cleared!";
        }

        // Clear the status label when user types into the text box
        private void textBoxURL_TextChanged(object sender, TextChangedEventArgs e)
        {
            labelUrlStatus.Content = "";
        }
        #endregion


        #region Layout Management
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
        #endregion


        #region Download Path
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
        #endregion
    }
}
