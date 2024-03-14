using Fungus;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Flowchart flowchart;
    [SerializeField] private int turn;

    public UnityEvent onTurnUpdate = new UnityEvent();

    private GameObject player;

    void Start()
    {
        // 起動するたびにゲームをリスタート
        // 初期化後のみturn == 0になるため、turnで初期化されているかどうかを判断
        // if (flowchart.GetIntegerVariable("turn") != 0)
        // {
        //     GameObject.Find("SaveMenu").GetComponent<SaveMenu>().Restart();
        // }
        
        player = GameObject.Find("Player");
    }

    public int GetCurrentTurn()
    {
        return turn;
    }

    public void OnSave()
    {
        flowchart.SetFloatVariable("player_x", player.transform.position.x);
        flowchart.SetFloatVariable("player_y", player.transform.position.y);
    }

    public void OnLoad()
    {
        turn = flowchart.GetIntegerVariable("turn");
        
        float x = flowchart.GetFloatVariable("player_x");
        float y = flowchart.GetFloatVariable("player_y");

        player.transform.position = new Vector3(x, y);

        // ロード時にも一度ターン更新イベントを発生させる
        InvokeOnTurnUpdate();
    }

    public void SendFungusMessage(string message)
    {
        print("send " + message);
        flowchart.SendFungusMessage(message);
    }

    // GameManagerのターンを同期する処理
    public void SetTurn(int _turn)
    {
        turn = _turn;
    }

    public void InvokeOnTurnUpdate()
    {
        onTurnUpdate.Invoke();
    }
}
