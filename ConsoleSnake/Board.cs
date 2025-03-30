using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSnake
{
    public class Board
    {
        private string[, ] _board;
        private int _length;
        private int _width;
        private List<int[]> _snake;
        private SnakeMovementDirection _snakeMovementDirection;
        public bool GameOver { get; set; }
        private string _empty = "_";
        private string _snakeBody = "#";
        private string _apple = "@";

        public Board(int length = 14, int width = 14)
        {
            _length = length;
            _width = width;
            _board = new string[_length, _width];
            InitBoard();
            _snake = InitSnake();
            _snakeMovementDirection = SnakeMovementDirection.Right;
            GameOver = false;
        }

        private List<int[]> InitSnake()
        {
            var snake = new List<int[]>
            {
                new[] { 0, 0 },
                new[] { 0, 1 },
                new[] { 0, 2 },
                new[] { 0, 4 },
                new[] { 0, 5 },
                new[] { 0, 6 },
            };

            return snake;
        }

        private void InitBoard()
        {
            for (int i = 0; i < _length; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    _board[i, j] = _empty;
                }
            }
        }

        private SnakeMovementDirection GetSnakeMovementDirection(ConsoleKey consoleKey)
        {
            return consoleKey switch
            {
                ConsoleKey.DownArrow => _snakeMovementDirection = SnakeMovementDirection.Down,
                ConsoleKey.UpArrow => _snakeMovementDirection = SnakeMovementDirection.Up,
                ConsoleKey.LeftArrow => _snakeMovementDirection = SnakeMovementDirection.Left,
                ConsoleKey.RightArrow => _snakeMovementDirection = SnakeMovementDirection.Right,
                _ => _snakeMovementDirection,
            };
        }

        public void UpdateSnakeMovementDirection(ConsoleKeyInfo consoleKeyInfo)
        {
            _snakeMovementDirection = GetSnakeMovementDirection(consoleKeyInfo.Key);
        }

        private int[] GetNewSnakeElement(SnakeMovementDirection snakeMovementDirection)
        {
            var lastSnakeElement = new[] { _snake[_snake.Count - 1][0], _snake[_snake.Count - 1][1] };

            return snakeMovementDirection switch
            {
                SnakeMovementDirection.Down => new[] { lastSnakeElement[0] + 1, lastSnakeElement[1] },
                SnakeMovementDirection.Up => new[] { lastSnakeElement[0] - 1, lastSnakeElement[1] },
                SnakeMovementDirection.Left => new[] { lastSnakeElement[0], lastSnakeElement[1] - 1 },
                SnakeMovementDirection.Right => new[] { lastSnakeElement[0], lastSnakeElement[1] + 1 },
                _ => throw new Exception(),
            };
        }

        public void Update()
        {
            _snake.Add(GetNewSnakeElement(_snakeMovementDirection));
            _board[_snake[0][0], _snake[0][1]] = _empty;
            _snake.RemoveAt(0);

            foreach (var s in _snake)
            {
                try
                {
                    _board[s[0], s[1]] = _snakeBody;
                }
                catch(Exception ex)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("======GAME OVER======");
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();

                    GameOver = true;
                }
            }
        }

        public void Print()
        {
            Console.SetCursorPosition(Console.WindowLeft, Console.WindowTop);

            Console.WriteLine("USE ARROWS TO CHANGE DIRECTION");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("DON'T CROSS THE BORDER");
            Console.ResetColor();

            for (int i = 0; i < _length; i++)
            {
                Console.Write("\n");

                for (int j = 0; j < _width; j++)
                {
                    Console.Write(_board[i, j] + " ");
                }
            }
        }
    }
}
