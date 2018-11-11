using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для WndGCircles.xaml
    /// </summary>
    public partial class WndGCircles : Window
    {
        public WndGCircles()
        {
            InitializeComponent();
            Line Ox = new Line();
            Line Oy = new Line();
            Ox.X1 = 0;
            Ox.X2 = grdGraf.Width;
            Ox.Y1 = grdGraf.Height/2;
            Ox.Y2 = grdGraf.Height / 2;
            Oy.X1 = grdGraf.Width/2;
            Oy.X2 = grdGraf.Width/2;
            Oy.Y1 = 0;
            Oy.Y2 = grdGraf.Height;
            Ox.Stroke = Brushes.Red;
            Oy.Stroke = Brushes.Green;
            grdGraf.Children.Add(Ox);
            grdGraf.Children.Add(Oy);
        }

        public void DrawCircles(Matr m)
        {
            double mult = 1;
            Fraction r = new Fraction();
            Fraction a = new Fraction();

            for (int i = 0; i < m.NCol; i++)
            {
                for (int j = 0; j < m.NCol; j++)
                {
                    if (i != j)
                    {
                        a = m.A[i, j].CopyFr;
                        if (a < 0)
                        {
                            a = -1 * a;
                        }
                        r += a;
                    }
                }
                a = m.A[i, i].CopyFr;
                if (Math.Min((grdGraf.Width-40) / (2 * (r + (a < 0 ? -1 * a : a))).ToDouble(), (grdGraf.Height - 40) / (2 * r).ToDouble()) <= mult || mult == 1)
                {
                    mult = Math.Min((grdGraf.Width - 40) / (2 * (r + (a < 0 ? -1 * a : a))).ToDouble(), (grdGraf.Height-40) / (2 * r).ToDouble());
                }        
                r.FractTxt = "0";
                a.FractTxt = "0";
            }

            for (int i = 0; i < m.NCol; i++)
            {
                for (int j = 0; j < m.NCol; j++)
                {
                    if (i!=j)
                    {
                        a = m.A[i, j].CopyFr;
                        if (a<0)
                        {
                            a = -1*a;
                        }
                        r += a;
                    }
                }
                a = m.A[i, i].CopyFr;
                Ellipse el = new Ellipse();
                TextBlock xVal = new TextBlock();
                TextBlock rT = new TextBlock();
                Line rad = new Line();
                el.Stroke = Brushes.Black;
                rad.Stroke = Brushes.Red;
                rad.StrokeThickness = 3;
                xVal.Width = 60;
                el.HorizontalAlignment = HorizontalAlignment.Left;
                xVal.HorizontalAlignment = HorizontalAlignment.Left;
                rT.HorizontalAlignment = HorizontalAlignment.Left;
                el.VerticalAlignment = VerticalAlignment.Top;
                xVal.VerticalAlignment = VerticalAlignment.Top;
                rT.VerticalAlignment = VerticalAlignment.Top;
                el.Margin = new Thickness(grdGraf.Width / 2- mult * r.ToDouble() + mult * a.ToDouble(), grdGraf.Height / 2 - mult * r.ToDouble(), 0, 0);
                xVal.Margin = new Thickness(grdGraf.Width / 2 + mult * a.ToDouble()-xVal.Width/2, grdGraf.Height / 2 + 10, 0, 0);
                rT.Margin = new Thickness(grdGraf.Width / 2 + mult * a.ToDouble(), grdGraf.Height / 2 - mult * r.ToDouble()-20, 0, 0);
                rad.X1 = rad.X2 = grdGraf.Width / 2 + mult * a.ToDouble();
                rad.Y1 = grdGraf.Height / 2;
                rad.Y2 = grdGraf.Height / 2 - mult * r.ToDouble();

                xVal.Text = "" + a.ToDouble();
                rT.Text = "r" + (i+1) + " = " + r.ToDouble();
                xVal.TextAlignment = TextAlignment.Center;
                el.Width = mult * r.ToDouble()*2;
                el.Height = mult * r.ToDouble()*2;
                grdGraf.Children.Add(el);
                grdGraf.Children.Add(xVal);
                grdGraf.Children.Add(rad);
                grdGraf.Children.Add(rT);
                r.FractTxt = "0";
            }
        }

    }
}
