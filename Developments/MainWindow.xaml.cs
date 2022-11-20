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

namespace Developments
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    Point p = e.GetPosition(this);
        //    MessageBox.Show("Координата равна x=" + p.X.ToString() + "; y=" + p.Y.ToString());
        //}

        //private void button1_Drop(object sender, DragEventArgs e)
        //{
        //    button1.Content = e.Data.GetData(DataFormats.Text);
        //}

        //private void textbox1_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    DragDrop.DoDragDrop(textbox1, textbox1.Text, DragDropEffects.Copy);
        //}
    }
}
