using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class BattleManager : MonoBehaviour
{
    // 外に出してPlayerとEnemyに持たせてもいいかも
    private class Entity
    {
        public int hp;
        public string side;

        public Entity(string side, int hp)
        {
            // playerサイドかenemyサイドか
            this.side = side;
            this.hp = hp;
        }
    }

    [SerializeField] private GameObject player;

    [SerializeField] private AttackManager attackManager;

    // ステータス表示
    [SerializeField] private StatusController playerStatus;
    [SerializeField] private StatusController enemyStatus;

    private Enemy enemy;

    private Entity playerEntity;
    private Entity enemyEntity;
    
    // 子オブジェクトからの参照用としても使う
    public Flowchart flowchart;
    
    // Start is called before the first frame update
    void Start()
    {
        int playerMaxHp = GetMaxHp("player");
        int EnemyMaxHp = GetMaxHp("enemy")

        // Entityを初期化
        playerEntity = new Entity("player", playerMaxHp);
        enemyEntity = new Entity("enemy", EnemyMaxHp);

        // 表示の更新
        // Player
        playerStatus.UpdateHp(playerMaxHp, playerMaxHp);
        // Enemy
        enemyStatus.UpdateHp(EnemyMaxHp, EnemyMaxHp);
        enemyStatus.UpdateName(enemy._name, enemy.lv);
    }

    public void Encounter()
    {
        // バトル相手となるEnemyを取得
        enemy = GetEnemy();

        // バトルの設定をEnemyから適用する
        SetBattleConfigByEnemy(enemy);

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
        flowchart.SetIntegerVariable("damage", (int)damage);

        // custom_textを編集
        string customText = "{$player_name}の攻撃！\n" + (isCritical ? "クリティカル！" : "") + "{$enemy_name}に{$damage}ダメージ！";
        flowchart.SetStringVariable("custom_text", customText);

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
            playerStatus.UpdateHp(target.hp, GetMaxHp("player"));
        }
        else if (target.side == "enemy")
        {
            enemyStatus.UpdateHp(target.hp, GetMaxHp("enemy"));
        }
    }


    private Enemy GetEnemy()
    {
        // FlowChartからバトル相手となるEnemyを取得
        GameObject enemyObject = flowchart.GetGameObjectVariable("enemy");
        return enemyObject.GetComponent<Enemy>();
    }

    private int GetMaxHp(string side)
    {
        // FlowChartから取得
        if (side == "player")
        {
            return (int)flowchart.GetIntegerVariable("player_hp");
        }
        else if (side == "enemy")
        {
            return (int)flowchart.GetIntegerVariable("enemy_hp");
        }
        else
        {
            return -1;
        }
    }

    private void SetBattleConfigByEnemy(Enemy enemy)
    {
        // Enemyからバトルの設定（主にAttackManager）を適用する
        attackManager.animationCurve = enemy.animationCurve;
        attackManager.duration = enemy.duration;
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
        if (flag === true)
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
