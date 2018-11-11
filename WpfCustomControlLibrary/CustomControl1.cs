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

namespace WpfCustomControlLibrary
{
    /// <summary>
    /// Выполните шаги 1a или 1b, а затем 2, чтобы использовать этот настраиваемый элемент управления в файле XAML.
    ///
    /// Шаг 1a. Использование настраиваемого элемента управления в файле XAML, существующем в текущем проекте.
    /// Добавьте атрибут XmlNamespace к корневому элементу файла разметки, где он 
    /// должен использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfCustomControlLibrary"
    ///
    ///
    /// Шаг 1b. Использование этого настраиваемого элемента управления в файле XAML, существующем в текущем проекте.
    /// Добавьте атрибут XmlNamespace к корневому элементу файла разметки, где он 
    /// должен использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfCustomControlLibrary;assembly=WpfCustomControlLibrary"
    ///
    /// Потребуется также добавить ссылку на проект из проекта, в котором находится файл XAML
    /// в данный проект и пересобрать во избежание ошибок компиляции:
    ///
    ///     Правой кнопкой мыши щелкните проект в обозревателе решений и выберите команду
    ///     "Добавить ссылку"->"Проекты"->[Выберите этот проект]
    ///
    ///
    /// Шаг 2)
    /// Продолжайте дальше и используйте элемент управления в файле XAML.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    [TemplatePart(Name = "PART_btnUp", Type = typeof(Button))]
    [TemplatePart(Name = "PART_btnDn", Type = typeof(Button))]
    public class Spinner : TextBox
    {
        private int minV = 1;
        private int maxV = 10;
        static Spinner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Spinner), new FrameworkPropertyMetadata(typeof(Spinner)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var btnUp = this.GetTemplateChild("PART_btnUp") as Button;
            var btnDn = this.GetTemplateChild("PART_btnDn") as Button;
            if (btnUp != null)
            {
                btnUp.Click += btnUp_Click;
            }
            if (btnDn != null)
            {
                btnDn.Click += btnDn_Click;
            }
        }
        public static readonly DependencyProperty minValProperty = DependencyProperty.Register("minVal", typeof(int), typeof(Spinner), new UIPropertyMetadata(0));
        public int minVal
        { get { return (int)GetValue(minValProperty); }
          set { SetValue(minValProperty, value); minV = value; }
        }
        public static readonly DependencyProperty maxValProperty = DependencyProperty.Register("maxVal", typeof(int), typeof(Spinner), new UIPropertyMetadata(0));
        public int maxVal
        {
            get { return (int)GetValue(maxValProperty); }
            set { SetValue(maxValProperty, value); maxV = value; }
        }


        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            if (int.Parse(Text) >= maxVal)
            {
                Text = "" + (maxVal-1);
            }
            Text = (int.Parse(Text) + 1) + "";
        }

        private void btnDn_Click(object sender, RoutedEventArgs e)
        {
            if (int.Parse(Text) <= minVal)
            {
                Text = "" + minVal;
            }
            else
            {
                Text = (int.Parse(Text) - 1) + "";
            }
            
        }

        public event EventHandler<SpinnerUpArgs> SpinnerUp;
        public event EventHandler<SpinnerDnArgs> SpinnerDn;

        protected void RaiseSpinnerUp(string oldVal)
        {
            if (SpinnerUp !=  null)
            {
                SpinnerUp(this, new SpinnerUpArgs(oldVal));
            }            
        }
        protected void RaiseSpinnerDn(string oldVal)
        {
            if (SpinnerUp != null)
            {
                SpinnerDn(this, new SpinnerDnArgs(oldVal));
            }
        }

    }

    public class SpinnerDnArgs
    {
        public SpinnerDnArgs(string oldValue)
        {
            this.OldValue = oldValue;
        }
        public string OldValue { get; private set; }
    }

    public class SpinnerUpArgs : EventArgs
    {
        public SpinnerUpArgs(string oldValue)
        {
            this.OldValue = oldValue;
        }
        public string OldValue { get; private set; }
    }
}
