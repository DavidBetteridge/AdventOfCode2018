using System;
using System.Linq;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            var claims = System.IO.File.ReadAllLines("Fabrics.txt")
                        .Select(line => new Claim(line));

            var maxWidth = claims.Max(s => s.Left + s.Width);
            var maxHeight = claims.Max(s => s.Top + s.Height);

            var squares = new short[maxWidth, maxHeight];

            var answer = 0;
            foreach (var claim in claims)
            {
                for (int c = claim.Left; c < claim.Left + claim.Width; c++)
                {
                    for (int r = claim.Top; r < claim.Top + claim.Height; r++)
                    {
                        switch (squares[c, r])
                        {
                            case 1:
                                answer++;
                                break;

                            default:
                                break;
                        }
                        squares[c, r] = (short)(squares[c, r] + 1);

                    }
                }
            }
            //116140
            Console.WriteLine(answer);

            foreach (var claim in claims)
            {
                var overlaps = 0;
                for (int c = claim.Left; c < claim.Left + claim.Width; c++)
                {
                    for (int r = claim.Top; r < claim.Top + claim.Height; r++)
                    {
                        if (squares[c, r] > 1) overlaps++;
                    }
                }

                if (overlaps == 0)
                {
                    Console.WriteLine(claim.Id);
                }
            }

        }
    }
}
