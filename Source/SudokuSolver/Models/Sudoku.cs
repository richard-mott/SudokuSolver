using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Models
{
    public class Sudoku
    {
        public List<Cell> Cells { get; private set; }

        public Sudoku()
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

        public void Solve()
        {
            while (Cells.Any(cell => cell.FinalValue == 0))
            {
                for (var row = 0; row < 9; row++)
                {
                    var cells = GetCellsByRow(row);
                    UpdateCells(cells);
                }

                for (var column = 0; column < 9; column++)
                {
                    var cells = GetCellsByColumn(column);
                    UpdateCells(cells);
                }

                for (var row = 0; row < 9; row += 3)
                {
                    for (var column = 0; column < 9; column += 3)
                    {
                        var cells = GetCellsByBlock(row, column);
                        UpdateCells(cells);
                    }
                }
            }
        }

        private void UpdateCells(IEnumerable<Cell> cells)
        {
            var finalValues = GetFinalValues(cells);

            foreach (var cell in cells)
            {
                foreach (var finalValue in finalValues)
                {
                    cell.RemoveValue(finalValue);
                }
            }
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

    }
}
