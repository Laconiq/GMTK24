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
        if(b==true)
        AudioManager.PlayOneShot(FMODEvents.instance.OpenRadialMenu);
        else AudioManager.PlayOneShot(FMODEvents.instance.CloseRadialMenu);
        radialMenu.gameObject.SetActive(b);
    }
}
