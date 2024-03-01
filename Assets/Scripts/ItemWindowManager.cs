using UnityEngine;

public class ItemWindowManager : MonoBehaviour
{
    public GameObject itemPanel;

    public void OnClickWindowButton()
    {
        Debug.Log("[ItemWindowManager] OnClickWindowButton");
        itemPanel.SetActive(!itemPanel.activeSelf);
    }
}
