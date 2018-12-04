from datetime import datetime

#[1518-07-23 00:03] Guard #631 begins shift
#[1518-10-03 00:14] falls asleep
#[1518-09-06 00:57] wakes up
lines = open("C:\Code\AdventOfCode2018\Day4\ShiftPattern.txt", "r",encoding='utf-8-sig')
sortedLines = sorted(lines, key=lambda line: line[1:17])

currentGuard = -1
fallsSleep = ""
minutesByGuard = {}
for line in sortedLines:
  datetime_object = datetime.strptime(line[1:17], '%Y-%m-%d %H:%M')
  action = line[19:24]   #Guard,  falls,  wakes

  if action == "Guard":
      end = 26
      while line[end:end+1] != " ":
          end = end + 1

      currentGuard = line[26:end]


  elif action == "falls":
      fallsSleep = datetime_object

  elif action == "wakes":

       if currentGuard in minutesByGuard:
            minutes = minutesByGuard[currentGuard]            
       else:
            minutes = [] 
            for minute in range(0, 59):
                minutes.append(0)
            minutesByGuard[currentGuard] = minutes

       for minute in range(fallsSleep.minute, datetime_object.minute -1):
           minutes[minute] = minutes[minute] + 1
  else:
    print ("Unknown Action" )     
      
worstGuard = sorted(minutesByGuard, key=(lambda guardNumber:sum(minutesByGuard[guardNumber])))[-1]

minutesForGuard = minutesByGuard[worstGuard]
worstMinute = 0
worstScore = 0
for minute in range(0, 59):
    if minutesForGuard[minute] > worstScore:
        worstScore = minutesForGuard[minute]
        worstMinute = minute

part1 = worstMinute * int(worstGuard)


maxNumberOfMinutes = 0
maxGuard = 0
maxMinute = 0
for guard in minutesByGuard:
    minutesForGuard = minutesByGuard[guard]
    for minute in range(0, 59):
        if (minutesForGuard[minute] > maxNumberOfMinutes):
            maxNumberOfMinutes = minutesForGuard[minute]
            maxGuard = guard
            maxMinute = minute

part2 = maxMinute * int(maxGuard)


print("Part 1 is", part1)
print("Part 2 is", part2)