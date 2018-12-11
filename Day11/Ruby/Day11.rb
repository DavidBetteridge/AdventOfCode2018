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