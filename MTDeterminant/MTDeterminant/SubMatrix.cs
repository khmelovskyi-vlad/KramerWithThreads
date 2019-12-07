using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTDeterminant
{
    public class SubMatrix<T> : Matrix<T>
    {
        public SubMatrix(T[,] array, Func<T, T, T> sum, Func<T, T, int, T> signMul, int[] mArray, int[] nArray, int m, int n) : base(array, sum, signMul, mArray, nArray, m, n)
        {
            (MArray, NArray) = CreateSubArray(mArray, nArray, m, n);
        }

        private static (int[] mArray, int[] nArray) CreateSubArray(int[] MArray, int[] NArray, int m, int n)
        {
            return AddIndex(MArray, NArray, m, n);
        }
        private static (int i, int j) FindNeedNum(int[] M, int[] N, int m, int n)
        {
            if (M.Length == 0)
            {
                return (m, n);
            }
            var needI = m;
            var needJ = n;
            for (int i = 0; i < M.Length; i++)
            {
                if (M[i] <= m)
                {
                    needI++;
                }
                if (N[i] <= n)
                {
                    needJ++;
                }
            }
            return (needI, needJ);
        }
        private static (int[] mArray, int[] nArray) AddIndex(int[] MArray, int[] NArray, int m, int n)
        {
            var (i, j) = FindNeedNum(MArray, NArray, m, n);
            var matrixM = MArray;
            MArray = new int[MArray.Length + 1];
            var matrixN = NArray;
            NArray = new int[NArray.Length + 1];
            Array.Copy(matrixM, 0, MArray, 0, matrixM.Length);
            Array.Copy(matrixN, 0, NArray, 0, matrixN.Length);
            MArray[MArray.Length - 1] = i;
            NArray[NArray.Length - 1] = j;
            return (MArray, NArray);
        }

        #region homework
        #endregion
    }
}
