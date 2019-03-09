using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{
    class Node
    {
        private int[,] state;
        public static int Total = 45;
        public static int row,colomn;
        public static int red, kolona,kvadrat;


        public Node(int[,] state)
        {
            this.state = state;
        }
        public int [,] getState(){
            return this.state;
        }
        public void setState(int [,] state){
            this.state = state;
        }

        public bool checkBoard()
        {
            int[,] board = this.state;
            int suma = 0; // suma po vrstama 

            for (int i = 0; i < 9; i++)
            {
                    for (int j=0 ; j<9;j++)
                    {
                        suma += board[i, j];
                    }

                    if (suma != Total)
                    {
                        return false;
                    }
                    else suma = 0;
            }

            suma = 0;

            for (int j = 0; j < 9; j++)       // suma po kolonama
            {
                for (int i = 0; i < 9; i++)
                {
                    suma += board[i, j];
                }
                if (suma != Total)
                {
                    return false;
                }
                else suma = 0;
            }

            suma = 0;         //suma po kvadratima
              
            for (int s = 1; s <= 3; s++)
            {
                for (int i = 1; i <= 3; i++)
                {
                    for (int k = i * 3 - 3; k < i * 3; k++)
                    {
                        for (int l = s * 3 - 3; l < s * 3; l++)
                        {
                            suma += board[k, l];
                        }
                    }
                    if (suma != Total)
                    {
                        return false;
                    }
                    else
                    {
                        suma = 0;
                    }
                }
            }         
                
            return true;                  // TODO  radi sa boardom
        }


        public static int[] get_Spot( int [,] board)   // metoda koja trazi prvo slobodno mesto na tabli
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[i, j] == 0)
                    {
                        int[] RC = new int[2];
                        RC[0] = i;
                        RC[1] = j;
                        return RC;
                    }
                }
            }
            return null;
        }

        public static List<Node> new_States(int[,] board,int row,int colomn)   // metoda vraca sva moguca stanja
        {
                                                                                 // da se kreiraju nodovi koji odgovaraju row i colimn slobodnom polju
            List<Node> possibleStates = new List<Node>();
            List<int> options = new List<int>(); // sva moguca stanja
            for (int i = 1; i < 10; i++)
            {
                options.Add(i);
            }
            List<int> pom = new List<int>();

            for (int j = 0; j < 9; j++)                              // opcije za red
            {
                if (board[row,colomn] != board[row,j])
                {
                    if (options.Contains(board[row, j]))
                    {
                        options.Remove(board[row, j]);
                    }  
                }
            }
            for (int i = 0; i < 9; i++)
            {
                if (board[row, colomn] != board[i, colomn])            // opcije za kolonu
                {
                    if (options.Contains(board[i, colomn]))
                    {
                        options.Remove(board[i, colomn]);
                    }
                }
            }
            int[] koordinate = new int[2];    // u koordinate smestamo pocetak svakog od 9 kvadrata
            if (row < 3)
            {
                if (colomn < 3)
                {
                    koordinate[0] = 0;
                    koordinate[1] = 0;
                }
                else if (colomn >= 3 && colomn < 6)
                {
                    koordinate[0] = 0;
                    koordinate[1] = 3;
                }
                else
                {
                    koordinate[0] = 0;
                    koordinate[1] = 6;
                }
            }
            else if (row >= 3 && row < 6)
            {
                if (colomn < 3)
                {
                    koordinate[0] = 3;
                    koordinate[1] = 0;
                }
                else if (colomn >= 3 && colomn < 6)
                {
                    koordinate[0] = 3;
                    koordinate[1] = 3;
                }
                else
                {
                    koordinate[0] = 3;
                    koordinate[1] = 6;
                }
            }
            else
            {
                if (colomn < 3)
                {
                    koordinate[0] = 6;
                    koordinate[1] = 0;
                }
                else if (colomn >= 3 && colomn < 6)
                {
                    koordinate[0] = 6;
                    koordinate[1] = 3;
                }
                else
                {
                    koordinate[0] = 6;
                    koordinate[1] = 6;
                }
            }

            for (int i = koordinate[0]; i <koordinate[0]+ 3; i++)
            {
                for (int j = koordinate[1]; j <koordinate[1] + 3; j++)
                {
                    if (board[i, j] != board[row, colomn])
                    {
                        if (options.Contains(board[i, j]))
                        {
                            options.Remove(board[i, j]);
                        }
                    }
                }
            }
            foreach (int i in options)
            {
                int[,] tabla = new int[9,9];
                Array.Copy(board, tabla, 81);
                tabla[row, colomn] = i;
                Node n = new Node(tabla);
                possibleStates.Add(n);
            }
            return possibleStates;
        }

        public List<Node> possibleStates()  // TODO uraditi filtriranje i izbaciti moguca stanja
        {
            List<Node> possibleStates = new List<Node>();
            int[] RC = get_Spot(this.state);

            if (RC != null)
            {
                 row = RC[0];
                 colomn = RC[1];
            }

            possibleStates = new_States(this.state,row,colomn);

            return possibleStates;
        }


        public  List<Node> getBest()
        {
            List<Node> possibleStates = new List<Node>();
            int[] RC = getSpot_AStar(this.state);

            if (RC != null)
            {
                row = RC[0];
                colomn = RC[1];
            }

            possibleStates = new_States(this.state, row, colomn);

            return possibleStates;
            
        }
        public static int[] getSpot_AStar(int[,] board)
        {
            List<int> nuleRed = new List<int>();
            List<List<int>> listanulared = new List<List<int>>();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[i, j] == 0)
                    {
                        nuleRed.Add(board[i, j]);
                    }
                }
                listanulared.Add(nuleRed);
                nuleRed = new List<int>();
            }
            int min = 9;
            for (int i = 0; i < listanulared.Count; i++)
            {
                if (listanulared.ElementAt(i).Count <= min && listanulared.ElementAt(i).Count > 0)
                {
                    min = listanulared.ElementAt(i).Count();
                    red = i;
                }
            }


            List<int> nuleKolona = new List<int>();
            List<List<int>> listanulaKolona = new List<List<int>>();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[j, i] == 0)
                    {
                        nuleKolona.Add(board[j, i]);
                    }
                }
                listanulaKolona.Add(nuleKolona);
                nuleKolona = new List<int>();
            }
            int minKolona = 9;
            for (int i = 0; i < listanulaKolona.Count; i++)
            {
                if (listanulaKolona.ElementAt(i).Count <= minKolona && listanulaKolona.ElementAt(i).Count>0)
                {
                    minKolona = listanulaKolona.ElementAt(i).Count();
                    kolona = i;
                }
            }

            List<int> nuleKvadtata = new List<int>();
            List<List<int>> listanulaKvadrata = new List<List<int>>();
            for (int s = 1; s <= 3; s++)
            {
                for (int i = 1; i <= 3; i++)
                {
                    for (int k = i * 3 - 3; k < i * 3; k++)
                    {
                        for (int l = s * 3 - 3; l < s * 3; l++)
                        {
                            nuleKvadtata.Add(board[k, l]);
                        }
                    }
                    listanulaKvadrata.Add(nuleKvadtata);
                    nuleKvadtata = new List<int>();     
                }
            }

            int minKvadtata = 9;
            for (int i = 0; i < listanulaKvadrata.Count; i++)
            {
                if (listanulaKvadrata.ElementAt(i).Count <= minKvadtata && listanulaKvadrata.ElementAt(i).Count > 0)
                {
                    minKvadtata = listanulaKvadrata.ElementAt(i).Count();
                    kvadrat = i;
                }
            }

            int[] redKolona = new int[2];
            if (kvadrat == 0)
                redKolona = RowColomn(board, 0, 0);
            else if (kvadrat == 1)
                redKolona = RowColomn(board, 0, 3);
            else if (kvadrat == 2)
                redKolona = RowColomn(board, 0, 6);
            else if (kvadrat == 3)
                redKolona = RowColomn(board, 3, 0);
            else if (kvadrat == 4)
                redKolona = RowColomn(board, 3, 3);
            else if (kvadrat == 5)
                redKolona = RowColomn(board, 3, 6);
            else if (kvadrat == 6)
                redKolona = RowColomn(board, 6, 0);
            else if (kvadrat == 7)
                redKolona = RowColomn(board, 6, 3);
            else
                redKolona = RowColomn(board, 6, 6);

        

            if (min <= minKolona && min <=minKvadtata )
            {
                for (int i = 0; i < 9; i++)
                {
                    if (board[red, i] == 0)
                    {
                        int[] RS = new int[2];
                        RS[0] = red;
                        RS[1] = i;
                        return RS;
                    }
                }
            }
            else if (minKolona <= minKvadtata && minKolona <= minKvadtata)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (board[i, kolona] == 0)
                    {
                        int[] RS = new int[2];
                        RS[0] = i;
                        RS[1] = kolona;
                        return RS;
                    }
                }
            }
            else
            {
                return redKolona;
            }

            return null;
        }

        public static int [] RowColomn(int [,] board,int row,int colomn){

            for (int i=row;i<row+3;i++){
                for (int j = colomn; j < colomn + 3; j++)
                {
                    if (board[i, j] == 0)
                    {
                        int[] RC = new int[2];
                        RC[0] = i;
                        RC[1] = j;
                        return RC;
                    }
                }
            }
            return null;
        }

    }
}
