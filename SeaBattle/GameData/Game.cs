namespace SeaBattle;

public class Game
{
    private Field _field1;
    private Field _field2;

    public Game()
    {
        _field1 = new Field("Игрок 1");
        _field2 = new Field("Игрок 2");
    }

    public void Start()
    {
        SetShip(ref _field1);
        Console.WriteLine("Нажмите любую клавишу для передачи хода");
        Console.ReadKey();
        
        SetShip(ref _field2);
        Thread.Sleep(100);
        for (int i = 2; i < 102; i++)
        {
            ref Field currentField = ref _field1;
            ref Field enemyField = ref _field2;
            if (i % 2 == 1)
            {
                currentField = ref _field2;
                enemyField = ref _field1;
            }

            WriteFields(ref currentField, ref enemyField);

            Console.WriteLine("Ход игрока " + (i % 2 == 0 ? "1" : "2"));
            Console.WriteLine("Выберите точку для удара");
            enemyField.Shoot(Console.ReadLine());
            Thread.Sleep(500);
            Console.Clear();

            WriteFields(ref currentField, ref enemyField);

            if (!enemyField.GetShips())
            {
                Console.WriteLine(currentField.Name + " победил!");
                return;
            }

            Console.WriteLine("Нажмите любую клавишу для передачи хода");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private void WriteFields(ref Field currentField, ref Field enemyField)
    {
        Console.WriteLine("Ваше поле ");
        Console.WriteLine(currentField.GetFieldToEnemy(false));
        Console.WriteLine("Поле врага ");
        Console.WriteLine(enemyField.GetFieldToEnemy(false));
        Console.WriteLine();
    }


    private void SetShip(ref Field field)
    {
        void SetShipMessage(Field field, string name)
        {
            Console.WriteLine("Ваше поле ");
            Console.WriteLine(field.GetFieldToEnemy(false));
            Console.WriteLine();
            Console.WriteLine(field.Name + " ставит корабли");
            Console.WriteLine($"Напишите точку для {name} корабля в виде \"а1 В\" (в - вертикал, оставьте пустым если хотите поставить горизонтальный корабль)");
        }
        Console.Clear();
        List<string> ships = new List<string>
            { "однопалубного", "двупалобного", "трёх-палобного", "четырёх-палобного" };
        for (int i = 1; i < 4; i++)
        {
            SetShipMessage(field, ships[i]);
            for (int j = 1; j <= 4 - i; j++)
            {
                bool isError;
                do
                {
                    isError = !field.SetShip( Console.ReadLine(), i+1);
                    if (!isError)
                    {
                        Console.Clear();
                        SetShipMessage(field, ships[i]);
                    }
                    else Console.WriteLine("В этой точке нельзя поставить корабль!");
                } 
                while (isError);
            }
            Console.Clear();
        }
    }
}