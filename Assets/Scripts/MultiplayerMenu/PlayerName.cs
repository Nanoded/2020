using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_InputField))]
public class PlayerName : MonoBehaviour
{
    [SerializeField] private Button _applyButton;
    private TMP_InputField _inputFieldPlayerName;

    void Start()
    {
        if(_applyButton != null)
        {
            _applyButton.onClick.AddListener(ApplyName);
        }

        _inputFieldPlayerName = GetComponent<TMP_InputField>();
        if(PlayerPrefs.HasKey("PlayerName"))
        {
            _inputFieldPlayerName.text = PlayerPrefs.GetString("PlayerName");
            PhotonNetwork.NickName = _inputFieldPlayerName.text;
        }
    }

    
    void Update()
    {
        if(_inputFieldPlayerName.text == string.Empty)
        {
            _applyButton.interactable = false;
        }
        else
        {
            _applyButton.interactable = true;
        }    
    }

    private void ApplyName()
    {
        PhotonNetwork.NickName = _inputFieldPlayerName.text;
        PlayerPrefs.SetString("PlayerName", _inputFieldPlayerName.text);
    }
}
