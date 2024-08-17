using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RadialMenu radialMenu;
    
    public void Initialize()
    {
        var radialMenuObject = radialMenu.gameObject;
        if (radialMenuObject.activeSelf)
            radialMenuObject.SetActive(false);
        
        radialMenu.ArrangeChildrenInCircle();
    }

    public void OpenRadialMenu(bool b)
    {
        radialMenu.gameObject.SetActive(b);
    }
}
