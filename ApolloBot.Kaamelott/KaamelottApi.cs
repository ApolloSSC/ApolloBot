using ApolloBot.Core;
using ApolloBot.Kaamelott.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WMPLib;

namespace ApolloBot.Kaamelott
{
    public class KaamelottApi : IApi
    {
        private const string URL = "https://raw.githubusercontent.com/2ec0b4/kaamelott-soundboard/master/sounds/sounds.json";
        private const string URL_SOUND = "https://kaamelott-soundboard.2ec0b4.fr/sounds/";

        private HttpClient _client;

        private List<Sound> _sounds;

        private WindowsMediaPlayer _wplayer;

        public KaamelottApi()
        {
            _client = new HttpClient();

            _wplayer = new WindowsMediaPlayer();
        }

        public async Task GetAllSounds()
        {
            if (_sounds == null)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, URL);

                var response = await _client.SendAsync(request);
                var responseStr = await response.Content.ReadAsStringAsync();

                _sounds = JsonConvert.DeserializeObject<List<Sound>>(responseStr);

                int cpt = 0;
                foreach(var sound in _sounds)
                {
                    sound.Id = cpt;
                    cpt++;
                }
            }
        }

        public async Task<List<Sound>> Search(string text)
        {
            await GetAllSounds();

            text = text.ToLowerInvariant();

            return _sounds.Where(s => 
                s.Character.ToLowerInvariant().Contains(text) 
                || s.Title.ToLowerInvariant().Contains(text)
                || s.Episode.ToLowerInvariant().Contains(text))
            .ToList();
        }

        public async Task<bool> Play(string name)
        {
            await GetAllSounds();

            var sound = _sounds.Where(s => s.File.ToLowerInvariant().StartsWith(name)).Select(s => URL_SOUND + s.File).FirstOrDefault();

            /*
            if(sound != null)
            {
                _wplayer.URL = sound;
                _wplayer.controls.play();

                return true;
            }
            */
            return false;
        }

        public async Task<bool> PlayById(int id)
        {
            await GetAllSounds();

            var sound = _sounds.Where(s => s.Id == id).Select(s => URL_SOUND + s.File).FirstOrDefault();

            /*
            if (sound != null)
            {
                _wplayer.URL = sound;
                _wplayer.controls.play();

                return true;
            }
            */

            return false;
        }

        public async Task<string> GetMp3(int id)
        {
            await GetAllSounds();

            var sound = await GetSound(id);

            if(sound == null)
            {
                return null;
            }

            return URL_SOUND + sound.File;
       }

        public async Task<Sound> GetSound(int id)
        {
            await GetAllSounds();

            return _sounds.Where(s => s.Id == id).FirstOrDefault();
        }

        public async Task<List<Sound>> GetAll()
        {
            await GetAllSounds();

            return _sounds;
        }

        public void SetVolume(int volume)
        {
            _wplayer.settings.volume = volume;
        }

        public void Init(IConfigurationRoot configuration)
        {

        }

        public IEnumerable<BotAction> GetActions()
        {
            var listActions = new List<BotAction>();

            var searchAction = new BotAction()
            {
                CommandLine = Constants.CMD_KAAMELOTT_SEARCH,
                Description = Constants.DESC_KAAMELOTT_SEARCH,
                Category = Constants.CAT_KAAMELOTT,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    if (parameters.Length != 1)
                    {
                        return "La commande prend obligatoirement 1 paramètre, le nom à chercher";
                    }

                    var text = parameters.First();
                    var result = await Search(text);

                    if (!result.Any())
                    {
                        return "Pas de sons trouvés";
                    }

                    var str = "";

                    foreach (var sound in result)
                    {
                        str += $"*Id:* {sound.Id} | *Titre:* {sound.Title} | *Perso:* {sound.Character} | *Episode:* {sound.Episode}\n";
                    }

                    return str;
                }
            };
            listActions.Add(searchAction);

            var listAction = new BotAction()
            {
                CommandLine = Constants.CMD_KAAMELOTT_LIST,
                Description = Constants.DESC_KAAMELOTT_LIST,
                Category = Constants.CAT_KAAMELOTT,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    var result = await GetAll();

                    if (!result.Any())
                    {
                        return "Pas de sons trouvés";
                    }

                    var str = "";

                    foreach (var sound in result)
                    {
                        str += $"*Id:* {sound.Id} | *Titre:* {sound.Title} | *Perso:* {sound.Character} | *Episode:* {sound.Episode}\n";
                    }

                    return str;
                }
            };
            listActions.Add(listAction);

            var playAction = new BotAction()
            {
                CommandLine = Constants.CMD_KAAMELOTT_PLAY,
                Description = Constants.DESC_KAAMELOTT_PLAY,
                Category = Constants.CAT_KAAMELOTT,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    if (parameters.Length == 0)
                    {
                        return $"La commande  obligatoirement un paramètre, le texte à chercher";
                    }

                    if(currentReader == null)
                    {
                        return "Aucun récepteur sélectionné";
                    }

                    var param = parameters[0];
                    int paramId = 0;

                    var result = int.TryParse(param, out paramId);

                    if (result)
                    {
                        var sound = await GetSound(paramId);
                        var soundMp3 = await GetMp3(paramId);

                        if (sound == null)
                        {
                            return $"Le son n'a pas été trouvé pour l'id {paramId}";
                        }
                        else
                        {
                            var resultSendCast = currentReader.Play(log, soundMp3);

                            if (resultSendCast.Result)
                            {
                                return $"Son joué: {sound.Title} ({sound.Character})";
                            }
                            else
                            {
                                return $"Un problème est survenu lors de l'envoi pour le son: {sound.Title} ({sound.Character})";
                            }
                        }
                    }

                    return $"Le son n'a pas été trouvé pour le nom {parameters[0]}";
                }
            };
            listActions.Add(playAction);

            return listActions;
        }

        public IEnumerable<BotReader> GetReaders()
        {
            return null;
        }
    }
}
