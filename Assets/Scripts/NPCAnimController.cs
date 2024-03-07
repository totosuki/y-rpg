using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimController : MonoBehaviour
{
    // up, left, right, down
    public string facing;
    public float speed;

    private int currentFacingId;
    private bool isWalking;

    [SerializeField]
    private Animator animator;
    private Rigidbody2D rb;
    private NPCController npcController;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        npcController = GetComponent<NPCController>();
    }

    void Update()
    {
        if (isWalking)
        {
            rb.velocity = GetVectorById(currentFacingId) * speed;  
        }
    }

    Vector2 GetVectorById(int fId)
    {
        Dictionary<int, Vector2> vectorList = new Dictionary<int, Vector2>() {
            {0, new Vector2(0, -1)},
            {1, new Vector2(-1, 0)},
            {2, new Vector2(1, 0)},
            {3, new Vector2(0, 1)}
        };

        return vectorList[fId];
    }

    int getFacingId(string facing)
    {
        Dictionary<string, int> facingList = new Dictionary<string, int>() {
            {"up", 0}, {"left", 1}, {"right", 2}, {"down", 3}
        };

        return facingList[facing];
    }

    // 歩行アニメーション
    public void Walk(string facing, float duration)
    {
        StartWalk(getFacingId(facing));
        // {duration}秒後停止
        Invoke(nameof(StopWalk), duration);
    }

    public void StartWalk(int f)
    {   
        // 指定範囲外
        if (f > 3) return;

        isWalking = true;

        SetFacing(f);
        animator.SetBool("walking", true);
    }

    public void StopWalk()
    {
        isWalking = false;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0.0f;

        SetFacing(-1);
        animator.SetBool("walking", false);
    }

    public void SetFacing(int facingId)
    {
        currentFacingId = facingId;
        animator.SetInteger("facing", facingId);
    }
}
