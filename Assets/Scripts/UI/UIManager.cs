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
        AudioManager.instance.PlayOneShot(FMODEvents.instance.OpenRadialMenu);
        else AudioManager.instance.PlayOneShot(FMODEvents.instance.CloseRadialMenu);
        radialMenu.gameObject.SetActive(b);
    }
}
