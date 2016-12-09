using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using VideoLibrary;

namespace YouTubeVideoDownload
{
    public static class VideoDownloader
    {
        // TODO: Remove save path from this method and pass it in instead
        private static async Task _SaveVideoToDiskAsync(string videoID)
        {
            string savePath = Properties.Settings.Default.downloadPath;

            string videoURL = $"https://www.youtube.com/watch?v={videoID}";
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string relativePath = @"YouTubeVideoDownloader\~temp";
            string downloadPath = Path.Combine(appData, relativePath);

            var youTube = YouTube.Default;
            var video = await youTube.GetVideoAsync(videoURL);
            var bytes = await video.GetBytesAsync();

            Directory.CreateDirectory(downloadPath);
            File.WriteAllBytes(Path.Combine(downloadPath, video.FullName), bytes);

            File.Move(Path.Combine(downloadPath, video.FullName), Path.Combine(savePath, video.FullName));
        }

        // Saves the YouTube video specified by the provided video ID to disk at the path set by the user
        public static async void SaveVideoToDiskAsync(string videoID)
        {
            await _SaveVideoToDiskAsync(videoID);
        }

        // Saves the YouTube videos specified by the provided video IDs to disk at the path set by the user
        public static async void SaveVideosToDiskAsync(string[] videoIDs)
        {
            foreach (var videoID in videoIDs)
            {
                await _SaveVideoToDiskAsync(videoID);
            }
        }
    }
}
