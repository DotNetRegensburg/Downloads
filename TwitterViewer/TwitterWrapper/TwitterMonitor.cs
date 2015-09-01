namespace TwitterWrapper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Web;
    using System.Web.Script.Serialization;

    public class TwitterMonitor
    {
        private bool _running = false;
        private Dictionary<string, string> _subscriptions = new Dictionary<string, string>();

        public event EventHandler<NewTweetFoundEventArgs> NewTweetFound;

        public void Start()
        {
            _running = true;
            ThreadPool.QueueUserWorkItem((o) =>
            {
                while (_running)
                {
                    List<string> keys = _subscriptions.Keys.ToList();
                    foreach (string key in keys)
                    {
                        TwitterSearchResult searchResult = WebRequest(_subscriptions[key]);
                        _subscriptions[key] = searchResult.refresh_url;

                        foreach (Tweet tweet in searchResult.results)
                        {
                            if (NewTweetFound != null)
                            {
                                NewTweetFound(this, new NewTweetFoundEventArgs(tweet));
                            }
                        }

                    }

                    Thread.Sleep(10000);
                }
            });
        }

        public void Stop() { _running = false; }

        public void Subscribe(string query)
        {
            _subscriptions.Add(query, "?q=" + HttpUtility.HtmlEncode(query));
        }

        public void Describe(string query)
        {
            _subscriptions.Remove(query);
        }

        private TwitterSearchResult WebRequest(string query)
        {
            string responseData;

            HttpWebRequest webRequest = System.Net.WebRequest.Create("http://search.twitter.com/search.json" + query) as HttpWebRequest;
            webRequest.Method = "GET";

            try
            {
                using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                {
                    responseData = responseReader.ReadToEnd();
                }
            }
            catch
            {
                throw;
            }

            return new JavaScriptSerializer().Deserialize<TwitterSearchResult>(responseData);
        }
    }
}
