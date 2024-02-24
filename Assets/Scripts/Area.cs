using UnityEngine;

public class Area : MonoBehaviour
{
    // RememberListのキー・AreaInfoでの表示
    public string areaName;
    public string subtitle;

    private AreaManager areaManager;
    private bool inArea;

    void Start()
    {
        areaManager = GetComponentInParent<AreaManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            inArea = true;
            areaManager.ShowAreaInfo(areaName, subtitle);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (inArea)
            {
                inArea = false;
                areaManager.RememberArea(areaName);
            }
        }
    }
}
