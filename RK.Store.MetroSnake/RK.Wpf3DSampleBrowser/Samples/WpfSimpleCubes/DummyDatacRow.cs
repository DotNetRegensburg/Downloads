using System;

namespace RK.Wpf3DSampleBrowser.Samples.WpfSimpleCubes
{
    public class DummyDatacRow
    {
        private static Random s_randomizer = new Random(Environment.TickCount);

        public DummyDatacRow()
        {
            this.Row1 = s_randomizer.Next(0, 100);
            this.Row2 = s_randomizer.Next(0, 100);
            this.Row3 = s_randomizer.Next(0, 100);
            this.Row4 = s_randomizer.Next(0, 100);
            this.Row5 = s_randomizer.Next(0, 100);
            this.Row6 = s_randomizer.Next(0, 100);
            this.Row7 = s_randomizer.Next(0, 100);
            this.Row8 = s_randomizer.Next(0, 100);
            this.Row9 = s_randomizer.Next(0, 100);
        }

        public int Row1 { get; set; }
        public int Row2 { get; set; }
        public int Row3 { get; set; }
        public int Row4 { get; set; }
        public int Row5 { get; set; }
        public int Row6 { get; set; }
        public int Row7 { get; set; }
        public int Row8 { get; set; }
        public int Row9 { get; set; }
    }
}
