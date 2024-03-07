using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] private InteractionTrigger interactionTrigger;
    
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 会話可能圏内にいる時、ポップアップを表示
        if (interactionTrigger.CanInteract())
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}