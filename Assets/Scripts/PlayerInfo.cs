using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;

    public string GetPlayerName()
    {
        return playerName.text;
    }

    public void SetPlayerName(string name)
    {
        playerName.text = name;
    }
}
