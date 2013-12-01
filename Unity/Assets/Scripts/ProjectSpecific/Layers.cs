using UnityEngine;

// when you want only one layer, it's ~(1 << LayerMask.NameToLayer(""))

public static class Layers
{
    public static int defaultLayer = LayerMask.NameToLayer("Default");
    public static int uiLayer = LayerMask.NameToLayer("UI");
}
