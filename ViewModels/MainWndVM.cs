using SnakeGame.Models;
using System.Windows;
using System.Windows.Input;

namespace SnakeGame.ViewModels;
internal class MainWndVM : BindableBase
{
    private bool _continueGame;

    public bool ContinueGame
    {
        get => _continueGame;
        private set
        {
            _continueGame = value;
            RaisePropertyChanged(nameof(ContinueGame));

            if (_continueGame)
            {
                SnakeGo();
            }
        }
    }

    public List<List<CellVm>> AllCells { get; } = new List<List<CellVm>>();

    public DelegateCommand StartStopCommand { get; }
    private MoveDirection _currentMoveDirection = MoveDirection.Right;

    private int _rowCount = 10;
    private int _columnCount = 10;
    private const int SNAKESPEED = 400;
    private int _snakeSpeed = SNAKESPEED;

    private MainWindow _mainWindow;
    private Snake _snake;
    private CellVm _lastFood;

    public MainWndVM(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
        StartStopCommand = new DelegateCommand(() => ContinueGame = !ContinueGame);

        for (int row = 0; row < _rowCount; row++)
        {
            var rowList = new List<CellVm>();
            for (int col = 0; col < _columnCount; col++)
            {
                var cell = new CellVm(row, col);
                rowList.Add(cell);
            }

            AllCells.Add(rowList);
        }

        _snake = new Snake(AllCells, AllCells[_rowCount / 2][_columnCount / 2], CreateFood);
        CreateFood();

        _mainWindow.KeyDown += UserKeyDown;
    }

    private async Task SnakeGo()
    {
        while (ContinueGame)
        {
            await Task.Delay(_snakeSpeed);

            try
            {
                _snake.Move(_currentMoveDirection);

            }
            catch (Exception ex)
            {
                ContinueGame = false;
                MessageBox.Show(ex.Message);
                _snakeSpeed = SNAKESPEED;
                _snake.Restart();
                _lastFood.CellType = CellType.None;
                CreateFood();
            }
        }
    }

    private void UserKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.A:
                if (_currentMoveDirection != MoveDirection.Right)
                    _currentMoveDirection = MoveDirection.Left;
                break;
            case Key.S:
                if (_currentMoveDirection != MoveDirection.Up)
                    _currentMoveDirection = MoveDirection.Down;
                break;
            case Key.D:
                if (_currentMoveDirection != MoveDirection.Left)
                    _currentMoveDirection = MoveDirection.Right;
                break;
            case Key.W:
                if (_currentMoveDirection != MoveDirection.Down)
                    _currentMoveDirection = MoveDirection.Up;
                break;
            case Key.Left:
                if (_currentMoveDirection != MoveDirection.Right)
                    _currentMoveDirection = MoveDirection.Left;
                break;
            case Key.Right:
                if (_currentMoveDirection != MoveDirection.Left)
                    _currentMoveDirection = MoveDirection.Right;
                break;
            case Key.Up:
                if (_currentMoveDirection != MoveDirection.Down)
                    _currentMoveDirection = MoveDirection.Up;
                break;
            case Key.Down:
                if (_currentMoveDirection != MoveDirection.Up)
                    _currentMoveDirection = MoveDirection.Down;
                break;
            default:
                break;
        }
    }

    private void CreateFood()
    {
        var random = new Random();

        var row = random.Next(0, _rowCount);
        var col = random.Next(0, _columnCount);

        _lastFood = AllCells[row][col];

        if (_snake.SnakeCells.Contains(_lastFood))
        {
            CreateFood();
        }

        _lastFood.CellType = CellType.Food;
        _snakeSpeed = (int)(_snakeSpeed * 0.95);
    }
}
