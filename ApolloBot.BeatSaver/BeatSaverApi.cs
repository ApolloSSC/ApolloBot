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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApolloBot.BeatSaver
{

    public class BeatSaverApi: IApi
    {
        private const string URL_DOWNLOAD = "https://beatsaver.com/api/download/key/";
        private const string URL = "https://beatsaver.com/api/maps/";
        private const string URL_LASTEST = "latest/{0}";
        private const string URL_TOP_DOWNLOAD = "downloads/{0}";
        private const string URL_TOP_PLAYED = "plays/{0}";
        private const string URL_SEARCH = "https://beatsaver.com/api/search/text/0?q=";
        private const string URL_DETAIL = "detail/{0}";
        private const string URL_SCORESABER = "http://scoresaber.com/api.php?function=get-leaderboards&cat=1&page=1&limit=1000&ranked=1";

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
            var request = new HttpRequestMessage(HttpMethod.Get, $"{URL_SEARCH}{text}");

            var response = await _client.SendAsync(request);
            var responseStr = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<BeatSaverResultApi>(responseStr).Songs.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(text);
            }

            return new List<BeatSaverSongApi>();
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

        public async Task<List<BeatSaverSongApi>> GetRankedSongs()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{URL_SCORESABER}");

            var response = await _client.SendAsync(request);
            var responseStr = await response.Content.ReadAsStringAsync();

            var rankedSongs = JsonConvert.DeserializeObject<ScoreSaberResultApi>(responseStr).Songs;

            var ids = new List<string>();
            var songs = new List<BeatSaverSongApi>();

            foreach (var rank in rankedSongs)
            {
                if(ids.Contains(rank.Id))
                {
                    continue;
                }

                var songsResult = await Search(rank.Id);

                if(!songsResult.Any())
                {
                    continue;
                }

                songs.Add(songsResult.First());

                ids.Add(rank.Id);

                Thread.Sleep(200);
            }

            return songs;
        }

        public async Task<string> CreatePlaylist(List<BeatSaverSongApi> songs)
        {
            var playlist = new Playlist()
            {
                Title = $"AllRanked - {DateTime.Today.ToString("dd/MM/yy")}",
                Author = "Apollo",
                Image = "data:image/png;base64,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BJnaXr3rJDn164f4AQ722Koz5+RuHOSYIGRkQZifhQ9qHdSnPy/Yug4O8VRqqItOwMe982DZacE8Gb//PupGMcGv6ejMDb88G5VYD4T9TPjJSMFvd4SpT5bgKv7pN/zFWSsb5TW7TZLTjzlEpoyHKwX/o8Jn1mMPaxuCngWrh5CRBCGplhUw1MPgMalHPNUsMWwwKrr23E12XnC9TPnq56xYDKmuyOrzL5Fsda9FsPCpccShOowwOk2ZuxnWrk3l+rsWycSJsZz+1b+VKeOQISKEU/nRab68qn39BiJk5LHq5uTA+60NHyFVzy9lchHKbv/TH8orTv+ZpNOmis6e2dsjvTMuU+HWes+sgiMKxXuECRbNzFufkEnIV/K1d8mkSd7H6Ee5EVJWEE6467xZ0v2mN0rktLrnl2ebAYJUsn6WJzIyUD1GGLVSnEjfvPBF+dXXDpK9d50k9a+d1jUpOfBsdx9wgOw47zKZ9NEPyMZFi5z1fQn7a7YCUcZ4npHFfbtz7nlW3rj7NHntrlO9XzE2k5xNJyk76KJRHzdcf6mzvM+R9bPnyY7zLxZW8JGF4j3CII8JEvjY0GPEb5ufW0MB2SNPyo6Kd/ND5IaHHpZxzhovridbFor3ViXV+SljJLX31gprNykzQTzsiRIGShdHWG4laPZtDWCdaBsZSxxbWJZ+9azfpOQ0AqNSwaQNGkGF2G9Gmow4FO+tgD0+eqXOkPo1bAmhgoSUnKyiRgrcgFq9YXmzeo8odJsQQkgJoeVNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCElhOJNCCEl5P8H3K33f5KB2XwAAAAASUVORK5CYII=",
                Songs = new List<PlaylistSong>()
            };

            foreach(var song in songs)
            {
                playlist.Songs.Add(new PlaylistSong()
                {
                    Key = song.Key,
                    Hash = song.Hash,
                    SongName = song.Metadata.SongName,
                    Uploader = song.Metadata.AuthorName
                });
            }

            return JsonConvert.SerializeObject(playlist);
        }

        public async Task<string> GetAudioByKey(string key)
        {
            //var result = await DownloadByKey(key);

            //if(result)
            //{
            //    var song = await GetByKey(key);

            //    if (Directory.Exists(Directory.GetCurrentDirectory() + $"Songs\\{key}"))
            //    {
            //        var directory = Directory.GetDirectories(Directory.GetCurrentDirectory() + $"Songs\\{key}\\").First();
            //        var audio = song.Difficulties.First().Value.AudioPath;
            //        return Directory.GetCurrentDirectory() + $"\\{directory}\\{audio}";
            //    }
            //}

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
                            str += $"*{song.Name}* ({song.Metadata.AuthorName}) (id: {song.Key}, :arrow_down: {song.Stats.DownloadCount}, :video_game: {song.Stats.PlayedCount}, :+1: {song.Stats.UpVotes}, :-1: {song.Stats.DownVotes})\n";
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
                            str += $"*{song.Name}* ({song.Metadata.AuthorName}) (id: {song.Key}, :arrow_down: {song.Stats.DownloadCount}, :video_game: {song.Stats.PlayedCount}, :+1: {song.Stats.UpVotes}, :-1: {song.Stats.DownVotes})\n";
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
                            str += $"*{song.Name}* ({song.Metadata.AuthorName}) (id: {song.Key}, :arrow_down: {song.Stats.DownloadCount}, :video_game: {song.Stats.PlayedCount}, :+1: {song.Stats.UpVotes}, :-1: {song.Stats.DownVotes})\n";
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
                            str += $"*{song.Name}* ({song.Metadata.AuthorName}) (id: {song.Key}, :arrow_down: {song.Stats.DownloadCount}, :video_game: {song.Stats.PlayedCount}, :+1: {song.Stats.UpVotes}, :-1: {song.Stats.DownVotes})\n";
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

            var playlistAction = new BotAction()
            {
                CommandLine = Constants.CMD_BEATSAVER_RANKED,
                Description = Constants.DESC_BEATSAVER_RANKED,
                Category = Constants.CAT_BEATSABER,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    var songs = await GetRankedSongs();
                    return await CreatePlaylist(songs);
                }
            };
            listActions.Add(playlistAction);

            return listActions;
        }

        public IEnumerable<BotReader> GetReaders()
        {
            return null;
        }
    }
}
