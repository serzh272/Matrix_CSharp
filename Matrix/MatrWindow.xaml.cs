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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MatrWindow : Window
    {
        MyMatr myM1, myM2, myMRez;
        Matr m1, m2, mRez;
        private RezWindow RezWnd;
        private WndGCircles GCirclesWnd;
        public MatrWindow()
        {            
            InitializeComponent();
            m1 = new Matr(int.Parse(Sp1_1Txt.Text), int.Parse(Sp1_2Txt.Text));
            m2 = new Matr(int.Parse(Sp2_1Txt.Text), int.Parse(Sp2_2Txt.Text));
            myM1 = new MyMatr(ref Matr1Grid, ref m1, 5);
            myM2 = new MyMatr(ref Matr2Grid, ref m2, 5);
            myM1.isDouble = (bool)isDbl.IsChecked;
            myM2.isDouble = (bool)isDbl.IsChecked;
            Sp1_1Txt.Text = "" + myM1.NRow;
            Sp1_2Txt.Text = "" + myM1.NCol;
            Sp2_1Txt.Text = "" + myM2.NRow;
            Sp2_2Txt.Text = "" + myM2.NCol;
        }       

        private void Sp1_1Txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (myM1 != null)
            {
                myM1.MatrWrite();
                if (myM1.NRow < int.Parse(Sp1_1Txt.Text))
                {
                   myM1.matr.InsertRow(int.Parse(Sp1_1Txt.Text) - myM1.NRow);
                }
                if (myM1.NRow > int.Parse(Sp1_1Txt.Text))
                {
                    myM1.matr.DeleteRow(myM1.NRow - int.Parse(Sp1_1Txt.Text));
                }
                myM1.Redraw();
            }
        }

        private void Sp1_2Txt_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (myM1 != null && Sp1_2Txt.Text != "")
            {
                myM1.MatrWrite();
                if (myM1.NCol < int.Parse(Sp1_2Txt.Text))
                {
                    myM1.matr.InsertCol(int.Parse(Sp1_2Txt.Text) - myM1.NCol);
                }
                if (myM1.NCol > int.Parse(Sp1_2Txt.Text))
                {
                    myM1.matr.DeleteCol(myM1.NCol - int.Parse(Sp1_2Txt.Text));
                }
                myM1.Redraw();
            }
        }

        private void Sp2_1Txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (myM2 != null)
            {
                myM2.MatrWrite();
                if (myM2.NRow < int.Parse(Sp2_1Txt.Text))
                {
                    myM2.matr.InsertRow(int.Parse(Sp2_1Txt.Text) - myM2.NRow);
                }
                if (myM2.NRow > int.Parse(Sp2_1Txt.Text))
                {
                    myM2.matr.DeleteRow(myM2.NRow - int.Parse(Sp2_1Txt.Text));
                }
                myM2.Redraw();
            }
        }

        private void Sp2_2Txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (myM2 != null && Sp2_2Txt.Text != "")
            {
                myM2.MatrWrite();
                if (myM2.NCol < int.Parse(Sp2_2Txt.Text))
                {
                    myM2.matr.InsertCol(int.Parse(Sp2_2Txt.Text) - myM2.NCol);
                }
                if (myM2.NCol > int.Parse(Sp2_2Txt.Text))
                {
                    myM2.matr.DeleteCol(myM2.NCol - int.Parse(Sp2_2Txt.Text));
                }
                myM2.Redraw();
            }
        }

        private void SwapMatr_Click(object sender, RoutedEventArgs e)
        {
            myM1.MatrWrite();
            myM2.MatrWrite();
            Matr ms = myM1.matr;
            myM1.matr = myM2.matr;
            myM2.matr = ms;
            myM1.Redraw();
            myM2.Redraw();
        }

        private void btnObrMatr_Click(object sender, RoutedEventArgs e)
        {
            myM1.MatrWrite();
            myM1.Redraw();
            if (myM1.NCol == myM1.NRow)
            {                
                try
                {
                    Matr m = myM1.matr.toQuadMatr().TransformMatr(1);
                    RezWnd = new RezWindow();
                    RezWnd.Owner = this;
                    RezWnd.Title = "Обратная матрица";
                    myMRez = new MyMatr(ref RezWnd.MatrRezGrid, ref m, 5);
                    myMRez.isDouble = (bool)isDbl.IsChecked;
                    myMRez.Redraw();
                    RezWnd.btnCopyTo1.Click += btnCopyTo1_Click;
                    RezWnd.btnCopyTo2.Click += btnCopyTo2_Click;
                    RezWnd.ShowDialog();
                    myMRez = null;
                }
                catch (Exception ex)
                {
                    var msg = MessageBox.Show(ex.Message);
                }                
            }
            else
            {                
                MessageBox.Show("Матрица должна быть квадратной");
            }
        }

        private void btnCopyTo1_Click(object sender, RoutedEventArgs e)
        {
            myM1.matr = myMRez.matr;
            myM1.Redraw(); 
        }

        private void btnCopyTo2_Click(object sender, RoutedEventArgs e)
        {
            myM2.matr = myMRez.matr;
            myM2.Redraw();

        }

        private void btnDnTrMatr_Click(object sender, RoutedEventArgs e)
        {
            myM1.MatrWrite();
            myM1.Redraw();
            if (myM1.NCol == myM1.NRow)
            {                
                try
                {
                    Matr m = myM1.matr.toQuadMatr().TransformMatr(2);
                    RezWnd = new RezWindow();
                    RezWnd.Owner = this;
                    RezWnd.Title = "Нижняя треугольная матрица";
                    myMRez = new MyMatr(ref RezWnd.MatrRezGrid, ref m, 5);
                    myMRez.isDouble = (bool)isDbl.IsChecked;
                    myMRez.Redraw();
                    RezWnd.btnCopyTo1.Click += btnCopyTo1_Click;
                    RezWnd.btnCopyTo2.Click += btnCopyTo2_Click;
                    RezWnd.ShowDialog();
                    myMRez = null;
                }                
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                MessageBox.Show("Матрица должна быть квадратной");
            }
        }

        private void btnUpTrMatr_Click(object sender, RoutedEventArgs e)
        {
            myM1.MatrWrite();
            myM1.Redraw();
            if (myM1.NCol == myM1.NRow)
            {
                
                try
                {
                    Matr m = myM1.matr.toQuadMatr().TransformMatr(3);
                    RezWnd = new RezWindow();
                    RezWnd.Owner = this;
                    RezWnd.Title = "Верхняя треугольная матрица";
                    myMRez = new MyMatr(ref RezWnd.MatrRezGrid, ref m, 5);
                    myMRez.isDouble = (bool)isDbl.IsChecked;
                    myMRez.Redraw();
                    RezWnd.btnCopyTo1.Click += btnCopyTo1_Click;
                    RezWnd.btnCopyTo2.Click += btnCopyTo2_Click;
                    RezWnd.ShowDialog();
                    myMRez = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Матрица должна быть квадратной");
            }
        }

        private void btnFrobMatr_Click(object sender, RoutedEventArgs e)
        {
            myM1.MatrWrite();
            myM1.Redraw();
            if (myM1.NCol == myM1.NRow)
            {                
                try
                {                    
                    Matr m = myM1.matr.toQuadMatr().getFrobeniusMatr();
                    RezWnd = new RezWindow();
                    RezWnd.Owner = this;
                    RezWnd.Title = "Матрица Фробениуса";
                    myMRez = new MyMatr(ref RezWnd.MatrRezGrid, ref m, 5);
                    myMRez.isDouble = (bool)isDbl.IsChecked;
                    myMRez.Redraw();
                    RezWnd.btnCopyTo1.Click += btnCopyTo1_Click;
                    RezWnd.btnCopyTo2.Click += btnCopyTo2_Click;
                    RezWnd.ShowDialog();
                    myMRez = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                MessageBox.Show("Матрица должна быть квадратной");
            }
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            myM1.MatrWrite();
            myM1.Redraw();
            myM2.MatrWrite();
            myM2.Redraw();
            try
            {
                Matr m = myM1.MyMatrToMatr() + myM2.MyMatrToMatr();
                RezWnd = new RezWindow();
                RezWnd.Owner = this;
                RezWnd.Title = "Сумма матриц";
                myMRez = new MyMatr(ref RezWnd.MatrRezGrid, ref m, 5);
                myMRez.isDouble = (bool)isDbl.IsChecked;
                myMRez.Redraw();
                RezWnd.btnCopyTo1.Click += btnCopyTo1_Click;
                RezWnd.btnCopyTo2.Click += btnCopyTo2_Click;
                RezWnd.ShowDialog();
                myMRez = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnMunus_Click(object sender, RoutedEventArgs e)
        {
            myM1.MatrWrite();
            myM1.Redraw();
            myM2.MatrWrite();
            myM2.Redraw();
            try
            {
                Matr m = myM1.MyMatrToMatr() - myM2.MyMatrToMatr();
                RezWnd = new RezWindow();
                RezWnd.Owner = this;
                RezWnd.Title = "Разность матриц";
                myMRez = new MyMatr(ref RezWnd.MatrRezGrid,ref m, 5);
                myMRez.isDouble = (bool)isDbl.IsChecked;
                myMRez.Redraw();
                RezWnd.btnCopyTo1.Click += btnCopyTo1_Click;
                RezWnd.btnCopyTo2.Click += btnCopyTo2_Click;
                RezWnd.ShowDialog();
                myMRez = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnMult_Click(object sender, RoutedEventArgs e)
        {
            myM1.MatrWrite();
            myM1.Redraw();
            myM2.MatrWrite();
            myM2.Redraw();
            try
            {
                Matr m = myM1.MyMatrToMatr() * myM2.MyMatrToMatr();
                RezWnd = new RezWindow();
                RezWnd.Owner = this;
                RezWnd.Title = "Произведение матриц";
                myMRez = new MyMatr(ref RezWnd.MatrRezGrid,ref m, 5);
                myMRez.isDouble = (bool)isDbl.IsChecked;
                myMRez.Redraw();
                RezWnd.btnCopyTo1.Click += btnCopyTo1_Click;
                RezWnd.btnCopyTo2.Click += btnCopyTo2_Click;
                RezWnd.ShowDialog();
                myMRez = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SystUr_Click(object sender, RoutedEventArgs e)
        {
            myM1.MatrWrite();
            myM1.Redraw();
            myM2.MatrWrite();
            myM2.Redraw();
            try
            {
                SystUrav s = new SystUrav(myM1.matr.toQuadMatr(), myM2.matr);
                Matr m = s.GaussRoots();
                RezWnd = new RezWindow();
                RezWnd.Owner = this;
                RezWnd.Title = "Корни системы уравнений";
                myMRez = new MyMatr(ref RezWnd.MatrRezGrid, ref m, 5);
                myMRez.isDouble = (bool)isDbl.IsChecked;
                myMRez.Redraw();
                RezWnd.btnCopyTo1.Visibility = Visibility.Hidden;
                RezWnd.btnCopyTo2.Visibility = Visibility.Hidden;
                TextBox[,] b = myMRez.GetMatrBx();
                b[0, 0].Text = "корни";
                b[0, 0].FontWeight = FontWeights.Bold;
                b[0, 1].Text = "невязка";
                b[0, 1].FontWeight = FontWeights.Bold;
                b[0, 2].Text = "верность";
                b[0, 2].FontWeight = FontWeights.Bold;
                for (int i = 1; i < myMRez.NRow; i++)
                {
                    if (b[i, 2].Text == "1")
                    {
                        b[i, 2].Text = "верно";
                    }
                    else
                    {
                        b[i, 2].Text = "неверно";
                    }
                    
                }

                RezWnd.ShowDialog();
                myMRez = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnDet1_Click(object sender, RoutedEventArgs e)
        {
            myM1.MatrWrite();
            myM1.Redraw();
            try
            {
                myM1.MatrWrite();
                if ((bool)isDbl.IsChecked)
                {
                    Det1.Text = "" + myM1.matr.toQuadMatr().determinant().ToDouble();
                }
                else
	            {                    
                    Det1.Text = myM1.matr.toQuadMatr().determinant().FractTxt;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void btnDet2_Click(object sender, RoutedEventArgs e)
        {
            myM2.MatrWrite();
            myM2.Redraw();
            try
            {
                Det2.Text = myM2.matr.toQuadMatr().determinant().FractTxt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGCircle_Click(object sender, RoutedEventArgs e)
        {
            myM1.MatrWrite();
            myM1.Redraw();
            if (myM1.NCol == myM1.NRow)
            {

                try
                {
                    //QuadMatr m = m1.MyMatrToQuadMatr().TransformMatr(1);
                    GCirclesWnd = new WndGCircles();
                    GCirclesWnd.Owner = this;
                    GCirclesWnd.Title = "Круги Гершгорина";
                    GCirclesWnd.DrawCircles(myM1.matr);
                    GCirclesWnd.ShowDialog();
                    myMRez = null;
                }
                catch (Exception ex)
                {
                    var msg = MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Матрица должна быть квадратной");
            }
        }

        private void SystUr_simple_Click(object sender, RoutedEventArgs e)
        {
            myM1.MatrWrite();
            myM2.MatrWrite();
            try
            {
                SystUrav s = new SystUrav(myM1.matr.toQuadMatr(), myM2.matr);
                Fraction ep = new Fraction();
                ep.FractTxt = MyFuncs.convertToFract(eps.Text);
                Matr m = s.SimpleIterRoots(ep,int.Parse(SimpleIterTxt.Text));
                RezWnd = new RezWindow();
                RezWnd.Owner = this;
                RezWnd.Title = "Корни системы уравнений";
                myMRez = new MyMatr(ref RezWnd.MatrRezGrid, ref m, 5);
                myMRez.isDouble = (bool)isDbl.IsChecked;
                myMRez.Redraw();
                RezWnd.btnCopyTo1.Visibility = Visibility.Hidden;
                RezWnd.btnCopyTo2.Visibility = Visibility.Hidden;
                TextBox[,] b = myMRez.GetMatrBx();
                b[0, 0].Text = "корни";
                b[0, 0].FontWeight = FontWeights.Bold;
                b[0, 1].Text = "" + '\x03B4';
                b[0, 1].FontWeight = FontWeights.Bold;
                b[0, 2].Text = '\x03B4' + "(" + m.A[0,2].FractTxt + ")";
                b[0, 2].FontWeight = FontWeights.Bold;
                //for (int i = 1; i < mRez.NRow; i++)
                //{
                //    if (b[i, 2].Text == "1")
                //    {
                //        b[i, 2].Text = "верно";
                //    }
                //    else
                //    {
                //        b[i, 2].Text = "неверно";
                //    }

                //}

                RezWnd.ShowDialog();
                myMRez = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void SobstChisla_Click(object sender, RoutedEventArgs e)
        {
            myM1.MatrWrite();
            myM2.MatrWrite();
            try
            {
                //SystUrav s = new SystUrav(m1.MyMatrToQuadMatr(), m2.MyMatrToMatr());
                QuadMatr x = myM1.matr.toQuadMatr();
                Fraction fr = new Fraction();
                fr.FractTxt = MyFuncs.convertToFract(lambda.Text);
                Matr lbd = x.InverseIterMetod(fr, myM2.MyMatrToMatr());
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void SystUr_Orto_Click(object sender, RoutedEventArgs e)
        {
            myM1.MatrWrite();
            myM2.MatrWrite();
            SystUrav sr = new SystUrav((QuadMatr)myM1.matr, myM2.MyMatrToMatr());
            Matr r = sr.OrtoRoots();
        }

        private void isDbl_Checked(object sender, RoutedEventArgs e)
        {
            myM1.isDouble = (bool)isDbl.IsChecked;
            myM2.isDouble = (bool)isDbl.IsChecked;
        }

        private void isDbl_Unchecked(object sender, RoutedEventArgs e)
        {
            myM1.isDouble = (bool)isDbl.IsChecked;
            myM2.isDouble = (bool)isDbl.IsChecked;
        }

        private void ClearM1_Click(object sender, RoutedEventArgs e)
        {
            myM1.Clear();
        }
                
        private void TransponseM1_Click(object sender, RoutedEventArgs e)
        {            
            myM1.Tranponse();
            myM1.MatrWrite();
        }       
    }
}
