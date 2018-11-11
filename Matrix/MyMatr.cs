using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
namespace WpfApp1
{
    public class MyMatr
    {
        private int FontMult = 6;
        protected TextBox[,] txtBxs;
        private int nRow, nCol, sp;
        private Grid g;
        private string zapoln;
        public Matr matr;
        public bool isDouble;
        public MyMatr()
        {
            this.nRow = 3;
            this.nCol = 3;
            isDouble = false;
        }

        public MyMatr(ref Grid g, ref Matr m, int sp, string zapoln = "")
        {
            g.Children.Clear();
            this.zapoln = zapoln;
            this.g = g;
            this.matr = m;
            this.nRow = m.NRow;
            this.nCol = m.NCol;
            this.sp = sp;
            double hMar, vMar, bW, bH;
            bW = (g.Width - (nCol + 1) * sp) / nCol;
            bH = (g.Height - (nRow + 1) * sp) / nRow;
            if (bH > bW)
            {
                bH = bW;
            }
            else
            {
                bW = bH;
            }
            hMar = (g.Width - sp * (nCol + 1) - bW * nCol)/2;
            vMar = (g.Height - sp * (nRow + 1) - bH * nRow)/2;
            txtBxs = new TextBox[nRow, nCol];
            for (int i = 1; i <= nRow; i++)
            {
                for (int j = 1; j <= nCol; j++)
                {
                    TextBox txtBx = new TextBox();
                    txtBx.LostFocus += new RoutedEventHandler(TextBox_LostFocus);
                    txtBx.GotFocus += new RoutedEventHandler(TextBox_GotFocus);
                    txtBxs[i-1, j-1] = txtBx;
                    txtBx.Name = g.Name + "_txtBx" + i + "_" + j;
                    if (this.isDouble)
                    {
                        txtBx.Text = "" + matr.Ad[i-1, j-1];
                    }
                    else
                    {
                        txtBx.Text = matr.A[i-1, j-1].FractTxt;
                    }
                    txtBx.Width = bW;
                    txtBx.Height = bH;                   
                    txtBx.VerticalAlignment = VerticalAlignment.Top;
                    txtBx.HorizontalAlignment = HorizontalAlignment.Left;
                    txtBx.TextAlignment = TextAlignment.Center;
                    txtBx.VerticalContentAlignment = VerticalAlignment.Center;
                    txtBx.TextWrapping = TextWrapping.Wrap;
                    txtBx.FontSize = txtBx.Height / FontMult;
                    Thickness myTh = new Thickness();
                    myTh.Left = hMar + sp * j + txtBx.Width * (j - 1);
                    myTh.Top = vMar + sp * i + txtBx.Height * (i - 1);
                    txtBx.Margin = myTh;
                   
                    g.Children.Add(txtBx);
                }
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox b = (TextBox)sender;
            b.SelectAll();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //TextBox b = (TextBox)sender;
            //if (!this.isDouble)
            //{
            //    b.Text = MyFuncs.convertToFract(b.Text);
            //}
        }



        ~MyMatr()
        {
            txtBxs = null;
            matr = null;
        }

        public TextBox[,] GetMatrBx()
        {
            return txtBxs;
        }

        public int NRow
        {
            get
            {
                return this.nRow;
            }
            set
            {
                this.nRow = value;
            }
        }

        public int NCol
        {
            get
            {
                return this.nCol;
            }
            set
            {
                this.nCol = value;
            }
        }
                
        public virtual void InsertCol(int nC)
        {
            if (nC<0)
            {
                nC = 0;
            }
            double hMar, vMar, bW, bH;
            bW = (g.Width - (this.nCol + nC + 1) * this.sp) / (this.nCol+ nC);
            bH = (g.Height - (this.nRow + 1) * this.sp) / this.nRow;
            if (bH > bW)
            {
                bH = bW;
            }
            else
            {
                bW = bH;
            }
            hMar = (g.Width - this.sp * (this.nCol + nC + 1) - bW * (this.nCol + nC)) / 2;
            vMar = (g.Height - this.sp * (this.nRow + 1) - bH * this.nRow) / 2;
            TextBox[,] txtBxs2 = new TextBox[this.nRow, this.nCol + nC];
            for (int i = 0; i < this.nRow; i++)
            {
                for (int j = 0; j < this.nCol; j++)
                {
                    txtBxs2[i, j] = txtBxs[i, j];
                }
            }
            txtBxs = txtBxs2;
            txtBxs2 = null;
            for (int i = 0; i < this.nRow; i++)
            {
                for (int j = 0; j < this.nCol; j++)
                {
                    TextBox tb = txtBxs[i,j];
                    tb.Width = bW;
                    tb.Height = bH;
                    tb.FontSize = tb.Height / FontMult;
                    Thickness myTh = new Thickness();
                    myTh.Left = hMar + this.sp * (j+1) + tb.Width * j;
                    myTh.Top = vMar + this.sp * (i+1) + tb.Height * i;
                    tb.Margin = myTh;
                }
            }
            for (int j = this.nCol; j < this.nCol + nC; j++)
            {
                for (int i = 0; i < this.nRow; i++)
                {
                    TextBox txtBx = new TextBox();
                    txtBxs[i, j] = txtBx;
                    txtBx.Name = g.Name + "_txtBx" + i + "_" + j;
                    txtBx.Text =this.zapoln;
                    txtBx.LostFocus += new RoutedEventHandler(TextBox_LostFocus);
                    txtBx.GotFocus += new RoutedEventHandler(TextBox_GotFocus);
                    txtBx.VerticalAlignment = VerticalAlignment.Top;
                    txtBx.HorizontalAlignment = HorizontalAlignment.Left;
                    txtBx.TextAlignment = TextAlignment.Center;
                    txtBx.VerticalContentAlignment = VerticalAlignment.Center;
                    txtBx.TextWrapping = TextWrapping.Wrap;
                    txtBx.Width = bW;
                    txtBx.Height = bH;
                    txtBx.FontSize = txtBx.Height / FontMult;
                    Thickness myTh = new Thickness();
                    myTh.Left = hMar + this.sp * (j+1) + txtBx.Width * j;
                    myTh.Top = vMar + this.sp * (i+1) + txtBx.Height * i;
                    txtBx.Margin = myTh;
                    this.g.Children.Add(txtBx);
                }
            }
            this.nCol += nC;
        }

        public virtual void InsertRow(int nR)
        {
            if (nR < 0)
            {
                nR = 0;
            }
            double hMar, vMar, bW, bH;
            bW = (g.Width - (this.nCol + 1) * this.sp) / this.nCol;
            bH = (g.Height - (this.nRow + nR + 1) * this.sp) / (this.nRow + nR);
            if (bH > bW)
            {
                bH = bW;
            }
            else
            {
                bW = bH;
            }
            hMar = (g.Width - this.sp * (this.nCol + 1) - bW * this.nCol) / 2;
            vMar = (g.Height - this.sp * (this.nRow + nR + 1) - bH * (this.nRow + nR)) / 2;
            TextBox[,] txtBxs2 = new TextBox[this.nRow + nR, this.nCol];
            for (int i = 0; i < this.nRow; i++)
            {
                for (int j = 0; j < this.nCol; j++)
                {
                    txtBxs2[i, j] = txtBxs[i, j];
                }
            }
            txtBxs = txtBxs2;
            txtBxs2 = null;
            for (int i = 0; i < this.nRow; i++)
            {
                for (int j = 0; j < this.nCol; j++)
                {
                    TextBox tb = txtBxs[i, j];
                    tb.Width = bW;
                    tb.Height = bH;
                    tb.FontSize = tb.Height / FontMult;
                    Thickness myTh = new Thickness();
                    myTh.Left = hMar + this.sp * (j + 1) + tb.Width * j;
                    myTh.Top = vMar + this.sp * (i + 1) + tb.Height * i;
                    tb.Margin = myTh;
                }
            }
            for (int j = 0; j < this.nCol; j++)
            {
                for (int i = this.nRow; i < this.nRow + nR; i++)
                {
                    TextBox txtBx = new TextBox();
                    txtBxs[i, j] = txtBx;
                    txtBx.Name = g.Name + "_txtBx" + i + "_" + j;
                    txtBx.Text = this.zapoln;
                    txtBx.LostFocus += new RoutedEventHandler(TextBox_LostFocus);
                    txtBx.GotFocus += new RoutedEventHandler(TextBox_GotFocus);
                    txtBx.VerticalAlignment = VerticalAlignment.Top;
                    txtBx.HorizontalAlignment = HorizontalAlignment.Left;
                    txtBx.TextAlignment = TextAlignment.Center;
                    txtBx.VerticalContentAlignment = VerticalAlignment.Center;
                    txtBx.TextWrapping = TextWrapping.Wrap;
                    txtBx.Width = bW;
                    txtBx.Height = bH;
                    txtBx.FontSize = txtBx.Height / FontMult;
                    Thickness myTh = new Thickness();
                    myTh.Left = hMar + this.sp * (j + 1) + txtBx.Width * j;
                    myTh.Top = vMar + this.sp * (i + 1) + txtBx.Height * i;
                    txtBx.Margin = myTh;
                    this.g.Children.Add(txtBx);
                }
            }
            this.nRow += nR;
        }

        public virtual void DeleteCol(int nC)
        {
            if (nC < 0 ||nC >=this.nCol)
            {
                nC = 0;
            }
            double hMar, vMar, bW, bH;
            bW = (this.g.Width - (this.nCol - nC + 1) * this.sp) / (this.nCol - nC);
            bH = (this.g.Height - (this.nRow + 1) * this.sp) / this.nRow;
            if (bH > bW)
            {
                bH = bW;
            }
            else
            {
                bW = bH;
            }
            hMar = (this.g.Width - this.sp * (this.nCol - nC + 1) - bW * (this.nCol - nC)) / 2;
            vMar = (this.g.Height - this.sp * (this.nRow + 1) - bH * this.nRow) / 2;
            TextBox[,] txtBxs2 = new TextBox[this.nRow, this.nCol - nC];
            for (int i = 0; i < this.nRow; i++)
            {
                for (int j = 0; j < this.nCol - nC; j++)
                {
                    txtBxs2[i, j] = txtBxs[i, j];
                }
            }
            for (int j = this.nCol - nC; j < this.nCol; j++)
            {
                for (int i = 0; i < this.nRow; i++)
                {
                    this.g.Children.Remove(txtBxs[i, j]);
                }
            }
            txtBxs = txtBxs2;
            
            txtBxs2 = null;
            for (int i = 0; i < this.nRow; i++)
            {
                for (int j = 0; j < this.nCol - nC; j++)
                {
                    TextBox tb = txtBxs[i, j];
                    tb.Width = bW;
                    tb.Height = bH;
                    tb.FontSize = tb.Height / FontMult;
                    Thickness myTh = new Thickness();
                    myTh.Left = hMar + this.sp * (j + 1) + tb.Width * j;
                    myTh.Top = vMar + this.sp * (i + 1) + tb.Height * i;
                    tb.Margin = myTh;
                }
            }            
            this.nCol -= nC;

        }

        public virtual void DeleteRow(int nR)
        {
            if (nR < 0 || nR >= this.nRow)
            {
                nR = 0;
            }
            double hMar, vMar, bW, bH;
            bW = (this.g.Width - (this.nCol + 1) * this.sp) / this.nCol;
            bH = (this.g.Height - (this.nRow - nR + 1) * this.sp) / (this.nRow - nR);
            if (bH > bW)
            {
                bH = bW;
            }
            else
            {
                bW = bH;
            }
            hMar = (this.g.Width - this.sp * (this.nCol + 1) - bW * this.nCol) / 2;
            vMar = (this.g.Height - this.sp * (this.nRow - nR + 1) - bH * (this.nRow - nR)) / 2;
            TextBox[,] txtBxs2 = new TextBox[this.nRow - nR, this.nCol];
            for (int i = 0; i < this.nRow - nR; i++)
            {
                for (int j = 0; j < this.nCol; j++)
                {
                    txtBxs2[i, j] = txtBxs[i, j];
                }
            }
            for (int j = 0; j < this.nCol; j++)
            {
                for (int i = this.nRow - nR; i < this.nRow; i++)
                {
                    this.g.Children.Remove(txtBxs[i, j]);
                }
            }
            txtBxs = txtBxs2;
            txtBxs2 = null;
            for (int i = 0; i < this.nRow - nR; i++)
            {
                for (int j = 0; j < this.nCol; j++)
                {
                    TextBox tb = txtBxs[i, j];
                    tb.Width = bW;
                    tb.Height = bH;
                    tb.FontSize = tb.Height / FontMult;
                    Thickness myTh = new Thickness();
                    myTh.Left = hMar + this.sp * (j + 1) + tb.Width * j;
                    myTh.Top = vMar + this.sp * (i + 1) + tb.Height * i;
                    tb.Margin = myTh;
                }
            }
            this.nRow -= nR;

        }

        public Matr MyMatrToMatr()
        {
            Matr m = new Matr(this.NRow, this.nCol);
            for (int i = 0; i < this.NRow; i++)
            {
                for (int j = 0; j < this.nCol; j++)
                {
                    m.A[i, j].FractTxt = this.txtBxs[i, j].Text;
                    m.Ad[i, j] = m.A[i, j].ToDouble();
                }
            }
            return m;
        }

        public void MatrWrite()
        {
            for (int i = 0; i < this.NRow; i++)
            {
                for (int j = 0; j < this.nCol; j++)
                {
                    this.matr.A[i, j].FractTxt = MyFuncs.convertToFract(this.txtBxs[i, j].Text);
                    this.matr.Ad[i, j] = this.matr.A[i, j].ToDouble();
                }
            }
        }       

        public void Redraw()
        {
            nRow = matr.NRow;
            NCol = matr.NCol;
            g.Children.Clear();
            double hMar, vMar, bW, bH;
            bW = (g.Width - (nCol + 1) * sp) / nCol;
            bH = (g.Height - (nRow + 1) * sp) / nRow;
            if (bH > bW)
            {
                bH = bW;
            }
            else
            {
                bW = bH;
            }
            hMar = (g.Width - sp * (nCol + 1) - bW * nCol) / 2;
            vMar = (g.Height - sp * (nRow + 1) - bH * nRow) / 2;
            txtBxs = new TextBox[nRow, nCol];
            for (int i = 1; i <= nRow; i++)
            {
                for (int j = 1; j <= nCol; j++)
                {
                    TextBox txtBx = new TextBox();
                    txtBx.LostFocus += new RoutedEventHandler(TextBox_LostFocus);
                    txtBx.GotFocus += new RoutedEventHandler(TextBox_GotFocus);
                    txtBxs[i - 1, j - 1] = txtBx;
                    txtBx.Name = g.Name + "_txtBx" + i + "_" + j;
                    if (this.isDouble)
                    {
                        txtBx.Text = "" + this.matr.Ad[i - 1, j - 1];
                    }
                    else
                    {
                        txtBx.Text = this.matr.A[i - 1, j - 1].FractTxt;
                    }
                    txtBx.Width = bW;
                    txtBx.Height = bH;
                    txtBx.VerticalAlignment = VerticalAlignment.Top;
                    txtBx.HorizontalAlignment = HorizontalAlignment.Left;
                    txtBx.TextAlignment = TextAlignment.Center;
                    txtBx.VerticalContentAlignment = VerticalAlignment.Center;
                    txtBx.TextWrapping = TextWrapping.Wrap;
                    txtBx.FontSize = txtBx.Height / FontMult;
                    Thickness myTh = new Thickness();
                    myTh.Left = hMar + sp * j + txtBx.Width * (j - 1);
                    myTh.Top = vMar + sp * i + txtBx.Height * (i - 1);
                    txtBx.Margin = myTh;

                    g.Children.Add(txtBx);
                }
            }
        }

        public void Clear()
        {
            foreach (TextBox tb in g.Children)
            {
                tb.Text = this.zapoln;
            }
            MatrWrite();
        }

        public void Tranponse()
        {
            string n;
            if (this.nCol == this.NRow)
            {
                for (int i = 0; i < this.NRow; i++)
                {
                    for (int j = 0; j < this.nCol; j++)
                    {
                        if (i < j)
                        {
                            n = this.txtBxs[i, j].Text;
                            this.txtBxs[i, j].Text = this.txtBxs[j, i].Text;
                            this.txtBxs[j, i].Text = n;
                        }
                    }
                }
            }
            else
            {

            }
            
        }

    }
}
