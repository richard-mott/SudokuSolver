using System;
using SudokuSolver.Models;

namespace SudokuSolver.ViewModels
{
    public class CellViewModel
    {
        private readonly Cell _cell;
        private readonly Sudoku _sudoku;

        public string FinalValue
        {
            get 
            {
                return _cell.FinalValue == 0 
                    ? "  " 
                    : _cell.FinalValue.ToString();
            }
            set
            {
                int val;

                _cell.FinalValue = int.TryParse(value, out val) 
                    ? val 
                    : 0;
            }
        }

        public string PossibleValues
        {
            get
            {
                var result = "";

                if (_cell.PossibleValues.Count == 0)
                    return result;

                for (var i = 0; i < _cell.PossibleValues.Count - 1; i++)
                {
                    result += _cell.PossibleValues[i] + ",";
                    if ((i + 1) % 2 == 0)
                        result += Environment.NewLine;
                    else
                        result += " ";
                }

                result += _cell.PossibleValues[_cell.PossibleValues.Count - 1];
                return result;
            }
        }

        public CellViewModel(Sudoku sudoku, Cell cell)
        {
            _sudoku = sudoku;
            _cell = cell;
        }
    }
}
