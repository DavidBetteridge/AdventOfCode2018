namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {

            System.Console.WriteLine($"Part 1 -> {Part1()}");

            Timer.TimeMe(() =>
            {
                System.Console.WriteLine($"Part 2 -> {Part2_SummedArea()}");
            });

            Timer.TimeMe(() =>
            {
                System.Console.WriteLine($"Part 2 -> {Part2()}");
            });

            System.Console.ReadKey();
        }

        private static string Part2_SummedArea()
        {
            const int grid_serial_number = 2568;

            var powerLevels = new int[301, 301];

            powerLevels[1, 1] = CalculatePowerLevel(1, 1, grid_serial_number);
            for (int x = 1; x <= 300; x++)
            {
                for (int y = 1; y <= 300; y++)
                {
                    if (x > 1 || y > 1)
                    {
                        powerLevels[x, y] = CalculatePowerLevel(x, y, grid_serial_number);
                    }

                    if (x > 1)
                    {
                        powerLevels[x, y] += powerLevels[x - 1, y];
                    }

                    if (y > 1)
                    {
                        powerLevels[x, y] += powerLevels[x, y - 1];
                    }

                    if (y > 1 && x > 1)
                    {
                        powerLevels[x, y] -= powerLevels[x - 1, y - 1];
                    }
                }
            }

            var bestRegionSize = 0;
            var bestTotal = int.MinValue;
            var topLeftX = 0;
            var topLeftY = 0;

            for (int regionSize = 1; regionSize <= 300; regionSize++)
            {
                for (int x = 1; x <= 301 - regionSize-1; x++)
                {
                    for (int y = 1; y <= 301 - regionSize-1; y++)
                    {
                        var A = powerLevels[x, y];
                        var B = powerLevels[x + regionSize, y];
                        var C = powerLevels[x, y + regionSize];
                        var D = powerLevels[x + regionSize, y + regionSize];
                        var score = D + A - B - C;

                        if (score > bestTotal)
                        {
                            bestTotal = score;
                            topLeftX = x;
                            topLeftY = y;
                            bestRegionSize = regionSize;
                        }
                    }
                }
            }

            return $"{topLeftX+1},{topLeftY+1},{bestRegionSize},{bestTotal}";
        }

        private static string Part1()
        {
            const int grid_serial_number = 2568;
            //const int grid_serial_number = 18;

            var powerLevels = new int[300, 300];
            for (int x = 0; x < 300; x++)
            {
                for (int y = 0; y < 300; y++)
                {
                    powerLevels[x, y] = CalculatePowerLevel(x, y, grid_serial_number);
                }
            }

            var bestTotal = int.MinValue;
            var topLeftX = 0;
            var topLeftY = 0;
            for (int x = 0; x < 300 - 2; x++)
            {
                for (int y = 0; y < 300 - 2; y++)
                {
                    var regionTotal = powerLevels[x, y] + powerLevels[x + 1, y] + powerLevels[x + 2, y] +
                                      powerLevels[x, y + 1] + powerLevels[x + 1, y + 1] + powerLevels[x + 2, y + 1] +
                                      powerLevels[x, y + 2] + powerLevels[x + 1, y + 2] + powerLevels[x + 2, y + 2];

                    if (regionTotal > bestTotal)
                    {
                        bestTotal = regionTotal;
                        topLeftX = x;
                        topLeftY = y;
                    }
                }
            }
            return $"{topLeftX},{topLeftY}";
        }

        private static string Part2()
        {
            const int grid_serial_number = 2568;
            //const int grid_serial_number = 18;

            var regions = new int[300, 300];
            var powerLevels = new int[300, 300];


            var bestRegionSize = 0;
            var bestTotal = int.MinValue;
            var topLeftX = 0;
            var topLeftY = 0;

            for (int x = 0; x < 300; x++)
            {
                for (int y = 0; y < 300; y++)
                {
                    powerLevels[x, y] = CalculatePowerLevel(x, y, grid_serial_number);
                    regions[x, y] = powerLevels[x, y];

                    if (regions[x, y] > bestTotal)
                    {
                        bestTotal = regions[x, y];
                        topLeftX = x;
                        topLeftY = y;
                        bestRegionSize = 1;
                    }
                }
            }

            for (int regionSize = 2; regionSize <= 300; regionSize++)
            {
                for (int x = 0; x < 300 - regionSize - 1; x++)
                {
                    for (int y = 0; y < 300 - regionSize - 1; y++)
                    {
                        for (int r = 0; r < regionSize; r++)
                        {
                            regions[x, y] += powerLevels[x + (regionSize - 1), y + r];
                        }

                        for (int c = 0; c < regionSize - 1; c++)
                        {
                            regions[x, y] += powerLevels[x + c, y + (regionSize - 1)];
                        }

                        if (regions[x, y] > bestTotal)
                        {
                            bestTotal = regions[x, y];
                            topLeftX = x;
                            topLeftY = y;
                            bestRegionSize = regionSize;
                        }
                    }
                }
            }

            return $"{topLeftX},{topLeftY},{bestRegionSize},{bestTotal}";
        }

        private static int CalculatePowerLevel(int x, int y, int grid_serial_number)
        {
            var rackID = x + 10;
            var powerLevel = rackID * y;
            powerLevel = powerLevel + grid_serial_number;
            powerLevel = powerLevel * rackID;
            powerLevel = powerLevel / 100;
            powerLevel = powerLevel % 10;

            return powerLevel - 5;
        }
    }
}
