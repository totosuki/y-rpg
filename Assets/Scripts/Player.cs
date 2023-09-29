using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    // プレイヤー情報
    public string _name;
    // ゲーム内情報
    // TODO MP, XP, 所持金 etc
    public Dictionary<string, int> inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = new Dictionary<string, int>() {};
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // インベントリにアイテムを追加
    public void AddItem(string name, int count)
    {
        string[] keyList = new string[inventory.Keys.Count];
        inventory.Keys.CopyTo(keyList, 0);

        if (keyList.Contains(name))
        {
            inventory[name] += count;
        }
        else
        {
            inventory.Add(name, count);
        }

    }
}
