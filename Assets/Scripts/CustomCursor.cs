using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D clickCursor;
    [SerializeField] private Texture2D hoverCursor;
    
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    
    private void Start()
    {
        SetDefaultCursor();
    }
    
    public void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, hotSpot, cursorMode);
    }
    
    public void SetClickCursor()
    {
        Cursor.SetCursor(clickCursor, hotSpot, cursorMode);
    }
    
    public void SetHoverCursor()
    {
        Cursor.SetCursor(hoverCursor, hotSpot, cursorMode);
    }
}
