using SnakeGame.ViewModels;

namespace SnakeGame.Models;
internal class Snake
{
    public Queue<CellVm> SnakeCells { get; } = new Queue<CellVm>();

    private List<List<CellVm>> _allCells;
    private CellVm _start;
    private Action _generateFood;

    public Snake(List<List<CellVm>> allCells, CellVm start, Action generateFood)
    {
        _start = start;
        _allCells = allCells;
        _start.CellType = CellType.Snake;
        SnakeCells.Enqueue(start);
        _generateFood = generateFood;
    }

    public void Restart()
    {
        foreach (var cell in SnakeCells)
        {
            cell.CellType = CellType.None;
        }

        SnakeCells.Clear();
        _start.CellType = CellType.Snake;
        SnakeCells.Enqueue(_start);
    }

    public void Move(MoveDirection direction)
    {
        var leader = SnakeCells.Last();

        var nextRow = leader.Row;
        var nextCol = leader.Column;

        switch (direction)
        {
            case MoveDirection.Up:
                nextRow--;
                break;
            case MoveDirection.Down:
                nextRow++;
                break;
            case MoveDirection.Left:
                nextCol--;
                break;
            case MoveDirection.Right:
                nextCol++;
                break;
            default:
                break;
        }

        try
        {
            var nextCell = _allCells[nextRow][nextCol];
            switch (nextCell.CellType)
            {
                case CellType.None:
                    nextCell.CellType = CellType.Snake;
                    SnakeCells.Dequeue().CellType = CellType.None;
                    SnakeCells.Enqueue(nextCell);
                    break;
                case CellType.Food:
                    nextCell.CellType = CellType.Snake;
                    SnakeCells.Enqueue(nextCell);
                    _generateFood?.Invoke();
                    break;
                default:
                    throw _gameOverEx;
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            throw _gameOverEx;
        }
    }

    private Exception _gameOverEx => throw new Exception("Game over");
}
