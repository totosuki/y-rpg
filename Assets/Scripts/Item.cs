using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { None, Item, Coin }

    [SerializeField]
    private ItemManager itemManager;

    [Header("アイテムの設定")]
    [SerializeField]
    private ItemType itemType = ItemType.None;
    
    [Header("アイテムのID")]
    public int itemId;

    [Header("アイテムの名前")]
    public string itemName;

    [Header("アイテムの説明")]
    [TextArea(3, 10)]
    public string description;

    private void ItemTriggerEnter() 
    {
        itemManager.AddCollectItemBit(itemId);
    }

    private void CoinTriggerEnter() 
    {
        Debug.Log("[ItemScript] コインを取った");
    }

    public void CollectItem()
    {
        switch (itemType) {
            case ItemType.Item:
                ItemTriggerEnter();
                break;
            case ItemType.Coin:
                CoinTriggerEnter();
                break;
            default:
                break;
        }

        Disable();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}