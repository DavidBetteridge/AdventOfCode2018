#include <stdio.h>
#include <stdlib.h>
#include <time.h>

#define NUMBER_OF_MARBLES 7165700
#define NUMBER_OF_PLAYERS 476

typedef struct circle {
    long values[NUMBER_OF_MARBLES];
    long next[NUMBER_OF_MARBLES];
    long previous[NUMBER_OF_MARBLES];
    long scores[NUMBER_OF_PLAYERS];
    long current;
    long nextSlot;
} circleType;

long RemoveMarbles(long marble, circleType *);
void AddMarble(long marble, circleType *);


void AddMarble(long marble, circleType *Circle)
{
    Circle->values[Circle->nextSlot] = marble;

    long indexOfPlusOne = Circle->next[Circle->current];
    long indexOfPlusTwo = Circle->next[indexOfPlusOne];

    Circle->next[indexOfPlusOne] = Circle->nextSlot;
    Circle->previous[Circle->nextSlot] = indexOfPlusOne;

    Circle->next[Circle->nextSlot] = indexOfPlusTwo;
    Circle->previous[indexOfPlusTwo] = Circle->nextSlot;

    Circle->current = Circle->nextSlot;
    Circle->nextSlot++;
}

long RemoveMarbles(long marble, circleType *Circle)
{
    Circle->current = Circle->previous[Circle->current];
    Circle->current = Circle->previous[Circle->current];
    Circle->current = Circle->previous[Circle->current];
    Circle->current = Circle->previous[Circle->current];
    Circle->current = Circle->previous[Circle->current];
    Circle->current = Circle->previous[Circle->current];

    long sevenBack = Circle->previous[Circle->current];

    Circle->next[Circle->previous[sevenBack]] = Circle->next[sevenBack];
    Circle->previous[Circle->next[sevenBack]] = Circle->previous[sevenBack];

    return marble + Circle->values[sevenBack];
}

int main()
{

    clock_t begin = clock();

    circleType* Circle = malloc(sizeof *Circle);

    for (long player = 0; player < NUMBER_OF_PLAYERS; player++)
        Circle->scores[player] = 0;

    Circle->values[0] = 0;
    Circle->current = 0;
    Circle->nextSlot = 1;        

    long playerNumber = 0;
    for (long marble = 1; marble <= NUMBER_OF_MARBLES; marble++)
    {
        if (marble % 23 != 0)
        {
            AddMarble(marble, Circle);
        }
        else
        {
            long score = RemoveMarbles(marble, Circle);
            Circle->scores[playerNumber] += score;
        }
        playerNumber = (playerNumber + 1) % NUMBER_OF_PLAYERS;
    }


    long highestScore = 0;
    for (long player = 0; player < NUMBER_OF_PLAYERS; player++)  
    {
        unsigned long playersScore = Circle->scores[player];
        if (playersScore > highestScore)
        {
            highestScore = playersScore;
        }
    }

    clock_t end = clock();
    double time_spent = (double)(end - begin) / CLOCKS_PER_SEC;

    printf("Part 2 is %ld", highestScore);



    return 0;
}

