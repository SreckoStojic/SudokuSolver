using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.ComponentModel;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public static Grid grid;
        public static bool stop = true;
        public static int[,] saveBoard;
        public static int ms = 100;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            grid = Grid;
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    TextBox tb = new TextBox();
                    if ((i < 4 && j < 4) || (i > 6 && j > 6) || (i > 6 && j > 0 && j < 4) || (i > 0 && i < 4 && j > 6) ||
                        (i > 3) && (i < 7) && (j > 3) && (j < 7))
                        tb.Background = new SolidColorBrush(Colors.GreenYellow);
                    else
                    {
                        tb.Background = new SolidColorBrush(Colors.White);
                    }
                    /*    Binding bb = new Binding("Test1");
                        bb.Source = textBox1;
                        bb.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                        bb.Mode = BindingMode.TwoWay;
                        tb.SetBinding(TextBox.TextProperty, bb);*/

                    tb.Height = 35;
                    tb.Width = 35;
                    tb.TextAlignment = TextAlignment.Center;
                    tb.TextChanged += textCheck;
                    tb.Name = "textBox" + i.ToString() + j.ToString();
                    Grid.RowDefinitions.Add(new RowDefinition());
                    Grid.RowDefinitions[j - 1].Height = new GridLength(35);
                    Grid.ColumnDefinitions.Add(new ColumnDefinition());
                    Grid.ColumnDefinitions[i - 1].Width = new GridLength(35);
                    Grid.SetRow(tb, i - 1);
                    Grid.SetColumn(tb, j - 1);
                    Grid.Children.Add(tb);
                }
            }
        }

        void textCheck(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text.Length > 1)
            {
                tb.Text = tb.Text.Substring(0, 1);
            }
            if (tb.Text == "0" || tb.Text == "")
            {
                //  tb.Background = Brushes.White;
                string s = tb.Name.Substring(7);
                string j1 = tb.Name.Substring(8);
                int j = Int32.Parse(j1);
                int i = Int32.Parse(s.Substring(0, 1));
                if ((i < 4 && j < 4) || (i > 6 && j > 6) || (i > 6 && j > 0 && j < 4) || (i > 0 && i < 4 && j > 6) ||
                        (i > 3) && (i < 7) && (j > 3) && (j < 7))
                    tb.Background = new SolidColorBrush(Colors.GreenYellow);
                else
                {
                    tb.Background = new SolidColorBrush(Colors.White);
                }
            }
        }

        private void Button_Click_DFS(object sender, RoutedEventArgs e)
        {

            int[,] board = MainWindow.initialization();
            if (MainWindow.checkTableRow(board) != null)
            {
                int[] i = MainWindow.checkTableRow(board);
                foreach (TextBox tb in grid.Children)
                {
                    if (tb.Name == "textBox" + i[0].ToString() + i[1].ToString())
                    {
                        tb.Background = Brushes.PaleVioletRed;
                        return;
                    }
                }
            }
            if (MainWindow.checkTableColomn(board) != null)
            {
                int[] l = MainWindow.checkTableColomn(board);
                foreach (TextBox tb in grid.Children)
                {
                    if (tb.Name == "textBox" + l[0].ToString() + l[1].ToString())
                    {
                        tb.Background = Brushes.PaleVioletRed;
                        return;
                    }
                }
            }
            if (MainWindow.checkTableSquere(board) != null)
            {
                int[] l = MainWindow.checkTableSquere(board);
                foreach (TextBox tb in grid.Children)
                {
                    if (tb.Name == "textBox" + l[0].ToString() + l[1].ToString())
                    {
                        tb.Background = Brushes.PaleVioletRed;
                        return;
                    }
                }
            }

            // Pocetak algoritma
            DFS_Sudoku dfs = new DFS_Sudoku();
            List<Node> steps_and_solution = dfs.solve_BFS(board);   // u matricu resenje smestamo rezultat
            /*  int[,] resenje = steps_and_solution[steps_and_solution.Count - 1].getState();
              int r = 0;
              int c = 0;

              foreach (TextBox tb in grid.Children)
              {
                  if (c == 9)
                  {
                      c = 0;
                      r++;
                      tb.Text = resenje[r, c].ToString();
                      c++;
                  }
                  else
                  {
                      tb.Text = resenje[r, c].ToString();
                      c++;
                  }
              }*/
            if (stop)
            {
                System.Threading.Thread thread = new Thread(new ParameterizedThreadStart(ThreadMethod));
                thread.Name = "UpdateThread";
                thread.Start(steps_and_solution);
            }
        return;

        }
        private void Button_Click_BFS(object sender, RoutedEventArgs e)
        {
            int[,] board = MainWindow.initialization();
            if (MainWindow.checkTableRow(board) != null)
            {
                int[] i = MainWindow.checkTableRow(board);
                foreach (TextBox tb in grid.Children)
                {
                    if (tb.Name == "textBox" + i[0].ToString() + i[1].ToString())
                    {
                        tb.Background = Brushes.PaleVioletRed;
                        return;
                    }
                }
            }
            if (MainWindow.checkTableColomn(board) != null)
            {
                int[] l = MainWindow.checkTableColomn(board);
                foreach (TextBox tb in grid.Children)
                {
                    if (tb.Name == "textBox" + l[0].ToString() + l[1].ToString())
                    {
                        tb.Background = Brushes.PaleVioletRed;
                        return;
                    }
                }
            }
            if (MainWindow.checkTableSquere(board) != null)
            {
                int[] l = MainWindow.checkTableSquere(board);
                foreach (TextBox tb in grid.Children)
                {
                    if (tb.Name == "textBox" + l[0].ToString() + l[1].ToString())
                    {
                        tb.Background = Brushes.PaleVioletRed;
                        return;
                    }
                }
            }
            // Pocetak algoritma
            BFS_Sudoku bfs = new BFS_Sudoku();
            List<Node> steps_and_solution = bfs.solve_BFS(board);   // u matricu resenje smestamo rezultat
            /*   int[,] resenje = steps_and_solution[steps_and_solution.Count - 1].getState();
               int r = 0;
               int c = 0;

               foreach (TextBox tb in grid.Children)
               {
                   if (c == 9)
                   {
                       c = 0;
                       r++;
                       tb.Text = resenje[r, c].ToString();
                       c++;
                   }
                   else
                   {
                       tb.Text = resenje[r, c].ToString();
                       c++;
                   }
               }*/
            if (stop)
            {
                System.Threading.Thread thread = new Thread(new ParameterizedThreadStart(ThreadMethod));
                thread.Name = "UpdateThread";
                thread.Start(steps_and_solution);
            }
            return;

        }

        private void Button_Click_A(object sender, RoutedEventArgs e)
        {
            int[,] board = MainWindow.initialization();
            if (MainWindow.checkTableRow(board) != null)
            {
                int[] i = MainWindow.checkTableRow(board);
                foreach (TextBox tb in grid.Children)
                {
                    if (tb.Name == "textBox" + i[0].ToString() + i[1].ToString())
                    {
                        tb.Background = Brushes.PaleVioletRed;
                        return;
                    }
                }
            }
            if (MainWindow.checkTableColomn(board) != null)
            {
                int[] l = MainWindow.checkTableColomn(board);
                foreach (TextBox tb in grid.Children)
                {
                    if (tb.Name == "textBox" + l[0].ToString() + l[1].ToString())
                    {
                        tb.Background = Brushes.PaleVioletRed;
                        return;
                    }
                }
            }
            if (MainWindow.checkTableSquere(board) != null)
            {
                int[] l = MainWindow.checkTableSquere(board);
                foreach (TextBox tb in grid.Children)
                {
                    if (tb.Name == "textBox" + l[0].ToString() + l[1].ToString())
                    {
                        tb.Background = Brushes.PaleVioletRed;
                        return;
                    }
                }
            }
            A_Star_Sudoku astar = new A_Star_Sudoku();
            List<Node> steps_and_solution = astar.solve_A_Star(board);
            if (stop)
            {
                System.Threading.Thread thread = new Thread(new ParameterizedThreadStart(ThreadMethod));
                thread.Name = "UpdateThread";
                thread.Start(steps_and_solution);
               
            }
         
            return;


        }
        public static int[,] initialization()
        {
            List<List<int>> board;
            int[,] tabla;
            List<int> val;

            board = new List<List<int>>();
            int k = 0;
            val = new List<int>();
            foreach (TextBox tb in grid.Children)
            {
                if (k == 9)
                {
                    k = 1;
                    board.Add(val);
                    val = new List<int>();

                    if (tb.Text == "" || tb.Text == "0")
                        val.Add(0);
                    else
                    {
                        val.Add(Int32.Parse(tb.Text));
                     
                    }    
                }
                else
                {
                    if (tb.Text == "" || tb.Text == "0")
                        val.Add(0);
                    else
                    {
                        val.Add(Int32.Parse(tb.Text));          
                    }

                    k++;
                }
            }
            board.Add(val);

            tabla = new int[9, 9];
            for (int i = 0; i < board.Count; i++)
            {
                List<int> l = board.ElementAt(i);
                for (int j = 0; j < l.Count; j++)
                {
                    tabla[i, j] = l.ElementAt(j);
                }
            }
            return tabla;
        }

        public static int[] checkTableRow(int[,] table)
        {
            List<int> lista = new List<int>();
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    if (table[i, j] != 0)
                    {
                        if (!lista.Contains(table[i, j]))
                        {
                            lista.Add(table[i, j]);
                        }
                        else
                        {
                            int[] g = new int[2];
                            g[0] = i + 1;
                            g[1] = j + 1;
                            return g;
                        }
                    }
                }
                lista = new List<int>();
            }
            return null;
        }
        public static int[] checkTableColomn(int[,] table)
        {
            List<int> lista = new List<int>();
            for (int j = 0; j < table.GetLength(0); j++)
            {
                for (int i = 0; i < table.GetLength(1); i++)
                {
                    if (table[i, j] != 0)
                    {
                        if (!lista.Contains(table[i, j]))
                        {
                            lista.Add(table[i, j]);
                        }
                        else
                        {
                            int[] g = new int[2];
                            g[0] = i + 1;
                            g[1] = j + 1;
                            return g;
                        }
                    }
                }
                lista = new List<int>();
            }
            return null;
        }

        public static int[] checkTableSquere(int[,] table)
        {
            List<int> lista = new List<int>();
            for (int s = 1; s <= 3; s++)
            {
                for (int i = 1; i <= 3; i++)
                {
                    for (int k = i * 3 - 3; k < i * 3; k++)
                    {
                        for (int l = s * 3 - 3; l < s * 3; l++)
                        {
                            if (table[k, l] != 0)
                            {
                                if (!lista.Contains(table[k, l]))
                                {
                                    lista.Add(table[k, l]);
                                }
                                else
                                {
                                    int[] g = new int[2];
                                    g[0] = k + 1;
                                    g[1] = l + 1;
                                    return g;
                                }
                            }
                        }

                    }
                    lista = new List<int>();
                }
            }
            return null;
        }


        private void button1_Click_empty(object sender, RoutedEventArgs e)
        {

            foreach (TextBox tb in grid.Children)
            {
                tb.Text = "";
            }
            textBoxBroj.Text = "";
            textBoxTrenutno.Text = "";
        }

        public static void print(int[,] board)
        {
            int r = 0;
            int c = 0;
            foreach (TextBox tb in MainWindow.grid.Children)
            {
                if (c == 9)
                {
                    c = 0;
                    r++;
                    tb.Text = board[r, c].ToString();
                    c++;
                }
                else
                {
                    tb.Text = board[r, c].ToString();
                    c++;
                }
            }

        }

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void ThreadMethod(object parameter)
        {
            List<Node> stepDebug = (List<Node>)parameter;
            int i = 0;
            stop = true;
            while (stop)
            {
                this.Dispatcher.Invoke(new Action(delegate()
                {
                        textBoxBroj.Text = stepDebug.Count.ToString();
                    int[,] resenje = stepDebug.ElementAt(i).getState();
                    int r = 0;
                    int c = 0;
                    foreach (TextBox tb in grid.Children)
                    {
                        if (c == 9)
                        {
                            c = 0;
                            r++;
                            if (resenje[r, c] == 0)
                            {
                                tb.Text = "";
                            }
                            else
                                tb.Text = resenje[r, c].ToString();
                            c++;
                        }
                        else
                        {
                            if (resenje[r, c] == 0)
                            {
                                tb.Text = "";
                            }
                            else
                                tb.Text = resenje[r, c].ToString();
                            c++;
                        }
                    }
                    i++;
                    textBoxTrenutno.Text = i.ToString();
                }));
                System.Threading.Thread.Sleep(ms);
                if (i == stepDebug.Count)
                {
                 
                    return; 
                }
               
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            saveBoard = new int[9, 9];
            int r = 0;
            int c = 0;
            foreach (TextBox tb in grid.Children)
            {
                if (c == 9)
                {
                    c = 0;
                    r++;
                    if (tb.Text == "")
                    {
                        saveBoard[r, c] = 0;
                    }
                    else
                        saveBoard[r, c] = int.Parse(tb.Text);
                    c++;
                }
                else
                {
                    if (tb.Text == "")
                    {
                        saveBoard[r, c] = 0;
                    }
                    else
                        saveBoard[r, c] = int.Parse(tb.Text);
                    c++;
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            int r = 0;
            int c = 0;
            foreach (TextBox tb in grid.Children)
            {
                if (c == 9)
                {
                    c = 0;
                    r++;
                    if (saveBoard[r, c] == 0)
                    {
                        tb.Text = "";
                    }
                    else
                        tb.Text = saveBoard[r, c].ToString();
                    c++;
                }
                else
                {
                    if (saveBoard[r, c] == 0)
                    {
                        tb.Text = "";
                    }
                    else
                        tb.Text = saveBoard[r, c].ToString();
                    c++;
                }
            }
        }

        private void Value_Slider(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
          
            if(e.NewValue==0){
                label4MS.Content = "100ms";
                ms = 100;
            }
            else if (e.NewValue == 1)
            {
                label4MS.Content = "50ms";
                ms = 50;
            }
            else if (e.NewValue == 2)
            {
                label4MS.Content = "10ms";
                ms = 10;
            }
            else if (e.NewValue == 3)
            {
                label4MS.Content = "5ms";
                ms = 5;
            }
            else if (e.NewValue == 4)
            {
                label4MS.Content = "1ms";
                ms = 1;
            }
        
        }
    }
   
}
