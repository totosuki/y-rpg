using UnityEngine;

public class Area : MonoBehaviour
{
    // RememberListのキー・AreaInfoでの表示
    public string areaName;
    public string subtitle;

    private AreaManager areaManager;

    void Start()
    {
        areaManager = GetComponentInParent<AreaManager>();
    }
    
    public void ShowAreaInfo()
    {
        print("show");
        areaManager.ShowAreaInfo(areaName, subtitle);
    }
}
