using System;
using System.Collections.Generic;
using System.Linq;

namespace min_edit_distance
{
    class EditDistance
    {
        /*
         * Returns the number 0 if the two chars match.
         * Returns 2 otherwise. Utilized in MinEditDistance
         * to compare two chars without having to initialize
         * an extra value.
         **/
        public int SameLetter(char char1, char char2)
        {
            if (char1 == char2)
            {
                return 0;
            }
            else return 2;
        }

        /*
         * Returns a representation of the minimum edit distance
         * between two strings as an alignment. Letters on the
         * far left represent the source, letters in the middle
         * represent the target, and the lowercase letters on
         * the right represent the operations.
         **/
        public List<string> Alignment(Cell[,] cell, string target, string source)
        {
            List<string> backtrace = new List<string>();

            int s = source.Length;
            int t = target.Length;

            while (cell[t,s].Value != 0)
            {
                // Check if the Diagonal boolean is true.
                if (cell[t, s].Diagonal)//TODO: switched t and s
                {
                    // If the value is equal to the value in the cell
                    // located diagonally from it,
                    if (cell[t, s].Value == cell[t - 1, s - 1].Value)
                    {
                        // Create a new string with the letters and the operation, and add it to backtrace.
                        string i = target[t - 1].ToString().ToUpper() + " - " + source[s - 1].ToString().ToUpper();
                        backtrace.Insert(0, i);
                        // Decrement s and t to move to the cell located diagonally to the current cell.
                        if (s != 0) s -= 1;
                        if (t != 0) t -= 1;
                    }
                    else
                    {
                        // Create a new string with the letters and the operation, followed 
                        // by 's' for 'substitution'. Add it to backtrace.
                        string i = source[s - 1].ToString().ToUpper() + " - " + target[t - 1].ToString().ToUpper() + " s";
                        backtrace.Insert(0, i);
                        // Decrement s and t to move to the cell located diagonally to the current cell.
                        if (s != 0) s -= 1;
                        if (t != 0) t -= 1;
                    }
                }
                // Check if the Left boolean is true.
                else if (cell[t, s].Left)
                {
                    // Create a new string with only a letter from the target string,
                    // and the letter 'i' on the right for 'insert'. Add it to backtrace.
                    string i = "* - " + target[t - 1].ToString().ToUpper() + " i";//changed s to t
                    backtrace.Insert(0, i);
                    // Decrement only s to move to the cell located to the left.
                    if (t != 0) t -= 1;
                }
                // Check if the Bottom boolean is true.
                else if (cell[t, s].Bottom)
                {
                    // Create a string with only a letter from the source string,
                    // followed by 'd' for deletion. Add it to backtrace.
                    string i = source[s - 1].ToString().ToUpper() + " - * d";
                    backtrace.Insert(0, i);
                    // Decrement only t to move to the cell located below.
                    if (s != 0) s -= 1;
                }
                // If this condition is reached, the first column has
                // been reached where there are no activated booleans
                // on the cells.
                else
                {
                    // Take the final letter from the target, concatenate 'd' 
                    // for deletion, and add it to backtrace. Break from the loop.
                    // The algorithm is complete.
                    string i = source[t - 1].ToString().ToUpper() + " - * d";
                    backtrace.Insert(0, i);
                    break;
                }
            }
            return backtrace;
        }
        
     
        /*
         * Returns a two dimensional array of Cells used for
         * finding the minimum edit distance between two strings,
         * a target string and a source string. Utilizes the
         * Levenshtein distance where the deletion and insertion operations
         * only have a cost of 1, but the substitution operation has
         * a cost of 2.
         * */
        public Cell[,] MinEditDistance(string target, string source)
        {
            int t = target.Length;
            int s = source.Length;

            // Initialize an empty 2D array of cells with the number
            // of rows and columns corresponding to the lengths of
            // target and source. Initialize the cell at 0,0 to have
            // the value of 0 and all booleans set to false.
            Cell[,] distance = new Cell[t+1, s+1];
            distance[0, 0] = new Cell(false, false, false, 0);

            // Fill the first column with values up to the length of
            // the target. Only the left booleans are set to true.
            for(int i=1; i<=t; i++)
            {
                distance[i, 0] = new Cell(true, false, false, distance[i-1, 0].Value+1);
            }

            // Fill the first row with the values up to the length of
            // the target. Only the bottom booleans are set to true.
            for(int j=1; j<=s; j++)
            {
                distance[0, j] = new Cell(false, false, true, distance[0, j - 1].Value + 1);
            }

            // Loop through the columns by row. 
            for(int k=1; k<=t; k++)
            {
                for(int l=1; l<=s; l++)
                {
                    // Find the values of all the costs from the surrounding cells.
                    int[] nums = { distance[k-1,l].Value + 1,//insertion cost
                                   distance[k-1,l-1].Value + SameLetter(source[l-1], target[k-1]),//subtraction cost
                                   distance[k,l-1].Value + 1//deletion cost
                                   };
                    // Find the lowest of the costs and assign its value to the new cell.
                    // Set the booleans that correspond to the lowest number to true.
                    int m = nums.Min();
                    distance[k, l] = new Cell(m.Equals(nums[0]), m.Equals(nums[1]), m.Equals(nums[2]), m);
                }
            }

            return distance;
        }

        static void Main(string[] args)
        {
            EditDistance ed = new EditDistance();

            Console.Write("Enter the source string:\n");
            string source = Console.ReadLine();
            Console.Write("Enter the target string:\n");
            string target = Console.ReadLine();

            Cell[,] answer = ed.MinEditDistance(target, source);
            List<string> backtrace = ed.Alignment(answer, target, source);

            //Print the Minimum Edit Distance Table
            Console.WriteLine("Minimum Edit Distance Table:\n");
            for (int i = 0; i < answer.GetLength(0); i++)
            {
                for (int j = 0; j < answer.GetLength(1); j++)
                {
                    Console.Write(answer[i, j].Value + "\t");
                }
                Console.WriteLine();
            }

            //Print the Minimum Edit Distance Alignment
            Console.WriteLine("\n\nMinimum Edit Distance Alignment:\n");
            foreach (string s in backtrace)
            {
                Console.WriteLine(s);
            }
            Console.ReadLine();
        }
    }
}
