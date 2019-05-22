using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjpegProxyServer.BitmapMorph
{
    class BitmapEntity
    {
        protected float bSpeed = 5;
        protected float bX = 0, bY = 0;
        protected float bXV = 5, bYV = 5;
        protected int width = 0;
        protected int height = 0;
        public BitmapEntity(int w, int h)
        {
            this.width = w;
            this.width = h;
        }
    }
}
