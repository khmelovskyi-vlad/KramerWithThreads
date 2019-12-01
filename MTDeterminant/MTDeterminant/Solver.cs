using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTDeterminant
{
    public class Solver<T>
    {
        private int maxThreadCount = 4;

        private int currentThreadCount = 0;

        private Queue<Matrix<T>> queue = new Queue<Matrix<T>>();

        private void SovleWithContinuation(object r)
        {
            var result = Matrix<T>.DeterminantSingleThread((Matrix<T>)r);
        }

        public Solution Solve(Matrix<T> matrix)
        {
            lock (queue)
            {
                if (currentThreadCount < maxThreadCount)
                {
                    ThreadPool.QueueUserWorkItem(r => , matrix);
                }
            }
        }
    }
}
