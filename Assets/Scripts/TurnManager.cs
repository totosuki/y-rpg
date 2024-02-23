using Fungus;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{

    [SerializeField] private Flowchart flowchart;
    [SerializeField] private int turn;

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
        flowchart.SetIntegerVariable("turn", _turn);
        turn = _turn;

        onTurnUpdate.Invoke();
    }
}
