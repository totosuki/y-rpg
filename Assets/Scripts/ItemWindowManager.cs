using UnityEngine;
using UnityEngine.UI;

public class ItemWindowManager : MonoBehaviour
{
    public GameObject itemPanel;
    public Text iconText;
    public Text iconDescription;
    public GameObject iconImage;
    private ItemManager itemManager;

    public void OnClickWindowButton()
    {
        Debug.Log("[ItemWindowManager] OnClickWindowButton");
        itemPanel.SetActive(!itemPanel.activeSelf);
    }

    public void OnClickItemImage(int id)
    {
        Debug.Log("[ItemWindowManager] id : " + id);
        UpdateIcon(id);
    }

    private void UpdateIcon(int id) {
        Item item = itemManager.GetItem(id);
        if (item == null) 
        {
            return;
        }
        iconText.text = item.itemName;
        iconDescription.text = item.description;
        iconImage.GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
    }

    private void Start() 
    {
        itemManager = GameObject.Find("Items").GetComponent<ItemManager>();
    }
}
