using UnityEngine;

public interface ITakeble
{
    public bool IsTake
    {
        get;
        set;
    }
    public void Use(Transform usePoint);
}
