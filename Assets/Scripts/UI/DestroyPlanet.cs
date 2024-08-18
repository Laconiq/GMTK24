using UnityEngine;

public class DestroyPlanet : MonoBehaviour
{
    private bool _isHover;
    public bool IsHovering() { return _isHover; }
    public void Hover(bool isHover)
    {
        _isHover = isHover;
    }
}
