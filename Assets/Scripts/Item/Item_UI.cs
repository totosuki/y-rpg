using UnityEngine;

public class Item_UI : MonoBehaviour
{
    [SerializeField]
    private GameObject obj;
    void Start(){
        obj.SetActive(false);
    }
    
    public void click()
    {
        if(obj.activeSelf){
            obj.SetActive(false);
        }else{
            obj.SetActive(true);
        }
    }
}