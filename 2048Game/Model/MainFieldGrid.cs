using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace _2048Game.Model
{
    internal class MainFieldGrid
    {
        Grid Grid { get; set; }
        ObservableCollection<Border> Border { get; set; }
        ObservableCollection<Label> Label { get; set; }

        public MainFieldGrid(int size = 0)
        {
            Grid = new Grid();
        }
    }
}
