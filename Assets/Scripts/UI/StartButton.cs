using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class StartButton : MonoBehaviour {
    [SerializeField]
    private Flowchart flowchart;

    private Button button;

    void Start() {
        button = GetComponent<Button>();
    }

    // ゲームスタート
    public void GameStart() {
        flowchart.SendFungusMessage("start");
        button.interactable = false;
    }
}
