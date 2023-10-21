using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [SerializeField] private AttackManager attackManager;

    private Enemy enemy;
    
    // 子オブジェクトからの参照用としても使う
    public Flowchart flowchart;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
}
