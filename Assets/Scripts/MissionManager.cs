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

    public void ShowMission(int turn)
    {
        foreach (Mission mission in missions)
        {
            if (mission.turn == turn)
            {
                missionTitle.text = mission.title;
                missionDescription.text = mission.description;
                break;
            }
        }
    }
}
