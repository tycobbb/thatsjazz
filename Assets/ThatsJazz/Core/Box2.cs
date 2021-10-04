using System;
using UnityEngine;

/// a value that can be lerp'd
[Serializable]
public struct Box2 {
    // -- props --
    [Tooltip("the min x value")]
    public float X1;

    [Tooltip("the max x value")]
    public float X2;

    [Tooltip("the min y value")]
    public float Y1;

    [Tooltip("the max y value")]
    public float Y2;

    // -- lifetime --
    /// create a new linear value
    public Box2(float x1, float x2, float y1, float y2) {
        X1 = x1;
        X2 = x2;
        Y1 = y1;
        Y2 = y2;
    }

    // -- factories --
    /// creates a "zero" value
    public static Box2 Zero {
        get => new Box2(0.0f, 0.0f, 0.0f, 0.0f);
    }
}