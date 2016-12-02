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
        private const string videoLinkRegex = @"(?:youtu.be/|youtube.com(?:/v/|/embed/|.*/watch(?:/|.*v=)))([a-zA-Z0-9_-]{11})(?:[^a-zA-Z0-9_-]|$)";
        private static Regex regexExtractVideoID = new Regex(videoLinkRegex, RegexOptions.Compiled);

        // Parse a YouTube video url and extract the video ID
        // Return null if failed
        public static string GetVideoIDFromURL(string url)
        {
            try
            {
                // URI builder allows us to support lazy urls
                var uri = new UriBuilder(url).Uri;

                // Check if the url is a valid YouTube url
                string authority = uri.Authority;
                if (validAuthorities.Contains(authority))
                {
                    // Parse url and try to extract video ID
                    var regResult = regexExtractVideoID.Match(uri.ToString());
                    if (regResult.Success)
                    {
                        return regResult.Groups[1].Value;
                    }
                }
            }

            // Invalid url
            catch{ }
            return null;
        }

        public static string GetPlaylistIDFromURL(string url)
        {
            return url;
        }
    }
}
