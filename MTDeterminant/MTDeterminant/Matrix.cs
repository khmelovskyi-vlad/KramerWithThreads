using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTDeterminant
{
    public class Matrix<T>
    {
        protected T[,] MyArray;

        protected Func<T, T, T> Sum;
        protected Func<T, T, int, T> SignMul;

        protected int[] MArray;
        protected int[] NArray;
        protected T Result;

        public Matrix(T[,] array, Func<T, T, T> sum, Func<T, T, int, T> signMul, int[] mArray, int[] nArray, int m, int n)
        {
            MyArray = array;
            Sum = sum;
            SignMul = signMul;
            MArray = mArray;
            NArray = nArray;
        }

        public virtual T this[int i, int j]
        {
            get
            {
                return MyArray[i, j];
            }
        }

        public virtual Matrix<T> CreateSubmentrix(int[] mArray, int[] nArray, int m, int n)
        {
            var s = new SubMatrix<T>(MyArray, Sum, SignMul, mArray, nArray, m, n);
            return (Matrix<T>)s;
        }

        public T Determinant(bool singleThread)
        {
            if (singleThread)
            {
                return DeterminantSingleThread(this);
            }
            else
            {
                return DeterminantMultiThread();
            }
        }

        public static T DeterminantSingleThread(Matrix<T> matrix)
        {
            if (matrix.MArray.Length == matrix.MyArray.GetLength(0) - 1 && matrix.NArray.Length == matrix.MyArray.GetLength(1) - 1)
            {
                var i = GetSubI(matrix.MArray);
                var j = GetSubJ(matrix.NArray);
                return matrix[i, j];
            }
            T result = default(T);
            var s = matrix.MyArray.GetLength(0) - matrix.MArray.Length;
            var subMatrix = matrix.CreateSubmentrix(matrix.MArray, matrix.NArray, 0, 0);
            for (int i = 0; i < s; i++)
            {
                if (i!=0)
                {
                    subMatrix = ChangeIndex(subMatrix, i);
                }

                //var submatrix = matrix.CreateSubmentrix(i, 0);
                var innerDet = DeterminantSingleThread(subMatrix);
                var (m, n) = FindNeedNum(subMatrix, i, 0, matrix);
                var current = subMatrix.SignMul(innerDet, subMatrix[m, n], (int)Math.Pow(-1, m));
                result = subMatrix.Sum(current, result);
            }
            return result;
        }
        private static Matrix<T> ChangeIndex(Matrix<T> matrix, int m)
        {
            var needI = m;
            for (int i = 0; i < matrix.MArray.Length - 1; i++)
            {
                if (matrix.MArray[i] <= m)
                {
                    needI++;
                }
            }
            matrix.MArray[matrix.MArray.Length - 1] = needI;
            return matrix;
        }
        private static (int i, int j) FindNeedNum(Matrix<T> matrix, int m, int n, Matrix<T> bigMatrix)
        {
            if (matrix.MArray.Length == 1)
            {
                return (m, n);
            }
            var needI = m;
            var needJ = n;
            for (int i = 0; i < matrix.MArray.Length; i++)
            {
                if (matrix.MArray[i] <= m)
                {
                    needI++;
                }
                if (matrix.MArray[i] <= n)
                {
                    needJ++;
                }
            }
            var needI2 = needI;
            var needJ2 = needJ;
            for (int i = 0; i < bigMatrix.MArray.Length; i++)
            {
                if (bigMatrix.MArray[i] >= needI)
                {
                    needJ2--;
                }
                if (bigMatrix.NArray[i] >= needJ)
                {
                    needI2--;
                }
            }
            return (needI2, needJ2);
        }

        private static int GetSubI(int[] m)
        {
            int[] allM = m;
            var findIndex = false;
            for (int i = 0; i < allM.Length + 1; i++)
            {
                for (int j = 0; j < allM.Length; j++)
                {
                    if (allM[j] == i)
                    {
                        findIndex = false;
                        break;
                    }
                    findIndex = true;
                }
                if (findIndex)
                {
                    return i;
                }
            }
            return allM[0];
        }

        private static int GetSubJ(int[] n)
        {
            int[] allN = n;
            var findIndex = false;
            for (int i = 0; i < allN.Length + 1; i++)
            {
                for (int j = 0; j < allN.Length; j++)
                {
                    if (allN[j] == i)
                    {
                        findIndex = false;
                        break;
                    }
                    findIndex = true;
                }
                if (findIndex)
                {
                    return i;
                }
            }
            return allN[0];
        }

        private T DeterminantMultiThread()
        {
            throw new NotImplementedException();
        }
    }
}
