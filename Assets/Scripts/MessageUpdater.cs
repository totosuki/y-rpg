using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageUpdater : MonoBehaviour
{
    [SerializeField] private MessageTrigger messageTrigger;

    // messageListの書式設定
    // reception[2:5]
    // reception[2,6,8]
    // reception[2]
    public List<string> messageList;

    private Dictionary<int,string> messageListDictionary = new Dictionary<int, string>();

    private GameManager gameManager;

    private void initMessageList() 
    {
        foreach (string text in messageList)
        {
            string message = "";
            int startId = 0;

            // Messageを見つける
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '[')
                {
                    startId = i+1;
                    break;
                }
                message += text[i];
            }

            // ここでmessageListの書式を判定する
            int pattern = text.Contains(":") ? 1 : text.Contains(",") ? 2 : 3;

            switch (pattern)
            {
                case 1:
                    string[] range = text.Substring(startId, text.Length-startId-1).Split(':');
                    int start = int.Parse(range[0]);
                    int end = int.Parse(range[1]);
                    for (int id1 = start; id1 <= end; id1++) messageListDictionary[id1] = message;
                    break;
                case 2:
                    string[] ids = text.Substring(startId, text.Length-startId-1).Split(',');
                    foreach (string id2 in ids) messageListDictionary[int.Parse(id2)] = message;
                    break;
                case 3:
                    int id3 = int.Parse(text.Substring(startId, text.Length-startId-1));
                    messageListDictionary[id3] = message;
                    break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameManagerObject = GameObject.Find("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();

        gameManager.onTurnUpdate.AddListener(UpdateMessage);

        initMessageList();
    }

    // MessageTriggerのmessageを更新する
    public void UpdateMessage()
    {
        int turn = gameManager.GetCurrentTurn();
        string message = GetMessageByTurn(turn);

        if (message != null)
        {
            messageTrigger.message = message;
        }
        else
        {
            messageTrigger.message = "";
        }
    }

    public string GetMessageByTurn(int turn)
    {
        string message;

        // ターンに対応したメッセージを返す
        if (messageListDictionary.ContainsKey(turn))
        {
            message = messageListDictionary[turn];
        }
        else
        {
            // 対応するメッセージが存在しない場合はnullを返す
            message = null;
        }

        return message;
    }
}
