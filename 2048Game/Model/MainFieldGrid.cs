using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace _2048Game.Model
{
    class MainFieldGrid :INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void Notify([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        int SizeofField { get; set; }
        public Grid FieldGrid { get; set; }
        public ObservableCollection<Border> PlateBorder { get; set; }
        public ObservableCollection<Label> PlateLabel { get; set; }
        List<ColumnDefinition> FieldGrid_Columns { get; set; }
        List<RowDefinition> FieldGrid_Rows { get; set; }

        public MainFieldGrid(int size = 4)
        {
            SizeofField = size;
            FieldGrid = new Grid();
            PlateBorder = new ObservableCollection<Border>();
            PlateLabel = new ObservableCollection<Label>();
            FieldGrid_Columns = new List<ColumnDefinition>();
            FieldGrid_Rows = new List<RowDefinition>();
            ChangeFieldSize(SizeofField);
        }


        public void ChangeFieldSize(int size)
        {
            PlateBorder.Clear();
            PlateLabel.Clear();
            FieldGrid_Columns.Clear();
            FieldGrid_Rows.Clear();
            FieldGrid = new Grid();
            for (int i = 0; i < size; i++)
            {
                FieldGrid_Columns.Add(new ColumnDefinition());
                FieldGrid_Columns[i].Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
                FieldGrid.ColumnDefinitions.Add(FieldGrid_Columns[i]);
                FieldGrid_Rows.Add(new RowDefinition());
                FieldGrid_Rows[i].Height = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
                FieldGrid.RowDefinitions.Add(FieldGrid_Rows[i]);
            }
            for (int i = 0; i < size * size; i++)
            {
                PlateBorder.Add(new System.Windows.Controls.Border());
                PlateBorder[i].Margin = new System.Windows.Thickness(3);
                PlateBorder[i].Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xEE, 0xE2, 0xD6));
                PlateBorder[i].CornerRadius = new System.Windows.CornerRadius(4);
                PlateBorder[i].Height = (455 - (3 * size * 2)) / size;
                PlateLabel.Add(new System.Windows.Controls.Label());
                PlateLabel[i].Content = "0";
                PlateLabel[i].HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                PlateLabel[i].VerticalAlignment = System.Windows.VerticalAlignment.Center;
                PlateLabel[i].FontSize = 24-size;
                PlateBorder[i].Child = PlateLabel[i];
                FieldGrid.Children.Add(PlateBorder[i]);
                Grid.SetColumn(PlateBorder[i], i % size);
                Grid.SetRow(PlateBorder[i], i / size);
            }
        }
        public bool NextStepPlateCreator()
        {
            Random random = new Random();
            List<int> availableTiles = new List<int>();
            for (int i = 0; i < PlateLabel.Count; i++)
            { if (PlateLabel[i].Content == "0") availableTiles.Add(i); }
            if (availableTiles.Count == 0)return false;
            int twoOrfour = random.Next(0, 100);
            int winner = availableTiles[random.Next(0, availableTiles.Count)];
            if (twoOrfour<10)
            {//10% появление плитки 4
                PlateLabel[winner].Content = "4";
                return true;
            }
            else
            {//90% появление плитки 2
                PlateLabel[winner].Content = "2";
                return true;
            }
        }
        public void MoveUp (bool Up = true)
        {
            for (int i = 0; i < SizeofField; i++)
            {
                for (int j = 0; j < SizeofField; j++)
                {
                    if (PlateLabel[i + j * 4].Content == "0") continue;
                    for (int k = j + 1; i + k * 4 < SizeofField*SizeofField;k++)
                    {
                        if (PlateLabel[i + k * 4].Content == "0") continue;
                        if (PlateLabel[i + j * 4].Content == PlateLabel[i + k * 4].Content)
                        {
                            PlateLabel[i + j * 4].Content = (Convert.ToInt32(PlateLabel[i + j * 4].Content) * 2).ToString();
                            PlateLabel[i + k * 4].Content = "0";
                            break;
                        }
                    }
                    for(int l = j; l>0; l--)
                    {
                        if (PlateLabel[i + (l - 1) * 4].Content == "0")
                        {
                            PlateLabel[i + (l - 1) * 4].Content = PlateLabel[i + l * 4].Content;
                            PlateLabel[i + l * 4].Content = "0";
                        }
                        else break;
                    }
                }
            }
        }
        public void MoveDown()
        {
            for (int i = 0; i < SizeofField; i++)
            {
                for (int j = SizeofField - 1; j > 0; j--)
                {
                    if (PlateLabel[i + j * 4].Content == "0") continue;
                    for (int k = j-1 ; k>=0 ; k--)
                    {
                        if(PlateLabel[i + k * 4].Content == "0") continue;
                        if(PlateLabel[i + k * 4].Content == PlateLabel[i + j * 4].Content)
                        {
                            PlateLabel[i + j * 4].Content = (Convert.ToInt32(PlateLabel[i + j * 4].Content) * 2).ToString();
                            PlateLabel[i + k * 4].Content = "0";
                            break;
                        }
                    }
                    for (int l = j; l < SizeofField-1; l++)
                    {
                        if (PlateLabel[i + (l + 1) * 4].Content == "0")
                        {
                            PlateLabel[i + (l + 1) * 4].Content = PlateLabel[i + l * 4].Content;
                            PlateLabel[i + l * 4].Content = "0";
                        }
                        else break;
                    }
                }
            }
        }
    }
}
