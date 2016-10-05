using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SafeWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int N;
        bool[,] field;
        Button[,] levers;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void size_PreviewTextInput(object sender, TextCompositionEventArgs e)//Разрешает ввод только цифр
        {
            int result;

            if (!int.TryParse(e.Text, out result))
            {
                e.Handled = true;
            }
        }
        private void startButton_Click(object sender, RoutedEventArgs e)//Проверка N и старт игры
        {
            if (size.Text != "")
            {
                N = Convert.ToInt16(size.Text);
                if (N <= 20 && N >= 2)
                {
                    startGame();
                }
                else
                {
                    MessageBox.Show("Размер должен быть от одного до двадцати");
                }
            }
            else
            {
                MessageBox.Show("Введите размер массива");
            }
        }

        private void startGame()
        {
            if (levers != null)
            {
                var result = MessageBox.Show("Are You sure want to start a new game?", "Are You shure?", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
            clearField();
            levers = new Button[N, N];
            field = new bool[N, N]; //true = vertical; false = horizontal
            Random rand = new Random();

            int randRotate = rand.Next(20, 100); //Изначально массив заполняется только true, затем перемешивается от 20 до 100 раз.

            gameField.Width = (30 * N) + (N * 2); //Размер поля регулируется от заданного N. 
            gameField.Height = (30 * N) + (N * 2);//Это нужно, чтобы все кнопки оставались на своих местах


            for (int i = 0; i < N; i++)//Заполнение поля
            {
                for (int j = 0; j < N; j++)
                {
                    levers[i, j] = new Button();
                    field[i, j] = true;
                    levers[i, j].Content = "|";
                    levers[i, j].Margin = new Thickness(1);
                    levers[i, j].Width = 30;
                    levers[i, j].Height = 30;
                    levers[i, j].Name = "lever" + i +""+ j;
                    levers[i, j].Click += new RoutedEventHandler(MyButtonHandler);
                    gameField.Children.Add(levers[i, j]);
                }
            }

            //Авторегулировка размера формы для красоты:) 
            this.Width = 265 + gameField.Width;
            this.Height = 75 + gameField.Height;

            //Исключить ситуацию, при которой после перемешки получится сразу сложенная головоломка 
            while (win() != false)
            {
                for (int i = 0; i <= randRotate; i++)
                {
                    changePosition(rand.Next(0, N), rand.Next(0, N));
                }
            }
            updateField();
        }

        private void clearField()//Очистить поле 
        {
            if (levers != null)
            {
                foreach (Button b in levers)
                    gameField.Children.Remove(b);
            }
            levers = null;
        }
        private void updateField()//Обновление значений на кнопках-вентилях
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (field[i, j] == true)
                        levers[i, j].Content = "|";
                    else
                        levers[i, j].Content = "--";
                }
            }
        }
        private bool win() // Победа, если все элементы одинаковы
        {
            bool startPos = field[0, 0];
            bool currPos;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    currPos = field[i, j];
                    if (currPos != startPos)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private void changePosition(int iBtnPos, int jBtnPos)//Смена положения вентилей
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (iBtnPos == i)
                    {
                        if (field[i, j] == true)
                        {
                            field[i, j] = false;
                        }
                        else field[i, j] = true;
                    }
                    else if (jBtnPos == j)
                    {
                        if (field[i, j] == true)
                        {
                            field[i, j] = false;
                        }
                        else field[i, j] = true;
                    }
                }
            }
        }

        private void MyButtonHandler(object sender, RoutedEventArgs e)//Обработка нажатия на вентиль
        {
            Button btn = (Button)sender;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (btn.Name == ("lever" + i + "" + j))
                    {
                        changePosition(i, j);
                        updateField();
                        if (win())
                        {
                            MessageBox.Show("Congratulations! You Won! Play again?");
                            clearField();
                        }
                    }
                }
            }
        }
    }
}
