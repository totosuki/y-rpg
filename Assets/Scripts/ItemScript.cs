using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public enum ItemType { None, Coin }

    [Header("アイテム設定")]
    [SerializeField]
    private ItemType itemType = ItemType.None;

    private string playerTag = "Player";

    private void CoinTriggerEnter() {
        Debug.Log("[ItemScript] コインを取った");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            Debug.Log("[ItemScript] OnTriggerEnter2D");

            switch (itemType) {
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
