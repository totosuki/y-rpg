using UnityEngine;
using Fungus;

public class EnemyController : MonoBehaviour {
    private NPCAnimController animController;

    [SerializeField] private MessageTrigger messageTrigger;

    private GameObject player;

    private Flowchart flowchart;

    private EntityStatus enemyEntityStatus;

    private bool chaseFlag = true;
    private bool collided = false;
    private int playerFacingMemory;

    void Start() {
        player = GameObject.Find("Player");

        GameObject flowchartObject = GameObject.Find("BattleFlowchart");
        flowchart = flowchartObject.GetComponent<Flowchart>();

        animController = GetComponent<NPCAnimController>();
        enemyEntityStatus = GetComponent<EntityStatus>();
    }

    void Update() {
        if (messageTrigger.CanInteract()) {
            // プレイヤーを認識したら追跡開始
            int playerFacing = GetPlayerFacing();

            // playerFacingの値が変わった時に実行
            if (playerFacing != playerFacingMemory) {
                playerFacingMemory = playerFacing;
                animController.SetFacing(playerFacing);
            }
            // 1回だけ実行
            if (chaseFlag){
                animController.StartWalk(playerFacing);
                chaseFlag = false;
            }
        }
        else {
            // 1回だけ実行
            if (!chaseFlag){
                animController.StopWalk();
                chaseFlag = true;
            }
        }
    }

    int GetPlayerFacing() {
        // 2点間の角度を取得(deg -180 ~ 180)
        int GetAngle(Vector2 start, Vector2 target) {
            // 2点間の差分を取得
            Vector2 dt = target - start;
            // rad = arctan y/x
            float rad = Mathf.Atan2(dt.y, dt.x);
            // 角度に変換
            float degree = rad * Mathf.Rad2Deg;

            return (int)degree;
        }

        // 角度から上下左右に絞る
        int GetFacingFromAngle(int angle) {
            if (-135 < angle && angle <= -45) {
                return 0;
            }
            else if (-45 < angle && angle <= 45) {
                return 2;
            }
            else if (45 < angle && angle <= 135) {
                return 3;
            }
            else {
                return 1;
            }
        }

        Vector2 enemyVector = transform.position;
        Vector2 playerVector = player.transform.position;

        int angle = GetAngle(enemyVector, playerVector);

        return GetFacingFromAngle(angle);
    }

    // 当たり判定
    // 2重の当たり判定を切り分けて処理することに注意
    void OnTriggerEnter2D(Collider2D other) {
        // プレイヤー以外は無視
        if (other.gameObject.tag != "Player") return;

        if (collided) {
            collided = false;
            Encountered();
        }
        else {
            collided = true;
            return;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        // プレイヤー以外は無視
        if (other.gameObject.tag != "Player") return;

        collided = false;
    }

    void StartBattle() {
        Debug.Log("battle!");
        animController.StopWalk();
        // ここの場合はバトル開始
        messageTrigger.InvokeBlock();
    }

    void Encountered()
    {
        SetSelfAsEnemy();
        StartBattle();
    }

    public void SetSelfAsEnemy()
    {
        // FungusのVariablesに自身をenemyとして登録
        flowchart.SetGameObjectVariable("enemy", gameObject);
        flowchart.SetStringVariable("enemy_name", enemyEntityStatus._name);
        flowchart.SetIntegerVariable("enemy_lv", enemyEntityStatus.lv);
    }
}
