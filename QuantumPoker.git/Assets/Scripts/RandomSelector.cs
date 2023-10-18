using System;

public class RandomSelector
{
    readonly Random random;

    public RandomSelector()
    {
        this.random = new System.Random();
    }

    public int GetRandomElementIndex(double[] probabilities)
    {
        var cumulativeProbabilities = new double[probabilities.Length];
        cumulativeProbabilities[0] = probabilities[0];

        for (int i = 1; i < probabilities.Length; i++)
        {
            cumulativeProbabilities[i] = cumulativeProbabilities[i - 1] + probabilities[i];
        }

        double randomValue = this.random.NextDouble();
        int index = Array.BinarySearch(cumulativeProbabilities, randomValue);

        if (index < 0)
        {
            index = ~index; // Find the bitwise complement to get the insertion point
        }

        return index;
    }
}