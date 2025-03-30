using ConsoleSnake;

var board = new Board();

while (true)
{
    if (Console.KeyAvailable)
    {
        var key = Console.ReadKey();

        board.UpdateSnakeMovementDirection(key);
    }

    board.Update();
    if (board.GameOver) break;

    board.Print();

    await Task.Delay(150);
}
