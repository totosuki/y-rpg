using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalManager : MonoBehaviour
{
    public bool isCritical;
    
    [SerializeField] private GameObject pointer;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == pointer)
        {
            isCritical = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject == pointer)
        {
            isCritical = false;
        }
    }
}
