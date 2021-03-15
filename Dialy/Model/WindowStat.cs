using System;

namespace Dialy
{
    public class WindowStat
    {
        private double _height;
        private double _width;
        private double _top;
        private double _left;
        public double Height
        {
            get => _height;
            set => _height = Math.Round(value);
        }
        public double Width
        {
            get => _width;
            set => _width = Math.Round(value);
        }
        public double Top
        {
            get => _top;
            set => _top = Math.Round(value);
        }
        public double Left
        {
            get => _left;
            set => _left = Math.Round(value);
        }
    }
}
