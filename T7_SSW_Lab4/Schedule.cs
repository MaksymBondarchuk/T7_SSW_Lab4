using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;

namespace T7_SSW_Lab4
{
    public class Schedule
    {
        public List<List<short>> Matrix;
        private readonly Random _rand = new Random();

        public void GenerateMarix(int size, int connectivity)
        {
            Matrix = new List<List<short>>();

            for (var i = 0; i < size; i++)
            {
                Matrix.Add(new List<short>(size));
                for (var j = 0; j < size; j++)
                    Matrix[i].Add(0);
            }

            for (var i = 0; i < connectivity; i++)
            {
                while (true)
                {
                    var x = _rand.Next(0, size);
                    var y = _rand.Next(0, size);
                    if (Matrix[x][y] != 0) continue;
                    Matrix[x][y] = 1;
                    break;
                }
            }
        }

        private void RemoveRowCol(int row, int col)
        {
            Matrix.RemoveAt(row);
            foreach (var r in Matrix)
                r.RemoveAt(col);
        }

        private void RemoveNeccesary()
        {
            for (var i = 0; i < Matrix.Count; i++)
                for (var j = 0; j < Matrix.Count; j++)
                    if (Matrix[i][j] == 1)
                    {
                        var countZerosCol = Matrix.Count(t => t[j] == 0);
                        if (countZerosCol != Matrix.Count - 1) continue;
                        {
                            var countZerosRow = Matrix.Where((t, j1) => Matrix[i][j1] == 0).Count();
                            if (countZerosRow != Matrix.Count - 1) continue;
                            RemoveRowCol(i, j);
                            RemoveNeccesary();
                            return;
                        }
                    }
        }

        private void SwapRows(int row1, int row2)
        {
            var buf = Matrix[row1];
            Matrix[row1] = Matrix[row2];
            Matrix[row2] = buf;
        }

        private void SwapCols(int col1, int col2)
        {
            foreach (var t in Matrix)
            {
                var tmp = t[col1];
                t[col1] = t[col2];
                t[col2] = tmp;
            }
        }

        private string MatrixToString()
        {
            var s = "";
            foreach (var row in Matrix)
            {
                for (var j = 0; j < Matrix.Count; j++)
                    s += $"{row[j],4}";
                s += "\n";
            }
            return s;
        }

        private async void MakeZeroMatrixRecursive(int offset)
        {
            if (offset == Matrix.Count)
                return;

            var imin = 0;
            var min = Matrix.Count;
            for (var i = offset; i < Matrix.Count; i++)
            {
                //var currentOnes = Matrix.Where((t, j) => Matrix[i][j] == 1).Count();
                var currentOnes = 0;
                for (var j = offset; j < Matrix.Count; j++)
                    if (Matrix[i][j] == 1)
                        currentOnes++;

                if (currentOnes >= min) continue;
                min = currentOnes;
                imin = i;
            }

            var s = MatrixToString();

            if (offset < imin)
                SwapRows(offset, imin);

            var s1 = MatrixToString();

            var dialog = new MessageDialog(s + $"{offset,4} min row - {imin,4}\n" + s1);
            //var dialog = new MessageDialog($"{offset,4} min row - {imin, 4}");
            await dialog.ShowAsync();


            var maxInitialised = false;
            var jmax = 0;
            var max = 0;
            for (var j = 0; j < Matrix.Count; j++)
                if (Matrix[offset][j] == 1)
                {
                    if (!maxInitialised)
                    {
                        jmax = j;
                        maxInitialised = true;
                    }

                    //var countOnes = Matrix.Count(t => t[j] == 1);
                    var countOnes = 0;
                    for (var i = offset; i < Matrix.Count; i++)
                        if (Matrix[i][j] == 1)
                            countOnes++;


                    if (max >= countOnes) continue;
                    max = countOnes;
                    jmax = j;
                }

            var s2 = MatrixToString();

            //var dialog1 = new MessageDialog($"{offset,4} max col - {jmax,4}");
            //dialog1.ShowAsync();

            if (offset < jmax)
                SwapCols(offset, jmax);

            var s3 = MatrixToString();
            var dialog1 = new MessageDialog(s2 + $"{offset,4} max col - {jmax,4}\n" + s3);
            //var dialog = new MessageDialog($"{offset,4} min row - {imin, 4}");
            await dialog1.ShowAsync();

            //await Task.Delay(100000);

            // ReSharper disable once TailRecursiveCall
            MakeZeroMatrixRecursive(offset + 1);
        }

        public void TransformMatrix()
        {
            // Look for nessesary solutions
            RemoveNeccesary();


            MakeZeroMatrixRecursive(0);

        }

        //private bool CheckForConflicts()
        //{
        //    return false;
        //}
    }
}

