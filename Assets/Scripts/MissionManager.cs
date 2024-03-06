using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
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
}
