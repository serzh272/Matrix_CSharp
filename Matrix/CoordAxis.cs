using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{    
    class CoordAxis
    {
        private double multX;
        private double multY;
        public double minX;
        public double maxX;
        public double minY;
        public double maxY;
        public double bord;
        public Point crossAxis;
        private Point zero;
        private Grid gr;
        public TextAlignment taY = TextAlignment.Left;
        public TextAlignment taX = TextAlignment.Center;
        Thickness mt = new Thickness(0,0,0,0);


        public CoordAxis(ref Grid gr, double minX, double maxX, double minY, double maxY, double bord)
        {
            double OxX1;
            double OxX2;
            double OxY;
            double OyY1;
            double OyY2;
            double OyX;
            this.gr = gr;
            this.multX = (gr.Width - 2 * bord) / (maxX - minX);
            this.multY = (gr.Height - 2 * bord) / (maxY - minY);
            this.bord = bord;
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
            if (maxX < 0)
            {
                OyX = gr.Width - this.bord;
                OyY1 = this.bord;
                OyY2 = gr.Height - this.bord;
                zero.X = OyX - maxX*multX;
                taY = TextAlignment.Right;
                mt.Left = OyX - 200;                
            }
            else if (minX < 0)
            {
                OyX = -minX * this.multX + this.bord;
                OyY1 = this.bord;
                OyY2 = gr.Height - this.bord;
                zero.X = OyX;
                if (gr.Width - OyX <= 200)
                {
                    taY = TextAlignment.Right;
                    mt.Left = OyX - 200;
                }
                else
                {
                    mt.Left = OyX;
                }
            }
            else
            {
                OyX = this.bord;
                OyY1 = this.bord;
                OyY2 = gr.Height - this.bord;
                zero.X = -minX*multX + OyX;
                mt.Left = OyX;
            }

            if (maxY < 0)
            {
                OxX1 = bord;
                OxX2 = gr.Width - this.bord;
                OxY = this.bord;
                zero.Y = OxY - maxY*multY;
                mt.Top = OxY;
            }
            else if (minY < 0)
            {
                OxX1 = this.bord;
                OxX2 = gr.Width - this.bord;
                OxY = this.bord + maxY * this.multY;
                zero.Y = OxY;
                mt.Top = OxY - 20;
            }
            else
            {
                OxX1 = this.bord;
                OxX2 = gr.Width - this.bord;
                OxY = gr.Height - this.bord;
                zero.Y = OxY + minY*multY;
                mt.Top = OxY - 20;
            }
            crossAxis.X = OyX;
            crossAxis.Y = OxY;
        }

        public Point ConvertCoord(double x, double y)
        {
            Point rez = new Point();
            rez.X = x * this.multX + zero.X;
            rez.Y = zero.Y - y * this.multY;
            return rez;
        }

        public void DrawAxis()
        {
            Line Ox = new Line();
            Line Oy = new Line();            
            Ox.X1 = this.bord;
            Ox.X2 = this.gr.Width - this.bord;
            Ox.Y1 = Ox.Y2 = crossAxis.Y;
            Oy.X1 = Oy.X2 = crossAxis.X;
            Oy.Y1 = this.bord;
            Oy.Y2 = this.gr.Height - this.bord;
            TextBlock z = new TextBlock();
            z.Text = "(" + ((crossAxis.X- this.bord) /multX + minX) + ", " + (-(crossAxis.Y- this.bord) /multY + maxY) + ")";
            z.HorizontalAlignment = HorizontalAlignment.Left;
            z.VerticalAlignment = VerticalAlignment.Top;
            z.Width = 200;
            z.Height = 20;
            z.Margin = this.mt;
           
            
           
            z.TextAlignment = taY;
            Oy.Stroke = Brushes.Green;
            Ox.Stroke = Brushes.Red;
            Oy.StrokeThickness = 2;
            Ox.StrokeThickness = 2;
            gr.Children.Add(Oy);
            gr.Children.Add(Ox);
            gr.Children.Add(z);
        }
    }
}
