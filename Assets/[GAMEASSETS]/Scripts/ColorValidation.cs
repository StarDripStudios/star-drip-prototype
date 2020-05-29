using UnityEngine;

public class ColorValidation : MonoBehaviour
{
    public Renderer renderer;
    public Color validColor;

    private Color originalColor;

    private void Start()
    {
        if (!renderer ||
            !renderer.material) return;

        originalColor = renderer.material.GetColor("_BaseColor");
    }

    public void ValidateColor()
    {
        if (!renderer ||
            !renderer.material) return;
        
        renderer.material.SetColor("_BaseColor", validColor);
    }

    public void ResetColor()
    {
        if (!renderer ||
            !renderer.material) return;
        
        renderer.material.SetColor("_BaseColor", originalColor);
    }
}
