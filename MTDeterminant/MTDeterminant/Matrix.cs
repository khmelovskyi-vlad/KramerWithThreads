using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTDeterminant
{
    public class Matrix<T>
    {
        protected T[,] Array;

        protected Func<T, T, T> Sum;
        protected Func<T, T, int, T> SignMul;

        public virtual int M => Array.GetLength(0);
        public virtual int N => Array.GetLength(1);

        public Matrix(T[,] array, Func<T, T, T> sum, Func<T, T, int, T> signMul)
        {
            Array = array;
            Sum = sum;
            SignMul = signMul;
        }

        public virtual T this[int i, int j]
        {
            get
            {
                return Array[i, j];
            }
        }

        public virtual Matrix<T> CreateSubmentrix(int m, int n)
        {
            return new SubMatrix<T>(Array, Sum, SignMul, m, n);
        }

        public T Determinant(bool singleThread)
        {
            if (singleThread)
            {
                return DeterminantSingleThread(this);
            }
            else
            {
                return DeterminantMultiThread(this).Result;
            }
        }

        public static T DeterminantSingleThread(Matrix<T> matrix)
        {
            if (matrix.N == 1 && matrix.M == 1)
            {
                return matrix[0, 0];
            }
            T result = default(T);
            for (int i = 0; i < matrix.M; i++)
            {
                var submatrix = matrix.CreateSubmentrix(i, 0);
                var innerDet = DeterminantSingleThread(submatrix);
                var current = matrix.SignMul(innerDet, matrix[i, 0], (int)Math.Pow(-1, i));
                result = matrix.Sum(current, result);
            }
            return result;
        }

        public async static Task<T> DeterminantMultiThread(Matrix<T> matrix)
        {
            if (matrix.N == 1 && matrix.M == 1)
            {
                return matrix[0, 0];
            }
            T result = default(T);
            async Task<T> GetSumItem(int i)
            {
                var submatrix = matrix.CreateSubmentrix(i, 0);
                var innerDet = await DeterminantMultiThread(submatrix);
                var current = matrix.SignMul(innerDet, matrix[i, 0], (int)Math.Pow(-1, i));
                return current;
            }
            return (await Task.WhenAll(Enumerable.Range(0, matrix.M).Select(GetSumItem)))
                .Aggregate(result, (acc, item) => matrix.Sum(item, acc));

        }
    }
}