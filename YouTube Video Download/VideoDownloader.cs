using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using VideoLibrary;

namespace YouTubeVideoDownload
{
    public class VideoDownloader
    {
        public static void SaveVideoToDisk(string videoID)
        {
            string savePath = Properties.Settings.Default.downloadPath;

            string videoURL = $"https://www.youtube.com/watch?v={videoID}";
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string relativePath = @"YouTubeVideoDownloader\~temp";
            string downloadPath = Path.Combine(appData, relativePath);

            var youTube = YouTube.Default;
            var video = youTube.GetVideo(videoURL);
            var bytes = video.GetBytes();

            Directory.CreateDirectory(downloadPath);
            File.WriteAllBytes(Path.Combine(downloadPath, video.FullName), bytes);

            File.Move(Path.Combine(downloadPath, video.FullName), Path.Combine(savePath, video.FullName));
        }
    }
}
