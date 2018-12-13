using System;

namespace Day13
{
    class Cart
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Direction CurrentDirection { get; set; }
        public Turn NextTurn { get; set; }
        public bool IsAlive { get; set; }

        private Turn WorkOutNextTurn(Turn turn)
        {
            switch (turn)
            {
                case Turn.Left:
                    return Turn.Straight;
                case Turn.Straight:
                    return Turn.Right;
                case Turn.Right:
                    return Turn.Left;
                default:
                    throw new Exception("Unknown turn");
            }
        }

        public void TurnCart()
        {
            switch (NextTurn)
            {
                case Day13.Turn.Left:

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
                    break;

                case Day13.Turn.Right:

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

            NextTurn = WorkOutNextTurn(NextTurn);
        }
    }
}
