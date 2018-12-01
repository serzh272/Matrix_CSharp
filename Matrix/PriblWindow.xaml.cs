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
    /// Логика взаимодействия для PriblWindow.xaml
    /// </summary>
    public partial class PriblWindow : Window
    {
        private Table tabl;
        private Matr tblMatr;
        public PriblWindow()
        {
            InitializeComponent();
            tblMatr = new Matr(int.Parse(tblRows.Text)+1, int.Parse(tblRows.Text)+1);
            tabl = new Table(ref GrdFunc, ref tblMatr, 2);
            tblIspRow.maxVal = tabl.NRow - 1;
            Poryadok.maxVal = tabl.NCol - 2;
            myWeb.NavigateToString("<h1>Test</h1>");
            txtPolynom.Text = MyFuncs.diffUr("y(1)=(2/x)*y+x",new Fraction(), new Fraction(1,1),new Fraction(1,20),new Fraction(3,2)).FractTxt;
           
        }        

        private void tblRows_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tabl != null)
            {
                if (tabl.NRow < int.Parse(tblRows.Text) + 1)
                {
                    tabl.InsertRow(int.Parse(tblRows.Text) - tabl.NRow+1);
                }
                if (tabl.NRow > int.Parse(tblRows.Text) + 1)
                {
                    tabl.DeleteRow(tabl.NRow - int.Parse(tblRows.Text)-1);
                }
                tblIspRow.maxVal = tabl.NRow - 1;
                Poryadok.maxVal = tabl.NCol - 2;
                if (int.Parse(tblRows.Text) <= int.Parse(tblIspRow.Text))
                {
                    tblIspRow.Text = tblRows.Text;
                }
            }            
        }

        private void btnKonRazn_Click(object sender, RoutedEventArgs e)
        {
            tabl.MatrWrite();
            tabl.GetTablKonRazn();
        }

        private void btnRazdelRazn_Click(object sender, RoutedEventArgs e)
        {
            tabl.GetTablRazdelRazn();
        }

        private void btnLagr_Click(object sender, RoutedEventArgs e)
        {
            if (tabl.isRavnom())
            {
                tabl.GetTablKonRazn();
            }
            else
            {
                tabl.GetTablRazdelRazn();
            }
            txtPolynom.Text = "";
            txtPolynom.Text = tabl.GetPolyLagrange();
            Fraction fr = new Fraction();
            fr.FractTxt = MyFuncs.convertToFract(valPoly.Text);
            txtPolynom.Text = txtPolynom.Text + "=" + tabl.GetPolyLagrangeVal(fr).FractTxt;
        }

        private void btnNewton_Click(object sender, RoutedEventArgs e)
        {
            if (tabl.isValid())
            {
                if (tabl.isRavnom())
                {
                    tabl.GetTablKonRazn();
                }
                else
                {
                    tabl.GetTablRazdelRazn();
                }
                txtPolynom.Text = "";
                Fraction fr = new Fraction();
                fr.FractTxt = MyFuncs.convertToFract(valPoly.Text);
                txtPolynom.Text = tabl.GetPolyNewton(int.Parse(Poryadok.Text), int.Parse(tblIspRow.Text)) + "=" + tabl.GetPolyNewtonVal(fr, int.Parse(Poryadok.Text), int.Parse(tblIspRow.Text)).FractTxt;
                GrdGraf.Children.Clear();
                tabl.DrawPolynom(ref GrdGraf, int.Parse(Poryadok.Text), fr, int.Parse(tblIspRow.Text), 50);
            }
            else
            {
                MessageBox.Show("Заполните все значения X и Y!");
            }            
        }

        private void tblIspRow_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Poryadok_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnTrapForm_Click(object sender, RoutedEventArgs e)
        {
            if (tabl.isRavnom())
            {
                tabl.GetTablKonRazn();
            }
            else
            {
                tabl.GetTablRazdelRazn();
            }
            Fraction a = new Fraction();
            Fraction b = new Fraction();
            Fraction fr = new Fraction();
            fr.FractTxt = MyFuncs.convertToFract(valPoly.Text);
            a.FractTxt = MyFuncs.convertToFract(val_a.Text);
            b.FractTxt = MyFuncs.convertToFract(val_b.Text);
            GrdGraf.Children.Clear();
            tabl.DrawIntegr(ref GrdGraf, a, b, int.Parse(numInt.Text));
            tabl.DrawPolynom(ref GrdGraf, tabl.NCol-2, fr, 1, 50);
            MessageBox.Show("Интеграл равен: " + tabl.GetTrapForm(a, b, int.Parse(numInt.Text)).ToDouble());
        }
    }
}
