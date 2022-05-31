using UnityEngine;
using UnityEngine.UI;

public class OxygenUI : MonoBehaviour
{
    [SerializeField] private Image oxygenFillImage;
    [SerializeField] private Gradient oxygenFillColourGradient;
    private void OnEnable()
    {
        Events.onUpdateOxygen += UpdateOxygen;
    }

    private void OnDisable()
    {
        Events.onUpdateOxygen -= UpdateOxygen;
    }
    
    private void UpdateOxygen()
    {
        var oxygenAmount = OxygenManager.Oxygen;
        oxygenFillImage.fillAmount = oxygenAmount;
        oxygenFillImage.color = oxygenFillColourGradient.Evaluate(oxygenAmount);
    }
}
