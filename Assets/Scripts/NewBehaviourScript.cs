using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;


public class NewBehaviourScript : MonoBehaviour
{
    public Fungus.Flowchart flowchart;
    public static float critical_damage = 1.2f;
    public static int critical_rate = 15;//15%クリティカル率

    public static int atk_rate = 2;
    public static int defence_rate = 3;


    // Start is called before the first frame update
    
    

    public class Player
    {
        // プレイヤーの基本ステータスを定義します
        //いるのか要らんのかわからん
        public string name;
        public int max_hp;
        public int current_hp;
        public int max_mp;
        public int current_mp;
        public int current_speed;
        public int speed;
        public int attack;
        public int defense;

        // コンストラクタで初期化を行います
        public Player(string name, int max_hp, int current_hp, int max_mp, int current_mp, int current_speed, int speed, int attack, int defense)
        {
            //名前
            this.name = name;
            //最大HP
            this.max_hp = max_hp;
            //現在HP
            this.current_hp = current_hp;
            //最大MP
            this.max_mp = max_mp;
            //現在MP
            this.current_mp = current_mp;
            //現在スピード(バトルが持ってても良いが複数戦の場合を考えるとこの方がいいかも)
            this.current_speed = current_speed;
            //初期スピード値
            this.speed = speed;
            //攻撃力
            this.attack = attack;
            //防御力
            this.defense = defense;

            //一旦保留
            //this.critical_damage = critical_damage;
            //this.critical_rate = critical_rate;
        }

        // ダメージ計算を行うメソッドです(引数には攻撃側の合計攻撃力が入ります)
        public int CalculateDamage(float ATK)
        {
            int Attack = (int)ATK;
            //ダメージ計算式(ダメージ = 攻撃÷攻撃軽減率 - 防御力÷防御軽減率)
            int damage = (Attack/NewBehaviourScript.atk_rate) - (defense/NewBehaviourScript.defence_rate);
    
            //現在HPをダメージ分減らしている
            this.current_hp -= damage;
            this.speed -=100;//後でスキルクール値によって変更予定

            //確かfungusと同期する為に書いたかも、変更した方がいいかな
            return damage;
        }

    }

    public class Enemy
    {
        public string name;
        public int max_hp;
        public int current_hp;
        public int max_mp;
        public int current_mp;
        public int current_speed;
        public int speed;
        public int attack;
        public int defense;

        // コンストラクタで初期化を行います
        public Enemy(string name, int max_hp, int current_hp, int max_mp, int current_mp, int current_speed, int speed, int attack, int defense)
        {
            //名前
            this.name = name;
            //最大HP
            this.max_hp = max_hp;
            //現在HP
            this.current_hp = current_hp;
            //最大MP
            this.max_mp = max_mp;
            //現在MP
            this.current_mp = current_mp;
            //現在スピード(バトルが持ってても良いが複数戦の場合を考えるとこの方がいいかも)
            this.current_speed = current_speed;
            //初期スピード値
            this.speed = speed;
            //攻撃力
            this.attack = attack;
            //防御力
            this.defense = defense;

            //一旦保留
            //this.critical_damage = critical_damage;
            //this.critical_rate = critical_rate;
        }

        // ダメージ計算を行うメソッドです
        public int CalculateDamage(float ATK)
        {
            int Attack = (int)ATK;
            //ダメージ計算式(ダメージ = 攻撃÷攻撃軽減率 - 防御力÷防御軽減率)
            int damage = (Attack/atk_rate) - (defense/defence_rate);
    
            //現在HPをダメージ分減らしている
            this.current_hp -= damage;
            this.speed -=100;//後でスキルクール値によって変更予定

            //確かfungusと同期する為に書いたかも、変更した方がいいかな
            return damage;
        }

    }


    public class Skill
    {
        public float atk;
        public Skill(float atk)
        {
            this.atk = atk;
        }

        public float ATK()
        {
            return this.atk;
        }
    }

    public class Battle
    {

        Player player;
        Enemy enemy;

        public Battle(Player player, Enemy enemy)
        {
            this.player = player;
            this.enemy = enemy;
        }

        public void Status()
        {
            Debug.Log("Enemy HP: " + enemy.current_hp);
            Debug.Log("Player HP: " + player.current_hp);
        }

        public void battle_Start()
        {
            this.enemy.current_speed += this.enemy.speed;
            this.player.current_speed += this.player.speed;
        }

        public void battle_loop()
        {

            while (true)
            {
                this.enemy.speed += 1;
                

                //flowchart.SetIntegerVariable("enemy_speed",this.enemy.speed);
                //flowchart.SetIntegerVariable("player_speed",this.player.speed);

                if(this.enemy.speed <= 100)
                {
                    break;
                }
                
                this.player.speed += 1;

                if(this.player.speed <= 100)
                {
                    break;
                }
            }   
        }

        //要らんくなったかも
        public void IsBattleFinished()
        {
            if(this.player.current_hp <= 0)
            {
                Debug.Log("あなたの負けです。敗北を認めてください(end処理)");
            }else if(this.enemy.current_hp <= 0)
            {
                Debug.Log("ここで死んでいただきます(end処理)");
            }

        }

        public int player_atk(Skill skill)
        {
            int damage = enemy.CalculateDamage(this.player.attack * skill.ATK());

            
            Debug.Log("プレイヤーの攻撃！モンスターに" + damage + "ダメージ！！");
            Debug.Log("Enemy HP: " + enemy.current_hp);
            return damage;
        }

        public int InstantDamage(Skill skill)
        {
            Debug.Log(skill.atk + "ここ");
            Debug.Log(skill.ATK()+"これ");
            int damage = this.player.CalculateDamage(this.enemy.attack * skill.ATK());

            
            Debug.Log("モンスターの攻撃！プレイヤーに " + damage + "ダメージ！！");
            Debug.Log("Player HP: " + this.player.current_hp);
            return damage;
        }

    }


    public string player_name;
    public int player_max_hp;
    public int player_current_hp;
    public int player_max_mp;
    public int player_current_mp;
    public int player_current_speed;
    public int player_speed;
    public int player_attack;
    public int player_defense;

    public string enemy_name;
    public int enemy_max_hp;
    public int enemy_current_hp;
    public int enemy_max_mp;
    public int enemy_current_mp;
    public int enemy_current_speed;
    public int enemy_speed;
    public int enemy_attack;
    public int enemy_defense;

    private Player player;
    private Enemy enemy;

    Battle battle;
    Skill kick = new Skill(1.1f);
    Skill punch= new Skill(1.2f);


    void Fung()
    {
        //fungusの変数の読み込み（多分読み込みが必要なんじゃなくて、アタッチが出来てない？）

        //fungusの変数の書き込み
        flowchart.SetIntegerVariable("player_hp",player.current_hp);
        flowchart.SetIntegerVariable("enemy_hp",enemy.current_hp);
    }

    void Const()
    {
        player = new Player(player_name, player_max_hp, player_current_hp, player_max_mp, player_current_mp, player_current_speed, player_speed, player_attack, player_defense);
        
        enemy = new Enemy(enemy_name, enemy_max_hp, enemy_current_hp, enemy_max_mp, enemy_current_mp, enemy_current_speed, enemy_speed, enemy_attack, enemy_defense);
        battle = new Battle(player, enemy);
        battle.battle_Start();
        Fung();

        Enemy_atk_1();
    }

    void Loop()
    {
        battle.battle_loop();
    }
    

    void Player_atk_1()
    {
        
        flowchart.SetIntegerVariable("damage",battle.player_atk(punch));
        battle.IsBattleFinished();
        Fung();
    }
    void Player_atk_2()
    {
        battle = new Battle(player, enemy);
        flowchart.SetIntegerVariable("damage",battle.player_atk(kick));
        battle.IsBattleFinished();
        Fung();
    }

    void Enemy_atk_1()
    {
        battle = new Battle(player, enemy);
        flowchart.SetIntegerVariable("damage",battle.InstantDamage(kick));
        battle.IsBattleFinished();
        Fung();
    }
    
}
