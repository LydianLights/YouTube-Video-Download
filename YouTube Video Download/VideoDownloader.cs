using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using VideoLibrary;

namespace YouTubeVideoDownload
{
    class VideoDownloader
    {
        public static void SaveVideoToDisk(string videoURL)
        {
            var test = Properties.Settings.Default.downloadPath;
            var youTube = YouTube.Default;
            var video = youTube.GetVideo(videoURL);
            var bytes = video.GetBytes();
            //File.WriteAllBytes($@"B:\{video.FullName}", bytes);
        }
    }
}
