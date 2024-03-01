using UnityEngine;

public class EntityStatus : MonoBehaviour {

    public string _name;
    public int lv;
    public int hp;

    public void SetName(string name)
    {
        this._name = name;
    }
}