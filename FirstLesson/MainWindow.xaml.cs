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

namespace FirstLesson
{
    public class Phone
    {
        public string Title { get; set; }
        public string Company { get; set; }
        public int Price { get; set; }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
        }

        //private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        //{
        //    TreeViewItem treeViewItem = (TreeViewItem)sender;
        //    MessageBox.Show("Узел " + treeViewItem.Header.ToString() + " раскрыт");
        //}

        //private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        //{
        //    TreeViewItem treeViewItem = (TreeViewItem)sender;
        //    MessageBox.Show("Узел " + treeViewItem.Header.ToString() + " выбран");
        //}
        
    }
}
