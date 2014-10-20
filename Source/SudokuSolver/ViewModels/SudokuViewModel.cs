using System.Windows.Input;
using Assisticant;
using SudokuSolver.Models;

namespace SudokuSolver.ViewModels
{
    public class SudokuViewModel
    {
        private readonly Sudoku _sudoku;

        public SudokuViewModel(Sudoku sudoku)
        {
            _sudoku = sudoku;

        }

        public BlockViewModel TopLeft
        {
            get { return new BlockViewModel(_sudoku, 0, 0); }
        }

        public BlockViewModel TopMiddle
        {
            get { return new BlockViewModel(_sudoku, 0, 3); }
        }

        public BlockViewModel TopRight
        {
            get { return new BlockViewModel(_sudoku, 0, 6); }
        }

        public BlockViewModel MiddleLeft
        {
            get { return new BlockViewModel(_sudoku, 3, 0); }
        }

        public BlockViewModel Middle
        {
            get { return new BlockViewModel(_sudoku, 3, 3); }
        }

        public BlockViewModel MiddleRight
        {
            get { return new BlockViewModel(_sudoku, 3, 6); }
        }

        public BlockViewModel BottomLeft
        {
            get { return new BlockViewModel(_sudoku, 6, 0); }
        }

        public BlockViewModel BottomMiddle
        {
            get { return new BlockViewModel(_sudoku, 6, 3); }
        }

        public BlockViewModel BottomRight
        {
            get { return new BlockViewModel(_sudoku, 6, 6); }
        }

        public ICommand Solve
        {
            get { return MakeCommand.Do(() => _sudoku.Solve()); }
        }
    }
}
