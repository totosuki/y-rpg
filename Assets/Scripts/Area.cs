using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Area : MonoBehaviour
{
    // RememberListのキー・AreaInfoでの表示
    public string areaName;
    public string subtitle;

    [Header("メッセージ設定")]
    [SerializeField]private bool sendEnterMessage;

    // ターン別メッセージリスト
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

    [Tooltip("ターン別メッセージリスト"), ConditionalDisableInInspector(nameof(sendEnterMessage), true)]
    [SerializeField] private SerializableKeyPair<int,string>[] messageList = default;

    // === === //

    // インスペクターのターン別メッセージリストをDictonaryに変換
    private Dictionary<int, string> _messageListDictionary;
    private Dictionary<int, string> messageListDictonary => _messageListDictionary ??= messageList.ToDictionary(p => p.Key, p => p.Value);


    private AreaManager areaManager;
    private bool inArea;

    private string currentTurnMessage;

    void Start()
    {
        areaManager = GetComponentInParent<AreaManager>();

        areaManager.gameManager.onTurnUpdate.AddListener(UpdateCurrentTurnMessage);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            inArea = true;

            // エリア侵入メッセージを送信
            if (sendEnterMessage)
            {
                if (currentTurnMessage != "")
                {
                    areaManager.SendOnAreaEnterMessage(currentTurnMessage);
                }
            }

            areaManager.ShowAreaInfo(areaName, subtitle);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (inArea)
            {
                inArea = false;
                areaManager.RememberArea(areaName);
            }
        }
    }

    void UpdateCurrentTurnMessage()
    {
        // ターンの更新時に呼び出される
        // currentTurnMessageを最新状態に更新
        int turn = areaManager.gameManager.GetCurrentTurn();

        if (messageListDictonary.ContainsKey(turn))
        {
            currentTurnMessage = messageListDictonary[turn];
        }
        else
        {
            currentTurnMessage = "";
        }
    }
}
