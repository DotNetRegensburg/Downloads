namespace TwitterWrapper
{
    using System;

    public class NewTweetFoundEventArgs : EventArgs
    {
        public NewTweetFoundEventArgs(Tweet tweet)
        {
            this.Tweet = tweet;
        }

        public Tweet Tweet { get; private set; }
    }
}
