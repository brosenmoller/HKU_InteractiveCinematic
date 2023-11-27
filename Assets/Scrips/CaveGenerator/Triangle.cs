using UnityEngine;

public struct Triangle
{
    public Vector3 a;
    public Vector3 b;
    public Vector3 c;

    public Vector3 this[int i]
    {
        get
        {
            return i switch
            {
                0 => a,
                1 => b,
                _ => c,
            };
        }
    }
}