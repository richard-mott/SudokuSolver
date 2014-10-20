using System;
using System.Linq;
using SudokuSolver.Models;

namespace SudokuSolver.ViewModels
{
    public class CellViewModel
    {
        private readonly Cell _cell;
        private readonly Sudoku _sudoku;

        public int FinalValue
        {
            get
            {
                return _cell.FinalValue;
            }
            set
            {
                _cell.FinalValue = value;
                _sudoku.Step();
            }
        }

        public string PossibleValues
        {
            get
            {
                return _cell.PossibleValues.Aggregate("", (current, value) => current + (value + " "));
            }
        }

        public CellViewModel(Sudoku sudoku, Cell cell)
        {
            _sudoku = sudoku;
            _cell = cell;
        }
    }
}
