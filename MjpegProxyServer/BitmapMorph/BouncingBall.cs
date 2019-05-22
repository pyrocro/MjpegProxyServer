using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjpegProxyServer.BitmapMorph
{
    class BouncingBall : BitmapEntity
    {
        public BouncingBall(int w, int h) : base(w, h)
        {
        }

        public Bitmap bouncingBall(Bitmap bmp)
        {
            int bDiameter = 20;
            float bRadius = bDiameter / 2;
            Graphics g = Graphics.FromImage(bmp);
            Pen pen = new Pen(Color.Red);
            Brush brush = new SolidBrush(Color.Red);
            bX += bXV; //move on x Axsis
            bY += bYV; //move on y Axsis
            if (bX > bmp.Width - bRadius)
            {
                bX = bmp.Width - bRadius;
                bXV = -bSpeed;
            }
            if (bX < 0 + bRadius)
            {
                bX = bRadius;
                bXV = bSpeed;
            }
            if (bY > bmp.Height - bRadius)
            {
                bY = bmp.Height - bRadius;
                bYV = -bSpeed;
            }
            if (bY < 0 + bRadius)
            {
                bY = bRadius;
                bYV = bSpeed;
            }
            //g.FillEllipse(pen, bX, bY, bDiameter, bDiameter);
            g.FillEllipse(brush, bX, bY, bDiameter, bDiameter);
            g.DrawString("This stream is empty", new Font(FontFamily.GenericSansSerif, 10f, FontStyle.Regular), brush, bX, bY + 20);
            //g.DrawString("Stream FPS: " + this.streamFPS, new Font(FontFamily.GenericSansSerif, 10f, FontStyle.Regular), brush, bX, bY + 10);
            g.Save();
            return bmp;
        }
    }
}
