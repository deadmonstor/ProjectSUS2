using System;
using UnityEngine;

public class OxygenManager : MonoBehaviour
{
    private static float maxOxygen = 1000;
    public static float MaxOxygen => maxOxygen;
    private static float oxygen;
    public static float Oxygen => oxygen;

    private static float timeToDrain = 30;
    private static float timer;
    private bool useOxygen;
    public static void FillOxygen()
    {
        oxygen = maxOxygen;
        timer = timeToDrain;
    }

    private void Start()
    {
        oxygen = maxOxygen;
        timer = timeToDrain;
    }

    private void OnEnable()
    {
        Events.onLevelLoaded += LevelLoaded;
    }

    private void OnDisable()
    {
        Events.onLevelLoaded -= LevelLoaded;
    }

    private void Update()
    {
        if (!useOxygen) return;

        if (oxygen <= 0) return;
        
        timer -= Time.deltaTime;
        oxygen = timer / timeToDrain;
        Events.OnUpdateOxygen();
        if (oxygen <= 0)
        {
            Events.OnOutOfOxygen();
        }
    }

    private void LevelLoaded(LevelSO level)
    {
        useOxygen = level.UseOxygen;
    }
}
