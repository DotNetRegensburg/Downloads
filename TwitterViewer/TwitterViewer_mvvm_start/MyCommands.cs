using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace TwitterViewer
{
    public static class MyCommands
    {
        static MyCommands()
        {
            Subscribe = new RoutedCommand();
        }

        public static RoutedCommand Subscribe { get; private set; }
    }
}
