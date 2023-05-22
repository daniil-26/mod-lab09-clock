using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;


namespace lab09
{
    public partial class Form1 : Form
    {
        Font font = new Font("ArialBlack", 20);
        StringFormat drawFormat = new StringFormat();

        Pen pen = new Pen(Color.Black, 3);
        Brush brush = new SolidBrush(Color.Beige);
        Brush brushEdge = new SolidBrush(Color.Coral);
        Brush brushText = new SolidBrush(Color.Black);
        Brush brushSecond = new SolidBrush(Color.Red);
        Brush brushMinute = new SolidBrush(Color.Blue);
        Brush brushHour = new SolidBrush(Color.Green);

        int widthOld = 0, heightOld = 0;

        public Form1()
        {
            InitializeComponent();

            ClientSize = new Size(500, 500);
            Location = new Point(200, 200);

            DoubleBuffered = true;

            Timer timer = new Timer { Interval = 1000 };
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();            
        }

        private void Form_Paint(object sender, PaintEventArgs e)
        {
            DateTime dt = DateTime.Now;
            Graphics g = e.Graphics;
            GraphicsState gs;

            bool isResize = ClientSize.Width != widthOld || ClientSize.Height != heightOld;

            g.TranslateTransform(ClientSize.Width / 2, ClientSize.Height / 2);
            g.ScaleTransform((float)(Math.Min(ClientSize.Width, ClientSize.Height)) / 500, (float)(Math.Min(ClientSize.Width, ClientSize.Height)) / 500);

            if (isResize)
            {
                g.Clear(BackColor);

                g.FillEllipse(brushEdge, -200, -200, 400, 400);
                g.DrawEllipse(pen, -200, -200, 400, 400);
                g.FillEllipse(brush, -190, -190, 380, 380);
            }
            else
            {
                g.FillEllipse(brush, -165, -165, 330, 330);
            }
            if (isResize)
            {
                g.DrawEllipse(pen, -190, -190, 380, 380);

                for (int i = 0; i < 360; i += 6)
                {
                    gs = g.Save();
                    g.RotateTransform(i);
                    if (i % 30 == 0)
                    {
                        g.DrawRectangle(pen, -3, 168, 6, 22);
                    }
                    else
                    {
                        g.DrawLine(pen, 0, 173, 0, 190);
                    }
                    g.Restore(gs);
                }

                widthOld = ClientSize.Width;
                heightOld = ClientSize.Height;
            }    

            g.DrawString("3", font, brushText, 132, -18, drawFormat);
            g.DrawString("6", font, brushText, -14, 125, drawFormat);
            g.DrawString("9", font, brushText, -160, -18, drawFormat);
            g.DrawString("12", font, brushText, -28, -160, drawFormat);

            gs = g.Save();
            g.RotateTransform(6 * (dt.Hour + (float)dt.Minute / 60) - 180);
            g.FillPolygon(brushHour, new PointF[] {
                new PointF(8, -10),
                new PointF(4, -14),
                new PointF(-4, -14),
                new PointF(-8, -10),
                new PointF(-8, 91),
                new PointF(-4, 95),
                new PointF(4, 95),
                new PointF(8, 91) });
            g.Restore(gs);

            gs = g.Save();
            g.RotateTransform(6 * (dt.Minute + (float)dt.Second / 60) - 180);
            g.FillPolygon(brushMinute, new PointF[] {
                new PointF(6, 0),
                new PointF(0, -18),
                new PointF(-6, 0),
                new PointF(-1, 135),
                new PointF(1, 135) });
            g.Restore(gs);

            gs = g.Save();
            g.RotateTransform(6 * dt.Second - 180);
            g.FillPolygon(brushSecond, new PointF[] {
                new PointF(-2, -14),
                new PointF(2, -14),
                new PointF(2, 164),
                new PointF(-2, 164) });
            g.Restore(gs);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Form_Paint(this, new PaintEventArgs(this.CreateGraphics(), this.Bounds));
        }
    }
}
