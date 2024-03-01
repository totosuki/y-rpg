using UnityEngine;
using UnityEngine.UI;

public class ItemWindowManager : MonoBehaviour
{
    [Header("アイテムパネル")]
    public GameObject itemPanel;

    [Header("アイコンにいるアイテムの名前")]
    public Text iconText;

    [Header("アイコンにいるアイテムの説明")]
    public Text iconDescription;

    [Header("アイコンにいるアイテムの画像")]
    public Image iconImage;
    private ItemManager itemManager;

    private void UpdateItemImage()
    {
        for (int i = 0; i < itemManager.numberOfItems; i++) 
        {
            Item item = itemManager.GetItem(i);
            if (item == null) continue;
            if (itemManager.collectItemList[i] == false) continue;
            Image itemImage = GameObject.Find("ItemImage" + (i+1)).GetComponent<Image>();
            itemImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            itemImage.color = new Color(1, 1, 1, 1);
        }
    }

    private void UpdateIcon(int id)
    {
        Item item = itemManager.GetItem(id);
        if (item == null) return;
        iconText.text = item.itemName;
        iconDescription.text = item.description;
        iconImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
        iconImage.color = new Color(1, 1, 1, 1);
    }

    public void OnClickWindowButton()
    {
        itemPanel.SetActive(!itemPanel.activeSelf);
        UpdateItemImage(); // アイテムウィンドウが開いたときにアイテムの画像を更新
    }

    public void OnClickItemImage(int id)
    {
        if (id >= itemManager.numberOfItems) return;
        if (itemManager.collectItemList[id] == false) return;
        UpdateIcon(id);
    }

    private void Start() 
    {
        itemManager = GameObject.Find("Items").GetComponent<ItemManager>();
    }
}
