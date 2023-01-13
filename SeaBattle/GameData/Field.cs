using System.Text;

namespace SeaBattle;

public class Field
{
    private Cell[,] _field = new Cell[10, 10];
    public string Name;
    private List<string> _letters = new List<string> { "а", "б", "в", "г", "д", "е", "ж", "з", "и", "к" };

    public Field(string name)
    {
        Name = name;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
                _field[i, j] = Cell.Empty;
        }
    }

    private string GetStringCell(Cell cell) => cell switch
    {
        Cell.Empty => " ",
        Cell.Ship => "+",
        Cell.Miss => "*",
        Cell.Hit => "x",
        Cell.Dead => "X",
        _ => " "
    };

    public void Shoot(string id)
    {
        int x = _letters.IndexOf(id[0].ToString());
        int y = Convert.ToInt32(id.Substring(1)) - 1;
        if (_field[x, y].Equals(Cell.Ship))
        {
            _field[x, y] = Cell.Hit;
            CellInfo hitCellInfo = CheckShipPosition(1, y, x, Cell.Hit);
            if (CheckShipPosition(1, y, x, Cell.Ship).IsEmpty && !hitCellInfo.IsEmpty)
            {
                do
                {
                    _field[hitCellInfo.X, hitCellInfo.Y] = Cell.Dead;
                    hitCellInfo = CheckShipPosition(1, hitCellInfo.X, hitCellInfo.Y, Cell.Hit);
                } while (!hitCellInfo.IsEmpty);
                
                _field[x, y] = Cell.Dead;
            }
        }
        
        if (_field[x, y].Equals(Cell.Empty))
            _field[x, y] = Cell.Miss;

    }

    public bool SetShip(string id, int size)
    {
        
        int x = _letters.IndexOf(id[0].ToString());
        int y = Convert.ToInt32(id.Substring(1)) - 1;
        bool isVertical = id.Substring(id.Length - 2).Equals("В");
        if (size < 1) size = 1;
        if (x < 0 || y < 0) return false;
        if (!CheckShipPosition(size, y, x, Cell.Ship).IsEmpty) return false;


        try
        {
            if (isVertical)
                for (int i = x; i < x + size; i++)
                    _field[i, y] = Cell.Ship;
            else 
                for (int i = y; i < y + size; i++)
                    _field[x, i] = Cell.Ship;

        }
        catch (Exception e) { return false; }
        

            
        return true;
    }

    private CellInfo CheckShipPosition(int size, int y, int x, Cell checkCell)
    {
        // снизу сверху по много клеток
        for (int i = y - 1; i <= y + size; i++)
        {
            if(i<0) continue;
            if (x >= 1 && _field[x - 1, i].Equals(checkCell)) return new CellInfo(x - 1, i, false);
            if (x <= 8 && _field[x + 1, i].Equals(checkCell)) return new CellInfo(x + 1, i, false);
        }


        // слева справа 1 клетка
        if(y>= 1 && _field[x, y-1].Equals(checkCell)) return new CellInfo(x, y-1, false);
        if(y<= 9-size && _field[x, y + size].Equals(checkCell)) return new CellInfo(x, y+size, false);

        return new CellInfo(-1, -1, true);
    }

    public bool GetShips()
    {
        foreach (var cell in _field)
            if (cell.Equals(Cell.Ship))
                return true;
        return false;
    }

    public string GetFieldToEnemy(bool isEnemy)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("  | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10");
        for (int i = 0; i < 10; i++)
        {
            stringBuilder.Append(_letters[i] + " ");
            for (int j = 0; j < 10; j++)
            {
                string res = GetStringCell(_field[i, j]);
                if (isEnemy && _field[i, j].Equals(Cell.Ship))
                    res = " ";
                stringBuilder.Append($"| {res} ");
            }
            stringBuilder.Append("\n");
        }
        return stringBuilder.ToString();
    }
}