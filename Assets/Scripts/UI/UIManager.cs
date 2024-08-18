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
        AudioManager.instance.PlayOneShot(FMODEvents.instance.OpenRadialMenu);
        radialMenu.gameObject.SetActive(b);
    }
}
