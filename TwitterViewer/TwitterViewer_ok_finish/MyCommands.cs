namespace TwitterViewer
{
    using System.Windows.Input;

    public static class MyCommands
    {
        static MyCommands()
        {
            Subscribe = new RoutedCommand();
        }

        public static RoutedCommand Subscribe { get; private set; }
    }
}
