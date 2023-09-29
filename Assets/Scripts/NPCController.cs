using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Fungus;


public class NPCController : MonoBehaviour
{
    public string message;

    // Fungus
    [SerializeField]
    private Flowchart flowchart;

    public GameObject player;
    private PlayerController plc;

    [SerializeField]
    private GameObject popup;

    // 会話できるかどうか
    [SerializeField]
    private bool canTalk;
    // 当たり判定への侵入をトリガーに会話を始めるかどうか
    [SerializeField]
    private bool FireOnCollision;
    
    public bool canActivate;
    public bool canCallback;
    private bool isRunning;


    void Start()
    {
        plc = player.GetComponent<PlayerController>();
        popup = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (canActivate && canTalk && !isRunning)
        {
            if (Input.GetMouseButtonDown(0) || FireOnCollision)
            {
                StartTalk();
                // 無限ループ防止
                if (FireOnCollision) FireOnCollision = false;
            }
            popup.SetActive(true);
        }
        else
        {
            popup.SetActive(false);
        }
    }

    public void StartTalk()
    {
        plc.DisableCanMove();

        StartCoroutine(Talk(() => {
            // callback
            plc.EnableCanMove();
        }));
    }

    // 当たり判定
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") canActivate = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") canActivate = false;
    }

    public void DisableCanActivate()
    {
        canActivate = false;
    }

    // Fungusを呼び出して会話を始める
    IEnumerator Talk(System.Action callback)
    {
        isRunning = true;
        canCallback = true;

        flowchart.SendFungusMessage(message);
        yield return new WaitUntil(() => flowchart.GetExecutingBlocks().Count == 0);

        if (canCallback) callback();

        isRunning = false;
    }

    // 呼び出すと一回だけコールバックを無効化
    public void DisableCallback()
    {
        canCallback = false;
    }
}
