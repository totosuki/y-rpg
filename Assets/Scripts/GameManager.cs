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
        player = GameObject.Find("Player");
    }

    public int GetCurrentTurn()
    {
        return turn;
    }

    public void OnSave()
    {
        turn = flowchart.GetIntegerVariable("turn");

        flowchart.SetFloatVariable("player_x", player.transform.position.x);
        flowchart.SetFloatVariable("player_y", player.transform.position.y);

        onTurnUpdate.Invoke();
    }

    public void OnLoad()
    {
        float x = flowchart.GetFloatVariable("player_x");
        float y = flowchart.GetFloatVariable("player_y");

        player.transform.position = new Vector3(x, y);
    }
}
