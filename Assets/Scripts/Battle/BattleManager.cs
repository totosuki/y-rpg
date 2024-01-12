using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class BattleManager : MonoBehaviour
{
    private class Entity
    {
        public int hp;

        public Entity(int hp)
        {
            this.hp = hp;
        }
    }


    [SerializeField] private GameObject player;

    [SerializeField] private AttackManager attackManager;

    private Enemy enemy;

    private Entity playerEntity;
    private Entity enemyEntity;
    
    // 子オブジェクトからの参照用としても使う
    public Flowchart flowchart;
    
    // Start is called before the first frame update
    void Start()
    {
        playerEntity = new Entity(100);
        enemyEntity = new Entity(enemy.lv * 10);
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

        // この攻撃で相手を倒せるかどうか
        if (enemyEntity.hp - damage <= 0)
        {
            print("enemy lose");
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

    public void EndBattle()
    {
        player.GetComponent<Collider2D>().enabled = true;
    }

    private Enemy GetEnemy()
    {
        GameObject enemyObject = flowchart.GetGameObjectVariable("enemy");
        return enemyObject.GetComponent<Enemy>();
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
        target.hp -= damage;
    }
}
