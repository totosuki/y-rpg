using Fungus;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Flowchart flowchart;

    [Header("現在のターン")]
    public int turn;

    public UnityEvent onTurnUpdate = new UnityEvent();

    private GameObject player;

    private MissionManager missionManager;

    void Start()
    {
        player = GameObject.Find("Player");
        missionManager = GameObject.Find("MissionView").GetComponent<MissionManager>();
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
        TellTurnUpdate(); // ここでターン更新を通知
    }

    // ターン更新を通知する処理
    private void TellTurnUpdate()
    {
        missionManager.ListenTurnUpdate();
    }
}
