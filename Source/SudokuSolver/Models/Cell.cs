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
        public bool HasChanged { get; private set; }

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

                if (_finalValue.Value == 0)
                { 
                    for (var i = 1; i < 10; i++)
                        _possibleValues.Add(i);
                }
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

        public bool RemoveValue(int value)
        {
            var changed = false;

            if (PossibleValues.Contains(value))
            {
                PossibleValues.Remove(value);
                changed = true;
            }
            
            return changed;
        }

        public void Reset()
        {
            FinalValue = 0;
            PossibleValues.Clear();

            for (int i = 1; i < 10; i++)
                PossibleValues.Add(i);
        }

        public Cell Clone()
        {
            var copy = new Cell(Row, Column)
            {
                FinalValue = FinalValue,
                Hash = Hash
            };

            copy.PossibleValues.Clear();
            foreach (var value in PossibleValues)
                copy.PossibleValues.Add(value);

            return copy;
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
