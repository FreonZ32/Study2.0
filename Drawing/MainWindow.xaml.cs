using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
        int thikness = 1;
        double first_pointX = 0;
        double first_pointY = 0;
        Color SelectedColor = Color.FromRgb(0, 0, 0);
        bool MoveIsStart = false;
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void DrawField_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            first_pointX = e.GetPosition(DrawingField).X;
            first_pointY = e.GetPosition(DrawingField).Y;
        }

        private void DrawingField_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(Toolss.SelectedIndex == 0) DrawingPoint(e.GetPosition(DrawingField).X, e.GetPosition(DrawingField).Y);
        }
        private void DrawingField_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void DrawingField_MouseMove(object sender, MouseEventArgs e)
        {
            if(Mouse.LeftButton != MouseButtonState.Pressed) MoveIsStart = false;
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (Toolss.SelectedIndex == 0)
                {
                    DrawingPoint(e.GetPosition(DrawingField).X, e.GetPosition(DrawingField).Y);
                }
                if (Toolss.SelectedIndex == 1)
                {
                    if (MoveIsStart == true)
                    {
                        Thread.Sleep(10);
                        UIElement Child = DrawingField.Children[DrawingField.Children.Count - 1];
                        DrawingField.Children.Remove(Child);
                    }
                    DrawingLine(e.GetPosition(DrawingField).X, e.GetPosition(DrawingField).Y);
                    MoveIsStart = true;
                }
                if (Toolss.SelectedIndex == 2)
                {
                    if (MoveIsStart == true)
                    {
                        Thread.Sleep(10);
                        UIElement Child = DrawingField.Children[DrawingField.Children.Count - 1];
                        DrawingField.Children.Remove(Child);
                    }
                    DrawingEllipse(e.GetPosition(DrawingField).X, e.GetPosition(DrawingField).Y);
                    MoveIsStart = true;
                }
            }
            if(e.GetPosition(DrawingField).X>0 && e.GetPosition(DrawingField).Y>0) PositionOfCoursor.Content = Convert.ToInt32(e.GetPosition(DrawingField).X).ToString() + "x" + Convert.ToInt32(e.GetPosition(DrawingField).Y).ToString() + "пкс";
        }
        private void thikness_status_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            thikness = Convert.ToInt32(e.NewValue);
        }
        private void DrawingPoint(double eX, double eY)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Fill = new SolidColorBrush(SelectedColor);
            ellipse.Width = thikness;
            ellipse.Height = thikness;
            double PositionX = eX - thikness / 2;
            if(PositionX < 0 )PositionX = 0;
            if( PositionX > DrawingField.Width) PositionX = DrawingField.Width;//Не обязательно,но для перестраховки думаю стоит
            double PositionY = eY - thikness / 2;
            if (PositionY < 0) PositionY = 0;
            if(PositionY > DrawingField.Height) PositionY = DrawingField.Height;
            Canvas.SetLeft(ellipse, PositionX);
            Canvas.SetTop(ellipse, PositionY);
            DrawingField.Children.Add(ellipse);
        }
        private void DrawingLine( double eX, double eY)
        {
            Line BrushLine = new Line();
            BrushLine.Fill = new SolidColorBrush(SelectedColor);
            BrushLine.Stroke = new SolidColorBrush(SelectedColor);
            BrushLine.StrokeThickness = thikness;
            BrushLine.SnapsToDevicePixels = true;
            BrushLine.X1 = first_pointX;
            BrushLine.Y1 = first_pointY;
            BrushLine.X2 = eX;
            BrushLine.Y2 = eY;
            DrawingField.Children.Add(BrushLine);
        }
        private void DrawingEllipse(double eX, double eY)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Stroke = new SolidColorBrush(SelectedColor);
            ellipse.StrokeThickness = thikness;
            double PositionX, PositionY;
            if (eX > first_pointX)
            {PositionX = first_pointX;ellipse.Width = eX - first_pointX;}
            else
            {PositionX = eX;ellipse.Width = first_pointX - eX;}
            if (eY > first_pointY)
            {PositionY = first_pointY;ellipse.Height = eY - first_pointY;}
            else
            {PositionY = eY;ellipse.Height = first_pointY - eY;}
            Canvas.SetLeft(ellipse, PositionX);
            Canvas.SetTop(ellipse, PositionY);
            DrawingField.Children.Add(ellipse);
        }
    }
}
