#perl 5.22.1 

sub calculate_score {
    my ($state) = @_;
    
    $score = 0;
    for ($position = 0 ; $position < length $state; $position++) {
        $plant = substr $state, $position, 1;
        if ($plant eq '#') {
            $score = $score + $position - 20;
        }
    }
    return $score;
}


$current_state = "#.#####.##.###...#...#.####..#..#.#....##.###.##...#####.#..##.#..##..#..#.#.#.#....#.####....#..#";
$current_state = ("." x 20) . $current_state . ("." x 20);

my @rules = ();
push @rules, '#.#.. => .';
push @rules, '..### => .';
push @rules, '...## => .';
push @rules, '.#### => #';
push @rules, '.###. => #';
push @rules, '#.... => .';
push @rules, '#.#.# => .';
push @rules, '###.. => #';
push @rules, '#..#. => .';
push @rules, '##### => #';
push @rules, '.##.# => #';
push @rules, '.#... => .';
push @rules, '##.## => #';
push @rules, '#...# => #';
push @rules, '.#.## => .';
push @rules, '##..# => .';
push @rules, '..... => .';
push @rules, '.#.#. => #';
push @rules, '#.### => #';
push @rules, '....# => .';
push @rules, '...#. => #';
push @rules, '..#.# => #';
push @rules, '##... => #';
push @rules, '####. => #';
push @rules, '#..## => #';
push @rules, '##.#. => #';
push @rules, '###.# => .';
push @rules, '#.##. => .';
push @rules, '..#.. => #';
push @rules, '.#..# => .';
push @rules, '..##. => .';
push @rules, '.##.. => #';


for ($generation = 0 ; $generation < 20; $generation++) {
    $original_state = $current_state;
    
    $size = length $current_state;
    for ($count = 2 ; $count < $size - 4; $count++) {
        foreach my $rule (@rules) {
            
            $l = substr $rule, 0, 5;
            $r = substr $original_state,$count - 2, 5;

            if ($l eq $r) {
                $replacement = substr $rule, 9, 1;
                substr($current_state, $count, 1) = $replacement
            }
        }
    }
}

my $score = calculate_score($current_state);    
print "$score\n";





