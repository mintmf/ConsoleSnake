using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSnake
{
    public class Board
    {
        private readonly string[, ] _board;
        private readonly int _length;
        private readonly int _width;
        private List<int[]> _snake;
        private SnakeMovementDirection _snakeMovementDirection;
        public bool GameOver { get; set; }
        private string _grass = "_";
        private readonly string _border = "X";
        private string _snakeBody = "#";
        private string _apple = "@";
        private int[] _applePostion;
        private bool _appleEaten = false;

        public Board(int length = 14, int width = 14)
        {
            _length = length;
            _width = width;
            _board = new string[_length, _width];
            InitBoard();
            _snake = InitSnake();
            _snakeMovementDirection = SnakeMovementDirection.Right;
            GameOver = false;
            AddApple();
        }

        private void AddApple()
        {
            _appleEaten = false;
            _applePostion = new[] { new Random().Next(14), new Random().Next(14) };
            _board[_applePostion[0], _applePostion[1]] = _apple;
        }

        private List<int[]> InitSnake()
        {
            var snake = new List<int[]>
            {
                new[] { 0, 0 },
                new[] { 0, 1 },
                new[] { 0, 2 },
                new[] { 0, 4 },
            };

            return snake;
        }

        private void InitBoard()
        {
            for (int i = 0; i < _length; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    _board[i, j] = _grass;
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

        private void MoveSnake(int[] newSnakeElement)
        {
            if (newSnakeElement[0] == _applePostion[0] && newSnakeElement[1] == _applePostion[1])
            {
                _appleEaten = true;
            }

            foreach (var s in _snake)
            {
                if (s[0] == newSnakeElement[0] && s[1] == newSnakeElement[1])
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("======GAME OVER======");
                    Console.ResetColor();

                    GameOver = true;

                    return;
                }
            }

            _snake.Add(newSnakeElement);
            _board[_snake[0][0], _snake[0][1]] = _grass;
            if (!_appleEaten) _snake.RemoveAt(0);

            foreach (var s in _snake)
            {
                try
                {
                    _board[s[0], s[1]] = _snakeBody;
                }
                catch (Exception ex)
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

        public void Update()
        {
            var newSnakeElement = GetNewSnakeElement(_snakeMovementDirection);

            MoveSnake(newSnakeElement);

            if (_appleEaten)
            {
                AddApple();
            }
        }

        public void Print()
        {
            Console.SetCursorPosition(Console.WindowLeft, Console.WindowTop);

            Console.WriteLine("USE ARROWS TO CHANGE DIRECTION");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("DON'T CROSS THE BORDER");
            Console.ResetColor();

            PrintHorizontalBorder();

            for (int i = 0; i < _length; i++)
            {
                PrintSideBorder();

                for (int j = 0; j < _width; j++)
                {
                    Console.Write(_board[i, j] + " ");
                }

                PrintSideBorder();
                Console.Write("\n");
            }

            PrintHorizontalBorder();
        }

        private void PrintSideBorder()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(_border + " ");
            Console.ResetColor();
        }

        private void PrintHorizontalBorder()
        {
            for (int k = 0; k < _length + 2; k++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(_border + " ");
                Console.ResetColor();
            }

            Console.Write("\n");
        }
    }
}
