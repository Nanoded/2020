using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowbar : MonoBehaviour, ITool
{
    [SerializeField] private string _name;

    public string Name()
    {
        return _name;
    }

    public void Take()
    {
        
    }

    public void Use()
    {
        
    }
}
