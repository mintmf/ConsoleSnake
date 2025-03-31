using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSnake
{
    public class Board
    {
        private readonly string _grass = "W";
        private readonly string _border = "X";
        private readonly string _snakeBody = "#";
        private readonly string _apple = "@";

        private readonly string[, ] _board;
        private readonly int _length;
        private readonly int _width;
        private List<int[]> snake;
        private SnakeMovementDirection snakeMovementDirection;
        private int[] applePostion;
        private bool appleEaten = false;
        public bool GameOver { get; set; }

        public Board(int length = 14, int width = 14)
        {
            _length = length;
            _width = width;
            _board = new string[_length, _width];
            InitBoard();
            snake = InitSnake();
            snakeMovementDirection = SnakeMovementDirection.Right;
            GameOver = false;
            AddApple();
        }

        private void AddApple()
        {
            appleEaten = false;
            applePostion = new[] { new Random().Next(14), new Random().Next(14) };
            _board[applePostion[0], applePostion[1]] = _apple;
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
                ConsoleKey.DownArrow => snakeMovementDirection = SnakeMovementDirection.Down,
                ConsoleKey.UpArrow => snakeMovementDirection = SnakeMovementDirection.Up,
                ConsoleKey.LeftArrow => snakeMovementDirection = SnakeMovementDirection.Left,
                ConsoleKey.RightArrow => snakeMovementDirection = SnakeMovementDirection.Right,
                _ => snakeMovementDirection,
            };
        }

        public void UpdateSnakeMovementDirection(ConsoleKeyInfo consoleKeyInfo)
        {
            snakeMovementDirection = GetSnakeMovementDirection(consoleKeyInfo.Key);
        }

        private int[] GetNewSnakeElement(SnakeMovementDirection snakeMovementDirection)
        {
            var lastSnakeElement = new[] { snake[snake.Count - 1][0], snake[snake.Count - 1][1] };

            return snakeMovementDirection switch
            {
                SnakeMovementDirection.Down => new[] { lastSnakeElement[0] + 1, lastSnakeElement[1] },
                SnakeMovementDirection.Up => new[] { lastSnakeElement[0] - 1, lastSnakeElement[1] },
                SnakeMovementDirection.Left => new[] { lastSnakeElement[0], lastSnakeElement[1] - 1 },
                SnakeMovementDirection.Right => new[] { lastSnakeElement[0], lastSnakeElement[1] + 1 },
                _ => throw new Exception(),
            };
        }

        private void MoveSnake()
        {
            var newSnakeElement = GetNewSnakeElement(snakeMovementDirection);

            if (newSnakeElement[0] == applePostion[0] && newSnakeElement[1] == applePostion[1])
            {
                appleEaten = true;
            }

            foreach (var s in snake)
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

            snake.Add(newSnakeElement);
            _board[snake[0][0], snake[0][1]] = _grass;
            if (!appleEaten) snake.RemoveAt(0);

            foreach (var s in snake)
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
            MoveSnake();

            if (appleEaten)
            {
                AddApple();
            }
        }

        public void Print()
        {
            Console.SetCursorPosition(Console.WindowLeft, Console.WindowTop);

            Console.WriteLine("USE ARROWS TO CHANGE DIRECTION");

            PrintHorizontalBorder();

            for (int i = 0; i < _length; i++)
            {
                PrintSideBorder();

                for (int j = 0; j < _width; j++)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;

                    if (i == applePostion[0] && j == applePostion[1])
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        foreach (var s in snake)
                        {
                            if (s[0] == i && s[1] == j)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;

                                break;
                            }
                        }
                    }

                    Console.Write(_board[i, j] + " ");
                    Console.ResetColor();
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
