using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    public float radius = 100f;

    public void ArrangeChildrenInCircle()
    {
        var childCount = transform.childCount;
        if (childCount == 0) 
            return;

        var angleStep = 360f / childCount;

        for (var i = 0; i < childCount; i++)
        {
            var child = transform.GetChild(i);
            var angle = (i * angleStep - 90) * Mathf.Deg2Rad;
            var x = Mathf.Cos(angle) * radius;
            var y = Mathf.Sin(angle) * radius;
            child.localPosition = new Vector3(x, y, 0);
        }
    }
}