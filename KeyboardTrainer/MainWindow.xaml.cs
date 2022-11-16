using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KeyboardTrainer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int fails = 0;
        double speed = 0;
        int diffculty = 1;
        String textToInput = "";
        String inputText = "";
        DispatcherTimer timer;
        DateTime timeStart;
        bool caseSensetive = false;
        int KBBCount;
        bool UpperCaseOn = false;
        List<ColumnDefinition> KBFGridFirstColumns;
        List<Button> KBFieldButtons;

        static List<string> KBButtonsContentLowCase = new List<string>
        {"`","1","2","3","4","5","6","7","8","9","0","-","=","Backspace","Tab","q","w","e","r","t", "y", "u", "i", "o","p","[","]","\\",
            "Caps Lock","a","s","d","f","g","h","j","k","l",";","'","Enter","Shift","z","x","c","v","b","n","m",",",".","/", "Shift",
            "Ctrl", "Win", "Alt","Space","Alt","Win","Ctrl" };
        static List<string> KBButtonsContentUpperCase = new List<string>
        {"~","!","@","#","$","%","^","&","*","(",")","_","+","Backspace","Tab","Q","W","E","R","T", "Y", "U", "I", "O","P","{","}","|",
            "Caps Lock","A","S","D","F","G","H","J","K","L",":","\"","Enter","Shift","Z","X","C","V","B","N","M","<",">","?", "Shift",
            "Ctrl", "Win", "Alt","Space","Alt","Win","Ctrl" };
        static List<int> KBButtonsKeyCode = new List<int>
        {146,35,36,37,38,39,40,41,42,43,34,143,141,2,
        3,60,66,48,61,63,68,64,52,58,59,149,151,150,
        8,44,62,47,49,50,51,53,54,55,140,152,6,
        116,69,67,46,65,45,57,56,142,144,145,117,
        118,70,156,18,118,70,119};
        static List<string> KBButtonsColor = new List<string>// Серый:#FFA3A3A3 Розовый:#FFED9B9B Желтый:#FFEBE871 Зеленый:#FF6BF378 Синий:#FF6BC7F3 Фиолетовый:#FFB78EE9 
        {"#FFED9B9B","#FFED9B9B","#FFED9B9B","#FFEBE871","#FF6BF378","#FF6BC7F3","#FF6BC7F3","#FFB78EE9","#FFB78EE9","#FFA3A3A3","#FFEBE871","#FF6BF378","#FF6BF378","#FFA3A3A3","#FFA3A3A3","#FFA3A3A3","#FFEBE871","#FF6BF378","#FF6BC7F3","#FF6BC7F3", "#FFB78EE9", "#FFB78EE9", "#FFA3A3A3", "#FFEBE871","#FF6BF378","#FF6BF378","#FF6BF378","#FF6BF378",
            "#FFA3A3A3","#FFA3A3A3","#FFEBE871","#FF6BF378","#FF6BC7F3","#FF6BC7F3","#FFB78EE9","#FFB78EE9","#FFA3A3A3","#FFEBE871","#FF6BF378","#FF6BF378","#FFA3A3A3","#FFA3A3A3","#FFA3A3A3","#FFEBE871","#FF6BF378","#FF6BC7F3","#FF6BC7F3","#FFB78EE9","#FFB78EE9","#FFA3A3A3","#FFEBE871","#FF6BF378", "#FFA3A3A3",
            "#FFA3A3A3", "#FFA3A3A3", "#FFA3A3A3","#FFEBE871","#FFA3A3A3","#FFA3A3A3","#FFA3A3A3" };
        int keyDown;

        public MainWindow()
        {
            InitializeComponent();
            DifficultyTextBlock.Text = "1";
            FailsTextBlock.Text = "0";
            SpeedTextBlock.Text = "0";
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 1);


            RowDefinition rowDefinition1 = new RowDefinition();
            RowDefinition rowDefinition2 = new RowDefinition();
            RowDefinition rowDefinition3 = new RowDefinition();
            RowDefinition rowDefinition4 = new RowDefinition();
            RowDefinition rowDefinition5 = new RowDefinition();
            KeyBoardField.RowDefinitions.Add(rowDefinition1);
            KeyBoardField.RowDefinitions.Add(rowDefinition2);
            KeyBoardField.RowDefinitions.Add(rowDefinition3);
            KeyBoardField.RowDefinitions.Add(rowDefinition4);
            KeyBoardField.RowDefinitions.Add(rowDefinition5);
            Grid KBFGridFirst = new Grid();
            Grid KBFGridSecond = new Grid();
            Grid KBFGridThird = new Grid();
            Grid KBFGridFouth = new Grid();
            Grid KBFGridFifth = new Grid();
            KeyBoardField.Children.Add(KBFGridFirst);
            Grid.SetRow(KBFGridFirst, 0);
            KeyBoardField.Children.Add(KBFGridSecond);
            Grid.SetRow(KBFGridSecond, 1);
            KeyBoardField.Children.Add(KBFGridThird);
            Grid.SetRow(KBFGridThird, 2);
            KeyBoardField.Children.Add(KBFGridFouth);
            Grid.SetRow(KBFGridFouth, 3);
            KeyBoardField.Children.Add(KBFGridFifth);
            Grid.SetRow(KBFGridFifth, 4);
            KBFGridFirstColumns = new List<ColumnDefinition>();
            KBFieldButtons = new List<Button>();
            //1ая строка
            for (KBBCount = 0; KBBCount < 14; KBBCount++)
            {
                KBFGridFirstColumns.Add(new ColumnDefinition());
                KBFGridFirst.ColumnDefinitions.Add(KBFGridFirstColumns[KBBCount]);
                KBFieldButtons.Add(new Button());
                KBFGridFirst.Children.Add(KBFieldButtons[KBBCount]);
                Grid.SetColumn(KBFieldButtons[KBBCount], KBBCount);
            }
            KBFGridFirstColumns[13].Width = new GridLength(2, GridUnitType.Star);
            //2ая строка
            List<ColumnDefinition> KBFGridSecondColumns = new List<ColumnDefinition>();
            for (int i = 0; i < 14; i++, KBBCount++)
            {
                KBFGridSecondColumns.Add(new ColumnDefinition());
                KBFGridSecond.ColumnDefinitions.Add(KBFGridSecondColumns[i]);
                KBFieldButtons.Add(new Button());
                KBFGridSecond.Children.Add(KBFieldButtons[KBBCount]);
                Grid.SetColumn(KBFieldButtons[KBBCount], i);
            }
            KBFGridSecondColumns[0].Width = new GridLength(1.5, GridUnitType.Star);
            KBFGridSecondColumns[13].Width = new GridLength(1.5, GridUnitType.Star);
            //3ая строка
            List<ColumnDefinition> KBFGridThirdColumns = new List<ColumnDefinition>();
            for (int i = 0; i < 13; i++, KBBCount++)
            {
                KBFGridThirdColumns.Add(new ColumnDefinition());
                KBFGridThird.ColumnDefinitions.Add(KBFGridThirdColumns[i]);
                KBFieldButtons.Add(new Button());
                KBFGridThird.Children.Add(KBFieldButtons[KBBCount]);
                Grid.SetColumn(KBFieldButtons[KBBCount], i);
            }
            KBFGridThirdColumns[0].Width = new GridLength(2, GridUnitType.Star);
            KBFGridThirdColumns[12].Width = new GridLength(2, GridUnitType.Star);
            //4ая строка
            List<ColumnDefinition> KBFGridFouthColumns = new List<ColumnDefinition>();
            for (int i = 0; i < 12; i++, KBBCount++)
            {
                KBFGridFouthColumns.Add(new ColumnDefinition());
                KBFGridFouth.ColumnDefinitions.Add(KBFGridFouthColumns[i]);
                KBFieldButtons.Add(new Button());
                KBFGridFouth.Children.Add(KBFieldButtons[KBBCount]);
                Grid.SetColumn(KBFieldButtons[KBBCount], i);
            }
            KBFGridFouthColumns[0].Width = new GridLength(2.5, GridUnitType.Star);
            KBFGridFouthColumns[11].Width = new GridLength(2.5, GridUnitType.Star);
            //5ая строка
            List<ColumnDefinition> KBFGridFifthColumns = new List<ColumnDefinition>();
            for (int i = 0; i < 7; i++, KBBCount++)
            {
                KBFGridFifthColumns.Add(new ColumnDefinition());
                KBFGridFifth.ColumnDefinitions.Add(KBFGridFifthColumns[i]);
                KBFieldButtons.Add(new Button());
                KBFGridFifth.Children.Add(KBFieldButtons[KBBCount]);
                Grid.SetColumn(KBFieldButtons[KBBCount], i);
            }
            KBFGridFifthColumns[3].Width = new GridLength(4, GridUnitType.Star);

            for (int i = 0; i < KBBCount; i++)
            {
                KBFieldButtons[i].Style = (Style)this.TryFindResource("RoundButtomBorder");
                KBFieldButtons[i].IsEnabled = false;
                KBFieldButtons[i].FontSize = 20;
                KBFieldButtons[i].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(KBButtonsColor[i]));
            }
            KBFieldButtons[0].Content = "~";
            ChangeCase();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            diffculty = Convert.ToInt32(e.NewValue);
            DifficultyTextBlock.Text = diffculty.ToString();
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            fails = 0;
            StartBtn.IsEnabled = false;
            DifficultySlider.IsEnabled = false;
            InputTextBox.IsEnabled = true;
            InputTextBox.Text = "";
            inputText = "";
            timeStart = DateTime.Now;
            timer.Start();
            TextGenerator(diffculty);
            TextToInputTextBlock.Text = textToInput;
            this.InputTextBox.Focus();

        }
        private void timerTick(object sender, EventArgs e)
        {
            if(inputText.Length!=0)
            speed = ((inputText.Length/(Convert.ToDouble((DateTime.Now - timeStart).TotalSeconds))*60));
            SpeedTextBlock.Text = speed.ToString("#.##");
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            StartBtn.IsEnabled = true;
            DifficultySlider.IsEnabled = true;
            if(InputTextBox.IsEnabled == true) InputTextBox.IsEnabled = false;
            if (timer.IsEnabled!=false)timer.Stop();
        }

        private void TextGenerator(int diffculty)
        {
            textToInput = "";
            Random random = new Random();
            for(int i = 0; i < diffculty*20; i++)
            {
                if(random.Next(20)>1)//рeгулировка частоты появления символов !/} и т д
                {
                    if (random.Next(10) >= 1)   //регулировка появления пробелов
                    {
                        if (caseSensetive == true)
                        {
                            if (random.Next(6) >= 3)   //Регулировка частоты Верхнего и Нижнего регистра.
                                textToInput += (char)random.Next(65, 90);
                            else
                                textToInput += (char)random.Next(97, 122);
                        }
                        else textToInput += (char)random.Next(97, 122);
                    }
                    else textToInput += " ";
                }
                else
                {
                    switch(random.Next(5))
                    {
                        case 0: textToInput += (char)random.Next(31, 47);break;
                        case 1: textToInput += (char)random.Next(48, 57);break;
                        case 2: textToInput += (char)random.Next(58, 64);break;
                        case 3: textToInput += (char)random.Next(91, 96);break;
                        case 4: textToInput += (char)random.Next(123, 126);break;
                            default:MessageBox.Show("!!!!"); break;
                    }
                }

            }
        }

        private void CaseSensetiveCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (CaseSensetiveCheckBox.IsChecked == true)
            {caseSensetive = true;}
            else
            {caseSensetive = false;}
        }

        private void InputTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            inputText = InputTextBox.Text;
            if (inputText.Length >= textToInput.Length) { StopBtn_Click(sender, e); return; }
            if (inputText.Length != 0)
                if (inputText[inputText.Length - 1] != textToInput[inputText.Length - 1]) fails++;
            if (inputText.Length + 1 >= textToInput.Length) { StopBtn_Click(sender, e); return; }
            FailsTextBlock.Text = fails.ToString();
        }

        private void ChangeCase()
        {
            if (UpperCaseOn == false)
            for (int i = 0; i < KBBCount; i++)
            {KBFieldButtons[i].Content = KBButtonsContentLowCase[i];}
            else
                for (int i = 0; i < KBBCount; i++)
                { KBFieldButtons[i].Content = KBButtonsContentUpperCase[i]; }
            UpperCaseOn = !UpperCaseOn;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift && KBFieldButtons[0].Content == "`") { ChangeCase(); }      
            for (int i = 0; i < KBButtonsKeyCode.Count; i++)
            {
                if (Convert.ToInt32(e.Key) == KBButtonsKeyCode[i])
                {
                    keyDown = i;
                    KBFieldButtons[keyDown].Background = new SolidColorBrush(Colors.White);
                }
            }
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.IsUp && e.Key == Key.LeftShift) ChangeCase();
            KBFieldButtons[keyDown].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(KBButtonsColor[keyDown]));
            for (int i = 0; i < KBBCount; i++)  //Устраняет залипание Shift+AnyKey,когда 2 нажатых клавишы не дают поменять цвет обратно.
            {KBFieldButtons[i].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(KBButtonsColor[i]));}
        }

    }
}
