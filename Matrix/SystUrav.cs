using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;


namespace WpfApp1
{
    class SystUrav
    {
        public int nUr;
        private QuadMatr a;
        private Matr b;

        public SystUrav(QuadMatr m1, Matr m2)
        {
            if (m1.NRow == m1.NCol && m2.NCol ==1 && m1.NRow == m2.NRow)
            {
                this.a = m1.CopyMatr();
                this.b = m2.CopyMatr();
                this.nUr = m1.NRow;
            }
            else
            {
                throw new Exception("Неверная компоновка матриц для системы уравнений!");
            }
        }

        public Matr GaussRoots()
        {
            //начало
            Matr x = new Matr(this.nUr+1, 3);
            //этап 1
            Fraction m = new Fraction();
            for (int shag = 0; shag < this.nUr - 1; shag++)
            {
                for (int i = shag + 1; i < this.nUr; i++)
                {
                    m = this.a.A[i,shag] / this.a.A[shag,shag];
                    for (int j = 0; j < this.nUr; j++)
                    {
                        this.a.A[i,j] = this.a.A[i,j] - m * this.a.A[shag,j];

                    }
                    this.b.A[i,0] = this.b.A[i,0] - m * this.b.A[shag,0];
                }
            }
            //этап 2
            m.FractTxt = "0";
            x.A[this.nUr,0] = this.b.A[this.nUr - 1,0] / this.a.A[this.nUr - 1,this.nUr - 1];
            for (int i = 0; i <= this.nUr - 1; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    m += this.a.A[this.nUr - i - 1,this.nUr - j - 1] * x.A[this.nUr - j,0];
                }
                x.A[this.nUr - i,0] = (this.b.A[this.nUr - i - 1,0] - m) / this.a.A[this.nUr - i - 1,this.nUr - i - 1];
                m.FractTxt = "0";
            }
            //этап 3
            for (int i = 0; i < this.nUr; i++)
            {
                m.FractTxt = "0";
                for (int k = 0; k < this.nUr; k++)
                {
                    m += this.a.A[i,k] * x.A[k+1,0];
                }
                x.A[i+1,1] = m - this.b.A[i,0];

                if (x.A[i+1,1] == 0 || Abs(x.A[i+1,1].ToDouble()) < 0.001)
                {
                    x.A[i + 1, 2].FractTxt = "1";
                }
                else
                {
                    x.A[i + 1, 2].FractTxt = "0";
                }
            }
            return x;
        }

        public Matr GetGaussRootsMatr()
        {
            Matr x = new Matr(this.nUr, 1);
            Matr rt = GaussRoots();
            for (int i = 0; i < this.nUr; i++)
            {
                x.A[i, 0] = rt.A[i + 1, 0].CopyFr;
            }
            return x;
        }

        public Matr OrtoRoots()
        {
            QuadMatr t = new QuadMatr(this.nUr);
            QuadMatr r = new QuadMatr(this.nUr);
            Matr rR = new Matr(1, this.nUr);
            Matr rR2 = new Matr(1, this.nUr);
            Matr rC = new Matr(this.nUr, 1);
            Matr aC = new Matr(this.nUr, 1);
            Matr aR = new Matr(this.nUr, 1);
            t.setEMatr();
            try
            {
                for (int j = 0; j < this.nUr; j++)
                {
                    aC = Matr.CopyColMatr(this.a, j);
                    for (int i = 0; i < this.nUr; i++)
                    {
                        r.A[i, j] = this.a.A[i, j].CopyFr;
                        rR.A[0, j] = r.A[i, j].CopyFr;
                        rC.A[i, 0] = this.a.A[i, j].CopyFr;
                    }
                    
                    for (int i = j+1; i < this.nUr; i++)
                    {
                        aC = Matr.CopyColMatr(this.a, i);
                        for (int n = 0; n < this.nUr; n++)
                        {
                            aR.A[0, n] = aC.A[n, 0].CopyFr;
                        }
                        aC = Matr.CopyColMatr(this.a, i);

                        if (j == 0)
                        {                           
                            t.A[j, i] = (rR * aC).A[0, 0] / (rR * rC).A[0, 0];
                        }
                        else
                        {
                           
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show(ex.Message);
            }
            return new Matr();
        }

        public Matr SimpleIterRoots(Fraction eps, long nIter = 0)
        {
            Matr beta = new Matr(this.b.NRow, this.b.NCol);
            Matr alfa = new Matr(this.a.NRow, this.a.NCol);
            Matr x = new Matr(this.b.NRow+1, 3);
            Matr x2 = new Matr(this.b.NRow, 1);
            Fraction norm = new Fraction();
            Fraction delta = new Fraction();
            Fraction err = new Fraction();
            for (int i = 0; i < this.b.NRow; i++)
            {
                beta.A[i, 0].FractTxt = (this.b.A[i, 0] / this.a.A[i, i]).FractTxt;
                x.A[i+1, 0].FractTxt = (this.b.A[i, 0] / this.a.A[i, i]).FractTxt;
                for (int j = 0; j < this.b.NRow; j++)
                {
                    if (i == j)
                    {
                        alfa.A[i, j].FractTxt = beta.A[i, 0].FractTxt; ;
                    }
                    else
                    {
                        alfa.A[i, j].FractTxt = (-1 * this.a.A[i, j] / this.a.A[i, i]).FractTxt;
                        delta += alfa.A[i, j].frAbs();
                    }                    
                }
                if (norm == 0)
                {
                    norm.FractTxt = delta.FractTxt;
                }
                else
                {
                    if (norm < delta)
                    {
                        norm.FractTxt = delta.FractTxt;
                    }
                }
                delta.FractTxt = "0";
            }
            delta = eps * (1 - norm) / norm;
            if (nIter == 0)
            {

                while (err>delta || err == 0)
                {
                    for (int i = 0; i < this.a.NRow; i++)
                    {
                        for (int j = 0; j < this.a.NCol; j++)
                        {
                            if (i != j)
                            {
                                x2.A[i, 0] += alfa.A[i, j] * x.A[j + 1, 0];
                            }
                            else
                            {
                                x2.A[i, 0] += alfa.A[i, j];
                            }
                        }
                    }
                    ///////////////////////////////////////////////////
                    err.FractTxt = "0";
                    for (int i = 0; i < this.a.NRow; i++)
                    {
                        if (err == 0)
                        {
                            err = (x2.A[i, 0] - x.A[i + 1, 0]).frAbs();
                        }
                        else
                        {
                            if (err < (x2.A[i, 0] - x.A[i + 1, 0]).frAbs())
                            {
                                err = (x2.A[i, 0] - x.A[i + 1, 0]).frAbs();
                            }
                        }
                    }
                    ///////////////////////////////////////////////////
                    for (int i = 0; i < this.a.NRow; i++)
                    {
                        x.A[i + 1, 0].FractTxt = x2.A[i, 0].FractTxt;
                        x.A[i + 1, 2].FractTxt = err.FractTxt;

                        x2.A[i, 0].FractTxt = "0";
                    }
                    nIter++;
                    x.A[0, 2].FractTxt = "" + nIter;
                }
            }
            else
            {
                for (int n = 0; n < nIter; n++)
                {
                    for (int i = 0; i < this.a.NRow; i++)
                    {
                        for (int j = 0; j < this.a.NCol; j++)
                        {
                            if (i!=j)
                            {
                                x2.A[i, 0] += alfa.A[i, j] * x.A[j+1, 0];
                            }
                            else
                            {
                                x2.A[i, 0] += alfa.A[i, j];
                            }
                        }
                    }
                    ///////////////////////////////////////////////////
                    err.FractTxt = "0";
                    for (int i = 0; i < this.a.NRow; i++)
                    {
                        if (err == 0)
                        {
                            err = (x2.A[i, 0] - x.A[i + 1, 0]).frAbs();
                        }
                        else
                        {
                            if (err < (x2.A[i, 0] - x.A[i + 1, 0]).frAbs())
                            {
                                err = (x2.A[i, 0] - x.A[i + 1, 0]).frAbs();
                            }
                        }
                    }
                    ///////////////////////////////////////////////////
                    for (int i = 0; i < this.a.NRow; i++)
                    {
                        x.A[i+1, 0].FractTxt = x2.A[i, 0].FractTxt;
                        x.A[i+1,2].FractTxt = err.FractTxt;

                        x2.A[i, 0].FractTxt = "0";
                    }                    
                }
            }
            for (int i = 0; i < this.a.NRow; i++)
            {
                x.A[i + 1, 1].FractTxt = delta.FractTxt;
            }
            x.A[0, 2].FractTxt = "" + nIter;
            return x;
        }

    }
}
