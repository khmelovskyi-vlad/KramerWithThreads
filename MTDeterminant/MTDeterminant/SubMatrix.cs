using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTDeterminant
{
    public class SubMatrix<T> : Matrix<T>
    {
        public SubMatrix(T[,] array, Func<T, T, T> sum, Func<T, T, int, T> signMul, int m, int n) : base(array, sum, signMul)
        {
            Array = CreateSubArray(array, m, n);
        }

        private static T[,] CreateSubArray(T[,] array, int m, int n)
        {
            T[,] subarray = new T[array.GetLength(0) - 1, array.GetLength(1) - 1];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (i != m && j != n)
                    {
                        subarray[i < m ? i : i - 1, j < n ? j : j - 1] = array[i, j];
                    }
                }
            }
            return subarray;
        }

        #region homework
        private readonly int m;
        private readonly int n;

        private int GetSubI(int i)
        {
            return i < m ? i : i + 1;
        }

        private int GetSubJ(int j)
        {
            return j < n ? j : j + 1;
        }
        #endregion
    }
}
