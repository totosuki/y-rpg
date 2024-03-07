using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Tooltip("移動Speed")]
    [SerializeField] private int MoveSpeed;

    [Tooltip("移動可能かどうかを制御する")]
    public bool canMove;

    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private Renderer _renderer;


    void Start() {
        rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update() {
        if(canMove) Move();
    }


    void Move() {
        // プレイヤーの移動
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical")).normalized * MoveSpeed;
        // アニメーション
        Animation();
    }

    // プレイヤーのアニメーション
    void Animation() {
        // 入力なし
        if(rb.velocity == Vector2.zero) {
            StopAnimation();
            return;
        }

        if(Input.GetAxisRaw("Horizontal") != 0) {
            if(Input.GetAxisRaw("Horizontal") > 0) {
                SetAnimParameters(Vector2.right);
            }
            else {
                SetAnimParameters(Vector2.left);
            }
        }
        else if(Input.GetAxisRaw("Vertical") > 0) {
            SetAnimParameters(Vector2.up);
        }
        else {
            SetAnimParameters(Vector2.down);
        }
    }

    // animatorにvector2の値をそれぞれ代入
    void SetAnimParameters(Vector2 vector2) {
        // Animatorを有効化
        animator.speed = 1.0f;
        animator.SetFloat("X", vector2.x);
        animator.SetFloat("Y", vector2.y);
    }

    public void DisableCanMove() {
        canMove = false;
        rb.velocity = Vector2.zero;
        StopAnimation();
    }

    public void EnableCanMove() {
        canMove = true;
    }

    public void StopAnimation() {
        int hashName = animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
        animator.Play(hashName,0,0.5f);
        animator.speed = 0.0f;
    }

    public void Show() {
        _renderer.enabled = true;
    }

    public void Hide() {
        _renderer.enabled = false;
    }
}
