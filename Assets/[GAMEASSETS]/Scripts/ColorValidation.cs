using UnityEngine;

public class ColorValidation : MonoBehaviour
{
    public Renderer renderer;
    public Color validColor;
    public string property;
    
    private Color originalColor;

    private void Start()
    {
        if (!renderer ||
            !renderer.material) return;

        originalColor = renderer.material.GetColor(property);
    }

    public void ValidateColor()
    {
        if (!renderer ||
            !renderer.material) return;
        
        renderer.material.SetColor(property, validColor);
    }

    public void ResetColor()
    {
        if (!renderer ||
            !renderer.material) return;
        
        renderer.material.SetColor(property, originalColor);
    }
}
