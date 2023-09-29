using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fungus;

// 注意：冗長オブザイヤーなコードしてます。
//      でもこれで動くから許して

public class InputManager : MonoBehaviour
{
    // fungus
    [SerializeField]
    private Flowchart flowchart;

    [SerializeField]
    private GameObject input;
    private TMP_InputField _inputField;

    [SerializeField]
    private GameObject player;
    private PlayerController plc;
    private Player pl;

    [SerializeField]
    private GameObject npc;
    private NPCController npcController;

    void Start()
    {
        plc = player.GetComponent<PlayerController>();
        pl = player.GetComponent<Player>();
        npcController = npc.GetComponent<NPCController>();

        _inputField = input.GetComponent<TMP_InputField>();
        InitInput();
    }

    public void GetInput()
    {
        string name = _inputField.text;

        // 空文字列は返却
        if (name == "") 
        {
            RetryInput();
            return;
        }

        InitInput();
        flowchart.SetStringVariable("player_name", name);
        npcController.message = "register";
        npcController.StartTalk();
    }

    public void RetryInput()
    {
        InitInput();
        npcController.message = "retry";
        npcController.StartTalk();
    }

    public void SetName()
    {
        pl._name = flowchart.GetStringVariable("player_name");
        npcController.message = "registerCompleted";
    }

    void InitInput()
    {
        input.SetActive(false);
        _inputField.text = "";
    }

    void NameInput()
    {
        npcController.DisableCallback();

        npcController.DisableCanActivate();
        input.SetActive(true);
    }
}
