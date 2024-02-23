using System.Collections;
using System;
using UnityEngine;
using Fungus;


public class NPCController : MonoBehaviour {
    // Fungus
    public Flowchart flowchart;

    private GameObject player;
    private PlayerController plc;

    [SerializeField] private GameObject popup;

    private enum TypeEnum { NPC, ACTOR_NPC, ENEMY }

    // === NPC設定 インスペクターでいじるだけ === //
    [Header("NPC設定")]

    [Tooltip("NPCのタイプ")]
    [SerializeField]private TypeEnum type = TypeEnum.NPC;

    // never used 警告避け
    TypeEnum Pass() => type;
    
    [Tooltip("会話できるかどうか"), ConditionalDisableInInspector(nameof(type), (int)TypeEnum.NPC, conditionalInvisible: true)]
    [SerializeField] private bool talkable;

    [Tooltip("当たり判定への侵入をトリガーに会話を始めるかどうか"), ConditionalDisableInInspector(nameof(type), (int)TypeEnum.NPC, conditionalInvisible: true)]
    [SerializeField] private bool fireOnCollision;
    
    [Tooltip("会話終了後にEnableCanMove()するかどうか"), ConditionalDisableInInspector(nameof(type), (int)TypeEnum.NPC, conditionalInvisible: true)]
    [SerializeField] private bool enableCanMoveAfterTalk;

    [Tooltip("会話開始Fungusメッセージ"), ConditionalDisableInInspector(nameof(type), (int)TypeEnum.ACTOR_NPC, notEqualThenEnable: true, conditionalInvisible: true)]
    public string message;

    // === === //

    // 会話可能圏内に入っているかどうか
    private bool canTalk;
    // 会話中かどうか
    private bool isTalking;


    void Start()
    {
        player = GameObject.Find("Player");
        plc = player.GetComponent<PlayerController>();
        popup = transform.GetChild(0).gameObject;

        print(message);
        print(fireOnCollision);
    }

    void Update()
    {
        // 会話可能 && 会話可能圏内にいる && 会話中でない
        if (talkable && canTalk && !isTalking)
        {
            // クリックされたらいつでも会話できる状態にする
            if (Input.GetMouseButtonDown(0) || fireOnCollision)
            {
                StartTalk();
                // 無限ループ防止
                if (fireOnCollision)
                {
                    fireOnCollision = false;
                }
            }
            popup.SetActive(true);
        }
        else
        {
            popup.SetActive(false);
        }
    }

    // Fungusを呼び出して会話を始める
    IEnumerator Talk(Action callback)
    {
        // 会話開始
        isTalking = true;

        // Flowchartから会話を呼び出し
        flowchart.SendFungusMessage(message);
        yield return new WaitUntil(() => flowchart.GetExecutingBlocks().Count == 0);

        // 会話終了時にコールバック
        callback();

        // 会話終了
        isTalking = false;
    }

    public void StartTalk()
    {
        // プレイヤーの移動を制限
        plc.DisableCanMove();

        StartCoroutine(Talk(() => {
            // コールバック
            if (enableCanMoveAfterTalk)
            {
                plc.EnableCanMove();
            }
        }));
    }

    // 当たり判定
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") canTalk = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") canTalk = false;
    }
    
    // EnemyControllerから
    public bool GetCanTalk()
    {
        return canTalk;
    }
}
