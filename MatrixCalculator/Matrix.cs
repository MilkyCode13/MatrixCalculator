using System;

namespace MatrixCalculator
{
    /// <summary>
    /// Contains methods for interacting with matrices.
    /// </summary>
    internal static class Matrix
    {
        /// <summary>
        /// Calculates trace of a square matrix.
        /// Trace of a square matrix A is defined to be the sum of elements on the main diagonal of A.
        /// </summary>
        /// <param name="matrix">Square matrix to calculate trace.</param>
        /// <returns>Returns calculated trace of a matrix.</returns>
        /// <exception cref="ArgumentException">Matrix is not square.</exception>
        public static double Trace(double[,] matrix)
        {
            int size = matrix.GetLength(0);
            
            // Check that matrix is square.
            if (matrix.GetLength(1) != size)
            {
                throw new ArgumentException("Matrix must be square.");
            }

            double sum = 0;

            // Iterate over elements on the main diagonal of the matrix.
            for (var i = 0; i < size; i++)
            {
                sum += matrix[i, i];
            }

            return sum;
        }

        /// <summary>
        /// Returns transpose of a matrix.
        /// Transpose of a matrix is an operator which flips a matrix over its diagonal.
        /// </summary>
        /// <param name="matrix">Matrix to transpose.</param>
        /// <returns>Transpose of a matrix.</returns>
        public static double[,] Transpose(double[,] matrix)
        {
            // Create new matrix with flipped dimensions.
            var transpose = new double[matrix.GetLength(1), matrix.GetLength(0)];

            // Iterate over each element and add it to transpose with flipped indices.
            for (var i = 0; i < transpose.GetLength(0); i++)
            {
                for (var j = 0; j < transpose.GetLength(1); j++)
                {
                    transpose[i, j] = matrix[j, i];
                }
            }

            return transpose;
        }

        /// <summary>
        /// Adds two matrices.
        /// Sum of two matrices is a matrix in which every element is a sum of corresponding elements of input matrices.
        /// </summary>
        /// <param name="matrix1">First matrix to add.</param>
        /// <param name="matrix2">Second matrix to add.</param>
        /// <returns>Sum of two matrices.</returns>
        /// <exception cref="ArgumentException">Matrices have different dimensions.</exception>
        public static double[,] Sum(double[,] matrix1, double[,] matrix2)
        {
            int rows = matrix1.GetLength(0);
            int columns = matrix1.GetLength(1);
            
            // Check that matrices are of the same type.
            if (matrix2.GetLength(0) != rows || matrix2.GetLength(1) != columns)
            {
                throw new ArgumentException("Matrices must have the same dimensions.");
            }

            var sum = new double[rows, columns];
            
            // Iterate over corresponding elements of matrices put add their sum to matrix sum. 
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    sum[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }

            return sum;
        }
        
        /// <summary>
        /// Subtracts two matrices.
        /// Difference of two matrices is a matrix in which every element is a difference of corresponding elements
        /// of input matrices.
        /// </summary>
        /// <param name="matrix1">Matrix to subtract from.</param>
        /// <param name="matrix2">Matrix to subtract.</param>
        /// <returns>Difference of two matrices.</returns>
        /// <exception cref="ArgumentException">Matrices have different dimensions.</exception>
        public static double[,] Difference(double[,] matrix1, double[,] matrix2)
        {
            int rows = matrix1.GetLength(0);
            int columns = matrix1.GetLength(1);
            
            // Check that matrices are of the same type.
            if (matrix2.GetLength(0) != rows || matrix2.GetLength(1) != columns)
            {
                throw new ArgumentException("Matrices must have the same dimensions.");
            }

            var difference = new double[rows, columns];
            
            // Iterate over corresponding elements of matrices put add their difference to matrix difference. 
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    difference[i, j] = matrix1[i, j] - matrix2[i, j];
                }
            }

            return difference;
        }
        
        /// <summary>
        /// Multiplies two matrices.
        /// </summary>
        /// <param name="matrix1">Matrix to multiply.</param>
        /// <param name="matrix2">Matrix to multiply by.</param>
        /// <returns>Product of two matrices.</returns>
        /// <exception cref="ArgumentException">Number of columns of the first matrix is not equal to
        /// number of rows of the second matrix.</exception>
        public static double[,] Product(double[,] matrix1, double[,] matrix2)
        {
            int rows = matrix1.GetLength(0);
            int intermediate = matrix1.GetLength(1);
            int columns = matrix2.GetLength(1);
            
            // Check that number of columns of the first matrix is equal to number of rows of the second matrix.
            if (matrix2.GetLength(0) != intermediate)
            {
                throw new ArgumentException("Number of columns of the first matrix must be equal to " +
                                            "number of rows of the second matrix.");
            }

            var product = new double[rows, columns];

            // Calculating each element of a matrix.
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    for (var k = 0; k < intermediate; k++)
                    {
                        product[i, j] += matrix1[i, k] * matrix2[k, j];
                    }
                }
            }

            return product;
        }
        
        /// <summary>
        /// Multiplies a matrix by a scalar.
        /// </summary>
        /// <param name="matrix">Matrix to multiply.</param>
        /// <param name="scalar">Scalar to multiply by.</param>
        /// <returns>Product of a matrix and a scalar.</returns>
        public static double[,] Factorize(double[,] matrix, double scalar)
        {
            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            var product = new double[rows, columns];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    product[i, j] = scalar * matrix[i, j];
                }
            }

            return product;
        }
        
        /// <summary>
        /// Calculates determinant of a square matrix.
        /// </summary>
        /// <param name="matrix">Square matrix to calculate determinant.</param>
        /// <returns>Determinant of a matrix.</returns>
        /// <exception cref="ArgumentException">Matrix is not square.</exception>
        public static double Determinant(double[,] matrix)
        {
            int size = matrix.GetLength(0);
            
            // Check that matrix is square.
            if (matrix.GetLength(1) != size)
            {
                throw new ArgumentException("Matrix must be square.");
            }

            // Get triangular form and number of row swaps during transformations.
            int numberOfSwaps = SolveGaussian(matrix, out double[,] triangular, false);

            double determinant = numberOfSwaps % 2 == 0 ? 1.0 : -1.0;
            
            for (var i = 0; i < size; i++)
            {
                determinant *= triangular[i, i];
            }

            return determinant;
        }

        /// <summary>
        /// Solves a matrix to echelon or canonical form using Gaussian elimination.
        /// </summary>
        /// <param name="matrix">Matrix to transform to needed form.</param>
        /// <param name="solution">Result matrix.</param>
        /// <param name="toCanonical">If <c>true</c>, transform to canonical form, to echelon form otherwise.</param>
        /// <returns>Number of row swaps made.</returns>
        public static int SolveGaussian(double[,] matrix, out double[,] solution, bool toCanonical = true)
        {
            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            solution = new double[rows, columns];
            Array.Copy(matrix, solution, matrix.Length);

            var rowIndex = 0;
            var columnIndex = 0;
            var numberOfSwaps = 0;

            while (rowIndex < rows && columnIndex < columns)
            {
                // Make a step of the algorithm.
                GaussianStep(ref solution, ref rowIndex, ref columnIndex, ref numberOfSwaps, toCanonical);
            }

            return numberOfSwaps;
        }

        /// <summary>
        /// Makes one step of gaussian algorithm.
        /// </summary>
        /// <param name="solution">Solution matrix to process.</param>
        /// <param name="rowIndex">Current index of row.</param>
        /// <param name="columnIndex">Current index of column.</param>
        /// <param name="numberOfSwaps">Number of swaps.</param>
        /// <param name="toCanonical">If transforming to canonical form.</param>
        private static void GaussianStep(ref double[,] solution, ref int rowIndex, ref int columnIndex,
            ref int numberOfSwaps, bool toCanonical)
        {
            // The leading element is zero.
            if (solution[rowIndex, columnIndex] == 0)
            {
                if (GaussianZeroCase(ref solution, ref rowIndex, ref columnIndex))
                {
                    numberOfSwaps++;
                }
                return;
            }

            // Nullify all elements below (and above if making row canonical form)
            // by adding factored current row to others.
            for (var i = toCanonical ? 0 : rowIndex + 1; i < solution.GetLength(0); i++)
            {
                if (i != rowIndex && solution[i, columnIndex] != 0)
                {
                    double factor = -solution[i, columnIndex] / solution[rowIndex, columnIndex];
                    AddMultipleOfRow(ref solution, i, rowIndex, factor);
                }
            }

            // If making canonical form, make leading element equal 1 by dividing row.
            if (toCanonical && Math.Abs(solution[rowIndex, columnIndex] - 1.0) > 1e-12)
            {
                double factor = 1.0 / solution[rowIndex, columnIndex];
                MultiplyRow(ref solution, rowIndex, factor);
            }

            // Go to the next row and column.
            rowIndex++;
            columnIndex++;
        }

        /// <summary>
        /// Processes case when leading element equals zero.
        /// </summary>
        /// <param name="matrix">Matrix to process.</param>
        /// <param name="rowIndex">Row of leading element.</param>
        /// <param name="columnIndex">Column of leading element.</param>
        /// <returns><c>true</c>, if made a row swap, <c>false</c> otherwise.</returns>
        private static bool GaussianZeroCase(ref double[,] matrix, ref int rowIndex, ref int columnIndex)
        {
            // Searching for non-zero elements below leading element.
            for (var i = rowIndex + 1; i < matrix.GetLength(0); i++)
            {
                // If found non-zero element, swap corresponding row and current row.
                if (matrix[i, columnIndex] != 0)
                {
                    SwapRows(ref matrix, rowIndex, i);
                    return true;
                }
            }

            // If non-zero element not found, go to the next column.
            columnIndex++;
            return false;
        }

        /// <summary>
        /// Swaps two rows in the matrix.
        /// </summary>
        /// <param name="matrix">Matrix to process.</param>
        /// <param name="row1">First row to swap.</param>
        /// <param name="row2">Second row to swap.</param>
        /// <exception cref="ArgumentOutOfRangeException">Rows are out of matrix bounds.</exception>
        private static void SwapRows(ref double[,] matrix, int row1, int row2)
        {
            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            // Check that rows are in matrix bounds.
            if (row1 >= rows)
            {
                throw new ArgumentOutOfRangeException(nameof(row1), "Row must be in matrix bounds.");
            }
            if (row2 >= rows)
            {
                throw new ArgumentOutOfRangeException(nameof(row2), "Row must be in matrix bounds.");
            }

            // Swap corresponding elements in each row.
            for (var i = 0; i < columns; i++)
            {
                (matrix[row1, i], matrix[row2, i]) = (matrix[row2, i], matrix[row1, i]);
            }
        }

        /// <summary>
        /// Multiplies a row of a matrix by given non-zero factor.
        /// </summary>
        /// <param name="matrix">Matrix to process.</param>
        /// <param name="row">Row to multiply.</param>
        /// <param name="factor">Factor to multiply by.</param>
        /// <exception cref="ArgumentOutOfRangeException">Row is out of matrix bounds or factor is zero.</exception>
        private static void MultiplyRow(ref double[,] matrix, int row, double factor)
        {
            // Check that row is in matrix bounds.
            if (row >= matrix.GetLength(0))
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Row must be in matrix bounds.");
            }

            // Check that factor is not zero.
            if (factor == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(factor), "Factor must not be zero.");
            }
            
            int columns = matrix.GetLength(1);

            // Multiply each element of a row by the factor.
            for (var i = 0; i < columns; i++)
            {
                matrix[row, i] = factor * matrix[row, i];
            }
        }
        
        /// <summary>
        /// Adds a row multiplied by a factor to another row.
        /// </summary>
        /// <param name="matrix">Matrix to process.</param>
        /// <param name="row1">Row to add to.</param>
        /// <param name="row2">Row to add multiple of.</param>
        /// <param name="factor">Factor to multiply by.</param>
        /// <exception cref="ArgumentOutOfRangeException">Rows are out of matrix bounds.</exception>
        private static void AddMultipleOfRow(ref double[,] matrix, int row1, int row2, double factor)
        {
            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            // Check that rows are in matrix bounds.
            if (row1 >= rows)
            {
                throw new ArgumentOutOfRangeException(nameof(row1), "Row must be in matrix bounds.");
            }
            if (row2 >= rows)
            {
                throw new ArgumentOutOfRangeException(nameof(row2), "Row must be in matrix bounds.");
            }

            for (var i = 0; i < columns; i++)
            {
                matrix[row1, i] += factor * matrix[row2, i];
                if (Math.Abs(matrix[row1, i]) < 1e-12)
                {
                    matrix[row1, i] = 0;
                }
            }
        }
    }
}