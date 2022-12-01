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
using System.IO;
using System.Windows.Automation.Provider;
using System.Globalization;
using System.Printing;

namespace KeyboardTrainer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int fails = 0;  //Счетчик ошибок ввода
        double speed = 0;   //Счетчик скорости ввода
        int diffculty = 1;  //По умолчанию установка сложности
        String textToInput = "";    //Поле вывода текста
        String inputText = "";  //Поле ввода текста
        DispatcherTimer timer;
        DateTime timeStart;
        bool caseSensetive = false; //ЧекБокс Верхнего регистра
        bool KBIntialisation = false; //Состояние чтение файла языковой настройки
        int KBBCount;   //Общее количество кнопок
        bool CapsLockCaseOn = false;   //Состояние Верхнего регистра (Берется состояние CapsLock)
        bool ShiftCaseOn = false;   //Состояние Верхнего регистра (Берется состояние Shift)
        string InputLangSet = "en-US";  //Дефолтный язык кнопок программы
        List<Button> KBFieldButtons = new List<Button>();    //Лист всех кнопок для дальнейшего обращения и изменения их состояния.
        List<string> KBButtonsContentNormalCase = new List<string> { };    //Лист нижнего регистра кнопок
        List<string> KBButtonsContentShiftCase = new List<string> { };  //Лист кнопок при зажатом shift
        List<string> KBButtonsContentCapsLock = new List<string> { };  //Лист кнопок при зажатом CapsLock
        List<int> KBButtonsKeyCode = new List<int> { }; //Лист кодов каждой клавиши
        List<string> KBButtonsColor = new List<string> { }; //Лист цветов клавиш
        List<ColumnDefinition> KBFGridFirstColumns = new List<ColumnDefinition>();
        List<ColumnDefinition> KBFGridSecondColumns = new List<ColumnDefinition>();
        List<ColumnDefinition> KBFGridThirdColumns = new List<ColumnDefinition>();
        List<ColumnDefinition> KBFGridFouthColumns = new List<ColumnDefinition>();
        List<ColumnDefinition> KBFGridFifthColumns = new List<ColumnDefinition>();

        public MainWindow()
        {
            InitializeComponent();
            DifficultyTextBlock.Text = diffculty.ToString();
            FailsTextBlock.Text = "0";
            SpeedTextBlock.Text = "0";
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 1);
            InputLangSet = InputLanguageManager.Current.CurrentInputLanguage.ToString();
            RBLangStatusChange();
            KBIntialisation = ButtomInfoReader();
            if (KBIntialisation == true)
                ButtomFieldGenerator();
            else this.Close();
            if (Keyboard.GetKeyStates(Key.CapsLock).ToString() == "Toggled")
                CapsLockCaseOn = true;
            else CapsLockCaseOn = false;
            ChangeCase();
            MessageBox.Show(Convert.ToInt32("я").ToString());
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            diffculty = Convert.ToInt32(e.NewValue);
            DifficultyTextBlock.Text = diffculty.ToString();
        }
        private void timerTick(object sender, EventArgs e)
        {
            if (inputText.Length != 0)
                speed = ((inputText.Length / (Convert.ToDouble((DateTime.Now - timeStart).TotalSeconds)) * 60));
            SpeedTextBlock.Text = speed.ToString("#.##");
        }
        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            speed = 0;SpeedTextBlock.Text = "0";
            fails = 0;FailsTextBlock.Text = "0";
            StartBtn.IsEnabled = false;
            DifficultySlider.IsEnabled = false;
            InputTextBox.IsEnabled = true;
            CaseSensetiveCheckBox.IsEnabled = false;
            InputTextBox.Text = "";
            inputText = "";
            timeStart = DateTime.Now;
            timer.Start();
            TextGenerator(diffculty);
            TextToInputTextBlock.Text = textToInput;
            this.InputTextBox.Focus();
        }
        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            StartBtn.IsEnabled = true;
            DifficultySlider.IsEnabled = true;
            if(InputTextBox.IsEnabled == true) InputTextBox.IsEnabled = false;
            if (timer.IsEnabled!=false)timer.Stop();
            CaseSensetiveCheckBox.IsEnabled = true;
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
        //Так как мы еще не работали с потоками, пришлось применить костыль и разделить подсветку кнопок и действия shift и CapsLock таким вот образом.
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.IsKeyDown(Key.CapsLock))
            {
                if (e.KeyboardDevice.IsKeyToggled(Key.CapsLock))
                { CapsLockCaseOn = true; ChangeCase(); return; }
                else
                { CapsLockCaseOn = false; ChangeCase(); return; }
            }
            if(e.KeyboardDevice.Modifiers==ModifierKeys.Shift && ShiftCaseOn ==false)
            {
                ShiftCaseOn = true; ChangeCase(); return; 
            }
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyboardDevice.IsKeyUp(Key.LeftShift) && ShiftCaseOn == true)
            {
                ShiftCaseOn = false; ChangeCase(); return;
            }
        }
        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            for (int i = 0; i < KBButtonsKeyCode.Count; i++)
            {
                if (Convert.ToInt32(e.Key) == KBButtonsKeyCode[i])
                {KBFieldButtons[i].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(KBButtonsColor[i]));}
            }
            if(InputLanguageManager.Current.CurrentInputLanguage.ToString() != InputLangSet)
            {
                if(StartBtn.IsEnabled == true)
                {
                    InputLangSet = InputLanguageManager.Current.CurrentInputLanguage.ToString();
                    ButtomInfoReader();
                    RBLangStatusChange();
                    ChangeCase(); return;
                }
                else 
                {
                    InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo(InputLangSet);
                    MessageBox.Show("Нельзя менять язык пока запущенна программа!");
                }
            }
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            for (int i = 0; i < KBButtonsKeyCode.Count; i++)
            {
                if (Convert.ToInt32(e.Key) == KBButtonsKeyCode[i])
                { KBFieldButtons[i].Background = new SolidColorBrush(Colors.DarkSlateGray); }
            }
        }

        private void ChangeCase()
        {
            if(ShiftCaseOn == true)
                for (int i = 0; i < KBBCount; i++)
                { KBFieldButtons[i].Content = KBButtonsContentShiftCase[i]; }
            else if(CapsLockCaseOn == true)
                for (int i = 0; i < KBBCount; i++)
                { KBFieldButtons[i].Content = KBButtonsContentCapsLock[i]; }
            else
                for (int i = 0; i < KBBCount; i++)
                { KBFieldButtons[i].Content = KBButtonsContentNormalCase[i]; }
        }
        private void TextGenerator(int diffculty)
        {
            textToInput = "";
            Random random = new Random();
            for (int i = 0; i < diffculty * 20; i++)
            {
                if (random.Next(20) > 1)//рeгулировка частоты появления символов !/} и т д
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
                    switch (random.Next(5))
                    {
                        case 0: textToInput += (char)random.Next(31, 47); break;
                        case 1: textToInput += (char)random.Next(48, 57); break;
                        case 2: textToInput += (char)random.Next(58, 64); break;
                        case 3: textToInput += (char)random.Next(91, 96); break;
                        case 4: textToInput += (char)random.Next(123, 126); break;
                        default: MessageBox.Show("!!!!"); break;
                    }
                }
            }
        }
        private bool ButtomInfoReader()
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string path = "";
            if (InputLangSet == "en-US") path = System.IO.Path.Combine(exePath, "BTNInfo\\EnLang.txt");
            else path = System.IO.Path.Combine(exePath, "BTNInfo\\RuLang.txt");
            if (path == "") { MessageBox.Show("Не удается найти файл настроек кнопок!"); return false; }
            StreamReader streamsreader = new StreamReader(path);
            String line;
            try
            {
                ClearAllButtonList();
                line = streamsreader.ReadLine();
                line = streamsreader.ReadLine();
                while (line != "KBButtonsContentShiftCase")
                {
                    KBButtonsContentNormalCase.Add(line);
                    line = streamsreader.ReadLine();
                }
                line = streamsreader.ReadLine();
                while (line != "KBButtonsContentCapsLockCase")
                {
                    KBButtonsContentShiftCase.Add(line);
                    line = streamsreader.ReadLine();
                }
                line = streamsreader.ReadLine();
                while (line != "KBButtonsKeyCode")
                {
                    KBButtonsContentCapsLock.Add(line);
                    line = streamsreader.ReadLine();
                }
                line = streamsreader.ReadLine();
                while (line != "KBButtonsColor")
                {
                    KBButtonsKeyCode.Add(Int32.Parse(line));
                    line = streamsreader.ReadLine();
                }
                line = streamsreader.ReadLine();
                while (line != "End")
                {
                    KBButtonsColor.Add(line);
                    line = streamsreader.ReadLine();
                }
                streamsreader.Close();
                if (KBButtonsContentNormalCase.Count != 60 || KBButtonsContentShiftCase.Count != 60 || KBButtonsContentCapsLock.Count != 60 || KBButtonsKeyCode.Count != 60 || KBButtonsColor.Count != 60)
                {
                    MessageBox.Show("Ошибка в строении файла языковой настройки! Должно быть 60 60 60 60 60! " +
                        "Ваше количество: " + KBButtonsContentNormalCase.Count.ToString() + " " + KBButtonsContentShiftCase.Count.ToString() + " " + KBButtonsContentCapsLock.Count.ToString() + KBButtonsKeyCode.Count.ToString() + " " + KBButtonsColor.Count.ToString());
                    ClearAllButtonList();
                    return false;
                }
                return true;
            }
            catch(Exception ex)
            {
                streamsreader.Close();
                MessageBox.Show("Ошибка строения файла: " + ex);
                KBButtonsContentNormalCase.Clear();
                KBButtonsContentShiftCase.Clear();
                KBButtonsKeyCode.Clear();
                KBButtonsColor.Clear();
                return false;
            }
        }
        private void ButtomFieldGenerator()
        {
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
        }
        private void ClearAllButtonList()
        {
            KBButtonsContentNormalCase.Clear();
            KBButtonsContentShiftCase.Clear();
            KBButtonsContentCapsLock.Clear();
            KBButtonsKeyCode.Clear();
            KBButtonsColor.Clear();
        }
        private void RBLangStatusChange()
        {
            if (InputLangSet=="en-US")
            {
                EngRB.IsEnabled = true;
                EngRB.IsChecked = true;
                EngRB.IsEnabled = false;
            }
            else
            {
                RusRB.IsEnabled = true;
                RusRB.IsChecked = true;
                RusRB.IsEnabled = false;
            }
        }
    }
}
