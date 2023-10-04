using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static class for our extensions - just place somewhere in your project
/// </summary>
public static class ExtensionMethods
{

    // It's really a reverse recursive - If you moved from top down, the children of the children would be lost
    public static void SetParentRecursive(this Transform transform, Transform newParent)
    {

        List<Transform> dropChildren = new List<Transform>();

        foreach (Transform child in transform)
        {
            dropChildren.Add(child);
        }
        transform.parent = newParent;
        foreach (Transform child in dropChildren)
        {
            child.parent = transform;
        }
    }

    public static int BoolToInt(bool _bool)
    {
        if (_bool)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public static bool IntToBool(int _int)
    {
        if (_int == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}