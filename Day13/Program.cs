using System;
using System.Collections.Generic;
using System.Linq;

namespace Day13
{

    enum Direction
    {
        West = 0,
        North = 1,
        East = 2,
        South = 3
    }

    enum Turn
    {
        Left = 0,
        Straight = 1,
        Right = 2,
    }

    class Cart
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Direction CurrentDirection { get; set; }
        public Turn NextTurn { get; set; }

        public void Turn()
        {
            switch (NextTurn)
            {
                case Day13.Turn.Left:
                    NextTurn = Day13.Turn.Straight;
                    switch (CurrentDirection)
                    {
                        case Direction.West:
                            CurrentDirection = Direction.South;
                            break;
                        case Direction.North:
                            CurrentDirection = Direction.West;
                            break;
                        case Direction.East:
                            CurrentDirection = Direction.North;
                            break;
                        case Direction.South:
                            CurrentDirection = Direction.East;
                            break;
                        default:
                            break;
                    }
                    break;

                case Day13.Turn.Straight:
                    NextTurn = Day13.Turn.Right;
                    break;

                case Day13.Turn.Right:
                    NextTurn = Day13.Turn.Left;
                    switch (CurrentDirection)
                    {
                        case Direction.West:
                            CurrentDirection = Direction.North;
                            break;
                        case Direction.North:
                            CurrentDirection = Direction.East;
                            break;
                        case Direction.East:
                            CurrentDirection = Direction.South;
                            break;
                        case Direction.South:
                            CurrentDirection = Direction.West;
                            break;
                        default:
                            break;
                    }
                    break;

                default:
                    throw new Exception("Unknown turn");
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var rows = System.IO.File.ReadAllLines("Input.txt");
            var carts = new List<Cart>();

            for (int y = 0; y < rows.Length; y++)
            {
                var cells = rows[y].ToCharArray();
                for (int x = 0; x < cells.Length; x++)
                {

                    switch (cells[x])
                    {
                        case '<':
                            carts.Add(new Cart() { X = x, Y = y, CurrentDirection = Direction.West, NextTurn = Turn.Left });
                            cells[x] = '-';
                            break;

                        case '>':
                            carts.Add(new Cart() { X = x, Y = y, CurrentDirection = Direction.East, NextTurn = Turn.Left });
                            cells[x] = '-';
                            break;

                        case '^':
                            carts.Add(new Cart() { X = x, Y = y, CurrentDirection = Direction.North, NextTurn = Turn.Left });
                            cells[x] = '|';
                            break;

                        case 'v':
                            carts.Add(new Cart() { X = x, Y = y, CurrentDirection = Direction.South, NextTurn = Turn.Left });
                            cells[x] = '|';
                            break;

                        default:
                            break;
                    }
                }
                rows[y] = new string(cells);
            }

            while (true)
            {
                var orderedCarts = carts.OrderBy(cart => cart.Y).ThenBy(cart => cart.X);
                foreach (var cart in orderedCarts)
                {
                    var nextTrack = NextTrack(cart.X, cart.Y, cart.CurrentDirection, rows);
                    switch (nextTrack)
                    {
                        case '|':
                            cart.Y = cart.CurrentDirection == Direction.North ? cart.Y - 1 : cart.Y + 1;
                            break;

                        case '-':
                            cart.X = cart.CurrentDirection == Direction.West ? cart.X - 1 : cart.X + 1;
                            break;

                        case '/':
                            switch (cart.CurrentDirection)
                            {
                                case Direction.West:
                                    cart.CurrentDirection = Direction.South;
                                    cart.X = cart.X - 1;
                                    break;

                                case Direction.North:
                                    cart.CurrentDirection = Direction.West;
                                    cart.Y = cart.Y - 1;
                                    break;

                                case Direction.East:
                                    cart.CurrentDirection = Direction.North;
                                    cart.X = cart.X + 1;
                                    break;

                                case Direction.South:
                                    cart.CurrentDirection = Direction.East;
                                    cart.Y = cart.Y + 1;
                                    break;

                                default:
                                    throw new Exception("Unknown direction");
                            }
                            break;

                        case '\\':
                            switch (cart.CurrentDirection)
                            {
                                case Direction.West:
                                    cart.CurrentDirection = Direction.North;
                                    cart.X = cart.X - 1;
                                    break;

                                case Direction.North:
                                    cart.CurrentDirection = Direction.East;
                                    cart.Y = cart.Y - 1;
                                    break;

                                case Direction.East:
                                    cart.CurrentDirection = Direction.South;
                                    cart.X = cart.X + 1;
                                    break;

                                case Direction.South:
                                    cart.CurrentDirection = Direction.West;
                                    cart.Y = cart.Y + 1;
                                    break;

                                default:
                                    throw new Exception("Unknown direction");
                            }
                            break;

                        case '+':
                            switch (cart.CurrentDirection)
                            {
                                case Direction.West:
                                    cart.Turn();
                                    cart.X = cart.X - 1;
                                    break;

                                case Direction.North:
                                    cart.Turn();
                                    cart.Y = cart.Y - 1;
                                    break;

                                case Direction.East:
                                    cart.Turn();
                                    cart.X = cart.X + 1;
                                    break;

                                case Direction.South:
                                    cart.Turn();
                                    cart.Y = cart.Y + 1;
                                    break;

                                default:
                                    throw new Exception("Unknown direction");
                            }
                            break;

                        default:
                            throw new Exception("Unknown track type");

                    }
                }

                var result = carts.GroupBy(cart => (cart.X, cart.Y));
                var same = result.Where(grp => grp.Count() > 1).FirstOrDefault();
                if (same != null)
                {
                    Console.WriteLine($"{same.First().X},{same.First().Y}");
                    Console.ReadKey();
                }                    


            }


            Console.WriteLine("Hello World!");
        }



        private static char NextTrack(int x, int y, Direction currentDirection, string[] rows)
        {
            switch (currentDirection)
            {
                case Direction.West:
                    return rows[y][x - 1];
                case Direction.North:
                    return rows[y - 1][x];
                case Direction.East:
                    return rows[y][x + 1];
                case Direction.South:
                    return rows[y + 1][x];
                default:
                    throw new Exception("Unknown direction");
            }
        }
    }
}
