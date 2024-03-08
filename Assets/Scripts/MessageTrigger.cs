using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using Unity.VisualScripting;
using UnityEngine;

public class MessageTrigger : InteractionTrigger
{
    [SerializeField] private bool isEnemy;

    [ConditionalDisableInInspector(nameof(isEnemy), false, conditionalInvisible: true)]
    public string message;

    private Flowchart flowchart;
    private PlayerController playerController;
    private bool dontstop;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();

        string flowchartName;

        if (isEnemy)
        {
            message = "encounter";
            flowchartName = "BattleFlowchart";
        }
        else
        {
            flowchartName = "Flowchart";
        }

        GameObject flowchartObject = GameObject.Find(flowchartName);
        flowchart = flowchartObject.GetComponent<Flowchart>();

        onInteract.AddListener(InvokeBlock);
    }

    // Fungusのブロックを実行する
    IEnumerator SendMessage(Action callback)
    {
        // Flowchartから会話を呼び出し
        print(message);
        flowchart.SendFungusMessage(message);

        // 会話終了の待機
        int target = 0;
        yield return new WaitUntil(() => {
            // 現在実行されているFungusのブロックの数
            int count = flowchart.GetExecutingBlocks().Count;

            if (count == target)
            {
                // 1回目の会話終了
                if (dontstop)
                {
                    // コールバックせずに次の会話の待機に入る
                    target = 1;
                    dontstop = false;
                }
                else
                {
                    if (target == 1)
                    {
                        // 2回目の会話スタート、再び終了を待機
                        target = 0;
                    }
                    else
                    {
                        // 会話終了
                        return true;
                    }
                }
            }
            return false;
        });

        // 会話終了時にコールバック
        callback();
    }

    public void InvokeBlock()
    {
        if (message == "")
        {
            return;
        }

        playerController.DisableCanMove();

        StartCoroutine(SendMessage(() => {
            // コールバック
            playerController.EnableCanMove();
            interacted = false;
        }));
    }

    // 一回会話終了を見送る
    public void DontStopTalkOnce()
    {
        dontstop = true;
    }

    public void SetFireOnCollision(bool flag)
    {
        fireOnCollision = flag;
    }
}
