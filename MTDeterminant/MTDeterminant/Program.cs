using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTDeterminant
{
    class Program
    {
        static void Main(string[] args)
        {
                IntMatrix matrix = new IntMatrix(new int[,]
                {
                {1, 2, 3 },
                {4, 5, 6 },
                {7, 8, 9 }
                }, new int[0], new int[0], 0, 0);
                var det = matrix.Determinant(true);
            
        }
    }
}
