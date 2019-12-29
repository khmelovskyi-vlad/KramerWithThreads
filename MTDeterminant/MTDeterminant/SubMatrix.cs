using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTDeterminant
{
    public class SubMatrix<T> : Matrix<T>
    {
        public SubMatrix(T[,] array, Func<T, T, T> sum, Func<T, T, int, T> signMul, int m, int n, List<int> mSlices, List<int> nSlices) : base(array, sum, signMul)
        {
            (M, N) = AddElements(m, n, mSlices, nSlices);
        }

        private static (List<int>, List<int>) AddElements(int m, int n, List<int> mSlices, List<int> nSlices)
        {
            return (AddElement(m, mSlices), AddElement(n, nSlices));
        }
        private static List<int> AddElement(int coordinate, List<int> coordinateSlices)
        {
            foreach (var slice in coordinateSlices.OrderBy(x=>x))
            {
                if (coordinate >= slice)
                {
                    coordinate++;
                }
            }
            coordinateSlices.Add(coordinate);
            return coordinateSlices;
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
