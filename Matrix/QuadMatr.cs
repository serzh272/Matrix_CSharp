using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp1
{
    public class QuadMatr : Matr
    {
        public QuadMatr():base(3,3)
        {
        }

        public QuadMatr(int nRow):base(nRow,nRow)
        {
        }       

        public QuadMatr getFrobeniusMatr()
        {
            int n = this.NCol;
            QuadMatr mFr = new QuadMatr(n);
            QuadMatr e = new QuadMatr(n);
            QuadMatr m1 = new QuadMatr(n);
            Array.Copy(this.A, mFr.A, this.A.Length);
            for (int g = 0; g < n - 1; g++)
            {
                Fraction d = new Fraction();
                for (int i = 0; i <= n - g - 2; i++)
                {
                    d += mFr.A[n - g - 1, i];
                }
                if (d == 0)
                {
                    QuadMatr b = new QuadMatr(n - g - 1);
                    for (int i = 0; i < n - g - 1; i++)
                    {
                        for (int j = 0; j < n - g - 1; j++)
                        {
                           b.A[i, j] = mFr.A[i, j];
                        }
                    }
                    b = b.getFrobeniusMatr();
                    for (int i = 0; i < n - g - 1; i++)
                    {
                        for (int j = 0; j < n - g - 1; j++)
                        {
                            mFr.A[i, j] = b.A[i, j];
                            mFr.A[i, j].Color = Color.FromRgb(127, 251, 189);
                        }
                    }
                    for (int i = n - g - 1; i < n; i++)
                    {
                        for (int j = n - g - 1; j < n; j++)
                        {                            
                            mFr.A[i, j].Color = Color.FromRgb(127, 251, 189);
                        }
                    }
                    return mFr;
                }
                else if (mFr.A[n - g - 1,n - g - 2] == 0)
                {
                    mFr.swapCols(n - g - 1, n - g - 2);
                    mFr.swapRows(n - g - 1, n - g - 2);
                }
                e.setEMatr();
                for (int i = 0; i < n; i++)
                {
                    e.A[n - g - 2, i].FractTxt = mFr.A[n - g - 1, i].FractTxt;
                }
                if (e.determinant() !=0)
                {
                    mFr = (e * mFr * e.TransformMatr(1));
                }
                else
                {
                    mFr = (e * mFr * e.TransformMatr(1));
                }
                
            }
            return mFr;
        }

        public string GetPolynom(QuadMatr frobMatr)
        {

            return "";
        }

        public QuadMatr TransformMatr(int type)
        {
            if (this.determinant() == 0)
            {
                throw new Exception("Определитель матрицы равен нулю, обратная матрица не существует!");
            }
            else
            {
                Fraction m = new Fraction();
                int n = this.NCol;                
                QuadMatr e = new QuadMatr(n);
                QuadMatr x= new QuadMatr(n);
                QuadMatr y = new QuadMatr(n);
                QuadMatr b = new QuadMatr(n);
                QuadMatr c = new QuadMatr(n);
                e.setEMatr();
                for (int j = 0; j < n; j++)
                {
                    for (int i = j; i < n; i++)
                    {
                        for (int g = 0; g <= j - 1; g++)
                        {
                            m += b.A[i,g] * c.A[g,j];
                        }
                        b.A[i,j] = this.A[i,j] - m;
                        m.FractTxt = "0";
                        for (int g = 0; g <= j - 1; g++)
                        {
                            m += b.A[j,g] * c.A[g,i];
                        }
                        c.A[j,i] = (this.A[j,i] - m) / b.A[j,j];
                        m.FractTxt = "0";
                    }
                }
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int g = 0; g <= j - 1; g++)
                        {
                            m += b.A[j,g] * y.A[g,i];
                        }
                        y.A[j,i] = (e.A[j,i] - m) / b.A[j,j];
                        m.FractTxt = "0";
                    }
                }
                for (int i = 0; i < n; i++)
                {
                    for (int j = n - 1; j >= 0; j--)
                    {
                        for (int g = 0; g < n - j - 1; g++)
                        {
                            m += c.A[j,n - g - 1] * x.A[n - g - 1,i];
                        }
                        x.A[j,i] = y.A[j,i] - m;
                        m.FractTxt = "0";
                    }
                }
                switch (type)
                {
                    case 1:
                        return x;
                    case 2:
                        return b;
                    default:
                        return c;
                }
            }
        }

        public void setEMatr()
        {
            for (int i = 0; i < this.NCol; i++)
            {
                for (int j = 0; j < this.NCol; j++)
                {
                    if (i == j)
                    {
                        this.A[i, j].FractTxt = "1";
                    }
                    else
                    {
                        this.A[i,j].FractTxt = "0";
                    }
                }
            }
        }

        public Matr InverseIterMetod(Fraction lambda, Matr x0)
        {
            Matr rez = new Matr(this.nRow, 1);
            QuadMatr em = new QuadMatr(this.nRow);
            em.setEMatr();
            rez = x0.CopyMatr();
            for (int i = 0; i < 2; i++)            
            {                
                SystUrav su = new SystUrav(this-lambda*em, rez);
                rez = su.GetGaussRootsMatr();
            }
            return rez;
        }

        public new QuadMatr CopyMatr()
        {
            QuadMatr rez = new QuadMatr(this.nRow);
            for (int i = 0; i < this.nRow; i++)
            {
                for (int j = 0; j < this.nCol; j++)
                {
                    rez.A[i, j] = this.A[i, j].CopyFr;
                }
            }
            return rez;
        }

        public Fraction determinant()
        {
            QuadMatr m = new QuadMatr(this.NCol);

            Array.Copy(this.A, m.A, this.A.Length);
            int n = this.NCol;
            Fraction fr1, fr2;
            Fraction d = new Fraction(1, 1);
            double sumRow = 0, sumCol = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (m.A[i,j] == 0)
                    {
                        sumRow = 0;
                        sumCol = 0;
                        for (int p = 0; p < n; p++)
                        {
                            sumCol += Math.Abs(m.A[i,p].ToDouble());
                            sumRow += Math.Abs(m.A[p,j].ToDouble());
                        }
                        if (sumCol == 0 || sumRow == 0)
                        {
                            return new Fraction();
                        }
                    }
                }
            }
            for (int shag = 0; shag < n - 1; shag++)
            {
                for (int i = shag; i < n; i++)
                {
                    if (m.A[i,shag] != 0)
                    {
                        if (m.A[shag,shag] == 0)
                        {
                            this.swapRows(shag, i);
                        }
                        else
                        {
                            for (int j = 0; j < n; j++)
                            {
                                if (i != j && m.A[j,shag] != 0 && j >= shag)
                                {
                                    if (j < i)
                                    {
                                        fr1 = m.A[i,shag];
                                        fr2 = m.A[j,shag];
                                        m.multRows(j, i, fr1 / fr2);
                                    }
                                    else
                                    {
                                        fr1 = m.A[j,shag];
                                        fr2 = m.A[i,shag];
                                        m.multRows(i, j, fr1 / fr2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < n; i++)
            {
                d *= m.A[i,i];
            }
            return d;
        }

        public static QuadMatr operator *(QuadMatr m1, QuadMatr m2)
        {
            if (m1.NCol == m2.NRow)
            {
                QuadMatr rez = new QuadMatr(m1.NRow);
                for (int i = 0; i < m1.NRow; i++)
                {
                    for (int j = 0; j < m2.NCol; j++)
                    {
                        for (int g = 0; g < m1.NRow; g++)
                        {
                            rez.A[i, j] += m1.A[i, g] * m2.A[g, j];
                        }
                    }
                }
                return rez;
            }
            else
            {
                return null;//вызвать исключение
            }
        }

        public static Matr operator *(QuadMatr m1, Fraction fr)
        {
            QuadMatr rez = new QuadMatr(m1.nRow);
            for (int i = 0; i < m1.nRow; i++)
            {
                for (int j = 0; j < m1.nCol; j++)
                {
                    rez.A[i, j] = m1.A[i, j] * fr;
                }
            }
            return rez;
        }

        public static QuadMatr operator *(Fraction fr, QuadMatr m1)
        {

            QuadMatr rez = new QuadMatr(m1.nRow);
            for (int i = 0; i < m1.nRow; i++)
            {
                for (int j = 0; j < m1.nCol; j++)
                {
                    rez.A[i, j] = m1.A[i, j] * fr;
                }
            }
            return rez;
        }

        public static QuadMatr operator -(QuadMatr m1, QuadMatr m2)
        {
            if (m1.nCol != m2.nCol || m1.nRow != m2.nRow)
            {
                throw new Exception("Размерности матриц не совпадают");
            }
            QuadMatr rez = new QuadMatr(m2.nRow);
            for (int i = 0; i < m2.nRow; i++)
            {
                for (int j = 0; j < m2.nCol; j++)
                {
                    rez.A[i, j] = m1.A[i, j] - m2.A[i, j];
                }
            }
            return rez;
        }
    }
}
