using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YouTubeApi
{
    class YouTubeApi
    {
        // Authenticates and creates a service to connect with the YouTube API
        private static YouTubeService ytService = Authenticate();

        private static YouTubeService Authenticate()
        {
            UserCredential creds;
            using (var stream = new FileStream("youtube_client_secret.json", FileMode.Open, FileAccess.Read))
            {
                creds = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { YouTubeService.Scope.YoutubeReadonly },
                    "user",
                    CancellationToken.None,
                    new FileDataStore("YouTubeAPI")
                    ).Result;
            }

            var service = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = creds,
                ApplicationName = "YouTubeAPIStuff"
            });

            return service;
        }

        // Returns a YouTubeVideo object based on the provided ID
        // Returns null if video not found
        public static YouTubeVideo GetVideo(string videoID)
        {
            YouTubeVideo returnedVideo;

            // Creates a new request for video info snippet based on the video ID
            var request = ytService.Videos.List("snippet");
            request.Id = videoID;

            // Executes the request and captures the response
            var response = request.Execute();
            if (response.Items.Count > 0)
            {
                string title = response.Items[0].Snippet.Title;
                string description = response.Items[0].Snippet.Description;
                DateTime publishedDate = response.Items[0].Snippet.PublishedAt.Value;

                returnedVideo = new YouTubeVideo(videoID, title, description, publishedDate);
            }
            else
            {
                // Video wasn't found
                returnedVideo = null;
            }
            return returnedVideo;
        }

        // Returns a YouTubeVideo object array based on the provided IDs
        // Returns null if *any* IDs are invalid
        public static YouTubeVideo[] GetVideos(string[] videoIDs)
        {
            YouTubeVideo[] returnedVideos;

            // Transform videoIDs array into a CSV list
            string idList = "";

            for (int i = 0; i < videoIDs.Length; i++)
            {
                idList += videoIDs[i];
                if (i < videoIDs.Length - 1)
                {
                    idList += ",";
                }
            }

            // Creates a new request for video info snippet based on the video ID list
            var request = ytService.Videos.List("snippet");
            request.Id = idList;

            // Executes the request and captures the response
            var response = request.Execute();

            // If videos were found
            if (response.Items.Count > 0)
            {
                // Create new YouTubeVideo object for each response item, and add it to the array
                returnedVideos = new YouTubeVideo[response.Items.Count];
                for (int i = 0; i < returnedVideos.Length; i++)
                {
                    string id = response.Items[i].Id;
                    string title = response.Items[i].Snippet.Title;
                    string description = response.Items[i].Snippet.Description;
                    DateTime publishedDate = response.Items[i].Snippet.PublishedAt.Value;

                    returnedVideos[i] = new YouTubeVideo(id, title, description, publishedDate);
                }
            }
            else
            {
                // No videos found
                returnedVideos = null;
            }
            return returnedVideos;
        }

        // Returns an array of videos loaded from a youtube playlist
        // Returns empty array if playlist not found
        public static YouTubeVideo[] GetPlaylist(string playlistID)
        {
            // Creates a new request for playlist info based on the playlist ID
            var request = ytService.PlaylistItems.List("contentDetails");
            request.PlaylistId = playlistID;

            // Create a list of videos in the playlist
            var videos = new LinkedList<YouTubeVideo>();

            // Iterate through every page of results
            string nextPage = "";
            while (nextPage != null)
            {
                request.PageToken = nextPage;
                var response = request.Execute();

                // Add each video to the list
                foreach (var item in response.Items)
                {
                    videos.AddLast(GetVideo(item.ContentDetails.VideoId));
                }

                // Get the next page token, null if no next page
                nextPage = response.NextPageToken;
            }

            return videos.ToArray<YouTubeVideo>();
        }
    }
}
