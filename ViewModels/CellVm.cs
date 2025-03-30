using SnakeGame.Models;

namespace SnakeGame.ViewModels;
internal class CellVm : BindableBase
{
    public int Row { get; }
    public int Column { get; }

    private CellType _cellType = CellType.None;

    public CellType CellType
    {
        get => _cellType;
        set
        {
            _cellType = value;
            RaisePropertyChanged(nameof(CellType));
        }
    }

    public CellVm(int row, int column)
    {
        Row = row;
        Column = column;
    }

}
