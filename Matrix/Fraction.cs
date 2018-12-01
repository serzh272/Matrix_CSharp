using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Math;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace WpfApp1
{
    public class Fraction
    {
        private long numerator;
        private long denominator;
        private string fractTxt;
        public Fraction()
        {
            this.numerator = 0;
            this.denominator = 1;
            FractTxt = "0";
            this.Color = Colors.White;
        }

        public Fraction(long n, long d)
        {
            this.numerator = n;
            this.denominator = d;
            this.Normalize();
            if (this.denominator == 1)
            {
                FractTxt = "" + this.numerator;
            }
            else
            {
                FractTxt = this.numerator + "/" + this.denominator;
            }
            this.Color = Colors.White;
        }

        public string FractTxt {
            get
            {
                return this.fractTxt;
            }
            set
            {
                string pattern = @"(\-?\d+)\/?(\d*)";
                foreach (Match  m in Regex.Matches(value, pattern))
                {
                    if (m.Groups[1].Value != "")
                    {
                        this.numerator = long.Parse(m.Groups[1].Value);
                    }
                    
                    if (m.Groups[2].Value != "")
                    {
                        this.denominator = long.Parse(m.Groups[2].Value);
                    }
                    else
                    {
                        this.denominator = 1;
                    }
                }
                this.Normalize();
            }
        }

        public Fraction CopyFr
        {
            get
            {
                return new Fraction(this.numerator,this.denominator) ;
            }            
        }

        public Color Color { get; set; }

        private static long NOD(long a, long b)
        {
            a = Abs(a);
            b = Abs(b);
            while (a != 0 && b != 0)
            {
                if (a > b)
                {
                    a = a % b;
                }
                else
                {
                    b = b % a;
                }
            }
            return a + b;
        }

        private static long NOK(long a, long b)
        {
            a = Abs(a);
            b = Abs(b);
            a = a / NOD(a, b);
            return a * b;
        }

        private void Normalize()
        {
            if (this.numerator == 0)
            {
                this.denominator = 1;
                this.fractTxt = "0";
            }
            else
            {
                long nd = NOD(this.numerator, this.denominator);
                this.numerator /= nd;
                this.denominator /= nd;
                if (this.denominator == 1)
                {
                    this.fractTxt = "" + this.numerator;
                }
                else
                {
                    this.fractTxt = this.numerator + "/" + this.denominator;
                }
            }
            
        }

        public Fraction Invert()
        {
            Fraction rez = new Fraction(this.numerator, this.denominator);
            if (rez.numerator != 0)
            {
                long n = rez.denominator;
                if (rez.numerator > 0)
                {
                    rez.denominator = rez.numerator;
                    rez.numerator = n;
                }
                else
                {
                    rez.denominator = -rez.numerator;
                    rez.numerator = -n;
                }
                rez.fractTxt = rez.numerator + "/" + rez.denominator; ;
            }
            return rez;
        }

        public Fraction frAbs()
        {
            Fraction rez = new Fraction(this.numerator, this.denominator);
            if (rez < 0)
            {
                rez = -1 * rez;
            }
            return rez;
        }

        public static Fraction operator+(Fraction fr1, Fraction fr2)
        {
            long nk = NOK(fr1.denominator, fr2.denominator);
            long n1 = fr1.numerator * (nk / fr1.denominator);
            long n2 = fr2.numerator * (nk / fr2.denominator);
            return new Fraction(n1 + n2, nk);
        }

        public static Fraction operator +(Fraction fr1, long n)
        {
            return new Fraction(fr1.numerator + n * fr1.denominator, fr1.denominator);
        }

        public static Fraction operator +(long n, Fraction fr1)
        {
            return fr1 + n;
        }

        public static Fraction operator-(Fraction fr1, Fraction fr2)
        {
            long nk = NOK(fr1.denominator, fr2.denominator);
            long n1 = fr1.numerator * (nk / fr1.denominator);
            long n2 = fr2.numerator * (nk / fr2.denominator);
            return new Fraction(n1 - n2, nk);
        }

        public static Fraction operator -(Fraction fr1, long n)
        {           
            return new Fraction(fr1.numerator - n * fr1.denominator, fr1.denominator);
        }

        public static Fraction operator -(long n, Fraction fr1)
        {
            return new Fraction(n * fr1.denominator - fr1.numerator, fr1.denominator);
        }

        public static Fraction operator*(Fraction fr1, Fraction fr2) //перегрузка оператора умножения
        {
	        Fraction i = new Fraction(fr1.numerator, fr1.denominator); ;
            Fraction j = new Fraction(fr2.numerator, fr2.denominator); ;
            long l = j.numerator;
            j.numerator = i.numerator;
	        i.numerator = l;
	        j.Normalize();
	        i.Normalize();
	        return new Fraction(j.numerator * i.numerator, j.denominator * i.denominator);
        }

        public static Fraction operator *(Fraction fr1, long n) //перегрузка оператора умножения
        {
            Fraction i = new Fraction(fr1.numerator, fr1.denominator); ;
            Fraction j = new Fraction(n, 1); ;            
            return i*j;
        }

        public static Fraction operator *(long n, Fraction fr1) //перегрузка оператора умножения
        {
            return fr1 * n;
        }

        public static Fraction operator/(Fraction fr1, Fraction fr2) //необходимо проверить
        {
	        long l;
            Fraction i = new Fraction(fr2.numerator, fr2.denominator);
            Fraction j = new Fraction(fr1.numerator, fr1.denominator);
            if (fr2.numerator< 0)
	        {
		        l = -i.numerator;
		        i.numerator = i.denominator;
		        i.denominator = l;
		        j.numerator = -j.numerator;
	        }
	        else if (fr2.numerator == 0)
	        {
		        //выбросить исключение
	        }
	        else
	        {
		        l = i.numerator;
		        i.numerator = i.denominator;
		        i.denominator = l;
	        }
	        l = j.numerator;
	        j.numerator = i.numerator;
	        i.numerator = l;
	        j.Normalize();
	        i.Normalize();
	        return new Fraction(j.numerator* i.numerator, i.denominator* j.denominator);
        }

        public static Fraction operator /(Fraction fr1, long n) //необходимо проверить
        {
            long l;
            Fraction fr2 = new Fraction(n, 1);
            Fraction i = new Fraction(fr2.numerator, fr2.denominator);
            Fraction j = new Fraction(fr1.numerator, fr1.denominator);
            if (fr2.numerator < 0)
            {
                l = -i.numerator;
                i.numerator = i.denominator;
                i.denominator = l;
                j.numerator = -j.numerator;
            }
            else if (fr2.numerator == 0)
            {
                //выбросить исключение
            }
            else
            {
                l = i.numerator;
                i.numerator = i.denominator;
                i.denominator = l;
            }
            l = j.numerator;
            j.numerator = i.numerator;
            i.numerator = l;
            j.Normalize();
            i.Normalize();
            return new Fraction(j.numerator * i.numerator, i.denominator * j.denominator);
        }

        public static Fraction operator /(long n, Fraction fr1)
        {
           return  (fr1 / n).Invert();
        }

        public static bool operator==(Fraction fr1, long l)
        {
	        if (fr1.denominator == 1 && l == fr1.numerator)

            {
                    return true;
            }
	        else
            {
                return false;
            }
        }

        public static bool operator !=(Fraction fr1, long l)
        {
            if (fr1.denominator == 1 && l == fr1.numerator)

            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool operator >(Fraction fr1, long l)
        {            
            return (fr1.numerator > l * fr1.denominator);            
        }

        public static bool operator <(Fraction fr1, long l)
        {
            return !(fr1 > l) && (fr1 != l);
        }

        public static bool operator >=(Fraction fr1, long l)
        {
            return !(fr1 < l);
        }

        public static bool operator <=(Fraction fr1, long l)
        {
            return !(fr1 > l);
        }

        public static bool operator ==(Fraction fr1, Fraction fr2)
        {
            if (fr1.numerator == fr2.numerator && fr1.denominator == fr2.denominator)

            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(Fraction fr1, Fraction fr2)
        {
            if (fr1==fr2)

            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool operator >(Fraction fr1, Fraction fr2)
        {
            long nk = NOK(fr1.denominator, fr2.denominator);
            long n1 = fr1.numerator * (nk / fr1.denominator);
            long n2 = fr2.numerator * (nk / fr2.denominator);
            return n1 > n2;
        }

        public static bool operator <(Fraction fr1, Fraction fr2)
        {            
            return !(fr1 > fr2) && fr1 != fr2;//проверить
        }

        public static bool operator >=(Fraction fr1, Fraction fr2)
        {
            return !(fr1 < fr2);
        }

        public static bool operator <=(Fraction fr1, Fraction fr2)
        {
            return !(fr1 > fr2);
        }

        public double ToDouble()
        {
            return (double)this.numerator / this.denominator;
        }
        public Fraction Power(int power)
        {
            Fraction rez = new Fraction(1, 1);
            for (int i = 0; i < power; i++)
            {
                rez *= this;
            }
            return rez;
        }
    }
}
