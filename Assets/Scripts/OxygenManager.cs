using UnityEngine;

public class OxygenManager : MonoBehaviour
{
    private static float maxOxygen = 1000;
    public static float MaxOxygen => maxOxygen;
    private static float oxygen;
    public static float Oxygen => oxygen;

    private static float timeToDrain = 30;
    private static float timer;

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

    private void Update()
    {
        if (oxygen <= 0) return;
        
        timer -= Time.deltaTime;
        oxygen = timer / timeToDrain;
        Events.OnUpdateOxygen();
        if (oxygen <= 0)
        {
            Events.OnOutOfOxygen();
        }
    }
    
}
