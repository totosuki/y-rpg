using UnityEngine;

public class BattleLoader : MonoBehaviour
{
    public Fader base1;
    public Fader base2;
    public Fader player;
    public Fader enemy;
    public Mover line1;
    public Mover line2;
    public ToggleDisplay status1;
    public ToggleDisplay status2;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void LoadBattle()
    {
        gameObject.SetActive(true);

        base1.InvokeFade();
        base2.InvokeFade();
        player.InvokeFade();
        line1.InvokeMove();
        line2.InvokeMove();
        status1.InvokeToggle();
        status2.InvokeToggle();
        enemy.InvokeFade();
    }

    public void endBattle()
    {
        gameObject.SetActive(false);
    }
}
