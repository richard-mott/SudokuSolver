using Assisticant;
using SudokuSolver.Models;

namespace SudokuSolver.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private readonly Sudoku _sudoku;

        public object SudokuViewModel
        {
            get
            {
                return ViewModel(() =>
                    new SudokuViewModel(_sudoku));
            }
        }

        public ViewModelLocator()
        {
            _sudoku = new Sudoku();
        }
    }
}
