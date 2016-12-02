using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTubeApi
{
    class YouTubeVideo
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishedDate { get; set; }

        public YouTubeVideo()
        {

        }

        public YouTubeVideo (string id, string title, string description, DateTime publishedDate)
        {
            ID = id;
            Title = title;
            Description = description;
            PublishedDate = publishedDate;
        }

        // Gets data from YouTube and creates a video object
        // Return null if video not found
        public static YouTubeVideo GetVideoFromYouTube(string id)
        {
            return YouTubeApi.GetVideo(id);
        }
    }
}
