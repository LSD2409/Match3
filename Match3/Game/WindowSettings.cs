using System;
using System.Collections.Generic;
using System.Text;

namespace Match3
{
    static class WindowSetting
    {
        public static int Height { get; set; }
        public static int Width { get; set; }

        private static int defaultWidth = 1280;

        static WindowSetting()
        {
            Width = defaultWidth;
            Height = Width / 16 * 9;
        }
    }
}
