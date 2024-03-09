using UnityEngine;

public class EntityStatus : MonoBehaviour {

    public string _name;
    public int lv;
    public int hp;

    public void SetName(string name)
    {
        _name = name;
    }

    public void SetLv(int _lv)
    {
        lv = _lv;
    }

    public void SetHp(int _hp)
    {
        hp = _hp;
    }
}