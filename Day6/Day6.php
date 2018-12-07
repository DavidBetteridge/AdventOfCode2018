<html>
 <head>
  <title>PHP Test</title>
 </head>
 <body>
 <?php 
 class coordinates
 {
     public $x;
     public $y;
     public $id;
   
     function print()
     {
         if ($this->id <> -1)
            echo $this->id.") ".$this->x.",".$this->y;
         else
            echo $this->x.",".$this->y; 
     }

    function __construct($X, $Y, $ID = -1) {
        $this->x = $X;
        $this->y = $Y;
        $this->id = $ID;
    }

    function distance($other)
    {
        $x_diff = abs($this->x - $other->x);
        $y_diff = abs($this->y - $other->y);
        return $x_diff + $y_diff;
    }
 }

function totalDistanceTo($coordinates, $coordinate)
{
    return array_reduce($coordinates, function($sum, $otherCoordinate) use ($coordinate) {
        return $sum = $sum + $coordinate->distance($otherCoordinate);
      }, 0);
}

 $coordinates = array();
 if ($fh = fopen('C:\Code\AdventOfCode2018\Day6\Coordinates.txt', 'r')) {
    $id = 1; 
    while (!feof($fh)) {
        $line = fgets($fh);
        $pieces = explode(", ", $line);
        $coordinate = new coordinates((int)$pieces[0], (int)$pieces[1], $id);
        array_push($coordinates, $coordinate);
        $id = $id + 1;
    }
    fclose($fh);
}

$TargetDistance = 10000;
$numberOfCoordinates = count($coordinates);
$maxGridSize = $TargetDistance / $numberOfCoordinates;

$minX = array_reduce($coordinates, function($min, $coordinate) {
    return min($min, $coordinate->x);
  }, PHP_INT_MAX) - $maxGridSize;

$minY = array_reduce($coordinates, function($min, $coordinate) {
    return min($min, $coordinate->y);
  }, PHP_INT_MAX) - $maxGridSize;

$maxX = array_reduce($coordinates, function($max, $coordinate) {
    return max($max, $coordinate->x);
  }, PHP_INT_MIN) + $maxGridSize;

$maxY = array_reduce($coordinates, function($max, $coordinate) {
    return max($max, $coordinate->y);
  }, PHP_INT_MIN) + $maxGridSize;


  $answer = 0;
for ($x = $minX; $x <= $maxX; $x++) {
    for ($y = $minY; $y <= $maxY; $y++) {
        $coordinate = new coordinates($x, $y);
        $distance = totalDistanceTo($coordinates, $coordinate);
        if ($distance < $TargetDistance)
        {
            $answer++;
        }
    }
} 

 echo "Part2".$answer;

 ?> 
 </body>
</html> 


