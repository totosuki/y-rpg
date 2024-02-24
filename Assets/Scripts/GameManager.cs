using Fungus;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Flowchart flowchart;
    [SerializeField] private int turn;

    [SerializeField] private GameObject playerObject;

    public UnityEvent onTurnUpdate = new UnityEvent();

    private bool isCalledOnce = false;

    void Update()
    {
        // 一通りの読み込みが終わった後で更新をかける
        if (!isCalledOnce)
        {
            SetTurnTo(turn);
            isCalledOnce = true;
        }
    }

    public int GetCurrentTurn()
    {
        return turn;
    }

    public void SetTurnTo(int _turn)
    {
        flowchart.SetStringVariable("game_turn", $"{_turn}");
        turn = _turn;

        onTurnUpdate.Invoke();
    }

    // public void OnSave()
    // {
    //     flowchart.SetGameObjectVariable("player_transform", playerObject);
    // }

    // public void OnLoad()
    // {
    //     print(playerObject.transform.position);
    //     print(flowchart.GetGameObjectVariable("player_transform").transform.position);
    // }
}
