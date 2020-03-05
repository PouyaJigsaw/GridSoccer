using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static float GetBoundX(this BoxCollider boxCollider)
         {
             return boxCollider.bounds.size.x;
         }

    public static Vector2 ConvertToVector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }
}
