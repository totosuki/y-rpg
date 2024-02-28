using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<bool> collectItemList = new List<bool>();

    [Header("アイテムの数")]
    public int numberOfItems = 3;

    private int collectItemBit;


    public void AddCollectItemBit(int itemId) 
    {
        // 0b1000 | 0b0100 = 0b1100 となる
        // また、0b0100 == 1 << 2 である
        collectItemBit |= 1 << itemId;
        collectItemList[itemId] = true;

        Debug.Log("[ItemManager] collectItemBit : " + collectItemBit);
    }

    private void InitCollectItem() // collectItemBitの初期化用
    {
        collectItemBit = 0;
        for (int i = 0; i < numberOfItems; i++) 
        {
            collectItemList[i] = false;
        }
    }

    void Awake() 
    {
        collectItemBit = PlayerPrefs.GetInt("CollectItemBit", 0);
        for (int i = 0; i < numberOfItems; i++) 
        {
            collectItemList.Add((collectItemBit & (1 << i)) != 0);
        }

        InitCollectItem(); // デバッグで初期化したいときはコメントアウトを外す
    }

    void OnApplicationQuit() 
    {
        PlayerPrefs.SetInt("CollectItemBit", collectItemBit);
    }
}
