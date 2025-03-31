using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private int[] applePosition;
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
        
        

        private void PrintApple()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(ConsolePositionHelper.GetCorrectLeft(applePosition[1]), ConsolePositionHelper.GetCorrectTop(applePosition[0]));
            Console.Write(_apple);
            Console.ResetColor();
            Console.SetCursorPosition(0, 0);
        }

        private void AddApple()
        {
            appleEaten = false;
            applePosition = GenerateApplePosition();

            PrintApple();

            _board[applePosition[0], applePosition[1]] = _apple;
        }

        private int[] GenerateApplePosition()
        {
            var randomX = new Random().Next(_width);
            var randomY = new Random().Next(_length);

            foreach (var s in snake)
            {
                if (s[0] == randomX && s[1] == randomY)
                {
                    return GenerateApplePosition();
                }
            }

            return new[] {randomX, randomY };
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

        private void UpdateSnake()
        {
            var newSnakeElement = GetNewSnakeElement(snakeMovementDirection);

            if (newSnakeElement[0] == applePosition[0] && newSnakeElement[1] == applePosition[1])
            {
                appleEaten = true;
            }

            if (CheckForSelfEating(newSnakeElement))
            {
                return;
            }

            snake.Add(newSnakeElement);
            _board[snake[0][0], snake[0][1]] = _grass;

            int[] oldSnakeTail = snake[0];

            if (appleEaten)
            {
                MoveSnake();
            }
            else
            {
                snake.RemoveAt(0);
                MoveSnake(oldSnakeTail);
            }
        }

        private bool CheckForSelfEating(int[] newSnakeElement)
        {
            foreach (var s in snake)
            {
                if (s[0] == newSnakeElement[0] && s[1] == newSnakeElement[1])
                {
                    PrintGameOverScreen();

                    GameOver = true;

                    return true;
                }
            }

            return false;
        }

        private void MoveSnake(int[]? oldSnakeTail = null)
        {
            if (oldSnakeTail != null)
            {
                Console.SetCursorPosition(ConsolePositionHelper.GetCorrectLeft(oldSnakeTail[1]), ConsolePositionHelper.GetCorrectTop(oldSnakeTail[0]));
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(_grass);
                Console.ResetColor();
            }

            foreach(var s in snake)
            {
                try
                {
                    _board[s[0], s[1]] = _snakeBody;
                    Console.SetCursorPosition((s[1] + 1) * 2, s[0] + 1);
                }
                catch(Exception ex)
                {
                    PrintGameOverScreen();
                    GameOver = true;

                    return;
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(_snakeBody);
            Console.ResetColor();
        }

        public void Update()
        {
            UpdateSnake();

            if (appleEaten)
            {
                AddApple();
            }
        }

        public void PrintBoard()
        {
            PrintHorizontalBorder();

            for (int i = 0; i < _length; i++)
            {
                PrintSideBorder();

                for (int j = 0; j < _width; j++)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(_board[i, j] + " ");
                    Console.ResetColor();
                }

                PrintSideBorder();
                Console.Write("\n");
            }

            PrintHorizontalBorder();

            PrintApple();
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

        private void PrintGameOverScreen()
        {
            Console.SetCursorPosition(0, _length + 3);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("============GAME OVER===========");
            Console.ResetColor();
        }
    }
}
