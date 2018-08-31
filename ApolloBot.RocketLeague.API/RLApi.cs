using ApolloBot.RocketLeague.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ApolloBot.RocketLeague.API
{
    public class RLApi
    {
        private const string KEY = "#KEY TO SET#";
        private const string URL_PLAYER_SEARCH = "https://api.rocketleaguestats.com/v1/search/players";
        private const string URL_PLAYER = "https://api.rocketleaguestats.com/v1/player";

        private HttpClient _client;

        public RLApi()
        {
            _client = new HttpClient();
        }

        public async Task<Result<List<Player>>> GetPlayerByName(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                return null;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"{URL_PLAYER_SEARCH}?display_name={name}");
            request.Headers.Authorization = new AuthenticationHeaderValue(KEY);

            var response = await _client.SendAsync(request);
            var responseStr = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Result<List<Player>>>(responseStr);
        }

        public async Task<Player> GetPlayerById(string id, int platform)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"{URL_PLAYER}?unique_id={id}&platform_id={platform}");
            request.Headers.Authorization = new AuthenticationHeaderValue(KEY);

            var response = await _client.SendAsync(request);
            var responseStr = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Player>(responseStr);
        }

    }
}
