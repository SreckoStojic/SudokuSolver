﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Threading;

namespace Sudoku
{
    class DFS_Sudoku
    {
       public List<Node> solve_BFS(int [,] board){

           Node start = new Node(board);
           if (start.checkBoard())
           {
               List<Node> n = new List<Node>();
               n.Add(start);
               return n;
           }
           List<Node> States = new List<Node>();
           States.Add(start);
           List<Node> steps = new List<Node>();
           while (States.Count > 0)
           {
               Node onProgress = States[States.Count - 1];

                    steps.Add(onProgress);

                   if (onProgress.checkBoard())
                    {
                        steps.Add(onProgress);
                        return steps;
                    }
                    List<Node> nextStates = onProgress.possibleStates();

                    foreach (Node n in nextStates)
                    {
                           States.Add(n);
                    }
                       States.Remove(onProgress);
           }
           
           return null;
       }
     

    }
}
