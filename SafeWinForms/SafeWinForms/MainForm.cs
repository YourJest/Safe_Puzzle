using System;
using System.Drawing;
using System.Windows.Forms;

namespace SafeWinForms
{
    public partial class MainForm : Form
    {
        public int N;
        bool[,] field;
        Button[,] levers;
        public MainForm()
        {
            InitializeComponent();
        }
        private void Size_KeyPress(object sender, KeyPressEventArgs e)//Разрешает ввод только цифр
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }
        private void startButton_Click(object sender, EventArgs e)//Проверка N и старт игры
        {
            if (size.Text != "")
            {
                N = Convert.ToInt16(size.Text);
                if (N <= 20 && N >= 2) { 
                    startGame();
                }
                else
                {
                    MessageBox.Show("Size must be from 2 to 20");
                }
            }
            else
            {
                MessageBox.Show("Enter size of field");
            }           
        }

        private void startGame()
        {
            if (levers != null)
            {
                var result = MessageBox.Show("Are You sure want to start a new game?", "Are You shure?", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return;
                }
            }
            clearField();  
            levers = new Button[N, N];
            field = new bool[N, N]; //true = vertical; false = horizontal
            Random rand = new Random();
            int offset_x = 200, offset_y = 25; //Смещение кнопок
            int randRotate = rand.Next(20, 100); //Изначально массив заполняется только true, затем перемешивается от 20 до 100 раз.
            for (int i = 0; i < N; i++)
            {

                for (int j = 0; j < N; j++)
                {
                    levers[i, j] = new Button();
                    field[i, j] = true;
                    levers[i, j].Text = "|";
                    levers[i, j].Location = new Point(offset_x, offset_y);
                    levers[i, j].Size = new Size(30, 30);
                    levers[i, j].Name = "Lever " + i + "," + j;
                    levers[i, j].Click += new EventHandler(this.MyButtonHandler);
                    this.Controls.Add(levers[i, j]);
                    offset_x += 30;
                }
                offset_x = 200;
                offset_y += 30;
            }

            //Авторегулировка размера формы для красоты:) 
            this.Size = new Size(50 + levers[N - 1, N - 1].Location.X, 75 + levers[N - 1, N - 1].Location.Y);

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

        private void changePosition(int iBtnPos, int jBtnPos)
        {
            for(int i=0; i < N;i++)
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
                    }else if (jBtnPos == j)
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
        private void clearField()
        {
            if (levers != null)
            {
                foreach (Button b in levers)
                    this.Controls.Remove(b);
            }
            levers = null;
        }
        private void updateField()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (field[i, j] == true)
                        levers[i, j].Text = "|";
                    else
                        levers[i, j].Text = "--";
                }
            }
        }
        private bool win()
        {
            bool startPos = field[0,0];
            bool currPos;
            for (int i=0; i < N; i++)
            {
                for (int j=0; j < N; j++)
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

        private void MyButtonHandler(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            for (int i=0; i< N; i++)
            {
                for(int j=0; j< N;j++)
                {
                    if(btn.Name == ("Lever " + i + "," + j))
                    {
                        changePosition( i , j);
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
