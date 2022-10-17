using Photon.Pun;
using UnityEngine;

public class Flashlight : MonoBehaviourPun, ITool
{
    [SerializeField] private string _nameTool;
    [SerializeField] private GameObject _light;

    public string Name()
    {
        return _nameTool;
    }

    public void Take()
    {
        
    }

    public void Use()
    {
        _light.SetActive(!_light.activeInHierarchy);
    }
}
