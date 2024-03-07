using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class SettingMenu : MonoBehaviour
{
    public GameObject settingPanel;
    public Slider volumeSlider;

    public AudioSource story;
    public AudioSource talk;

    public PlayerController player;

    private MusicManager musicManager;


    void Start()
    {
        musicManager = FungusManager.Instance.MusicManager;

        CloseSettingMenu();
    }

    public void OpenSettingMenu()
    {
        player.DisableCanMove();
        settingPanel.SetActive(true);
    }

    public void CloseSettingMenu()
    {
        player.EnableCanMove();
        settingPanel.SetActive(false);
    }

    public void OnVolumeChanged()
    {
        SetVolume(volumeSlider.value);
    }

    // 音量を設定
    void SetVolume(float volume)
    {
        musicManager.SetAudioVolume(volume, 0, () => {});

        story.volume = volume;
        talk.volume = volume;
    }
}
