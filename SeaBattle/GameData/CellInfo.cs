namespace SeaBattle;

public class CellInfo
{
    public CellInfo(int x, int y, bool isEmpty)
    {
        X = x;
        Y = y;
        IsEmpty = isEmpty;
    }
    public int X { get; }
    public int Y { get; }
    public Boolean IsEmpty { get; }
}