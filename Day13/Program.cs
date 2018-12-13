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
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser();
            var (cells, carts)= parser.Load();

            var tick = 0;
            while (carts.Count(c => c.IsAlive) > 1)
            {
            //    Display(cells, carts);

                tick++;
                var orderedCarts = carts.OrderBy(cart => cart.Y).ThenBy(cart => cart.X);
                foreach (var cart in orderedCarts.Where(c => c.IsAlive))
                {
                    var nextTrack = NextTrack(cart.X, cart.Y, cart.CurrentDirection, cells);
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
                                    cart.CurrentDirection = Direction.East;
                                    cart.Y = cart.Y - 1;
                                    break;

                                case Direction.East:
                                    cart.CurrentDirection = Direction.North;
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

                        case '\\':
                            switch (cart.CurrentDirection)
                            {
                                case Direction.West:
                                    cart.CurrentDirection = Direction.North;
                                    cart.X = cart.X - 1;
                                    break;

                                case Direction.North:
                                    cart.CurrentDirection = Direction.West;
                                    cart.Y = cart.Y - 1;
                                    break;

                                case Direction.East:
                                    cart.CurrentDirection = Direction.South;
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

                        case '+':
                            switch (cart.CurrentDirection)
                            {
                                case Direction.West:
                                    cart.X = cart.X - 1;
                                    break;

                                case Direction.North:
                                    cart.Y = cart.Y - 1;
                                    break;

                                case Direction.East:
                                    cart.X = cart.X + 1;
                                    break;

                                case Direction.South:
                                    cart.Y = cart.Y + 1;
                                    break;

                                default:
                                    throw new Exception("Unknown direction");
                            }

                            cart.TurnCart();
                            break;

                        default:
                            throw new Exception("Unknown track type");

                    }

                    var result = carts.Where(c => c.IsAlive).GroupBy(c => (c.X, c.Y));
                    var crashes = result.Where(grp => grp.Count() > 1).ToList();

                    if (crashes.Any())
                    {
                        Console.WriteLine($"Part 1 :: {crashes.First().First().X},{crashes.First().First().Y}");
                        //   Console.ReadKey();
                    }

                    foreach (var crash in crashes)
                    {
                        foreach (var c in crash)
                        {
                            c.IsAlive = false;
                        }
                    }
                }
            }

            var remaining = carts.Single(c => c.IsAlive);
            Console.WriteLine($"Part 2 :: {remaining.X},{remaining.Y}");  //69,67
            Console.ReadKey();
        }


       static void Display(char[,] cells, IEnumerable<Cart> carts)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            for (int y = 0; y <= cells.GetUpperBound(1); y++)
            {
                for (int x = 0; x <= cells.GetUpperBound(0); x++)
                {
                    var cart = carts.FirstOrDefault(c => c.X == x && c.Y == y && c.IsAlive);
                    if (cart == null)
                    {
                        Console.Write(cells[x, y]);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        switch (cart.CurrentDirection)
                        {
                            case Direction.West:
                                Console.Write("<");
                                break;
                            case Direction.North:
                                Console.Write("^");
                                break;
                            case Direction.East:
                                Console.Write(">");
                                break;
                            case Direction.South:
                                Console.Write("v");
                                break;
                            default:
                                break;
                        }
                        Console.ResetColor();
                    }

                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }

        private static char NextTrack(int x, int y, Direction currentDirection, char[,] cells)
        {
            switch (currentDirection)
            {
                case Direction.West:
                    return cells[x - 1, y];
                case Direction.North:
                    return cells[x, y - 1];
                case Direction.East:
                    return cells[x + 1, y];
                case Direction.South:
                    return cells[x, y + 1];
                default:
                    throw new Exception("Unknown direction");
            }
        }
    }
}
