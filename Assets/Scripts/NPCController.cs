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

    public enum TypeEnum { NPC, ACTOR_NPC, ENEMY }

    // === NPC設定 インスペクターでいじるだけ === //
    [Header("NPC設定")]

    [Tooltip("NPCのタイプ")]
    [SerializeField]private TypeEnum type = TypeEnum.NPC;

    // never used 警告避け
    TypeEnum Pass() => type;
    
    [Tooltip("会話できるかどうか"), ConditionalDisableInInspector(nameof(type), (int)TypeEnum.NPC, conditionalInvisible: true)]
    [SerializeField] private bool _talkable;

    [Tooltip("当たり判定への侵入をトリガーに会話を始めるかどうか"), ConditionalDisableInInspector(nameof(type), (int)TypeEnum.NPC, conditionalInvisible: true)]
    [SerializeField] private bool _fireOnCollision;
    
    [Tooltip("会話終了後にEnableCanMove()するかどうか"), ConditionalDisableInInspector(nameof(type), (int)TypeEnum.NPC, conditionalInvisible: true)]
    [SerializeField] private bool _enableCanMoveAfterTalk;

    [Tooltip("会話開始Fungusメッセージ"), ConditionalDisableInInspector(nameof(type), (int)TypeEnum.ACTOR_NPC, notEqualThenEnable: true, conditionalInvisible: true)]
    public string message;

    // === === //

    // 会話可能圏内に入っているかどうか
    private bool canTalk = false;

    // 実際に使うのはこっち
    private bool talkable;
    private bool fireOnCollision;
    private bool enableCanMoveAfterTalk;

    public bool dontstop;

    void Start()
    {
        player = GameObject.Find("Player");
        plc = player.GetComponent<PlayerController>();
        popup = transform.GetChild(0).gameObject;
        // 設定を適用
        SetTypeTo(type);
    }

    void Update()
    {
        // 会話可能 && 会話可能圏内にいる
        if (talkable && canTalk)
        {
            // クリックされたらいつでも会話できる状態にする
            if (Input.GetMouseButtonDown(0) || fireOnCollision)
            {
                StartTalk();
            }
            popup.SetActive(true);
        }
        else
        {
            popup.SetActive(false);
        }
    }

    // タイプ別の設定を適用する
    public void SetTypeTo(TypeEnum _type)
    {
        type = _type;
        
        switch (type)
        {
            case TypeEnum.NPC:
                SetSetting(_talkable, _fireOnCollision, _enableCanMoveAfterTalk);
                break;

            case TypeEnum.ACTOR_NPC:
                SetSetting(false, false, false);
                break;

            case TypeEnum.ENEMY:
                SetSetting(false, false, false);
                break;

        }   
    }

    void SetSetting(bool _talkable, bool _fireOnCollision, bool _enableCanMoveAfterTalk)
    {
        talkable = _talkable;
        fireOnCollision = _fireOnCollision;
        enableCanMoveAfterTalk = _enableCanMoveAfterTalk;
    }

    // Fungusを呼び出して会話を始める
    IEnumerator Talk(Action callback)
    {
        // 会話開始
        SetTypeTo(TypeEnum.ACTOR_NPC);

        // Flowchartから会話を呼び出し
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

        // 会話終了
        SetTypeTo(TypeEnum.NPC);

        // 無限ループ防止
        if (fireOnCollision)
        {
            fireOnCollision = false;
        }

        // 会話終了時にコールバック
        callback();
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

    // 一回会話終了を見送る
    public void DontStopTalkOnce()
    {
        dontstop = true;
    }
}
