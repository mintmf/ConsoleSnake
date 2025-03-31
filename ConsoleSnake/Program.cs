using ConsoleSnake;

Console.CursorVisible = false;

var board = new Board(12, 12);

board.PrintBoard();

int k = 0;

while (true)
{
    if (Console.KeyAvailable)
    {
        var key = Console.ReadKey();

        board.UpdateSnakeMovementDirection(key);
    }

    board.Update();
    if (board.GameOver) break;

    await Task.Delay(190 - k);

    // k++;
}
