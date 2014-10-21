using System;
using System.Collections.Generic;
using System.Linq;
using Assisticant.Fields;

namespace SudokuSolver.Models
{
    public class Sudoku
    {
        private readonly Observable<bool> _isUnsolvable = new Observable<bool>(false);
        private readonly Observable<bool> _isSolved = new Observable<bool>(false);

        public bool IsUnsolvable
        {
            get { return _isUnsolvable; }
        }

        public bool IsSolved
        {
            get { return _isSolved; }
        }

        public List<Cell> Cells { get; private set; }

        public Sudoku()
        {
            Initialize();

            InitializePuzzle();
        }

        public void Solve()
        {
            bool hasPuzzleChanged;

            do
            {
                hasPuzzleChanged = UpdatePuzzle();
            } while (Cells.Any(cell => cell.FinalValue == 0) && hasPuzzleChanged);

            if (Cells.All(cell => cell.FinalValue != 0))
                _isSolved.Value = true;
        }

        public void Step()
        {
            UpdatePuzzle();
            
            if (Cells.All(cell => cell.FinalValue != 0))
                _isSolved.Value = true;
        }

        public void Reset()
        {
            foreach (var cell in Cells)
            {
                cell.Reset();
            }

            InitializePuzzle();

            _isSolved.Value = false;
            _isUnsolvable.Value = false;
        }

        public Sudoku Clone()
        {
            var copy = new Sudoku();

            copy.Cells.Clear();
            foreach (var cell in Cells)
                copy.Cells.Add(cell.Clone());

            return copy;
        }

        private bool UpdatePuzzle()
        {
            var hasRowChanged = UpdateRows();
            var hasColumnChanged = UpdateColumns();
            var hasBlockChanged = UpdateBlocks();

            return hasRowChanged || hasColumnChanged || hasBlockChanged;
        }

        private bool UpdateRows()
        {
            var hasRowChanged = false;

            for (var row = 0; row < 9; row++)
            {
                var cells = GetCellsByRow(row);
                if (UpdateCells(cells))
                    hasRowChanged = true;

                if (CheckForOnlyPossiblePositionInRow(cells))
                    hasRowChanged = true;
            }

            return hasRowChanged;
        }

        private bool UpdateColumns()
        {
            var hasColumnChanged = false;

            for (var column = 0; column < 9; column++)
            {
                var cells = GetCellsByColumn(column);
                if (UpdateCells(cells))
                    hasColumnChanged = true;

                if (CheckForOnlyPossiblePositionInColumn(cells))
                    hasColumnChanged = true;
            }

            return hasColumnChanged;
        }

        private bool UpdateBlocks()
        {
            var hasBlockChanged = false;

            for (var row = 0; row < 9; row += 3)
            {
                for (var column = 0; column < 9; column += 3)
                {
                    var cells = GetCellsByBlock(row, column);
                    if (UpdateCells(cells))
                        hasBlockChanged = true;

                    if (CheckForOnlyPossiblePositionInBlock(cells))
                        hasBlockChanged = true;
                }
            }

            return hasBlockChanged;
        }

        private bool UpdateCells(IEnumerable<Cell> cells)
        {
            var haveRemovedValues = RemoveFinalValues(cells);
            
            return haveRemovedValues;
        }

        private bool CheckForOnlyPossiblePositionInRow(IEnumerable<Cell> cells)
        {
            var hasCellChanged = false;
            var localCells = cells.ToList();

            for (var i = 1; i < 10; i++)
            {
                var possibleCells = localCells.Where(cell => cell.PossibleValues.Contains(i)).ToList();

                if (possibleCells.Count == 1)
                {
                    var cell = possibleCells[0];

                    var blockHasValue = GetCellsByBlock(cell.Row/3, cell.Column/3)
                        .Any(bCell => bCell.FinalValue == i);
                    var columnHasValue = GetCellsByColumn(cell.Column)
                        .Any(c => c.FinalValue == i);
                    
                    if (!blockHasValue && !columnHasValue)
                    {
                        cell.FinalValue = i;
                        hasCellChanged = true;
                    }
                }
            }

            return hasCellChanged;
        }

        private bool CheckForOnlyPossiblePositionInColumn(IEnumerable<Cell> cells)
        {
            var hasCellChanged = false;
            var localCells = cells.ToList();

            for (var i = 1; i < 10; i++)
            {
                var possibleCells = localCells.Where(cell => cell.PossibleValues.Contains(i)).ToList();

                if (possibleCells.Count == 1)
                {
                    var cell = possibleCells[0];

                    var blockHasValue = GetCellsByBlock(cell.Row / 3, cell.Column / 3)
                        .Any(bCell => bCell.FinalValue == i);
                    var rowHasValue = GetCellsByRow(cell.Row)
                        .Any(r => r.FinalValue == i);

                    if (!blockHasValue && !rowHasValue)
                    {
                        cell.FinalValue = i;
                        hasCellChanged = true;
                    }
                }
            }

            return hasCellChanged;
        }

        private bool CheckForOnlyPossiblePositionInBlock(IEnumerable<Cell> cells)
        {
            var hasCellChanged = false;
            var localCells = cells.ToList();

            for (var i = 1; i < 10; i++)
            {
                var possibleCells = localCells.Where(cell => cell.PossibleValues.Contains(i)).ToList();

                if (possibleCells.Count == 1)
                {
                    var cell = possibleCells[0];

                    var columnHasValue = GetCellsByColumn(cell.Column)
                        .Any(c => c.FinalValue == i);
                    var rowHasValue = GetCellsByRow(cell.Row)
                        .Any(r => r.FinalValue == i);

                    if (!columnHasValue && !rowHasValue)
                    {
                        cell.FinalValue = i;
                        hasCellChanged = true;
                    }
                }
            }

            return hasCellChanged;
        }
        private bool RemoveFinalValues(IEnumerable<Cell> cells)
        {
            var finalValues = GetFinalValues(cells);
            var hasCellChanged = false;

            foreach (var cell in cells)
            {
                var changed = false;

                foreach (var finalValue in finalValues)
                {
                    var hasChanged = cell.RemoveValue(finalValue);

                    if (hasChanged)
                        if (cell.PossibleValues.Count == 1)
                            if (finalValues.Contains(cell.PossibleValues[0]))
                                _isUnsolvable.Value = true;
                            else
                                cell.FinalValue = cell.PossibleValues[0];

                    if (!changed && hasChanged)
                        changed = true;
                }

                if (changed)
                    hasCellChanged = true;
            }

            return hasCellChanged;
        }

        private bool CheckForPossibleValuesInRowInBlock(IEnumerable<Cell> cells, int blockColumn)
        {
            var hasFoundValues = false;
            var localCells = cells.ToList();
            for (var i = 1; i < 10; i++)
            {
                var possibleCells = localCells.Where(cell => cell.PossibleValues.Contains(i)).ToList();

                if (possibleCells.Count > 0)
                {
                    var row = possibleCells[0].Row;

                    if (possibleCells.Any(cell => cell.Row != row))
                    {
                        return false;
                    }

                    var rowCells = GetCellsByRow(row);

                    foreach (var cell in rowCells)
                    {
                        if (cell.Column/3 != blockColumn)
                        {
                            cell.PossibleValues.Remove(i);
                            hasFoundValues = true;
                        }
                    }
                }
            }
            return hasFoundValues;
        }

        private bool CheckForPossibleValuesInColumnInBlock(IEnumerable<Cell> cells, int blockRow)
        {
            var hasFoundValues = false;
            var localCells = cells.ToList();
            for (var i = 1; i < 10; i++)
            {
                var possibleCells = localCells.Where(cell => cell.PossibleValues.Contains(i)).ToList();

                if (possibleCells.Count > 0)
                {
                    var column = possibleCells[0].Column;

                    if (possibleCells.Any(cell => cell.Column != column))
                    {
                        return false;
                    }

                    var rowCells = GetCellsByColumn(column);

                    foreach (var cell in rowCells)
                    {
                        if (cell.Column / 3 != blockRow)
                        {
                            cell.PossibleValues.Remove(i);
                            hasFoundValues = true;
                        }
                    }
                }
            }
            return hasFoundValues;
        }

        private IEnumerable<Cell> GetCellsByRow(int row)
        {
            return Cells.Where(cell => cell.Row == row);
        }

        private IEnumerable<Cell> GetCellsByColumn(int column)
        {
            return Cells.Where(cell => cell.Column == column);
        }

        private IEnumerable<Cell> GetCellsByBlock(int row, int column)
        {
            var rowBlock = row / 3;
            var columnBlock = column / 3;

            return Cells.Where(cell => (cell.Row / 3 == rowBlock) && (cell.Column / 3 == columnBlock));
        }

        private IEnumerable<int> GetFinalValues(IEnumerable<Cell> cells)
        {
            var cellsWithValues = cells.Where(cell => cell.FinalValue != 0).ToList();

            return cellsWithValues.Select(cell => cell.FinalValue);
        }

        private void Initialize()
        {
            Cells = new List<Cell>();
            var random = new Random();
            var randomList = new List<int>();

            for (var row = 0; row < 9; row++)
            {
                for (var column = 0; column < 9; column++)
                {
                    var cell = new Cell(row, column);
                    var hash = 0;

                    do
                    {
                        hash = random.Next(int.MaxValue);
                    } while (randomList.Contains(hash));

                    cell.Hash = hash;

                    Cells.Add(cell);
                }
            }
        }

        private void InitializePuzzle()
        {
            var values = new List<int>
            {
                8, 0, 2, 0, 6, 0, 0, 4, 0,
                0, 5, 0, 0, 0, 0, 8, 0, 3,
                0, 0, 0, 0, 0, 5, 7, 0, 0,
                0, 0, 8, 0, 9, 0, 0, 0, 0,
                9, 0, 7, 0, 8, 0, 5, 0, 4,
                0, 0, 0, 0, 1, 0, 6, 0, 0,
                0, 0, 1, 9, 0, 0, 0, 0, 0,
                4, 0, 6, 0, 0, 0, 0, 5, 0,
                0, 8, 0, 0, 7, 0, 4, 0, 2
            };

            for (var i = 0; i < values.Count; i++)
                Cells[i].FinalValue = values[i];
        }
    }
}
