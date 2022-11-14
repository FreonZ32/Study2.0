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
        int speed = 0;
        int diffculty = 1;
        String textToInput = "";
        String inputText = "";
        private DispatcherTimer timer = null;
        int timeStart;
        bool caseSensetive = false;

        public MainWindow()
        {
            InitializeComponent();
            DifficultyTextBlock.Text = "1";
            FailsTextBlock.Text = "0";
            SpeedTextBlock.Text = "0";
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 1);
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
            timeStart = DateTime.Now.Second;
            timer.Start();
            TextGenerator(diffculty);
            TextToInputTextBlock.Text = textToInput;
            this.InputTextBox.Focus();

        }
        private void timerTick(object sender, EventArgs e)
        {
            TimerDbg.Text = (DateTime.Now.Second - timeStart).ToString();
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            StartBtn.IsEnabled = true;
            DifficultySlider.IsEnabled = true;
            InputTextBox.IsEnabled = false;
            timer.Stop();
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
            if (inputText[inputText.Length-1]!=textToInput[inputText.Length-1])fails++; ///ОШИБКА!!!
            //for (int i = 0; i < inputText.Length; i++)
            //{
            //    if (inputText[i] != textToInput[i]) fails++;
            //}
            FailsTextBlock.Text = fails.ToString(); //Слишком медленно!!!!
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (inputText.Length + 1 > textToInput.Length) { StopBtn_Click(sender, e); return; }
        }
    }
}
