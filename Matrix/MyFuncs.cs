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
    static class MyFuncs
    {
        public static long factorial(int n) //определение функции для вычисления факториала
        {
            long fc;
            fc = 1;
            if ((n == 0) || (n == 1))
            {
                fc = 1;
            }
            if (n >= 2)
            {
                fc = n * factorial(n - 1);
            }
            return fc;
        }

        public static string convertToFract(string str)
        {
            string rez = "";
            Fraction f = new Fraction();
            rez = str;
            if (str.IndexOf(',') >= 0)
            {
                int dr = str.Length - str.IndexOf(',') - 1;
                f.FractTxt = double.Parse(str) * Math.Pow(10, dr) + "/" + Math.Pow(10, dr);
                rez = f.FractTxt;
            }
            if (str.IndexOf('.') >= 0)
            {
                int dr = str.Length - str.IndexOf('.') - 1;
                f.FractTxt = double.Parse(str.Replace(".", ",")) * Math.Pow(10, dr) + "/" + Math.Pow(10, dr);
                rez = f.FractTxt;
            }
            if (str.IndexOf('/') >= 0)
            {
                f.FractTxt = str;
                rez = f.FractTxt;
            }
            if (str == "")
            {
                rez = "0";
            }
            return rez;
        }

        public static Fraction diffUr(string ur,Fraction y0, Fraction x0, Fraction h, Fraction x, int por = 1)
        {

            return new Fraction();
        }

        public static double userFunction(double x)
        {
            return x*x*x - 6*x + 2;
        }

    }
}
