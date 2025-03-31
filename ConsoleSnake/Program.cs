using ConsoleSnake;

Console.CursorVisible = false;
Console.SetWindowSize(60, 20);

var board = new Board(13, 13);

board.PrintBoard();

while (true)
{
    if (Console.KeyAvailable)
    {
        var key = Console.ReadKey();

        board.UpdateSnakeMovementDirection(key);
    }

    board.Update();

    if (board.GameOver)
    {
        Console.ReadKey();
        // await Task.Delay(3000);

        Console.Clear();
        board = new Board(13, 13);
        board.PrintBoard();
    }

    await Task.Delay(190);
}
