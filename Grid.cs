using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeftLib
{
    // TODO: Implement IEnumerable
    public class Grid<T> : IEnumerable<T>
    {
        private List<List<T>> _cells;
        private int _cols;
        private int _rows;

        protected List<List<T>> Cells { get => _cells; }
        public int Cols { get => _cols; protected set => _cols = value; }
        public int Rows { get => _rows; protected set => _rows = value; }

        public T this[int col, int row]
        {
            get
            {
                if (col < 0 || col > Cols - 1 || row < 0 || row > Rows - 1)
                    return default(T);
                else
                    return _cells[col][row];
            }

            set
            {
                if (col < 0 || col > Cols - 1 || row < 0 || row > Rows - 1)
                    return;
                _cells[col][row] = value;
            }
        }

        public Grid()
        {
            _cells = new List<List<T>>();
            _cols = 0;
            _rows = 0;
        }

        public void AddColumn(List<T> newColumn)
        {
            _cells.Add(newColumn);
        }

        public void AddRow(List<T> newRow)
        {
            for (int col = 0; col < Cols; ++col)
                _cells[col].Add(newRow[col]);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int col = 0; col < Cols; ++col)
            {
                for (int row = 0; row < Rows; ++row)
                    yield return this[col, row];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
