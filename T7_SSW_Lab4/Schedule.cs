using System;
using System.Collections.Generic;
using System.Linq;

namespace T7_SSW_Lab4
{
    public class Schedule
    {
        private List<List<short>> _matrix;
        private readonly Random _rand = new Random();

        // Generates random matrix with specified size and connectivity
        private void GenerateMarix(int size, int connectivity)
        {
            _matrix = new List<List<short>>();

            for (var i = 0; i < size; i++)
            {
                _matrix.Add(new List<short>(size));
                for (var j = 0; j < size; j++)
                    _matrix[i].Add(0);
            }


            // Filling with ones
            for (var i = 0; i < connectivity * size * size * .01; i++)
            {
                while (true)
                {
                    var x = _rand.Next(0, size);
                    var y = _rand.Next(0, size);
                    if (_matrix[x][y] != 0) continue;
                    _matrix[x][y] = 1;
                    break;
                }
            }
        }

        // Removes specified row and column from matrix
        // Addidtional for RemoveNeccesary
        private void RemoveRowCol(int row, int col)
        {
            _matrix.RemoveAt(row);
            foreach (var r in _matrix)
                r.RemoveAt(col);
        }

        // Removes nessesary assignments
        private void RemoveNeccesary()
        {
            for (var i = 0; i < _matrix.Count; i++)
                for (var j = 0; j < _matrix.Count; j++)
                    // If current cell is 1 and all others in row and column are zeros
                    if (_matrix[i][j] == 1)
                    {
                        var countZerosCol = _matrix.Count(t => t[j] == 0);
                        if (countZerosCol != _matrix.Count - 1) continue;
                        {
                            var countZerosRow = _matrix.Where((t, j1) => _matrix[i][j1] == 0).Count();
                            if (countZerosRow != _matrix.Count - 1) continue;
                            RemoveRowCol(i, j);

                            // Do the same with lesser matrix
                            RemoveNeccesary();
                            return;
                        }
                    }
        }

        // Swaps two rows in matrix
        // Additional for MakeZeroMatrixRecursive
        private void SwapRows(int row1, int row2)
        {
            var buf = _matrix[row1];
            _matrix[row1] = _matrix[row2];
            _matrix[row2] = buf;
        }

        // Swaps two columns in matrix
        // Additional for MakeZeroMatrixRecursive
        private void SwapCols(int col1, int col2)
        {
            foreach (var t in _matrix)
            {
                var tmp = t[col1];
                t[col1] = t[col2];
                t[col2] = tmp;
            }
        }

        // Tries to make zero matrix in top right corner
        private void MakeZeroMatrixRecursive(int offset)
        {
            if (offset == _matrix.Count)
                return;

            // Find row with min number of zeros
            var imin = 0;
            var min = _matrix.Count;
            for (var i = offset; i < _matrix.Count; i++)
            {
                var currentOnes = 0;
                for (var j = offset; j < _matrix.Count; j++)
                    if (_matrix[i][j] == 1)
                        currentOnes++;

                if (currentOnes >= min) continue;
                min = currentOnes;
                imin = i;
            }

            // If found such row
            if (offset < imin)
                SwapRows(offset, imin);

            // Find column with max number of ones where cell on "first" line is 1
            var maxInitialised = false;
            var jmax = 0;
            var max = 0;
            for (var j = 0; j < _matrix.Count; j++)
                if (_matrix[offset][j] == 1)
                {
                    if (!maxInitialised)
                    {
                        jmax = j;
                        maxInitialised = true;
                    }

                    var countOnes = 0;
                    for (var i = offset; i < _matrix.Count; i++)
                        if (_matrix[i][j] == 1)
                            countOnes++;


                    if (max >= countOnes) continue;
                    max = countOnes;
                    jmax = j;
                }

            // If found such row
            if (offset < jmax)
                SwapCols(offset, jmax);

            // Do the same with lesser matrix
            // ReSharper disable once TailRecursiveCall
            MakeZeroMatrixRecursive(offset + 1);
        }

        // Checks matrix for conflict assignments
        private bool CheckConflict()
        {
            for (var i = 0; i < _matrix.Count; i++)
                if (_matrix[i][i] == 1)
                {
                    var zerosNumber = 0;
                    for (var i1 = 0; i1 <= i; i1++)
                        for (var j1 = i; j1 < _matrix.Count; j1++)
                            if (_matrix[i1][j1] == 0)
                                zerosNumber++;
                    if (zerosNumber == (i + 1) * (_matrix.Count - i) - 1)
                        return true;
                }

            return false;
        }

        // Generates and checks matrix for conflict assignments
        public bool CheckConflict(int size, int connectivity)
        {
            GenerateMarix(size, connectivity);
            RemoveNeccesary();
            MakeZeroMatrixRecursive(0);
            return CheckConflict();
        }
    }
}
