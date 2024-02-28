using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { None, Item, Coin }

    [Header("アイテム設定")]
    [SerializeField]
    private ItemType itemType = ItemType.None;

    private string playerTag = "Player";

    private void ItemTriggerEnter() {
        Debug.Log("[ItemScript] アイテムを取った");
    }

    private void CoinTriggerEnter() {
        Debug.Log("[ItemScript] コインを取った");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            Debug.Log("[ItemScript] OnTriggerEnter2D");

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

            Destroy(gameObject);
        }
    }
}
