using System.Linq;
using SudokuSolver.Models;

namespace SudokuSolver.ViewModels
{
    public class BlockViewModel
    {
        private readonly Sudoku _sudoku;
        
        private readonly Cell _topLeft;
        private readonly Cell _topMiddle;
        private readonly Cell _topRight;

        private readonly Cell _middleLeft;
        private readonly Cell _middle;
        private readonly Cell _middleRight;

        private readonly Cell _bottomLeft;
        private readonly Cell _bottomMiddle;
        private readonly Cell _bottomRight;
        
        public CellViewModel[][] Cells { get; private set;}

        public BlockViewModel(Sudoku sudoku, int startingRow, int startingColumn)
        {
            _sudoku = sudoku;

            _topLeft = _sudoku.Cells.First(cell => cell.Row == startingRow && cell.Column == startingColumn);
            _topMiddle = _sudoku.Cells.First(cell => cell.Row == startingRow && cell.Column == startingColumn + 1);
            _topRight = _sudoku.Cells.First(cell => cell.Row == startingRow && cell.Column == startingColumn + 2);

            _middleLeft = _sudoku.Cells.First(cell => cell.Row == startingRow + 1 && cell.Column == startingColumn);
            _middle = _sudoku.Cells.First(cell => cell.Row == startingRow + 1 && cell.Column == startingColumn + 1);
            _middleRight = _sudoku.Cells.First(cell => cell.Row == startingRow + 1 && cell.Column == startingColumn + 2);

            _bottomLeft = _sudoku.Cells.First(cell => cell.Row == startingRow + 2 && cell.Column == startingColumn);
            _bottomMiddle = _sudoku.Cells.First(cell => cell.Row == startingRow + 2 && cell.Column == startingColumn + 1);
            _bottomRight = _sudoku.Cells.First(cell => cell.Row == startingRow + 2 && cell.Column == startingColumn + 2);
        }

        public CellViewModel TopLeft
        {
            get { return new CellViewModel(_topLeft); }
        }

        public CellViewModel TopMiddle
        {
            get { return new CellViewModel(_topMiddle); }
        }

        public CellViewModel TopRight
        {
            get { return new CellViewModel(_topRight); }
        }

        public CellViewModel MiddleLeft
        {
            get { return new CellViewModel(_middleLeft); }
        }

        public CellViewModel Middle
        {
            get { return new CellViewModel(_middle); }
        }

        public CellViewModel MiddleRight
        {
            get { return new CellViewModel(_middleRight); }
        }
        
        public CellViewModel BottomLeft
        {
            get { return new CellViewModel(_bottomLeft); }
        }

        public CellViewModel BottomMiddle
        {
            get { return new CellViewModel(_bottomMiddle); }
        }

        public CellViewModel BottomRight
        {
            get { return new CellViewModel(_bottomRight); }
        }
    }
}
