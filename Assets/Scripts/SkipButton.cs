using UnityEngine;
using Fungus;

public class SkipButton : MonoBehaviour
{
    public Flowchart flowchart;
    public string nextBlockName;

    void Start()
    {
        Init();
    }

    void Init()
    {
        gameObject.SetActive(true);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Onclick()
    {
        gameObject.SetActive(false);

        // 現在のブロックを停止する
        flowchart.StopAllBlocks();
        // オープニングに遷移
        flowchart.ExecuteBlock(nextBlockName);
    }
}
