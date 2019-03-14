using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class PermutationAlgorithm
    {
        public static void SwapTwoNumber(ref string a, ref string b)
        {
            string temp = a;
            a = b;
            b = temp;
        }

        public static void Permutation(ref string[] list, int k, int m, ref string[,] array, ref int count)
        {
            if (k == m)
            {
                for (int i = 0; i <= m; i++)
                {
                    array[count, i] = list[i];
                }
                count++;
            }
            else
            {
                for (int i = k; i <= m; i++)
                {
                    SwapTwoNumber(ref list[k], ref list[i]);
                    Permutation(ref list, k + 1, m, ref array, ref count);
                    SwapTwoNumber(ref list[k], ref list[i]);
                }
            }
        }
    }
}
