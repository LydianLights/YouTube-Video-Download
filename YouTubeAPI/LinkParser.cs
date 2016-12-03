using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YouTubeApi
{
    public static class LinkParser
    {
        private static string[] validAuthorities = { "youtube.com", "www.youtube.com", "youtu.be", "www.youtu.be" };

        // Regex for video links
        private const string videoLinkRegex = @"(?:youtu.be/|youtube.com(?:/v/|/embed/|.*/watch(?:/|.*v=)))([a-zA-Z0-9_-]{11})(?:[^a-zA-Z0-9_-]|$)";
        private static Regex regexExtractVideoID = new Regex(videoLinkRegex, RegexOptions.Compiled);

        // Regex for playlist links
        private const string playlistLinkRegex = @"youtube.com/(?:playlist\?|watch.*)list=([a-zA-Z0-9_-]+)";
        private static Regex regexExtractPlaylistID = new Regex(playlistLinkRegex, RegexOptions.Compiled);


        // Parse a YouTube video url and extract the video ID
        // Return null if failed
        public static string GetVideoIDFromURL(string url)
        {
            return ExtractIdFromUrlUsingRegex(url, regexExtractVideoID);
        }

        // Parse a YouTube playlist url and extract the playlist ID
        // Return null if failed
        public static string GetPlaylistIDFromURL(string url)
        {
            return ExtractIdFromUrlUsingRegex(url, regexExtractPlaylistID);
        }

        // Checks that the given URL is from youtube
        public static bool IsYoutubeUrl (Uri uri)
        {
            return validAuthorities.Contains(uri.Authority);
        }


        // Uses the given regex to extract a video or playlist ID
        // Returns null if failed
        private static string ExtractIdFromUrlUsingRegex(string url, Regex regex)
        {
            try
            {
                // URI builder allows us to support lazy urls
                var uri = new UriBuilder(url).Uri;

                // Check if the url is a valid YouTube url
                if (IsYoutubeUrl(uri))
                {
                    // Parse url and try to extract playlist ID
                    var regResult = regex.Match(uri.ToString());
                    if (regResult.Success)
                    {
                        return regResult.Groups[1].Value;
                    }
                }
            }
            // Invalid url
            catch { }
            return null;
        }
    }
}
