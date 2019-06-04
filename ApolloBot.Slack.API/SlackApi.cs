using log4net;
using Newtonsoft.Json;
using Slack.Webhooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApolloBot.Slack.API
{

    public class SlackApi
    {

#if DEBUG
        private bool _sendMessagesToSlack = true;
#else
        private bool _sendMessagesToSlack = true;
#endif

        public SlackClient _client;
        public HttpClient _clientHttp;

        public SlackApi(string webhook = null)
        {
            _client = new SlackClient(webhook);

            _clientHttp = new HttpClient();
        }

        public void SendMessage(string channel, string text, Emoji icon, string username, ILog logger, List<SlackAttachment> attachments = null)
        {
            try
            {
                if (_sendMessagesToSlack)
                {
                    var slackMessage = new SlackMessage
                    {
                        //Channel = channel,
                        Text = text,
                        IconEmoji = icon,
                        Username = "Apollo"
                    };

                    _client.Post(slackMessage);
                }
                Console.WriteLine(text);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void UploadImage(string channel, byte[] image, ILog logger)
        {
            try
            {
                if (_sendMessagesToSlack)
                {
                    var slackMessage = new SlackMessage
                    {
                        Channel = channel,
                        Text = "Test",
                        Attachments = new List<SlackAttachment>() {
                            new SlackAttachment()
                            {
                                Title = "Test",
                                ImageUrl = Convert.ToBase64String(image)
                            }
                        }
                    };

                    _client.Post(slackMessage);
                }
                Console.WriteLine(Convert.ToBase64String(image));
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public async Task<IEnumerable<SlackMessageApi>> GetMessagesFromChannel(string token, string channelId, int count, string oldest = null)
        {
            string url = $"https://slack.com/api/channels.history?token={token}&channel={channelId}&count={count}&oldest={oldest}";

            if (oldest != null)
            {
                url += $"&oldest={oldest}";
            }

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await _clientHttp.SendAsync(request);

            var str = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<SlackResultApi>(str);

            return result.Messages.Where(m => m.User != null).ToList();
        }

        public async Task<bool> SendMessage2(string token, string channel, string text, Emoji icon, string username, ILog logger, List<SlackAttachment> attachments = null, string parent = null, bool hidden = false)
        {
            string url = $"https://slack.com/api/chat.postMessage?token={token}&channel={channel}&text={text}";

            if(parent != null)
            {
                url += $"&thread_ts={parent}&reply_broadcast={!hidden}";
            }

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            var response = await _clientHttp.SendAsync(request);

            var str = await response.Content.ReadAsStringAsync();

            return true;
        }
    }
}
