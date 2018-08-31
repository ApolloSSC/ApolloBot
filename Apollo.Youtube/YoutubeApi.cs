using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Youtube
{
    public class YoutubeApi
    {
        private readonly YouTubeService _youtubeService;
        private readonly ILog _log;

        public YoutubeApi(ILog log)
        {
            _log = log;
            _youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "#KEY TO SET#",
                ApplicationName = "ApolloBot"
            });

        }

        public async Task<string> Run()
        {
            try
            {
                var searchListRequest = _youtubeService.Search.List("snippet");
                searchListRequest.Q = "Google"; // Replace with your search term.
                searchListRequest.MaxResults = 50;

                // Call the search.list method to retrieve results matching the specified query term.
                var searchListResponse = await searchListRequest.ExecuteAsync();

                List<string> videos = new List<string>();
                List<string> channels = new List<string>();
                List<string> playlists = new List<string>();

                // Add each result to the appropriate list, and then display the lists of
                // matching videos, channels, and playlists.
                foreach (var searchResult in searchListResponse.Items)
                {
                    switch (searchResult.Id.Kind)
                    {
                        case "youtube#video":
                            videos.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
                            break;

                        case "youtube#channel":
                            channels.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.ChannelId));
                            break;

                        case "youtube#playlist":
                            playlists.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.PlaylistId));
                            break;
                    }
                }

                return string.Join("\n", videos) + string.Join("\n", channels) + string.Join("\n", playlists);
            }
            catch(Exception ex)
            {
                _log.Error(ex);
            }

            return "";
        }

        public async Task<Video> GetVideo(string id)
        {
            try
            {
                var videosListByIdRequest = _youtubeService.Videos.List("snippet");
                videosListByIdRequest.Id = id;

                var response = videosListByIdRequest.Execute();

                if (response.Items != null && response.Items.Any())
                {
                    return response.Items[0];
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }

            return null;
        }

    }
}
