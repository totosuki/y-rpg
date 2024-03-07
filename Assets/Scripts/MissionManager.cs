using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

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
    private CanvasGroup canvasGroup;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        canvasGroup = GetComponent<CanvasGroup>();
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

        StartCoroutine(FadeIn());
    }

    // ターン更新に反応する処理
    public void ListenTurnUpdate()
    {
        int turn = gameManager.turn;
        ShowMission(turn);
    }

    public IEnumerator FadeIn()
    {
        float t = 0f;
        canvasGroup.alpha = t;

        while (t < 1f)
        {
            t += 0.005f;
            canvasGroup.alpha = t;
            transform.localPosition += new Vector3(t/25, 0);

            yield return null;
        }
    }
}
