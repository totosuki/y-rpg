using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionManager : MonoBehaviour
{
    public TextMeshProUGUI missionTitle;
    public TextMeshProUGUI missionDescription;


    [System.Serializable]
    private class Mission
    {
        public int turn;
        public string title;
        public string description;
    }

    [SerializeField]
    [Header("ミッション一覧")]
    private List<Mission> missions;
    
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void ShowMission(int turn)
    {
        bool isChange = false;

        foreach (Mission mission in missions)
        {
            if (mission.turn == turn)
            {
                missionTitle.text = mission.title;
                missionDescription.text = mission.description;
                isChange = true;
                break;
            }
        }

        if (!isChange)
        {
            missionTitle.text = "";
            missionDescription.text = "";
        }
    }

    // ターン更新に反応する処理
    public void ListenTurnUpdate()
    {
        int turn = gameManager.turn;
        ShowMission(turn);
    }
}
