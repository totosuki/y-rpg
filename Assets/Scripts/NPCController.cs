using System.Collections;
using System;
using UnityEngine;
using Fungus;


public class NPCController : MonoBehaviour {
    [Tooltip("会話開始Fungusメッセージ")]
    public string message;

    // Fungus
    public Flowchart flowchart;

    private GameObject player;
    private PlayerController plc;

    [SerializeField] private GameObject popup;

    // 設定項目 インスペクターでいじるだけ
    [Tooltip("会話できるかどうか")]
    [SerializeField] private bool talkable;

    [Tooltip("当たり判定への侵入をトリガーに会話を始めるかどうか")]
    [SerializeField] private bool FireOnCollision;
    
    [Tooltip("会話終了後にEnableCanMove()するかどうか")]
    [SerializeField] private bool canCallback;


    // 会話可能圏内に入っているかどうか
    private bool canTalk;
    // 会話中かどうか
    private bool isTalking;


    void Start() {
        player = GameObject.Find("Player");
        plc = player.GetComponent<PlayerController>();
        popup = transform.GetChild(0).gameObject;
    }

    void Update() {
        if (canTalk && talkable && !isTalking) {
            if (Input.GetMouseButtonDown(0) || FireOnCollision) {
                StartTalk();
                // 無限ループ防止
                if (FireOnCollision) FireOnCollision = false;
            }
            popup.SetActive(true);
        }
        else {
            popup.SetActive(false);
        }
    }

    // Fungusを呼び出して会話を始める
    IEnumerator Talk(Action callback) {
        isTalking = true;
        canCallback = true;

        flowchart.SendFungusMessage(message);
        yield return new WaitUntil(() => flowchart.GetExecutingBlocks().Count == 0);

        if (canCallback) callback();

        isTalking = false;
    }

    public void StartTalk() {
        plc.DisableCanMove();

        StartCoroutine(Talk(() => {
            // callback
            plc.EnableCanMove();
        }));
    }

    // 当たり判定
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") canTalk = true;
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player") canTalk = false;
    }
    
    // EnemyControllerから
    public bool GetCanTalk() {
        return canTalk;
    }

    public void DisableCanActivate() {
        canTalk = false;
    }

    // 呼び出すと一回だけコールバックを無効化
    public void DisableCallback() {
        canCallback = false;
    }
}
