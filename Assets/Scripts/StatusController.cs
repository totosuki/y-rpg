using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusController : MonoBehaviour
{   
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI nameText;
    
    public void UpdateHp(int current, int max)
    {
        hpText.text = $"HP: {current}/{max}";
    }

    public void UpdateName(string name, int lv)
    {
        nameText.text = $"{name} Lv.{lv}";
    }
}
