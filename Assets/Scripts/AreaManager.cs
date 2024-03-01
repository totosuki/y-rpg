using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fungus;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    public AreaInfo areaInfo;

    public GameManager gameManager;

    [Tooltip("一定時間は同じエリア移動を再通知しない")]
    [SerializeField]private int rememberTime;

    private List<string> rememberList = new List<string>();

    public void RememberArea(string areaName)
    {
        // 数秒間記憶する
        rememberList.Add(areaName);
        StartCoroutine(RemoveAreaFromRememberList(areaName));
    }

    IEnumerator RemoveAreaFromRememberList(string areaName)
    {
        yield return new WaitForSeconds(rememberTime);
        rememberList.Remove(areaName);
    }

    public void ShowAreaInfo(string areaName, string subtitle)
    {
        if (!rememberList.Contains(areaName))
        {
            areaInfo.SetInfo(areaName, subtitle);
            areaInfo.Show();
        }
    }
}
