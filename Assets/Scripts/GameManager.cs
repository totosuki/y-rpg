using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    private PlayerController plController;
    private GameObject mainCamera;

    private GameObject adventurer;
    private GameObject pachi;

    private GameObject saveMenu;
    private GameObject xpBar;
    private GameObject tutorialUI;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        plController = player.GetComponent<PlayerController>();
        mainCamera = GameObject.Find("Main Camera");
        adventurer = GameObject.Find("Adventurer");
        pachi = GameObject.Find("Pachi");

        saveMenu = GameObject.Find("SaveMenu");
        xpBar = GameObject.Find("XPBar");
        tutorialUI = GameObject.Find("Tutorial");

        plController.Hide();
        adventurer.SetActive(false);
        saveMenu.SetActive(false);
        xpBar.SetActive(false);
        // tutorialUI.SetActive(false);
    }

    public void FinishPrologue()
    {
        plController.Show();
        CameraController camera = mainCamera.GetComponent<CameraController>();
        camera.StopOpening();
        saveMenu.SetActive(true);
        xpBar.SetActive(true);
    }

    public void TalkToAdventurer()
    {
        adventurer.SetActive(true);
        // adventurer.GetComponent<NPCAnimController>().Walk("left", 1.2f);
    }
}
