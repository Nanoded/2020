using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UseScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameObject;
    [SerializeField] private TextMeshProUGUI _descriptionObject;
    [SerializeField] private TextMeshProUGUI _doText;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void UpdateScreen(string nameObject, string descriprionObject, string doText)
    {
        _nameObject.text = nameObject;
        _descriptionObject.text = descriprionObject;
        _doText.text = doText;
    }
}
