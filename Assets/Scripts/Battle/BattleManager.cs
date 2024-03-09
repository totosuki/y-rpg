using UnityEngine;
using Fungus;

public class BattleManager : MonoBehaviour
{
    // 外に出してPlayerとEnemyに持たせてもいいかも
    private class Entity
    {
        public string side;
        public int hp;

        public Entity(string side, int hp)
        {
            // playerサイドかenemyサイドか
            this.side = side;
            this.hp = hp;
        }
    }

    [SerializeField] private GameObject player;
    private GameObject enemy;

    [SerializeField] private AttackManager attackManager;

    // ステータス表示
    [SerializeField] private StatusController playerStatus;
    [SerializeField] private StatusController enemyStatus;

    private EnemySetting enemySetting;

    private EntityStatus playerEntityStatus;
    private EntityStatus enemyEntityStatus;

    private Entity playerEntity;
    private Entity enemyEntity;
    
    // 子オブジェクトからの参照用としても使う
    public Flowchart flowchart;

    public void BattleInit()
    {
        // FlowChartから対戦相手のEnemyのGameObjectを取得する
        enemy = flowchart.GetGameObjectVariable("enemy");

        playerEntityStatus = player.GetComponent<EntityStatus>();
        enemyEntityStatus = enemy.GetComponent<EntityStatus>();

        // Entityを初期化
        playerEntity = new Entity("player", playerEntityStatus.hp);
        enemyEntity = new Entity("enemy", enemyEntityStatus.hp);

        // 表示の更新
        int playerMaxHp = playerEntityStatus.hp;
        int enemyMaxHp = enemyEntityStatus.hp;
        // Player
        string player_name = flowchart.GetStringVariable("_player_name");
        playerStatus.UpdateHp(playerMaxHp, playerMaxHp);
        playerStatus.UpdateName(player_name, playerEntityStatus.lv);
        // Enemy
        enemyStatus.UpdateHp(enemyMaxHp, enemyMaxHp);
        enemyStatus.UpdateName(enemyEntityStatus._name, enemyEntityStatus.lv);

        // バトルの設定をEnemyから適用する
        enemySetting = enemy.GetComponent<EnemySetting>();
        SetBattleConfigByEnemySetting(enemySetting);

        ToggleBattleModeTo(true);
    }

    public void EndBattle()
    {
        ToggleBattleModeTo(false);
    }

    public void PlayerAttack()
    {
        // 攻撃の入力を受け付ける
        attackManager.DoAttack();
        // -> FlowChartから PlayerTurnEnd() に受け渡し
    }

    public void PlayerTurnEnd()
    {
        bool isCritical = attackManager.IsCritical();
        // ATK ハードコードしてます
        int damage = CalculateDamage(3, isCritical);
        // 攻撃結果の表示用の情報を登録
        flowchart.SetIntegerVariable("damage", (int)damage);
        flowchart.SetBooleanVariable("is_critical", isCritical);

        // この攻撃で相手を倒せるかどうか
        if (enemyEntity.hp - damage <= 0)
        {
            // HPを0にする（倒した判定）
            applyDamage(enemyEntity, (int)enemyEntity.hp);
            BattleEndFlag(true);
        }
        else
        {
            applyDamage(enemyEntity, (int)damage);
        }
    }

    public void EnemyAttack()
    {
        // ATK ハードコードしてます
        int damage = CalculateDamage(3, false);
        flowchart.SetIntegerVariable("damage", (int)damage);

        // この攻撃で相手を倒せるかどうか
        if (playerEntity.hp - damage <= 0)
        {
            // HPを0にする（倒した判定）
            applyDamage(playerEntity, (int)playerEntity.hp);
            BattleEndFlag(true);
        }
        else
        {
            applyDamage(playerEntity, (int)damage);
        }
    }

    int CalculateDamage(float atk, bool isCritical)
    {
        // ATKとクリティカルかどうかでダメージを計算

        // ハードコード中 クリティカル倍率
        float criticalRate = 2.0f;
        // ダメージ計算式 要調整
        float damage = atk * 10;

        // クリティカルの場合はクリティカル倍率を適用
        if (isCritical) {
            damage *= criticalRate;
        }
        return (int)Mathf.Floor(damage);
    }

    void applyDamage(Entity target, int damage)
    {
        // 対象にダメージを適用する
        target.hp -= damage;

        // 表示を更新
        if (target.side == "player")
        {
            playerStatus.UpdateHp(target.hp, playerEntityStatus.hp);
        }
        else if (target.side == "enemy")
        {
            enemyStatus.UpdateHp(target.hp, enemyEntityStatus.hp);
        }
    }

    private void SetBattleConfigByEnemySetting(EnemySetting enemySetting)
    {
        // Enemyからバトルの設定（主にAttackManager）を適用する
        attackManager.animationCurve = enemySetting.animationCurve;
        attackManager.duration = enemySetting.duration;
    }

    private void BattleEndFlag(bool flag)
    {
        // バトルが終了したことをFlowChartに反映する
        flowchart.SetBooleanVariable("is_battle_ended", flag);
    }

    // 外部から使う機会があればpublicに
    private void ToggleBattleModeTo(bool flag)
    {
        // 通常画面とバトル画面で変更が必要な点を記載
        // 基本的には逆の設定になるはず
        if (flag == true)
        {
            // 通常画面のプレイヤーの当たり判定を無効化
            player.GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            player.GetComponent<Collider2D>().enabled = true;
        }
    }
}
