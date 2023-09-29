using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [SerializeField,Tooltip("移動スピード")]
    private int MoveSpeed;

    public bool canMove;

    [SerializeField]
    private Animator PlayerAnim;

    public Rigidbody2D rb;

    private Vector2 velocityMemory;

    private Dictionary<Vector2, Sprite> spriteDict;

    // Start is called before the first frame update
    void Start()
    {
        velocityMemory = Vector2.zero;

        Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/pipo-charachip021e");

        spriteDict = new Dictionary<Vector2, Sprite>(){
            {Vector2.up, FindSprite(sprites, "pipo-charachip021e_10")},
            {Vector2.down, FindSprite(sprites, "pipo-charachip021e_1")},
            {Vector2.left, FindSprite(sprites, "pipo-charachip021e_4")},
            {Vector2.right, FindSprite(sprites, "pipo-charachip021e_7")},
        };
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove) Move();
    }

    void Move()
    {
        // プレイヤーの移動
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical")).normalized * MoveSpeed;

        // アニメーション
        Animation(velocityMemory);

        // 次回Update用にvelocityを記憶させる
        velocityMemory = rb.velocity;
    }

    // プレイヤーのアニメーション
    void Animation(Vector2 memory)
    {
        // 入力なし
        if(rb.velocity == Vector2.zero)
        {   
            StopAnimation(memory);
        }
        else if(Input.GetAxisRaw("Horizontal") != 0)
        {

            if(Input.GetAxisRaw("Horizontal") > 0)
            {
                SetVector2Float(Vector2.right);
            }
            else
            {
                SetVector2Float(Vector2.left);
            }

        }
        else if(Input.GetAxisRaw("Vertical") > 0)
        {
            SetVector2Float(Vector2.up);
        }
        else
        {
            SetVector2Float(Vector2.down);
        }
    }

    // PlayerAnimにvector2の値をそれぞれ代入
    void SetVector2Float(Vector2 vector2)
    {   
        // Animatorを有効化
        PlayerAnim.speed = 1.0f;
        PlayerAnim.enabled = true;
        PlayerAnim.SetFloat("X", vector2.x);
        PlayerAnim.SetFloat("Y", vector2.y);
    }

    Sprite FindSprite(Sprite[] sprites, string spriteName)
    {
        // 対象のスプライトを取得
        return System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(spriteName));
    }

    public void DisableCanMove()
    {
        canMove = false;
        StopAnimation(rb.velocity);
        StopMoving();
    }

    public void EnableCanMove()
    {
        canMove = true;
    }

    public void Init()
    {
        this.gameObject.SetActive(true);
        // 下を向かせる
        StopAnimation(new Vector2(0.0f, -3.0f));
    }

    void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }

    public void StopAnimation(Vector2 vector)
    {
        Vector2[] keyList = new Vector2[spriteDict.Keys.Count];
        spriteDict.Keys.CopyTo(keyList, 0);

        Vector2 newVector = new Vector2(vector.x/3, vector.y/3);

        if(!keyList.Contains(newVector)) return;

        PlayerAnim.speed = 0.0f;
        PlayerAnim.enabled = false;

        // 画像を更新
        UpdateSprite(spriteDict[newVector]);
    }

    void UpdateSprite(Sprite sprite)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprite;
    }

    public void Turn(string facing)
    {
        Dictionary<string, Vector2> vectorList = new Dictionary<string, Vector2>() {
            {"right", Vector2.right},
            {"left", Vector2.left},
            {"up", Vector2.up},
            {"down", Vector2.down}
        };

        UpdateSprite(spriteDict[vectorList[facing]]);
    }
}
