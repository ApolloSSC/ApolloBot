using ApolloBot.Core;
using GoogleCast;
using GoogleCast.Channels;
using GoogleCast.Models.Media;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApolloBot.Cast
{
    public class ChromeCastApi : IApi
    {
        private Sender _sender;
        private DeviceLocator _deviceLocator;
        private IEnumerable<IReceiver> _devices;
        private string _currentDeviceName;

        public ChromeCastApi()
        {
            _deviceLocator = new DeviceLocator();
            _sender = new Sender();
        }

        public async Task<List<string>> Find()
        {
            try
            {
                // Use the DeviceLocator to find a Chromecast
                _devices = await _deviceLocator.FindReceiversAsync();
            }
            catch(Exception ex)
            {

                return null;
            }

            return _devices.Select(d => d.FriendlyName).ToList();
        }

        public async Task<bool> Send(string url, string receiverName = "Freebox Player Mini v2")
        {
            if (_devices == null)
            {
                await Find();
            }

            var receiver = _devices.Where(d => d.FriendlyName == receiverName).FirstOrDefault();

            if(receiver == null)
            {
                return false;
            }

            var result = false;

            try
            {
                await _sender.DisconnectAsync();

                await _sender.ConnectAsync(receiver);
                // Launch the default media receiver application
                
                var mediaChannel = _sender.GetChannel<IMediaChannel>();

                var isStopped = IsStopped(mediaChannel);

                await _sender.LaunchAsync(mediaChannel);
                // Load and play Big Buck Bunny video
                var mediaStatus = await mediaChannel.LoadAsync(
                    new MediaInformation() { ContentId = url }, false);

                await mediaChannel.PlayAsync();

                result = true;
            }
            catch(Exception ex)
            {
                result = false;
            }
            finally
            {
                await _sender.DisconnectAsync();
            }

            return result;
        }

        public async Task<bool> Pause(string receiverName)
        {
            try
            {
                var receiver = _devices.Where(d => d.FriendlyName == receiverName).FirstOrDefault();

                if (receiver == null)
                {
                    return false;
                }

                await _sender.ConnectAsync(receiver);
                // Launch the default media receiver application
                var mediaChannel = _sender.GetChannel<IMediaChannel>();
                await mediaChannel.PauseAsync();
            }
            catch (Exception ex)
            {

                return false;
            }

            return true;
        }

        public async Task<bool> Play(string receiverName)
        {
            try
            {
                var receiver = _devices.Where(d => d.FriendlyName == receiverName).FirstOrDefault();

                if (receiver == null)
                {
                    return false;
                }

                await _sender.ConnectAsync(receiver);
                // Launch the default media receiver application
                var mediaChannel = _sender.GetChannel<IMediaChannel>();
                await mediaChannel.PlayAsync();
            }
            catch (Exception ex)
            {

                return false;
            }

            return true;
        }

        public async Task<bool> Select(string receiverName)
        {
            try
            {
                var receiver = _devices.Where(d => d.FriendlyName == receiverName).FirstOrDefault();

                if (receiver == null)
                {
                    return false;
                }

                _currentDeviceName = receiverName;
            }
            catch (Exception ex)
            {

                return false;
            }

            return true;
        }

        public async Task<bool> Stop(string receiverName = "Freebox Player Mini v2")
        {
            try
            {
                if (_devices == null)
                {
                    await Find();
                }

                var receiver = _devices.Where(d => d.FriendlyName == receiverName).FirstOrDefault();

                if (receiver == null)
                {
                    return false;
                }

                await _sender.ConnectAsync(receiver);
                // Launch the default media receiver application
                var mediaChannel = _sender.GetChannel<IMediaChannel>();
                await mediaChannel.StopAsync();
            }
            catch (Exception ex)
            {

                return false;
            }

            return true;
        }

        private bool IsStopped(IMediaChannel mediaChannel)
        {
            return (mediaChannel.Status == null || !String.IsNullOrEmpty(mediaChannel.Status.FirstOrDefault()?.IdleReason));
        }

        public void Init(IConfigurationRoot configuration)
        {

        }

        public IEnumerable<BotAction> GetActions()
        {
            var listActions = new List<BotAction>();

            var findAction = new BotAction()
            {
                CommandLine = Constants.CMD_CHROMECAST_FIND,
                Description = Constants.DESC_CHROMECAST_FIND,
                Category = Constants.CAT_CHROMECAST,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    var result = await Find();

                    if (result == null || !result.Any())
                    {
                        return "Aucun récepteur trouvé";
                    }

                    return string.Join("\n", result);
                }
            };
            listActions.Add(findAction);

            var selectAction = new BotAction()
            {
                CommandLine = Constants.CMD_CHROMECAST_SELECT,
                Description = Constants.DESC_CHROMECAST_SELECT,
                Category = Constants.CAT_CHROMECAST,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    var name = string.Join(" ", parameters);
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        return $"La commande prend obligatoirement un paramètre, le nom du chrome cast";
                    }

                    var result = await Find();

                    if (result == null || !result.Any())
                    {
                        return "Aucun récepteur trouvé";
                    }

                    var resultSelect = await Select(name);

                    if(!resultSelect)
                    {
                        return $"Le récepteur avec le nom *{name}* n'a pas été trouvé";
                    }

                    return $"Le récepteur courant est maintenant *{name}*";
                }
            };
            listActions.Add(selectAction);

            return listActions;
        }

        public IEnumerable<BotReader> GetReaders()
        {
            var listReaders = new List<BotReader>();

            var reader = new BotReader()
            {
                Name = "ChromeCast",
                Play = async (log, path) =>
                {
                    if(_currentDeviceName == null)
                    {
                        return false;
                    }

                    return await Send(path, _currentDeviceName);
                },
                Stop = async (log) =>
                {
                    if (_currentDeviceName == null)
                    {
                        return false;
                    }

                    return await Stop(_currentDeviceName);
                }
            };
            listReaders.Add(reader);

            return listReaders;
        }
    }
}
