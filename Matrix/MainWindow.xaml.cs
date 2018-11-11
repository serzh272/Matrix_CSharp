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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>    
    public partial class MainWindow : Window
    {
        private MatrWindow MtrWnd;
        private PriblWindow PriblWnd;
        public MainWindow()
        {
            InitializeComponent();
        }
               
        private void btnMatr_Click(object sender, RoutedEventArgs e)
        {
            MtrWnd = new MatrWindow();
            MtrWnd.Owner = this;
            MtrWnd.Title = "Операции с матрицами";
            this.Hide();
            MtrWnd.ShowDialog();
            this.Show();
        }

        private void btnPriblFunc_Click(object sender, RoutedEventArgs e)
        {
            PriblWnd = new PriblWindow();
            PriblWnd.Owner = this;
            PriblWnd.Title = "Приближение функций";
            this.Hide();
            PriblWnd.ShowDialog();
            this.Show();
        }

        private void btnUrav_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
