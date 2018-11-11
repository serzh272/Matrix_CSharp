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
    class Table : MyMatr
    {
        private Brush br = new SolidColorBrush(Colors.Aqua);
        private Brush brc = new SolidColorBrush(Colors.Gold);
        public Table(ref Grid g, ref Matr m, int sp, string zapoln = "") : base(ref g, ref m, sp, zapoln)
        {
            for (int i = 0; i < this.NCol; i++)
            {
                this.txtBxs[0, i].IsEnabled = false;
                this.txtBxs[0, i].Background = br;
                if (i==0||i==1)
                {
                    for (int j = 1; j < this.NRow; j++)
                    {
                        this.txtBxs[j, i].Background = brc;
                    }
                    
                }
                switch (i)
                {
                    case 0:
                        this.txtBxs[0, i].Text = "X";
                        break;
                    case 1:
                        this.txtBxs[0, i].Text = "Y";
                        break;
                    case 2:
                        this.txtBxs[0, i].Text = '\u2206' + "Y";
                        break;
                    default:
                        this.txtBxs[0, i].Text = '\u2206' + "" + (i - 1) + "Y";
                        break;
                }
            }

        }
        public override void InsertCol(int nC)
        {
            
            base.InsertCol(nC);
            this.matr.InsertCol(nC);
            this.txtBxs[0, this.NCol - 1].Text = '\u2206' + "" + (this.NCol - 2) + "Y";
            this.txtBxs[0, this.NCol - 1].IsEnabled = false;
            this.txtBxs[0, this.NCol - 1].Background = br;
                       
        }

        public override void DeleteCol(int nC)
        {
            if (this.NCol > 3)
            {
                base.DeleteCol(nC);
                this.matr.DeleteCol(nC);
            }
        }

        public override void InsertRow(int nR)
        {
            base.InsertRow(nR);
            this.matr.InsertRow(nR);
            this.txtBxs[this.NRow-1, 0].Background = brc;
            this.txtBxs[this.NRow-1, 1].Background = brc;
            this.InsertCol(nR);
            this.matr.InsertCol(nR);
        }

        public override void DeleteRow(int nR)
        {
            if (this.NRow > 3)
            {
                base.DeleteRow(nR);
                this.matr.DeleteRow(nR);
                this.DeleteCol(nR);
                this.matr.DeleteCol(nR);
            }                
        }

        public void GetTablKonRazn()
        {
            Fraction fr1 = new Fraction();
            Fraction fr2 = new Fraction();
            if (isRavnom())
            {
                for (int j = 2; j < this.NCol; j++)
                {
                    for (int i = 1; i < this.NRow - j + 1; i++)
                    {
                        
                        fr1.FractTxt = MyFuncs.convertToFract(this.txtBxs[i, j - 1].Text);
                        fr2.FractTxt = MyFuncs.convertToFract(this.txtBxs[i + 1, j - 1].Text);
                        this.matr.A[i, j] = fr2 - fr1;
                        this.matr.Ad[i, j] = fr2.ToDouble() - fr1.ToDouble();
                        this.txtBxs[i, j].Text = this.matr.A[i, j].FractTxt;
                    }
                }
            }           
        }

        public void GetTablRazdelRazn()
        {
            Fraction fr1 = new Fraction();
            Fraction fr2 = new Fraction();           
            Fraction x0 = new Fraction();
            Fraction x1 = new Fraction();
            for (int j = 2; j < this.NCol; j++)
            {
                for (int i = 1; i < this.NRow - j + 1; i++)
                {
                    fr1.FractTxt = this.txtBxs[i, j - 1].Text;
                    fr2.FractTxt = this.txtBxs[i + 1, j - 1].Text;
                    x0.FractTxt = this.txtBxs[i, 0].Text;
                    x1.FractTxt = this.txtBxs[i + j - 1, 0].Text;
                    this.txtBxs[i, j].Text = ((fr2 - fr1) / (x1 - x0)).FractTxt;
                }
            }            
        }

        public void DrawPolynom(ref Grid gr, int por, Fraction x, int row = 1, int res = 100)
        {            
            Fraction xfrMin = new Fraction();
            Fraction yfrMin = new Fraction();
            Fraction xfrMax = new Fraction();
            Fraction yfrMax = new Fraction();
            //Fraction xfr0 = new Fraction((int)((gr.Width-20) / 2), 1);
            //Fraction yfr0 = new Fraction((int)((gr.Height-20) / 2), 1);            
            xfrMin.FractTxt = this.txtBxs[1, 0].Text;
            yfrMin.FractTxt = this.txtBxs[1, 1].Text;
            //double yMin = yfrMin.ToDouble();
            Fraction h = new Fraction();
            xfrMax.FractTxt = this.txtBxs[this.NRow - 1, 0].Text;
            //double xMax = xfrMax.ToDouble();
            yfrMax.FractTxt = this.txtBxs[this.NRow - 1, 1].Text;
            for (Fraction i = xfrMin; i <= xfrMax; i += (xfrMax - xfrMin) / res)
            {
                if (GetPolyNewtonVal(i, por, row) >= yfrMax)
                {
                    yfrMax = GetPolyNewtonVal(i, por, row);
                }
            }
            for (Fraction i = xfrMin; i <= xfrMax; i += (xfrMax - xfrMin) / res)
            {
                if (GetPolyNewtonVal(i, por, row) < yfrMin)
                {
                    yfrMin = GetPolyNewtonVal(i, por, row);
                }
            }
            CoordAxis axis = new CoordAxis(ref gr, xfrMin.ToDouble(), xfrMax.ToDouble(), yfrMin.ToDouble(), yfrMax.ToDouble(),10);
            axis.DrawAxis();
            //Fraction xMult = new Fraction((int)((gr.Width - 20)*1000), 1000);
            //xMult /= xfrMax*2;
            //Fraction yMult = new Fraction((int)((gr.Height - 20)*1000), 1000);
            //yMult /= yfrMax*2;
            h = (xfrMax - xfrMin) / res;
            
            Polyline pl = new Polyline();
            
            Ellipse el = new Ellipse();
            el.HorizontalAlignment = HorizontalAlignment.Left;
            el.VerticalAlignment = VerticalAlignment.Top;
            el.Width = 8;
            el.Height = 8;
            Point valXY = axis.ConvertCoord(x.ToDouble(), (GetPolyNewtonVal(x, por, row).ToDouble()));
            System.Windows.Thickness th = new System.Windows.Thickness(valXY.X - el.Width/2, valXY.Y-el.Height/2, 0, 0);
            el.Margin = th;
            
            el.Stroke = Brushes.Red;
            el.Fill = Brushes.Red;           

            PointCollection pc = new PointCollection();
            
            for (Fraction i = xfrMin; i <= xfrMax; i += h)
            {
                //pc.Add(new System.Windows.Point((xfr0 + i*xMult).ToDouble(), (yfr0 - GetPolyNewtonVal(i, por, row)*yMult).ToDouble()));
                pc.Add(axis.ConvertCoord(i.ToDouble(), GetPolyNewtonVal(i, por, row).ToDouble()));
            }
            pl.Points = pc;
            pl.Stroke = Brushes.Blue;
            pl.StrokeThickness = 4;
            double wdt = gr.Width-20;
            double hgt = gr.Height-20;
            Line xVal = new Line();
            //xVal.X1 = (xfr0 + x * xMult).ToDouble();
            xVal.X1 = axis.ConvertCoord(x.ToDouble(), GetPolyNewtonVal(x, por, row).ToDouble()).X;
            xVal.Y1 = axis.ConvertCoord(x.ToDouble(), GetPolyNewtonVal(x, por, row).ToDouble()).Y;
            xVal.X2 = xVal.X1;
            xVal.Y2 = axis.crossAxis.Y;
            xVal.Stroke = Brushes.Red;
            xVal.StrokeDashArray = new DoubleCollection() { 8, 4 };
            Line yVal = new Line();
            yVal.X1 = axis.ConvertCoord(x.ToDouble(), GetPolyNewtonVal(x, por, row).ToDouble()).X;
            yVal.Y1 = axis.ConvertCoord(x.ToDouble(), GetPolyNewtonVal(x, por, row).ToDouble()).Y;
            yVal.X2 = axis.crossAxis.X;
            yVal.Y2 = yVal.Y1;
            yVal.Stroke = Brushes.Red;
            yVal.StrokeDashArray = new DoubleCollection() { 8, 4 };
            gr.Children.Add(pl);
            gr.Children.Add(xVal);
            gr.Children.Add(yVal);
            
            
            TextBox valX = new TextBox();            
            valX.Text = "" + x.ToDouble();
            valX.Width = 100;
            valX.Height = 25;
            valX.HorizontalAlignment =  HorizontalAlignment.Left;
            valX.VerticalAlignment = VerticalAlignment.Top;
            TextBox valY = new TextBox();
            valY.Text = "" + GetPolyNewtonVal(x, por, row).ToDouble();
            valY.Width = 200;
            valY.Height = 25;
            valY.HorizontalAlignment = HorizontalAlignment.Left;
            valY.VerticalAlignment = VerticalAlignment.Top;
            TextBox polyErr = new TextBox();
            TextBox diff1 = new TextBox();
            TextBox diff2 = new TextBox();
            TextBox diff1Err = new TextBox();
            TextBox diff2Err = new TextBox();

            polyErr.Text = "погр. интерполирования равна " + GetPolyNewtonErr(x,por,row).ToDouble();
            diff1.Text = "первая производная равна " + GetPolyNewtonDiff1Val(x,por,row).ToDouble();
            diff1Err.Text = "ошибка первой производной " + GetPolyNewtonDiff1Err(x,por,row).ToDouble();
            diff2.Text = "вторая производная равна " + GetPolyNewtonDiff2Val(x,por,row).ToDouble();
            diff2Err.Text = "ошибка второй производной " + GetPolyNewtonDiff2Err(x,por,row).ToDouble();
            polyErr.Width = 300;
            polyErr.Height = 25;
            diff1.Width = 300;
            diff1.Height = 25;
            diff2.Width = 300;
            diff2.Height = 25;
            diff1Err.Width = 300;
            diff1Err.Height = 25;
            diff2Err.Width = 300;
            diff2Err.Height = 25;
            SolidColorBrush br = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            valY.BorderBrush = br;
            valX.BorderBrush = br;
            polyErr.BorderBrush = br;
            diff1.BorderBrush = br;
            diff2.BorderBrush = br;
            diff1Err.BorderBrush = br;
            diff2Err.BorderBrush = br;
            valX.Background = br;
            valY.Background = br;
            polyErr.Background = br;
            diff1.Background = br;
            diff2.Background = br;
            diff1Err.Background = br;
            diff2Err.Background = br;
            valX.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            diff1.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
            diff2.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
            polyErr.Margin = new System.Windows.Thickness(0, 0, wdt - diff1.Width, hgt - diff1.Height);
            diff1.Margin = new System.Windows.Thickness(0, 0, wdt - diff1.Width, hgt - 3*diff1.Height);
            diff1Err.Margin = new System.Windows.Thickness(0, 0, wdt - diff1.Width, hgt - 5*diff1.Height);
            diff2.Margin = new System.Windows.Thickness(0, 0, wdt - diff2.Width, hgt - 7*diff2.Height);
            diff2Err.Margin = new System.Windows.Thickness(0, 0, wdt - diff2.Width, hgt - 9*diff2.Height);
            if (valXY.Y <= axis.crossAxis.Y)
            {
                valX.Margin = new System.Windows.Thickness(valXY.X - valX.Width/2, axis.crossAxis.Y - valY.Height, 0, 0);                
            }
            else
            {
                valX.Margin = new System.Windows.Thickness(valXY.X - valX.Width / 2, axis.crossAxis.Y + valY.Height, 0, 0);
            }
            if (valXY.X <= axis.crossAxis.X)
            {
                valY.Margin = new System.Windows.Thickness(axis.crossAxis.X - axis.bord - valY.Width, valXY.Y - valY.Height, 0, 0);
                valY.TextAlignment = axis.taY;
            }
            else
            {
                valY.Margin = new System.Windows.Thickness(axis.crossAxis.X + axis.bord, valXY.Y - valY.Height, 0, 0);
                //valY.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                valY.TextAlignment = axis.taY;
            }
            
            gr.Children.Add(el);
            gr.Children.Add(valX);
            gr.Children.Add(valY);
            gr.Children.Add(polyErr);
            gr.Children.Add(diff1);
            gr.Children.Add(diff2);
            gr.Children.Add(diff1Err);
            gr.Children.Add(diff2Err);
        }

        public void DrawIntegr(ref Grid gr, Fraction a, Fraction b, int res = 100)
        {
            int por = this.NCol - 2;
            int row = 1;
            Fraction xfrMin = new Fraction();
            Fraction yfrMin = new Fraction();
            Fraction xfrMax = new Fraction();
            Fraction yfrMax = new Fraction();
            //Fraction xfr0 = new Fraction((int)((gr.Width - 20) / 2), 1);
            //Fraction yfr0 = new Fraction((int)((gr.Height - 20) / 2), 1);
            xfrMin.FractTxt = this.txtBxs[1, 0].Text;
            //yfrMin.FractTxt = this.txtBxs[1, 1].Text;
            //double yMin = yfrMin.ToDouble();
            Fraction h = new Fraction();
            xfrMax.FractTxt = this.txtBxs[this.NRow - 1, 0].Text;
            //double xMax = xfrMax.ToDouble();
            yfrMax.FractTxt = this.txtBxs[this.NRow - 1, 1].Text;
            yfrMin.FractTxt = this.txtBxs[1, 1].Text;
            for (Fraction i = xfrMin; i <= xfrMax; i += (xfrMax - xfrMin) / 50)
            {
                if (GetPolyNewtonVal(i, por, row) >= yfrMax)
                {
                    yfrMax = GetPolyNewtonVal(i, por, row);
                }
            }
            for (Fraction i = xfrMin; i <= xfrMax; i += (xfrMax - xfrMin) / 50)
            {
                if (GetPolyNewtonVal(i, por, row) < yfrMin)
                {
                    yfrMin = GetPolyNewtonVal(i, por, row);
                }
            }
            CoordAxis axis = new CoordAxis(ref gr, xfrMin.ToDouble(), xfrMax.ToDouble(), yfrMin.ToDouble(), yfrMax.ToDouble(), 10);
            //Fraction yMult = new Fraction((int)((gr.Height - 20) * 1000), 1000);
            h = (b - a) / res;
            PointCollection pc = new PointCollection();

            for (Fraction i = a; i <= b; i += h)
            {
                //pc.Add(new System.Windows.Point((xfr0 + i*xMult).ToDouble(), (yfr0 - GetPolyNewtonVal(i, por, row)*yMult).ToDouble()));
                pc.Add(axis.ConvertCoord(i.ToDouble(), GetPolyNewtonVal(i, por, row).ToDouble()));
            }
            
            //double wdt = gr.Width - 20;
            //double hgt = gr.Height - 20;
            
            PointCollection pgPt = new PointCollection();
            pgPt.Add(axis.ConvertCoord(a.ToDouble(), 0));
            foreach (Point pt in pc)
            {
                pgPt.Add(pt);
            }
            pgPt.Add(axis.ConvertCoord(b.ToDouble(), 0));
            Polygon pg = new Polygon();
            pg.Points = pgPt;
            pg.Fill = Brushes.Pink;
            gr.Children.Add(pg);            
        }

        public bool isRavnom()
        {
            bool ravn = true;
            Fraction m = new Fraction();
            Fraction l = new Fraction();
            Fraction k = new Fraction();
            m.FractTxt = this.txtBxs[1, 0].Text;
            l.FractTxt = this.txtBxs[this.NRow - 1, 0].Text;
            l -= m;
            l /= (this.NRow - 2);
            for (int i = 1; i < this.NRow - 1; i++)
            {
                m.FractTxt = this.txtBxs[i, 0].Text;
                k.FractTxt = this.txtBxs[i + 1, 0].Text;
                if (l != (k - m))
                {
                    ravn = false;
                    break;
                }
            }
            return ravn;
        }

        public bool isValid()
        {
            for (int i = 1; i <= this.NRow - 1; i++)
            {
                if (this.txtBxs[i, 0].Text == "")
                {
                    return false;
                }
                if (this.txtBxs[i, 1].Text == "")
                {
                    return false;
                }
            }
            return true;
        }

        public string GetPolyNewton(int poryadok, int row = 1)
        {            
            string str = this.txtBxs[row, 1].Text;
            string proizv = "";
            Fraction m = new Fraction();
            Fraction l = new Fraction();
            Fraction k = new Fraction();
            if (isRavnom())
            {
                Fraction h = new Fraction();
                m.FractTxt = this.txtBxs[1, 0].Text;
                k.FractTxt = this.txtBxs[2, 0].Text;
                h = k - m;
                if (poryadok >= this.NRow - row - 1)
                {
                    poryadok = this.NRow - row - 1;
                }
                for (int i = 1; i <= poryadok; i++)
                {
                    m.FractTxt = "" + MyFuncs.factorial(i);
                    m *= h.Power(i);
                    l.FractTxt = this.txtBxs[row, i + 1].Text;
                    l /= m;
                    //str += "+" + l.FractTxt;
                    str += (l < 0?"":"+") + l.FractTxt;
                    proizv = "";
                    for (int j = 1; j < i + 1; j++)
                    {
                        //proizv += "(" + this.txtBxs[0, 0].Text + "-" + this.txtBxs[j + row - 1, 0].Text + ")";
                        proizv += '\x00D7' + (this.txtBxs[j + row - 1, 0].Text == "0" ? "" :"(") + this.txtBxs[0, 0].Text +( this.txtBxs[j + row - 1, 0].Text=="0"?"": "-" + this.txtBxs[j + row - 1, 0].Text + ")");
                    }
                    str += proizv;
                }
            }
            else
            {
                if (poryadok >= this.NRow - row - 1)
                {
                    poryadok = this.NRow - row - 1;
                }
                for (int i = 1; i <= poryadok; i++)
                {
                    l.FractTxt = this.txtBxs[row, i + 1].Text;
                    str += (l < 0 ? "" : "+") + l.FractTxt;
                    proizv = "";
                    for (int j = 1; j < i + 1; j++)
                    {
                        proizv += '\x00D7' + (this.txtBxs[j + row - 1, 0].Text == "0" ? "" : "(") + this.txtBxs[0, 0].Text + (this.txtBxs[j + row - 1, 0].Text == "0" ? "" : "-" + this.txtBxs[j + row - 1, 0].Text + ")");
                    }
                    str += proizv;
                }

            }
            return str;            
        }

        public Fraction GetPolyNewtonVal(Fraction fr, int poryadok, int row = 1)
        {            
            Fraction rez = new Fraction();
            rez.FractTxt = this.txtBxs[row, 1].Text;
            Fraction m = new Fraction(1, 1);
            Fraction n = new Fraction(1, 1);
            Fraction l = new Fraction(1, 1);
            Fraction k = new Fraction(1, 1);
            if (poryadok >= this.NRow - row - 1)
            {
                poryadok = this.NRow - row - 1;
            }
            if (isRavnom())
            {
                Fraction h = new Fraction();
                m.FractTxt = this.txtBxs[1, 0].Text;
                k.FractTxt = this.txtBxs[2, 0].Text;
                h = k - m;
                
                for (int i = 1; i <= poryadok; i++)
                {
                    m.FractTxt = "" + MyFuncs.factorial(i);
                    m *= h.Power(i);
                    l.FractTxt = this.txtBxs[row, i + 1].Text;
                    l /= m;
                    n.FractTxt = "1";
                    for (int j = 1; j < i + 1; j++)
                    {
                        k.FractTxt = this.txtBxs[j + row - 1, 0].Text;
                        n *= (fr - k);
                    }
                    rez += l * n;
                }                
            }
            else
            {
                for (int i = 1; i <= poryadok; i++)
                {
                    l.FractTxt = this.txtBxs[row, i + 1].Text;
                    n.FractTxt = "1";
                    for (int j = 1; j < i + 1; j++)
                    {
                        k.FractTxt = this.txtBxs[j + row - 1, 0].Text;
                        n *= (fr - k);
                    }
                    rez += l * n;
                }
            }
            return rez;
        }

        public Fraction GetPolyNewtonErr(Fraction fr, int poryadok, int row = 1)
        {
            Fraction q = new Fraction();
            Fraction h = new Fraction();
            Fraction qM = new Fraction(1,1);
            Fraction y1 = new Fraction();
            if (poryadok >= this.NRow - row - 3)
            {
                poryadok = this.NRow - row - 3;
            }
            if (isRavnom())
            {
                q.FractTxt = this.txtBxs[1, 0].Text;
                h.FractTxt = this.txtBxs[2, 0].Text;
                h -= q;
                q.FractTxt = this.txtBxs[row, 0].Text;
                q = (fr - q) / h;
                qM.FractTxt = q.FractTxt;
                y1.FractTxt = this.txtBxs[row, poryadok+2].Text;
                for (int i = 1; i <= poryadok; i++)
                {
                    qM *= (q - i);
                }
                qM *= y1 / MyFuncs.factorial(poryadok + 1);
            }
            else
            {

            }

            return qM;
        }

        public Fraction GetPolyNewtonDiff1Val(Fraction fr, int poryadok, int row = 1)
        {
            if (poryadok >= this.NRow - row - 1)
            {
                poryadok = this.NRow - row - 1;
                if (poryadok >= 5)
                {
                    poryadok = 5;
                }
            }
            Fraction rez = new Fraction();
            if (isRavnom())            
            {
                Fraction q = new Fraction();
                Fraction l1 = new Fraction();
                Fraction l2 = new Fraction();
                Fraction l3 = new Fraction();
                Fraction l4 = new Fraction();
                Fraction l5 = new Fraction();                
                Fraction h = new Fraction();
                q.FractTxt = this.txtBxs[1, 0].Text;
                h.FractTxt = this.txtBxs[2, 0].Text;
                h -= q;
                q.FractTxt = this.txtBxs[row, 0].Text;
                q = (fr - q) / h;
                switch (poryadok)
                {
                    case 1:
                        l1.FractTxt = this.txtBxs[row, 2].Text;
                        rez = h.Invert() * (l1);
                        break;
                    case 2:
                        l1.FractTxt = this.txtBxs[row, 2].Text;
                        l2.FractTxt = this.txtBxs[row, 3].Text;
                        rez = h.Invert() * (l1 + l2 * (2 * q - 1) / 2);
                        break;
                    case 3:
                        l1.FractTxt = this.txtBxs[row, 2].Text;
                        l2.FractTxt = this.txtBxs[row, 3].Text;
                        l3.FractTxt = this.txtBxs[row, 4].Text;
                        rez = h.Invert() * (l1 + l2 * (2 * q - 1) / 2 + l3* (3 * q.Power(2) - 6 * q + 2) / 6);
                        break;
                    case 4:
                        l1.FractTxt = this.txtBxs[row, 2].Text;
                        l2.FractTxt = this.txtBxs[row, 3].Text;
                        l3.FractTxt = this.txtBxs[row, 4].Text;
                        l4.FractTxt = this.txtBxs[row, 5].Text;
                        rez = h.Invert() * (l1 + l2 * (2 * q - 1) / 2 + l3 * (3 * q.Power(2) - 6 * q + 2) / 6 + l4 * (2 * q.Power(3) - 9 * q.Power(2) + 11 * q - 3) / 12);
                        break;
                    case 5:
                        l1.FractTxt = this.txtBxs[row, 2].Text;
                        l2.FractTxt = this.txtBxs[row, 3].Text;
                        l3.FractTxt = this.txtBxs[row, 4].Text;
                        l4.FractTxt = this.txtBxs[row, 5].Text;
                        l5.FractTxt = this.txtBxs[row, 6].Text;
                        rez = h.Invert() * (l1 + l2 * (2 * q - 1) / 2 + l3 * (3 * q.Power(2) - 6 * q + 2) / 6 + l4 * (2 * q.Power(3) - 9 * q.Power(2) + 11 * q - 3) / 12 + l5 * (5 * q.Power(4) - 40 * q.Power(3) + 105 * q.Power(2) - 100 * q + 24) / 120);
                        break;
                    default:
                        MessageBox.Show("Максимальный поддерживаемый порядок = 5!");
                        break;
                }
            }
            return rez;
        }       

        public Fraction GetPolyNewtonDiff1Err(Fraction fr, int poryadok, int row = 1)
        {
            if (poryadok >= this.NRow - row - 1)
            {
                poryadok = this.NRow - row - 1;
                if (poryadok >= 5)
                {
                    poryadok = 5;
                }
                if (poryadok >= this.NCol-row - 2)
                {
                    poryadok = this.NCol - row - 2;
                }
            }
            Fraction rez = new Fraction();
            if (isRavnom())
            {                
                Fraction q = new Fraction();
                Fraction l = new Fraction();
                Fraction h = new Fraction();
                h.FractTxt = this.txtBxs[1, 0].Text;
                q.FractTxt = this.txtBxs[2, 0].Text;
                h = q - h;
                q.FractTxt = this.txtBxs[row, 0].Text;
                q = (fr - q) / h;
                switch (poryadok)
                {
                    case 1:
                        l.FractTxt = this.txtBxs[row, 3].Text;
                        rez = h.Invert() * l * (2 * q - 1) / 2;
                        break;
                    case 2:                        
                        l.FractTxt = this.txtBxs[row, 4].Text;
                        rez = h.Invert() * l * (3 * q.Power(2) - 6 * q + 2) / 6;
                        break;
                    case 3:                        
                        l.FractTxt = this.txtBxs[row, 5].Text;
                        rez = h.Invert() * l * (2 * q.Power(3) - 9 * q.Power(2) + 11 * q - 3) / 12;
                        break;
                    case 4:
                        l.FractTxt = this.txtBxs[row, 6].Text;//возможна ошибка
                        rez = h.Invert() * l * (5 * q.Power(4) - 40 * q.Power(3) + 105 * q.Power(2) - 100 * q + 24)/ 120;
                        break;
                    case 5:                       
                        l.FractTxt = this.txtBxs[row, 7].Text;//возможна ошибка
                        rez = h.Invert() * l * GetPolyNewtonDiff1Val(fr, poryadok, row) / 720;//доделать
                        break;
                    default:
                        MessageBox.Show("Максимальный поддерживаемый порядок = 5!");
                        break;
                }
            }
            return rez;
        }

        public Fraction GetPolyNewtonDiff2Val(Fraction fr, int poryadok, int row = 1)
        {
            if (poryadok >= this.NRow - row - 1)
            {
                poryadok = this.NRow - row - 1;
                if (poryadok >= 5)
                {
                    poryadok = 5;
                }
            }
            Fraction rez = new Fraction();
            if (isRavnom())
            {
                Fraction m = new Fraction();
                Fraction q = new Fraction();
                Fraction k = new Fraction();
                Fraction l1 = new Fraction();
                Fraction l2 = new Fraction();
                Fraction l3 = new Fraction();
                Fraction l4 = new Fraction();
                Fraction l5 = new Fraction();
                Fraction h = new Fraction();
                m.FractTxt = this.txtBxs[1, 0].Text;
                k.FractTxt = this.txtBxs[2, 0].Text;
                h = k - m;
                k.FractTxt = this.txtBxs[row, 0].Text;
                q = (fr - k) / h;
                switch (poryadok)
                {
                    case 1:
                        l1.FractTxt = this.txtBxs[row, 2].Text;
                        rez = l1;
                        break;
                    case 2:
                        l1.FractTxt = this.txtBxs[row, 2].Text;
                        l2.FractTxt = this.txtBxs[row, 3].Text;
                        rez = h.Invert() * (l1 + l2 * (q * 2 - 1) / MyFuncs.factorial(2));
                        break;
                    case 3:
                        l1.FractTxt = this.txtBxs[row, 2].Text;
                        l2.FractTxt = this.txtBxs[row, 3].Text;
                        l3.FractTxt = this.txtBxs[row, 4].Text;
                        rez = h.Invert().Power(2) * (l2 + l3 * (q - 1));
                        break;
                    case 4:
                        l1.FractTxt = this.txtBxs[row, 2].Text;
                        l2.FractTxt = this.txtBxs[row, 3].Text;
                        l3.FractTxt = this.txtBxs[row, 4].Text;
                        l4.FractTxt = this.txtBxs[row, 5].Text;
                        rez = h.Invert().Power(2) * (l2 + l3 * (q - 1) + l4 * (6 * q.Power(2) - 18 * q + 11) / 12);
                        break;
                    case 5:
                        l1.FractTxt = this.txtBxs[row, 2].Text;
                        l2.FractTxt = this.txtBxs[row, 3].Text;
                        l3.FractTxt = this.txtBxs[row, 4].Text;
                        l4.FractTxt = this.txtBxs[row, 5].Text;
                        l5.FractTxt = this.txtBxs[row, 6].Text;
                        rez = h.Invert().Power(2) * (l2 + l3 * (q - 1) + l4 * (6 * q.Power(2) - 18 * q + 11) / 12 + l5 * (2 * q.Power(3) - 12 * q.Power(2) + 21 * q - 10) / 12);
                        break;
                    default:
                        MessageBox.Show("Максимальный поддерживаемый порядок = 5!");
                        break;
                }
            }
            return rez;
        }

        public Fraction GetPolyNewtonDiff2Err(Fraction fr, int poryadok, int row = 1)
        {
            if (poryadok >= this.NRow - row - 1)
            {
                poryadok = this.NRow - row - 1;
                if (poryadok >= 5)
                {
                    poryadok = 5;
                }
                if (poryadok >= this.NCol - row - 2)
                {
                    poryadok = this.NCol - row - 2;
                }
            }
            Fraction rez = new Fraction();
            if (isRavnom())
            {
                Fraction q = new Fraction();
                Fraction l = new Fraction();
                Fraction h = new Fraction();
                h.FractTxt = this.txtBxs[1, 0].Text;
                q.FractTxt = this.txtBxs[2, 0].Text;
                h = q - h;
                q.FractTxt = this.txtBxs[row, 0].Text;
                q = (fr - q) / h;
                switch (poryadok)
                {
                    case 1:
                        l.FractTxt = this.txtBxs[row, 3].Text;
                        rez = h.Invert().Power(2) * l;
                        break;
                    case 2:
                        l.FractTxt = this.txtBxs[row, 4].Text;
                        rez = h.Invert().Power(2) * l * (q - 1);
                        break;
                    case 3:
                        l.FractTxt = this.txtBxs[row, 5].Text;
                        rez = h.Invert().Power(2) * l * (6 * q.Power(2) - 18 * q + 11) / 12;
                        break;
                    case 4:
                        l.FractTxt = this.txtBxs[row, 6].Text;//возможна ошибка
                        rez = h.Invert().Power(2) * l * (2 * q.Power(3) - 12 * q.Power(2) + 21 * q - 10) / 12;
                        break;
                    case 5:
                        l.FractTxt = this.txtBxs[row, 7].Text;//возможна ошибка
                        rez = h.Invert().Power(2) * l * GetPolyNewtonDiff1Val(fr, poryadok, row) / 720;//доделать
                        break;
                    default:
                        MessageBox.Show("Максимальный поддерживаемый порядок = 5!");
                        break;
                }
            }
            return rez;
        }

        public string GetPolyLagrange()
        {
            string str = "";
            for (int i = 1; i < this.NRow; i++)
            {
                str = str + this.txtBxs[i, 1].Text + '\x00D7';
                for (int g = 1; g < this.NRow; g++)
                {
                    if (i != g)
                    {
                        //str = str + "(" + this.txtBxs[0, 0].Text + "-" + this.txtBxs[g, 0].Text + ")";
                        str = str + (this.txtBxs[g, 0].Text == "" || this.txtBxs[g, 0].Text == "0" ? "" : "(") + this.txtBxs[0, 0].Text + (this.txtBxs[g, 0].Text == "" || this.txtBxs[g, 0].Text == "0" ? "" : "-" + this.txtBxs[g, 0].Text+ ")") ;
                    }
                }
                str = str + "/(";
                for (int g = 1; g < this.NRow; g++)
                {
                    if (i != g)
                    {
                        //str = str + "(" + this.txtBxs[i, 0].Text + "-" + this.txtBxs[g, 0].Text + ")";
                        str = str + "(" + (this.txtBxs[i, 0].Text == "" || this.txtBxs[i, 0].Text == "0" ? "" : this.txtBxs[i, 0].Text) + (this.txtBxs[g, 0].Text == "" || this.txtBxs[g, 0].Text == "0" ? "" : "-" + this.txtBxs[g, 0].Text) + ")";
                    }
                }
                if (i != this.NRow -1)
                {
                    str = str + ")+";
                }
                else
                {
                    str = str + ")";
                }
            }
            return str;
        }
        public Fraction GetPolyLagrangeVal(Fraction fr)
        {
            Fraction rez = new Fraction();
            Fraction m = new Fraction(1,1);
            Fraction n = new Fraction(1, 1);
            Fraction l = new Fraction(1, 1);
            Fraction k = new Fraction(1, 1);
            for (int i = 1; i < this.NRow; i++)
            {
                m.FractTxt = "1";
                k.FractTxt = "1";
                for (int g = 1; g < this.NRow; g++)
                {
                    if (i != g)
                    {
                        l.FractTxt = this.txtBxs[g, 0].Text;
                        m *= (fr - l);
                    }
                }
                n.FractTxt = this.txtBxs[i, 1].Text;
                m *= n;
                for (int g = 1; g < this.NRow; g++)
                {
                    if (i != g)
                    {
                        n.FractTxt = this.txtBxs[i, 0].Text;
                        l.FractTxt = this.txtBxs[g, 0].Text;
                        n -= l;
                        k *= n;
                    }
                }
                m /= k;
                rez += m;
            }
            return rez;
        }
        public Fraction GetTrapForm(Fraction a, Fraction b, int res)//доделать
        {
            Fraction m = new Fraction();
            Fraction rez = new Fraction();            
            Fraction h = (b-a)/res;                          
            m = a.CopyFr;
            while (m<=b)
            {
                if (m==a |m==b)
                {
                    rez += this.GetPolyLagrangeVal(m) * h/2;
                }
                else
                {
                    rez += this.GetPolyLagrangeVal(m) * h;
                }
                m += h;
            }            
            return rez;
        }
    }
}
