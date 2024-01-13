using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class BattleManager : MonoBehaviour
{
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
        playerEntity = new Entity("player", GetMaxHp("player"));
        enemyEntity = new Entity("enemy", GetMaxHp("enemy"));

        playerStatus.UpdateHp(GetMaxHp("player"), GetMaxHp("player"));
        enemyStatus.UpdateHp(GetMaxHp("enemy"), GetMaxHp("enemy"));
        enemyStatus.UpdateName(enemy._name, enemy.lv);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Encounter()
    {
        player.GetComponent<Collider2D>().enabled = false;
        enemy = GetEnemy();

        print(enemy);
        SetBattleConfigByEnemy();
    }

    public void PlayerTurnEnd()
    {
        bool isCritical = attackManager.IsCritical();
        // ATK ハードコードしてます
        float damage = CalculateDamage(3, isCritical);
        flowchart.SetIntegerVariable("damage", (int)damage);

        // custom_textを編集
        string customText = "{$player_name}の攻撃！\n" + (isCritical ? "クリティカル！" : "") + "{$enemy_name}に{$damage}ダメージ！";
        flowchart.SetStringVariable("custom_text", customText);

        // この攻撃で相手を倒せるかどうか
        if (enemyEntity.hp - damage <= 0)
        {
            // HPを0にする
            applyDamage(enemyEntity, (int)enemyEntity.hp);
            BattleEndFlag(true);
        }
        else
        {
            applyDamage(enemyEntity, (int)damage);
        }
    }

    public void PlayerAttack()
    {
        attackManager.DoAttack();
    }

    public void EnemyAttack()
    {
        // ATK ハードコードしてます
        float damage = CalculateDamage(3, false);
        flowchart.SetIntegerVariable("damage", (int)damage);

        // この攻撃で相手を倒せるかどうか
        if (playerEntity.hp - damage <= 0)
        {
            // HPを0にする
            applyDamage(playerEntity, (int)playerEntity.hp);
            BattleEndFlag(true);
        }
        else
        {
            applyDamage(playerEntity, (int)damage);
        }
    }

    public void EndBattle()
    {
        player.GetComponent<Collider2D>().enabled = true;
    }

    private Enemy GetEnemy()
    {
        GameObject enemyObject = flowchart.GetGameObjectVariable("enemy");
        return enemyObject.GetComponent<Enemy>();
    }

    private int GetMaxHp(string side)
    {
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
    
    void SetBattleConfigByEnemy()
    {
        attackManager.animationCurve = enemy.animationCurve;
        attackManager.duration = enemy.duration;
    }

    float CalculateDamage(float atk, bool isCritical)
    {
        float criticalRate = 2.0f;
        float damage = atk * 10;
        if (isCritical) {
            damage *= criticalRate;
        }

        return Mathf.Floor(damage);
    }

    void applyDamage(Entity target, int damage)
    {
        print($"{damage} damage to {target}");
        print(target.hp);
        target.hp -= damage;

        if (target.side == "player")
        {
            playerStatus.UpdateHp(target.hp, GetMaxHp("player"));
        }
        else if (target.side == "enemy")
        {
            enemyStatus.UpdateHp(target.hp, GetMaxHp("enemy"));
        }
    }

    private void BattleEndFlag(bool flag)
    {
        flowchart.SetBooleanVariable("is_battle_ended", flag);
    }
}
