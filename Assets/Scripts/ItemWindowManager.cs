using UnityEngine;

public class ItemWindowManager : MonoBehaviour
{
    public GameObject itemPanel;

    public void OnClickWindowButton()
    {
        Debug.Log("[ItemWindowManager] OnClickWindowButton");
        itemPanel.SetActive(!itemPanel.activeSelf);
    }

    public void OnClickItemImage(GameObject clickedImage) 
    {
        Debug.Log($"[ItemWindowManager] OnClickItemImage: {clickedImage.name}");
    }
}
