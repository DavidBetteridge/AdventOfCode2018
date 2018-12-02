package adventofcode;

import java.util.*;

public class day14 {
    public static void main(String[] args) {
        WorkoutScores(9);
        WorkoutScores(5);
        WorkoutScores(18);
        WorkoutScores(2018);
        WorkoutScores(157901);  /* 9 4 1 1 1 3 7 1 3 3 */

        Lookfor("51589");
        Lookfor("01245");
        Lookfor("92510");
        Lookfor("59414");
        Lookfor("157901");
    }

    private static void Lookfor(String lookfor)
    {
        Integer elf1 = 0;
        Integer elf2 = 1;

        ArrayList<Integer> recipes = new ArrayList<>();
        recipes.add(3);
        recipes.add(7);

        String recipesText = "000037".substring(6-lookfor.length());

        while (true)
        {
            Integer nextReceipe = recipes.get(elf1) + recipes.get(elf2);

            if (nextReceipe > 9)
            {
                Integer first = nextReceipe / 10;
                Integer second = nextReceipe % 10;

                recipes.add(first);
                recipesText = recipesText.substring(1) + first.toString();

                if (recipesText.equals(lookfor))
                {
                    System.out.println(recipes.size() - lookfor.length());
                    return;
                }

                recipes.add(second);
                recipesText = recipesText.substring(1) + second.toString();

                if (recipesText.equals(lookfor))
                {
                    System.out.println(recipes.size() - lookfor.length());
                    return;
                }
            }
            else
            {
                recipes.add(nextReceipe);

                recipesText = recipesText.substring(1) + nextReceipe.toString();

                if (recipesText.equals(lookfor))
                {
                    System.out.println(recipes.size() - lookfor.length());
                    return;
                }
            }

            elf1 = (elf1 + recipes.get(elf1) + 1) % recipes.size();
            elf2 = (elf2 + recipes.get(elf2) + 1) % recipes.size();
        }
    }



    private static void WorkoutScores(int after)
    {
        Integer elf1 = 0;
        Integer elf2 = 1;

        ArrayList<Integer> recipes = new ArrayList<>();
        recipes.add(3);
        recipes.add(7);

        while (recipes.size() < 10 + after)
        {
            Integer nextReceipe = recipes.get(elf1) + recipes.get(elf2);

            if (nextReceipe > 9)
            {
                Integer first = nextReceipe / 10;
                Integer second = nextReceipe % 10;
                recipes.add(first);
                recipes.add(second);
            }
            else
            {
                recipes.add(nextReceipe);
            }

            elf1 = (elf1 + recipes.get(elf1) + 1) % recipes.size();
            elf2 = (elf2 + recipes.get(elf2) + 1) % recipes.size();
        }

        String answer = "";
        for (Integer i=after; i<after+10; i++)
        {
            answer = answer + recipes.get(i);
        }
        System.out.println(answer);

    }


}
