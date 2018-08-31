using ApolloBot.Kaamelott.Models;
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
    public class KaamelottApi
    {
        private const string URL = "https://kaamelott-soundboard.2ec0b4.fr/sounds/sounds.0178a81c.json";
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

        public async Task<List<string>> Search(string text)
        {
            await GetAllSounds();

            text = text.ToLowerInvariant();

            return _sounds.Where(s => 
                s.Character.ToLowerInvariant().Contains(text) 
                || s.Title.ToLowerInvariant().Contains(text)
                || s.Episode.ToLowerInvariant().Contains(text))
            .Select(s => s.File.Replace(".mp3", "") + $" (id: {s.Id})").ToList();
        }

        public async Task<bool> Play(string name)
        {
            await GetAllSounds();

            var sound = _sounds.Where(s => s.File.ToLowerInvariant().StartsWith(name)).Select(s => URL_SOUND + s.File).FirstOrDefault();

            if(sound != null)
            {
                _wplayer.URL = sound;
                _wplayer.controls.play();

                return true;
            }

            return false;
        }

        public async Task<bool> PlayById(int id)
        {
            await GetAllSounds();

            var sound = _sounds.Where(s => s.Id == id).Select(s => URL_SOUND + s.File).FirstOrDefault();

            if (sound != null)
            {
                _wplayer.URL = sound;
                _wplayer.controls.play();

                return true;
            }

            return false;
        }

        public async Task<string> GetMp3(int id)
        {
            await GetAllSounds();

            var sound = _sounds.Where(s => s.Id == id).Select(s => URL_SOUND + s.File).FirstOrDefault();

            if (sound != null)
            {
                _wplayer.URL = sound;

                return _wplayer.URL;
            }

            return null;
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
    }
}
