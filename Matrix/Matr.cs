using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{


    public class Matr
    {
        protected int nRow, nCol;
        public Fraction[,] A;
        public double[,] Ad;
        public Matr()
        {
            this.nRow = 3;
            this.nCol = 3;
            this.A = new Fraction[this.nRow, this.nCol];
            this.Ad = new double[this.nRow, this.nCol];

            for (int i = 0; i < nRow; i++)
            {
                for (int j = 0; j < nCol; j++)
                {
                    this.A[i, j] = new Fraction(); //инициализация элементов матрицы нулями
                    this.Ad[i, j] = 0;
                }
            }
        }

        public Matr(int nRow, int nCol)
        {
            this.nRow = nRow;
            this.nCol = nCol;
            this.A = new Fraction[this.nRow, this.nCol];
            this.Ad = new double[this.nRow, this.nCol];
            for (int i = 0; i < nRow; i++)
            {
                for (int j = 0; j < nCol; j++)
                {
                    this.A[i, j] = new Fraction(); //инициализация элементов матрицы нулями
                    this.Ad[i, j] = 0;
                }
            }
        }
        protected void toDbl()
        {
            for (int i = 0; i < nRow; i++)
            {
                for (int j = 0; j < nCol; j++)
                {
                    this.Ad[i, j] = this.A[i,j].ToDouble();
                }
            }
        }

        public QuadMatr toQuadMatr()
        {
            int nC = Math.Min(this.NCol, this.NRow);
            QuadMatr rez = new QuadMatr(nC);
            for (int i = 0; i < nC; i++)
            {
                for (int j = 0; j < nC; j++)
                {
                    rez.A[i, j] = this.A[i, j].CopyFr;
                    rez.Ad[i, j] = this.Ad[i, j];
                }
            }
            return rez;
        }

        public int NCol
        {
            get
            {
                return this.nCol;
            }
        }

        public int NRow
        {
            get
            {
                return this.nRow;
            }
        }

        public virtual void InsertCol(int nC)
        {
            if (nC < 0)
            {
                nC = 0;
            }
            Fraction [,] A2 = new Fraction[this.nRow, this.nCol + nC];
            double [,] Ad2 = new double[this.nRow, this.nCol + nC];
            for (int i = 0; i < this.nRow; i++)
            {
                for (int j = 0; j < this.nCol; j++)
                {
                   A2[i, j] = this.A[i, j].CopyFr;
                   Ad2[i, j] = this.Ad[i, j];
                }
            }
            A = A2;
            A2 = null;
            Ad = Ad2;
            Ad2 = null;
            for (int j = this.nCol; j < this.nCol + nC; j++)
            {
                for (int i = 0; i < this.nRow; i++)
                {
                    A[i, j] = new Fraction();
                    Ad[i, j] = 0;
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
            Fraction[,] A2 = new Fraction[this.nRow + nR, this.nCol];
            double[,] Ad2 = new double[this.nRow + nR, this.nCol];
            for (int i = 0; i < this.nRow; i++)
            {
                for (int j = 0; j < this.nCol; j++)
                {
                    A2[i, j] = this.A[i, j].CopyFr;
                    Ad2[i, j] = this.Ad[i, j];
                }
            }
            A = A2;
            A2 = null;
            Ad = Ad2;
            Ad2 = null;
            for (int j = 0; j < this.nCol; j++)
            {
                for (int i = this.nRow; i < this.nRow + nR; i++)
                {
                    A[i, j] = new Fraction();
                    Ad[i, j] = 0;
                }
            }
            this.nRow += nR;
        }

        public virtual void DeleteCol(int nC)
        {
            if (nC < 0 || nC >= this.nCol)
            {
                nC = 0;
            }
            Fraction[,] A2 = new Fraction[this.nRow, this.nCol - nC];
            double[,] Ad2 = new double[this.nRow, this.nCol - nC];
            for (int i = 0; i < this.nRow; i++)
            {
                for (int j = 0; j < this.nCol - nC; j++)
                {
                    A2[i, j] = this.A[i, j].CopyFr;
                    Ad2[i, j] = this.Ad[i, j];
                }
            }            
            A = A2;
            A2 = null;
            Ad = Ad2;
            Ad2 = null;
            this.nCol -= nC;
        }

        public virtual void DeleteRow(int nR)
        {
            if (nR < 0 || nR >= this.nRow)
            {
                nR = 0;
            }
            Fraction[,] A2 = new Fraction[this.nRow - nR, this.nCol];
            double[,] Ad2 = new double[this.nRow - nR, this.nCol];
            for (int i = 0; i < this.nRow - nR; i++)
            {
                for (int j = 0; j < this.nCol; j++)
                {
                    A2[i, j] = this.A[i, j].CopyFr;
                    Ad2[i, j] = this.Ad[i, j];
                }
            }
            A = A2;
            A2 = null;
            Ad = Ad2;
            Ad2 = null;
            this.nRow -= nR;
        }

        public void swapRows(int r1, int r2)
        {
            Fraction f;
            for (int j = 0; j < this.nCol; j++)
            {
                f = this.A[r1, j];
                this.A[r1, j] = this.A[r2, j];
                this.A[r2, j] = f;
            }
            this.toDbl();
        }

        public void swapCols(int c1, int c2)
        {
            Fraction f;
            for (int i = 0; i < this.nRow; i++)
            {
                f = this.A[i, c1];
                this.A[i, c1] = this.A[i, c2];
                this.A[i, c2] = f;
            }
            this.toDbl();
        }

        public void multRows(int r1, int r2, Fraction fr)
        {
            for (int j = 0; j < this.nCol; j++)
            {
                this.A[r2, j] -= this.A[r1, j] * fr;
            }
            this.toDbl();
        }

        public void multCols(int c1, int c2, Fraction fr)
        {
            for (int i = 0; i < this.nCol; i++)
            {

                this.A[i, c2] = this.A[i, c2] - this.A[i, c1] * fr;
            }
            this.toDbl();
        }

        public Matr CopyMatr()
        {
            Matr rez = new Matr(this.nRow, this.nCol);
            for (int i = 0; i < this.nRow; i++)
            {
                for (int j = 0; j < this.nCol; j++)
                {
                    rez.A[i, j] = this.A[i, j].CopyFr;
                    rez.Ad[i, j] = this.Ad[i, j];
                    
                }
            }
            return rez;
        }

        public static Matr CopyRowMatr(Matr m, int Row)
        {
            if (Row <= m.nRow)
            {
                Matr rez = new Matr(1, m.nCol);
                for (int j = 0; j < m.nCol; j++)
                {
                    rez.A[0, j] = m.A[Row, j].CopyFr;
                    rez.Ad[0, j] = m.Ad[Row, j];
                }
                return rez;
            }
            else
            {
                throw new Exception("Номер строки должен быть не больше количества строк матрицы!");
            }
        }

        public static Matr CopyColMatr(Matr m, int Col)
        {
            if (Col <= m.nCol)
            {
                Matr rez = new Matr(m.nRow,1);
                for (int i = 0; i < m.nRow; i++)
                {
                    rez.A[i, 0] = m.A[i, Col].CopyFr;
                    rez.Ad[i, 0] = m.Ad[i, Col];
                }
                return rez;
            }
            else
            {
                throw new Exception("Номер столбца должен быть не больше количества столбцов матрицы!");
            }
        }

        public static Matr operator *(Matr m1, Matr m2)
        {
            if (m1.nCol == m2.nRow)
            {
                Matr rez = new Matr(m1.nRow, m2.nCol);
                for (int i = 0; i < m1.nRow; i++)
                {
                    for (int j = 0; j < m2.nCol; j++)
                    {
                        for (int g = 0; g < m2.nRow; g++)
                        {
                            rez.A[i, j] += m1.A[i, g] * m2.A[g, j];
                            rez.Ad[i, j] += m1.Ad[i, g] * m2.Ad[g, j];
                        }
                    }
                }
                return rez;
            }
            else
            {
                throw new Exception("Количество столбцов матрицы1 должно быть равно количеству строк матрицы2!");
            }
        }

        

        public static Matr operator *(Matr m1, Fraction fr)
        {
            Matr rez = new Matr(m1.nRow, m1.nCol);
            for (int i = 0; i < m1.nRow; i++)
            {
                for (int j = 0; j < m1.nCol; j++)
                {
                    rez.A[i, j] = m1.A[i, j] * fr;
                    rez.Ad[i, j] = m1.Ad[i, j] * fr.ToDouble();
                }
            }
            
            return rez;
        }

        public static Matr operator *(Fraction fr, Matr m1)
        {

            Matr rez = new Matr(m1.nRow, m1.nCol);
            for (int i = 0; i < m1.nRow; i++)
            {
                for (int j = 0; j < m1.nCol; j++)
                {
                    rez.A[i, j] = m1.A[i, j] * fr;
                    rez.Ad[i, j] = m1.Ad[i, j] * fr.ToDouble();
                }
            }
            return rez;
        }

        public static Matr operator /(Matr m1, Fraction fr)
        {
            Matr rez = new Matr(m1.nRow, m1.nCol);
            for (int i = 0; i < m1.nRow; i++)
            {
                for (int j = 0; j < m1.nCol; j++)
                {
                    rez.A[i, j] = m1.A[i, j] / fr;
                    rez.Ad[i, j] = m1.Ad[i, j] / fr.ToDouble();
                }
            }

            return rez;
        }

        public static Matr operator+(Matr m1, Matr m2)
        {
            if (m1.nCol != m2.nCol || m1.nRow != m2.nRow)
            {
                throw new Exception("Размерности матриц не совпадают");
            }
            Matr rez = new Matr(m2.nRow, m2.nCol);
            for (int i = 0; i < m2.nRow; i++)
            {
                for (int j = 0; j < m2.nCol; j++)
                {
                    rez.A[i,j] = m1.A[i,j] + m2.A[i,j];
                    rez.Ad[i,j] = m1.Ad[i,j] + m2.Ad[i,j];
                }
            }
            return rez;
        }

        public static Matr operator-(Matr m1, Matr m2)
        {
            if (m1.nCol != m2.nCol || m1.nRow != m2.nRow)
            {
                throw new Exception("Размерности матриц не совпадают");
            }
            Matr rez = new Matr(m2.nRow, m2.nCol);
            for (int i = 0; i < m2.nRow; i++)
            {
                for (int j = 0; j < m2.nCol; j++)
                {
                    rez.A[i,j] = m1.A[i,j] - m2.A[i,j];
                    rez.Ad[i,j] = m1.Ad[i,j] - m2.Ad[i,j];
                }
            }
            return rez;
        }
    }
}
