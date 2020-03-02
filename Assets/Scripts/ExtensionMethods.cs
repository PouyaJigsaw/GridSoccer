using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static float GetBoundX(this BoxCollider boxCollider)
    {
        return boxCollider.bounds.size.x;
    }
}
