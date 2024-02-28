using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { None, Item, Coin }

    [Header("アイテム設定")]
    [SerializeField]
    private ItemType itemType = ItemType.None;

    private void ItemTriggerEnter() 
    {
        Debug.Log("[ItemScript] アイテムを取った");
    }

    private void CoinTriggerEnter() 
    {
        Debug.Log("[ItemScript] コインを取った");
    }

    public void GetItem() 
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            GetItem();
            Destroy(gameObject);
        }
    }
}
