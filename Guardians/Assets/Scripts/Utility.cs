using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static float fit(float value, float from, float to, float from2,  float to2)
    {
        return Mathf.Lerp (from2, to2, Mathf.InverseLerp (from, to, value));
    }
}
