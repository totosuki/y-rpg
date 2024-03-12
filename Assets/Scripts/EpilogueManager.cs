using System.Collections;
using UnityEngine;

public class EpilogueManager : MonoBehaviour
{
    [Header("エピローグのキャンバス")]
    public GameObject epilogueCanvas;

    [Header("エピローグのテキスト")]
    public GameObject epilogueText;

    [Header("エピローグの速度")]
    public float speed;

    [Header("エピローグのスクロールが終わる位置")]
    public float endPosition;

    public void StartEpilogue()
    {
        Debug.Log("StartEpilogue");
        epilogueCanvas.SetActive(true);

        // スクロール開始！！
        StartCoroutine(ScrollEpilogue());
    }

    private IEnumerator ScrollEpilogue() 
    {
        while (true) 
        {
            Vector3 position = epilogueText.transform.localPosition;
            position.y += 0.1f * speed;
            epilogueText.transform.localPosition = position;
            if (position.y > endPosition) break; // ある程度までスクロールしたら終了
            yield return new WaitForSeconds(0.05f);
        }
    }
}
