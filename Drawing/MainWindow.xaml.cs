using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
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

namespace Drawing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool MouseIsStillDown = false;
        int thikness = 1;
        double first_pointX = 0;
        double first_pointY = 0;
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void DrawField_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MouseIsStillDown = true;
            first_pointX = e.GetPosition(DrawingField).X;
            first_pointY = e.GetPosition(DrawingField).Y;
        }

        private void DrawingField_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MouseIsStillDown=false;
        }

        private void DrawingField_MouseMove(object sender, MouseEventArgs e)
        {
            if(MouseIsStillDown == true)
            {
                if (Toolss.SelectedIndex == 0)
                {
                    var myRect = new Rectangle();
                    myRect.Fill = Brushes.Black;
                    myRect.Width = thikness;
                    myRect.Height = thikness;
                    int PositionX = (int)(Math.Floor(e.GetPosition(DrawingField).X / thikness) * thikness);
                    // вычисляем позицию по оси X для добавления прямоугольника 
                    int PositionY = (int)(Math.Floor(e.GetPosition(DrawingField).Y / thikness) * thikness);
                    // вычисляем позицию по оси Y для добавления прямоугольника
                    Canvas.SetLeft(myRect, PositionX);
                    Canvas.SetTop(myRect, PositionY);
                    DrawingField.Children.Add(myRect);
                }
            }
        }
        private void thikness_status_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            thikness = Convert.ToInt32(e.NewValue);
        }
    }
}
