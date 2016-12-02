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
    public class YouTubeApiService
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
            if (videoID != null)
            {
                // Use GetVideos method with a single-entry array for code reuse
                YouTubeVideo[] returnedVideo = GetVideos(new string[] { videoID });

                if (returnedVideo != null)
                {
                    return returnedVideo[0];
                }
            }
            // If null ID or no video found
            return null;
        }

        // Returns a YouTubeVideo object array based on the provided IDs
        // Returns null if all IDs are invalid, else returns all valid ID requests
        public static YouTubeVideo[] GetVideos(string[] videoIDs)
        {
            const int maxVideosPerRequest = 49;

            YouTubeVideo[] returnedVideos;

            // Check for empty array
            if (videoIDs != null && videoIDs.Length > 0)
            {
                // Convert videoID array into requests of the maximum allowed number of IDs
                // Find the number of requests we'll be making
                int numberOfRequests = videoIDs.Length / maxVideosPerRequest + 1;
                string[] requestIds = new string[numberOfRequests];

                // Transform array of IDs into an array of CSV requests
                int videoIdIndex = 0;
                for (int i = 0; i < numberOfRequests; i++)
                {
                    requestIds[i] = "";

                    for (int j = 0; videoIdIndex < videoIDs.Length && j < maxVideosPerRequest; j++)
                    {
                        requestIds[i] += videoIDs[videoIdIndex];
                        if (j < maxVideosPerRequest - 1)
                        {
                            requestIds[i] += ",";
                        }
                        videoIdIndex++;
                    }
                }

                // Send and capture each request
                VideoListResponse[] responses = new VideoListResponse[numberOfRequests];
                int numberOfResponses = 0;

                for (int i = 0; i < responses.Length; i++)
                {
                    // Request video info snippet
                    var request = ytService.Videos.List("snippet");
                    request.Id = requestIds[i];

                    // Executes the request and captures the response
                    responses[i] = request.Execute();

                    // Count how many valid responses are returned
                    numberOfResponses += responses[i].Items.Count;
                }


                // If any videos were found
                if (numberOfResponses > 0)
                {
                    // Create new YouTubeVideo object for each response item, and add it to the array
                    returnedVideos = new YouTubeVideo[numberOfResponses];
                    int returnedVideoIndex = 0;

                    foreach (var response in responses)
                    {
                        for (int i = 0; i < response.Items.Count; i++)
                        {
                            string id = response.Items[i].Id;
                            string title = response.Items[i].Snippet.Title;
                            string description = response.Items[i].Snippet.Description;
                            DateTime publishedDate = response.Items[i].Snippet.PublishedAt.Value;

                            returnedVideos[returnedVideoIndex] = new YouTubeVideo(id, title, description, publishedDate);
                            returnedVideoIndex++;
                        }
                    }
                    return returnedVideos;
                }
            }
            // Empty input or no responses
            return null;
        }

        // Returns an array of videos loaded from a youtube playlist
        // Returns empty array if playlist not found
        public static YouTubeVideo[] GetPlaylist(string playlistID)
        {
            // Creates a new request for playlist info based on the playlist ID
            var request = ytService.PlaylistItems.List("contentDetails");
            request.PlaylistId = playlistID;

            // Create a list of video IDs from the playlist
            var videoIDList = new LinkedList<string>();

            // Iterate through every page of results
            string nextPage = "";
            while (nextPage != null)
            {
                request.PageToken = nextPage;
                var response = request.Execute();

                // Add each video's ID to the list
                foreach (var item in response.Items)
                {
                    videoIDList.AddLast(item.ContentDetails.VideoId);
                }

                // Get the next page token, null if no next page
                nextPage = response.NextPageToken;
            }

            // Get video info using the collected IDs
            return GetVideos(videoIDList.ToArray());
        }
    }
}
