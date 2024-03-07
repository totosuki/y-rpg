using UnityEngine;
using Fungus;

public class SkipButtonManager : MonoBehaviour
{
    public Flowchart flowchart;
    private string nextBlockName;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 「SetSkip」コマンドから呼び出し
    /// 遷移先のブロック名を登録
    /// </summary>
    /// <param name="blockName"></param>
    public void SetNextBlock(string blockName)
    {
        nextBlockName = blockName;
    }

    public void Onclick()
    {
        gameObject.SetActive(false);

        // 現在のブロックを停止する
        flowchart.StopAllBlocks();
        // 指定された次のブロックに移動
        flowchart.ExecuteBlock(nextBlockName);
    }
}
