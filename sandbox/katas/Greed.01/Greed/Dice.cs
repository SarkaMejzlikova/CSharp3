using System;

namespace Greed;

public class Dice
{
    public bool ExtraCredit { get; set; }
    private int[] RandomDice { get; set; } = new int[5];
    private int[] NumbersOnDice { get; set; } = new int[6];

    private int score;


    public Dice(bool extraCredit)
    {
        ExtraCredit = extraCredit;
        GetRandomDice();
        GetScore();
    }

    private void GetRandomDice()
    {
        var rnd = new Random();
        for (int i = 0; i < RandomDice.Length; i++)
        {
            RandomDice[i] = rnd.Next(1, 7); // 1 až 6
        }

        Array.Clear(NumbersOnDice, 0, NumbersOnDice.Length);
        foreach (int die in RandomDice)
        {
            if (die >= 1 && die <= NumbersOnDice.Length)
            {
                NumbersOnDice[die - 1]++;
            }
        }
    }

    public int GetScore()
    {
        score = 0;
        for (int i = 0; i < NumbersOnDice.Length; i++)
        {
            int count = NumbersOnDice[i];

            // trojice
            if (count >= 3)
            {
                if (i == 0) // jedničky
                {
                    score += 1000;
                }
                else
                {
                    score += (i + 1) * 100;
                }

                count -= 3; // odečti trojici
            }

            // jednotlivé jedničky a pětky
            if (i == 0) // jedničky
            {
                score += count * 100;
            }
            else if (i == 4) // pětky
            {
                score += count * 50;
            }
        }

        return score;
    }

    public void Result()
    {
        Console.WriteLine($"Hodnoty kostek: {string.Join(", ", RandomDice)}");
        Console.WriteLine($"Vaše skóre je: {score}");
    }
}
