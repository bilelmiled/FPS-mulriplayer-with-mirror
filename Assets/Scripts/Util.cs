using UnityEngine;

public class Util
{
    public static void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform item in obj.transform)
        {
            SetLayerRecursively(item.gameObject, newLayer);
        }
    }
}
