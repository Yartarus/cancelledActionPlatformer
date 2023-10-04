using UnityEngine;

public static class RNG
{
    private static int[] seeds = { 3, 12, 16, 32, 34 };
    private static int seedIndex;
    private static int seed = 0;

    private static void InitializeRNG()
    {
        if (seed == 0)
        {
            seedIndex = (int)Random.Range(0, seeds.Length);
        }
        else
        {
            seedIndex++;
            if (seedIndex >= seeds.Length)
            {
                seedIndex = 0;
            }
        }
        //Debug.Log(seedIndex);
        seed = seeds[seedIndex];
        Random.InitState(seed);
    }

    public static int getRN(int _min, int _max)
    {
        if (seed == 0)
        {
            InitializeRNG();
        }
        return Random.Range(_min, _max);
    }

    public static float getFloatRN(float _min, float _max)
    {
        if (seed == 0)
        {
            InitializeRNG();
        }
        return Random.Range(_min, _max);
    }

}
