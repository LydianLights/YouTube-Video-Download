﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouTubeApi;

namespace YouTubeVideoDownload.Data
{
    public enum ProgressToken
    {
        Ready,
        Queued,
        Downloading,
        Done,
        Paused,
        Canceled,
        Error
    }

    public class VideoListEntry : INotifyPropertyChanged
    {
        // Title and ProgressText are bound to the ListView displying the videos
        public string Title { get; }
        public string ProgressText { get; private set; }

        public YouTubeVideo Video { get; }
        public ProgressToken Progress { get; private set; }

        // Use YouTubeVideo to construct entry
        public VideoListEntry(YouTubeVideo video)
        {
            Video = video;
            Title = video.Title;
            SetProgress(ProgressToken.Ready);
        }

        // Raised when a property is changed
        public event PropertyChangedEventHandler PropertyChanged;

        // Call this whenever a property is changed to fire the PropertyChanged event
        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        // Sets the displayed progress of the entry
        public void SetProgress(ProgressToken progress)
        {
            Progress = progress;

            switch (progress)
            {
                case ProgressToken.Ready:
                    ProgressText = "Ready!";
                    break;

                case ProgressToken.Queued:
                    ProgressText = "Waiting...";
                    break;

                case ProgressToken.Downloading:
                    ProgressText = "Downloading...";
                    break;

                case ProgressToken.Done:
                    ProgressText = "Done!";
                    break;

                case ProgressToken.Paused:
                    ProgressText = "Paused.";
                    break;

                case ProgressToken.Canceled:
                    ProgressText = "Canceled.";
                    break;

                case ProgressToken.Error:
                    ProgressText = "Error!";
                    break;

                default:
                    ProgressText = "";
                    break;
            }

            OnPropertyChanged(new PropertyChangedEventArgs("ProgressText"));
        }
    }
}
