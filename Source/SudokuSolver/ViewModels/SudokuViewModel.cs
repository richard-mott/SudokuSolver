﻿using System.Windows.Input;
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

        public string PuzzleStatus
        {
            get
            {
                if (_sudoku.IsUnsolvable)
                    return "Puzzle is unsolvable from this state.";

                return _sudoku.IsSolved 
                    ? "Puzzle solved." 
                    : "Puzzle in progress.";
            }
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

        public ICommand Step
        {
            get { return MakeCommand.Do(() => _sudoku.Step()); }
        }

        public ICommand Reset
        {
            get { return MakeCommand.Do(() => _sudoku.Reset()); }
        }
    }
}
