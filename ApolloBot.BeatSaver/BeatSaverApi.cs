using ApolloBot.BeatSaver.Models;
using ApolloBot.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ApolloBot.BeatSaver
{

    public class BeatSaverApi: IApi
    {
        private const string URL_DOWNLOAD = "https://beatsaver.com/download/";
        private const string URL = "https://beatsaver.com/api/songs/";
        private const string URL_LASTEST = "new/{0}";
        private const string URL_TOP_DOWNLOAD = "top/{0}";
        private const string URL_TOP_PLAYED = "plays/{0}";
        private const string URL_SEARCH = "search/all/{0}";
        private const string URL_DETAIL = "details/{0}";

        private HttpClient _client;

        public BeatSaverApi()
        {
            _client = new HttpClient();
        }

        public async Task<List<BeatSaverSongApi>> GetLastest()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{URL}{string.Format(URL_LASTEST, 0)}");

            var response = await _client.SendAsync(request);
            var responseStr = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BeatSaverResultApi>(responseStr).Songs.ToList();
        }

        public async Task<List<BeatSaverSongApi>> GetTopDownload()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{URL}{string.Format(URL_TOP_DOWNLOAD, 0)}");

            var response = await _client.SendAsync(request);
            var responseStr = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BeatSaverResultApi>(responseStr).Songs.ToList();
        }

        public async Task<List<BeatSaverSongApi>> GetTopPlayed()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{URL}{string.Format(URL_TOP_PLAYED, 0)}");

            var response = await _client.SendAsync(request);
            var responseStr = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BeatSaverResultApi>(responseStr).Songs.ToList();
        }

        public async Task<List<BeatSaverSongApi>> Search(string text)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{URL}{string.Format(URL_SEARCH, text)}");

            var response = await _client.SendAsync(request);
            var responseStr = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BeatSaverResultApi>(responseStr).Songs.ToList();
        }

        public async Task<BeatSaverSongApi> GetByKey(string key)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{URL}{string.Format(URL_DETAIL, key)}");

            var response = await _client.SendAsync(request);
            var responseStr = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BeatSaverResultApi>(responseStr).Song;
        }

        public async Task<bool> DownloadByKey(string key)
        {
            if (!File.Exists(@"C:\Apollo\ApolloBot\" + $"Songs\\{key}.zip"))
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{URL_DOWNLOAD}{key}");

                var response = await _client.SendAsync(request);
                var responseStr = await response.Content.ReadAsStringAsync();

                byte[] bytes = null;
                using (var ms = new MemoryStream())
                {
                    await response.Content.CopyToAsync(ms);
                    bytes = ms.ToArray();
                }

                if (bytes == null || !bytes.Any())
                {
                    return false;
                }

                File.WriteAllBytes(@"C:\Apollo\ApolloBot\" + $"Songs\\{key}.zip", bytes);

                if (File.Exists(@"C:\Apollo\ApolloBot\" + $"Songs\\{key}.zip"))
                {
                    ZipFile.ExtractToDirectory(@"C:\Apollo\ApolloBot\" + $"Songs\\{key}.zip", $"Songs\\{key}\\");
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<string> GetAudioByKey(string key)
        {
            var result = await DownloadByKey(key);

            if(result)
            {
                var song = await GetByKey(key);

                if (Directory.Exists(Directory.GetCurrentDirectory() + $"Songs\\{key}"))
                {
                    var directory = Directory.GetDirectories(Directory.GetCurrentDirectory() + $"Songs\\{key}\\").First();
                    var audio = song.Difficulties.First().Value.AudioPath;
                    return Directory.GetCurrentDirectory() + $"\\{directory}\\{audio}";
                }
            }

            return null;
        }

        public void Init(Microsoft.Extensions.Configuration.IConfigurationRoot configuration)
        {
            
        }

        public IEnumerable<BotAction> GetActions()
        {
            var listActions = new List<BotAction>();

            var lastestAction = new BotAction()
            {
                CommandLine = Constants.CMD_BEATSAVER_LASTEST,
                Description = Constants.DESC_BEATSAVER_LASTEST,
                Category = Constants.CAT_BEATSABER,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    var size = 10;

                    if(parameters.Length > 0)
                    {
                        int.TryParse(parameters[0], out size);
                    }

                    var result = await GetLastest();

                    if (result.Any())
                    {
                        var str = "";
                        foreach (var song in result.Take(size))
                        {
                            str += $"*{song.SongName}* - {song.SongSubName} (id: {song.Key})\n";
                        }

                        return str;
                    }

                    return $"Une erreur est survenue";
                }
            };
            listActions.Add(lastestAction);

            var topDownloadAction = new BotAction()
            {
                CommandLine = Constants.CMD_BEATSAVER_TOP_DOWNLOAD,
                Description = Constants.DESC_BEATSAVER_TOP_DOWNLOAD,
                Category = Constants.CAT_BEATSABER,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    var size = 10;

                    if (parameters.Length > 0)
                    {
                        int.TryParse(parameters[0], out size);
                    }

                    var result = await GetTopDownload();

                    if (result.Any())
                    {
                        var str = "";
                        foreach (var song in result.Take(size))
                        {
                            str += $"*{song.SongName}* - {song.SongSubName} (id: {song.Key}, :arrow_down: {song.DownloadCount}, :video_game: {song.PlayedCount}, :+1: {song.UpVotes}, :-1: {song.DownVotes})\n";
                        }

                        return str;
                    }

                    return $"Une erreur est survenue";
                }
            };
            listActions.Add(topDownloadAction);

            var topPlayedAction = new BotAction()
            {
                CommandLine = Constants.CMD_BEATSAVER_TOP_PLAYED,
                Description = Constants.DESC_BEATSAVER_TOP_PLAYED,
                Category = Constants.CAT_BEATSABER,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    var size = 10;

                    if (parameters.Length > 0)
                    {
                        int.TryParse(parameters[0], out size);
                    }

                    var result = await GetTopDownload();

                    if (result.Any())
                    {
                        var str = "";
                        foreach (var song in result.Take(size))
                        {
                            str += $"*{song.SongName}* - {song.SongSubName} (id: {song.Key}, :arrow_down: {song.DownloadCount}, :video_game: {song.PlayedCount}, :+1: {song.UpVotes}, :-1: {song.DownVotes})\n";
                        }

                        return str;
                    }

                    return $"Une erreur est survenue";
                }
            };
            listActions.Add(topPlayedAction);

            var searchAction = new BotAction()
            {
                CommandLine = Constants.CMD_BEATSAVER_SEARCH,
                Description = Constants.DESC_BEATSAVER_SEARCH,
                Category = Constants.CAT_BEATSABER,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    if (parameters.Length != 1)
                    {
                        return "La commande prend obligatoirement 1 paramètre, le nom à chercher";
                    }
                    
                    var text = parameters.First();
                    var result = await Search(text);

                    if (result.Any())
                    {
                        var str = "";
                        foreach (var song in result.Take(5))
                        {
                            str += $"*{song.SongName}* - {song.SongSubName} (id: {song.Key}, :arrow_down: {song.DownloadCount}, :video_game: {song.PlayedCount}, :+1: {song.UpVotes}, :-1: {song.DownVotes})\n";
                        }

                        return str;
                    }
                    else
                    {
                        return "Pas de sons trouvés";
                    }
                }
            };
            listActions.Add(searchAction);

            return listActions;
        }

        public IEnumerable<BotReader> GetReaders()
        {
            return null;
        }
    }
}
