using SnakeGame.Models;

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
        }
    }

    public List<List<CellVm>> AllCells { get; } = new List<List<CellVm>>();

    public DelegateCommand StartStopCommand { get; }
    private MoveDirection _currentMoveDirection = MoveDirection.Right;

    private int _rowCount = 10;
    private int _columnCount = 10;

    public MainWndVM()
    {
        StartStopCommand = new DelegateCommand(() => ContinueGame = !ContinueGame);

        for (int row = 0; row < _rowCount; row++)
        {
            var rowList = new List<CellVm>();
            for (int col = 0; col < _columnCount; col++)
            {
                var cell = new CellVm(row, col);
            }

            AllCells.Add(rowList);
        }
    }

}
