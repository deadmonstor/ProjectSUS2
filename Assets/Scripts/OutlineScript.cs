using UnityEngine;

public class OutlineScript : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private float outlineScaleFactor;
    [SerializeField] private Color outlineColor;
    public Renderer outlineRenderer;

    void Start()
    {
        outlineRenderer = CreateOutline(outlineMaterial, outlineScaleFactor, outlineColor);
    }

    Renderer CreateOutline(Material outlineMat, float scaleFactor, Color color)
    {
        GameObject outlineObject = Instantiate(this.gameObject, transform.position, transform.rotation ,transform);
        Renderer rend = outlineObject.GetComponent<Renderer>();
        
        Material[] mats = rend.materials;
        for (var index = 0; index < mats.Length; index++)
        {
            mats[index] = outlineMat;
        }

        rend.materials = mats;
        outlineMat.SetColor("_OutlineColor", color);
        outlineMat.SetFloat("_OutlineWidth", scaleFactor);
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        
        outlineObject.GetComponent<OutlineScript>().enabled = false;
        outlineObject.TryGetComponent<Collider>(out var collider);
        
        if (collider)
            collider.enabled = false;
            
        rend.enabled = false;

        return rend;
    }
}