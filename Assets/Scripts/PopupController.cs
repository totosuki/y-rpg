using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] private MessageTrigger messageTrigger;
    
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 会話可能圏内にいるかつメッセージを持っている時、ポップアップを表示
        if (messageTrigger.CanInteract() && messageTrigger.message != "")
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}