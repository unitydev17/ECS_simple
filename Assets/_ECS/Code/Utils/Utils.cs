using UnityEngine;

public static class Utils
{
    public static void SetX(this Transform tr, float value)
    {
        var pos = tr.position;
        pos.x = value;
        tr.position = pos;
    }
    
    public static void SetY(this Transform tr, float value)
    {
        var pos = tr.position;
        pos.y = value;
        tr.position = pos;
    }
}