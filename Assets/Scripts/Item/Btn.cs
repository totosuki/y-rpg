using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn : MonoBehaviour
{
    [SerializeField]
    private GameObject obj;
    public void btn(){
        obj.SetActive(false);
    }
}
