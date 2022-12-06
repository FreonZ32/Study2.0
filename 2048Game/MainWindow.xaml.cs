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

namespace _2048Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int size_of_field;  //размер MainPlayField (изменяется в RBSize_Click)
        public MainWindow()
        {
            InitializeComponent();
            size_of_field = 4;
        }

        private void RBSize_Click(object sender, RoutedEventArgs e)
        {
            string name = (sender as RadioButton).Name;
            switch (name)
            {
                case "RBSize4": size_of_field = 4; break;
                case "RBSize8": size_of_field = 8; break;
                case "RBSize16": size_of_field = 16; break;
                default:
                    break;
            }
        }
    }
}
