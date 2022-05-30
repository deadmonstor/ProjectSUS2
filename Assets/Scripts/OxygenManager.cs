using UnityEngine;

public class OxygenManager : MonoBehaviour
{
    private static float maxOxygen = 1000;
    public static float MaxOxygen => maxOxygen;
    private static float oxygen;
    public static float Oxygen => oxygen;

    [SerializeField] private float oxygenToLosePerSecond;

    public static void FillOxygen()
    {
        oxygen = maxOxygen;
    }

    private void Start()
    {
        oxygen = maxOxygen;
    }

    private void Update()
    {
        if (oxygen <= 0) return;

        oxygen -= oxygenToLosePerSecond * Time.deltaTime;
        Events.OnUpdateOxygen();
        if (oxygen <= 0)
        {
            Events.OnOutOfOxygen();
        }
    }
    
}
