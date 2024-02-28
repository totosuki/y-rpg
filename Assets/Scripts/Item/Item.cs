using UnityEngine;

public class Item : MonoBehaviour {
    [SerializeField]
    private bool discovery = false; // 発見したか？(最初は持っていない為だが、保存データ参照にしないとセーブの意味がない)
    [SerializeField]
    private int id = 0;
    [SerializeField]
    private string imagePath = "";
    [SerializeField]
    private GameObject obj;


    void Start() {
        gameObject.SetActive(discovery);
    }

    void getItem() {
        discovery = true;
        gameObject.SetActive(discovery);
    }
    
    public void click() {
        obj.SetActive(true);
    }
}
