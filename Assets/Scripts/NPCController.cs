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

    [Tooltip("当たり判定への侵入をトリガーに会話を始めるかどうか"), ConditionalDisableInInspector(nameof(type), (int)TypeEnum.NPC, conditionalInvisible: true)]
    [SerializeField] private bool fireOnCollision;

    // メッセージ設定
    [Header("メッセージ設定")]

    [Tooltip("ターン別メッセージを使用可能にする"), ConditionalDisableInInspector(nameof(type), (int)TypeEnum.NPC, conditionalInvisible: true)]
    [SerializeField] private bool multipleMessages;

    [Tooltip("単体メッセージ"), ConditionalDisableInInspector(nameof(multipleMessages), false)]
    public string message;

    // ターン:メッセージの組
    [Serializable]
    public class SerializableKeyPair<TKey, TValue>
    {
        [Tooltip("このメッセージが適用されるターン")]
        [SerializeField] private TKey turn;
        [Tooltip("このターンに適用されるメッセージ")]
        [SerializeField] private TValue message;

        public TKey Key => turn;
        public TValue Value => message;
    }

    [Tooltip("ターン別メッセージリスト"), ConditionalDisableInInspector(nameof(multipleMessages), true)]
    [SerializeField] private SerializableKeyPair<int,string>[] messageList = default;

    // === === //

    // 会話可能圏内に入っているかどうか
    private bool inCollision = false;
    // 会話を止めないフラグ
    private bool dontstop;

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
        if (type == TypeEnum.NPC && IsPlayerInCollision())
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

    // 新しいタイプを適用する
    public void SetTypeTo(TypeEnum _type)
    {
        type = _type;
    }

    // Fungusを呼び出して会話を始める
    IEnumerator Talk(Action callback)
    {
        // 会話開始
        SetTypeTo(TypeEnum.ACTOR_NPC);

        // Flowchartから会話を呼び出し
        SendFungusMessage();

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
            plc.EnableCanMove();
        }));
    }

    void SendFungusMessage()
    {
        if (multipleMessages)
        {
            // TODO リストから適したメッセージを送信
            return;
        }
        else
        {
            flowchart.SendFungusMessage(message);
        }
    }

    // 当たり判定
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") inCollision = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") inCollision = false;
    }
    
    // EnemyControllerから
    public bool IsPlayerInCollision()
    {
        return inCollision;
    }

    // 一回会話終了を見送る
    public void DontStopTalkOnce()
    {
        dontstop = true;
    }
}
