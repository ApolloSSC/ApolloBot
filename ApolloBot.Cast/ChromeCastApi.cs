using GoogleCast;
using GoogleCast.Channels;
using GoogleCast.Models.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApolloBot.Cast
{
    public class ChromeCastApi
    {
        private Sender _sender;
        private DeviceLocator _deviceLocator;
        private IEnumerable<IReceiver> _devices;

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
            }
            catch(Exception ex)
            {

            }
            finally
            {
                await _sender.DisconnectAsync();
            }

            return true;
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

        public async Task<bool> Stop(string receiverName)
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
    }
}
