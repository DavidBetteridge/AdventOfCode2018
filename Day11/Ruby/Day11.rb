GRID_SERIAL_NUMBER = 2568  

def calculate_power_level(x, y)
    rackID = x + 10
    powerLevel = rackID * y
    powerLevel = powerLevel + GRID_SERIAL_NUMBER
    powerLevel = powerLevel * rackID
    powerLevel = powerLevel / 100
    powerLevel = powerLevel % 10
    powerLevel = powerLevel - 5

    return powerLevel
end

def build_column(x)
  return (1..300).map { |y| calculate_power_level x, y }
end


def part_one()
  powerLevels = (1..300).map { |x| build_column x }

  bestTotal = -999999;
  topLeftX = 0;
  topLeftY = 0;

  (1..297).each do |x|
    (1..297).each do |y|
          regionTotal = powerLevels[x][y] + powerLevels[x + 1][y] + powerLevels[x + 2][y] +
                        powerLevels[x][y + 1] + powerLevels[x + 1][y + 1] + powerLevels[x + 2][y + 1] +
                        powerLevels[x][y + 2] + powerLevels[x + 1][y + 2] + powerLevels[x + 2][y + 2]  

          if (regionTotal > bestTotal)
              bestTotal = regionTotal
              topLeftX = x
              topLeftY = y
          end

    end
  end

  puts topLeftX+1
  puts topLeftY+1
end

def part_two()
  powerLevels = Array.new(300) { Array.new(300) }
  powerLevels.unshift([0] * (300 + 1))

  powerLevels[1][1] = calculate_power_level 1, 1

  (1..300).each do |x|
    (1..300).each do |y|

      total = 0

      if (x > 1 || y > 1)
        total = calculate_power_level x, y
      end

      if (x > 1)
        total = total + powerLevels[x - 1][y]
      end

      if (y > 1)
        total = total + powerLevels[x][y - 1]
      end

      if (y > 1) && (x > 1)
        total = total - powerLevels[x - 1][y - 1]
      end

      powerLevels[x][y] = total

    end
  end

  puts "Built powerLevels"

  bestRegionSize = 0
  bestTotal = -99999
  topLeftX = 0
  topLeftY = 0

  (1..300).each { |regionSize|
      max = 301 - regionSize - 1  
      (1..max).each { |x|
        ysLeft = powerLevels[x]
        ysRight = powerLevels[x + regionSize]
        (1..max).each { |y|
              score = ysRight[y + regionSize] + ysLeft[y] - ysRight[y] - ysLeft[y + regionSize]
              if (score > bestTotal)
                  bestTotal = score
                  topLeftX = x
                  topLeftY = y
                  bestRegionSize = regionSize
              end
          }
      }
  }

  puts topLeftX+1
  puts topLeftY+1
  puts bestRegionSize
  puts bestTotal

end

part_two