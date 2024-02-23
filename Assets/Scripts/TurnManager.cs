using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    [SerializeField] private Flowchart flowchart;
    [SerializeField] private int turn;

    void Start()
    {
        SetTurnTo(turn);
    }

    public void SetTurnTo(int _turn)
    {
        flowchart.SetIntegerVariable("turn", _turn);
        turn = _turn;
    }
}
