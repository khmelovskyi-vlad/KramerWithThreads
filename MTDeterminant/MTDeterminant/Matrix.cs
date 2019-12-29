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

        protected List<int> M = new List<int>();
        protected List<int> N = new List<int>();

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
            return new SubMatrix<T>(Array, Sum, SignMul, m, n, M, N);
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
            var s = matrix.Array.GetLength(0) - matrix.M.Count;
            if (matrix.Array.GetLength(0) - matrix.N.Count == 1 && matrix.Array.GetLength(1) - matrix.M.Count == 1)
            {
                return FindElement(matrix);
            }
            T result = default(T);
            for (int i = 0; i < s; i++)
            {
                var submatrix = matrix.CreateSubmentrix(i, 0);
                var innerDet = DeterminantSingleThread(submatrix);
                var(m, n) = matrix.DeleterIndex(submatrix);
                var current = matrix.SignMul(innerDet, matrix[m, n], (int)Math.Pow(-1, i));
                result = matrix.Sum(current, result);
            }
            return result;
        }
        //public static Task<T> DeterminantMultiThread(Matrix<T> matrix)
        //{
        //    var s = matrix.Array.GetLength(0) - matrix.M.Count;
        //    if (matrix.Array.GetLength(0) - matrix.N.Count == 1 && matrix.Array.GetLength(1) - matrix.M.Count == 1)
        //    {
        //        return Task.FromResult(FindElement(matrix));
        //    }
        //    T result = default(T);
        //    Task<T> GetSumItem(int i)
        //    {
        //        var submatrix = matrix.CreateSubmentrix(i, 0);
        //        var innerDet = DeterminantMultiThread(submatrix);
        //        var (m, n) = matrix.DeleterIndex(submatrix);
        //        return innerDet.ContinueWith(task => matrix.SignMul(innerDet.Result, matrix[m, n], (int)Math.Pow(-1, i)));
        //    }
        //    var tasks = Enumerable.Range(0, s).Select(i => GetSumItem(i)).ToArray();
        //    return Task.WhenAll(tasks).ContinueWith(task => task.Result.Aggregate(result, (acc, item) => matrix.Sum(item, acc)));
        //}

        public async static Task<T> DeterminantMultiThread(Matrix<T> matrix)
        {
            if (matrix.Array.GetLength(0) - matrix.N.Count == 1 && matrix.Array.GetLength(1) - matrix.M.Count == 1)
            {
                return FindElement(matrix);
            }
            T result = default(T);
            async Task<T> GetSumItem(int i)
            {
                var submatrix = matrix.CreateSubmentrix(i, 0);
                var innerDet = await DeterminantMultiThread(submatrix);
                var (m, n) = matrix.DeleterIndex(submatrix);
                var current = matrix.SignMul(innerDet, matrix[m, n], (int)Math.Pow(-1, i));
                return current;
            }
            return (await Task.WhenAll(Enumerable.Range(0, matrix.Array.GetLength(0) - matrix.M.Count).Select(GetSumItem)))
                .Aggregate(result, (acc, item) => matrix.Sum(item, acc));

        }
        private (int m, int n) DeleterIndex(Matrix<T> matrix)
        {
            var m = matrix.M[matrix.M.Count - 1];
            var n = matrix.N[matrix.N.Count - 1];
            matrix.M.RemoveAt(matrix.M.Count - 1);
            matrix.N.RemoveAt(matrix.N.Count - 1);
            return (m, n);
        }
        private (int m, int n, T) DeleterIndexTask(Matrix<T> matrix, T innerDet)
        {
            var m = matrix.M[matrix.M.Count - 1];
            var n = matrix.N[matrix.N.Count - 1];
            matrix.M.RemoveAt(matrix.M.Count - 1);
            matrix.N.RemoveAt(matrix.N.Count - 1);
            return (m, n, innerDet);
        }

        private static T FindElement(Matrix<T> matrix)
        {
            return matrix.Array[FindCoordinate(matrix.M, matrix.Array.GetLength(0)), FindCoordinate(matrix.N, matrix.Array.GetLength(1))];
        }
        private static int FindCoordinate(List<int> coordinateSlices, int range)
        {
            int coordinate = 0;
            var findCoordinate = true;
            for (int i = 0; i < range; i++)
            {
                foreach (var slices in coordinateSlices)
                {
                    if (i == slices)
                    {
                        findCoordinate = false;
                        continue;
                    }
                }
                if (findCoordinate)
                {
                    coordinate = i;
                    return coordinate;
                }
                findCoordinate = true;
            }
            return coordinate;
        }
    }
}
