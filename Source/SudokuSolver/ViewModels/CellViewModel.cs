using System;
using System.Linq;
using SudokuSolver.Models;

namespace SudokuSolver.ViewModels
{
    public class CellViewModel
    {
        private readonly Cell _cell;

        public int FinalValue
        {
            get
            {
                return _cell.FinalValue;
            }
            set
            {
                _cell.FinalValue = value;
            }
        }

        public string PossibleValues
        {
            get
            {
                return _cell.PossibleValues.Aggregate("", (current, value) => current + (value + " "));
            }
        }

        public CellViewModel(Cell cell)
        {
            _cell = cell;
        }
    }
}
