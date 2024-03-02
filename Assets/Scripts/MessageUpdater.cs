using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MessageUpdater : MonoBehaviour
{
    [SerializeField] private MessageTrigger messageTrigger;

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

    [SerializeField] private SerializableKeyPair<int,string>[] messageList = default;

    // インスペクターのターン別メッセージリストをDictonaryに変換
    private Dictionary<int, string> _messageListDictionary;
    private Dictionary<int, string> messageListDictonary => _messageListDictionary ??= messageList.ToDictionary(p => p.Key, p => p.Value);


    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameManagerObject = GameObject.Find("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();

        gameManager.onTurnUpdate.AddListener(UpdateMessage);
    }

    // MessageTriggerのmessageを更新する
    public void UpdateMessage()
    {
        int turn = gameManager.GetCurrentTurn();
        string message = GetMessageByTurn(turn);

        if (message != null)
        {
            messageTrigger.message = message;
            // インタラクト可能にする
            messageTrigger.SetInteractable(true);
        }
        else
        {
            messageTrigger.message = "";
            // 返せるメッセージが無いのでインタラクト不可にする
            messageTrigger.SetInteractable(false);
        }
    }

    public string GetMessageByTurn(int turn)
    {
        string message;

        // ターンに対応したメッセージを返す
        if (messageListDictonary.ContainsKey(turn))
        {
            message = messageListDictonary[turn];
        }
        else
        {
            // 対応するメッセージが存在しない場合はnullを返す
            message = null;
        }

        return message;
    }
}
