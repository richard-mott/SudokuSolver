using System.Collections.Generic;
using Assisticant.Collections;
using Assisticant.Fields;

namespace SudokuSolver.Models
{
    public class Cell
    {
        private readonly Observable<int> _finalValue = new Observable<int>();
        private readonly ObservableList<int> _possibleValues = new ObservableList<int>();

        public int Hash { get; set; }

        public int FinalValue
        {
            get
            {
                return _finalValue.Value;
            }
            set
            {
                _finalValue.Value = value;
                _possibleValues.Clear();
            }
        }

        public IList<int> PossibleValues
        {
            get { return _possibleValues; }
        }

        public int Row { get; private set; }
        public int Column { get; private set; }

        public Cell(int row, int column)
        {
            Row = row;
            Column = column;

            for (var number = 1; number < 10; number++)
                _possibleValues.Add(number);
        }

        public void RemoveValue(int value)
        {
            if (PossibleValues.Contains(value))
                PossibleValues.Remove(value);

            if (PossibleValues.Count == 1)
            {
                FinalValue = PossibleValues[0];
            }
        }

        public void Reset()
        {
            FinalValue = 0;
            PossibleValues.Clear();

            for (int i = 1; i < 10; i++)
                PossibleValues.Add(i);
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            var that = obj as Cell;

            if (that == null)
                return false;

            return Equals(Row, that.Row) && Equals(Column, that.Column);
        }

        public override int GetHashCode()
        {
            return Hash;
        }
    }
}
