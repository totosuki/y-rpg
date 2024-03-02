using UnityEngine;

public class AreaManager : MonoBehaviour
{
    public AreaInfo areaInfo;

    public void ShowAreaInfo(string areaName, string subtitle)
    {
        areaInfo.SetInfo(areaName, subtitle);
        areaInfo.Show();
    }
}
