using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHandler : MonoBehaviour
{
    private Transform[] NPCList;

    // Start is called before the first frame update
    void Start()
    {
        NPCList = GetChildren();
    }

    // 子オブジェクトを一括で取得
    Transform[] GetChildren()
    {
        var children = new Transform[transform.childCount];
        var childIndex = 0;

        // 子オブジェクトを順番に配列に格納
        foreach (Transform child in transform)
        {
            children[childIndex++] = child;
        }

        return children;
    }
}
