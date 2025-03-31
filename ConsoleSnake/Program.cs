using ConsoleSnake;

Console.CursorVisible = false;

var board = new Board();

board.PrintBoard();

while (true)
{
    if (Console.KeyAvailable)
    {
        var key = Console.ReadKey();

        board.UpdateSnakeMovementDirection(key);
    }

    board.Update();
    if (board.GameOver) break;

    await Task.Delay(200);
}
