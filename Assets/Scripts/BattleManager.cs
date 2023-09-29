using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    static int ATK_RATE = 2;
    static int DEFENSE_RATE = 3;

    //ステータス最小保証値
    static int min_current_hp = 0;
    static int min_max_hp = 0;
    static int min_attack = 0;
    static int min_defense = 0;

    System.Random random = new System.Random();

    void Start() 
    { 
        Main(); 
    }

    public class Job
    {
        public string name;
        public int max_hp, speed, attack, defense;

        public Job(string name, int max_hp, int speed, int attack, int defense)
        {
            (this.name, this.max_hp, this.speed, this.attack, this.defense) = (name, max_hp, speed, attack, defense);
        }
    }

    public class Effect
    {
        public string type;
        public int strength, turn;

        public Effect(string type, int strength, int turn)
        {
            (this.type, this.strength, this.turn) = (type, strength, turn);
        }
    }

    public class Entity
    {
        public string name;
        public int current_hp, xp, lv, current_speed;
        public Job job;
        public List<Cson.SkillData> skills;
        public List<Effect> effect;
        public Cson.JobData jobData;

        public Entity(string name, int current_hp, int xp)
        {
            (this.name, this.current_hp, this.xp, this.lv, this.current_speed) = (name, current_hp, xp, 1, 0);
            job = null;
            skills = new List<Cson.SkillData>();
            effect = new List<Effect>();
        }

        public void appendSkills(List<Cson.SkillData> skills)
        {
            skills.ForEach(skill => this.skills.Add(skill));
        }

        public void appendEffect(Effect effect)
        {
            this.effect.Add(effect);
        }

        public void adjustHp()
        {
            // HPの最大値を調整
            int max_hp = job.max_hp * lv + min_max_hp;
            current_hp = Mathf.Min(current_hp, max_hp);
        }

        public void setJob(string jobName)
        {
            jobData = new Cson.JobData(name: jobName);
            job = new Job(jobData.name, jobData.maxHP, jobData.speed, jobData.atk, jobData.def);

            adjustHp();
        }

        public void heal(int amount)
        {
            current_hp += amount;
            adjustHp();

            Debug.Log($"{name}は回復した");
            Debug.Log($"{name}のHPは{current_hp}");
        }

        public void effect_clear()
        {
            effect.Clear();
        }

        // new_lv までレベルアップさせる
        public void lv_update(int? new_lv = null)
        {
            // レベルアップが発生するかどうかのフラグ
            bool lvUpFlag = lv < new_lv;

            // _lvまでに必要な経験値の計算式
            int CalculateWantedXp(int _lv)
            {
                return _lv ^ 2 * 5;
            }

            // 引数がない場合（初期化用）
            if (new_lv == null)
            {
                // 経験値に対応するレベルまで上げる
                while (xp >= CalculateWantedXp(lv))
                {   
                    lv++;
                }
            }
            else
            {
                // 獲得経験値 = 目標lvに必要な経験値 / 3
                int earned_xp = CalculateWantedXp((int)new_lv) / 3;
                Debug.Log($"{earned_xp}xp獲得");
                xp += earned_xp;
                
                while (xp >= CalculateWantedXp(lv))
                {
                    lv++;
                    Debug.Log("レベルアップ！");
                }
                int next_xp = CalculateWantedXp(lv) - xp;

                Debug.Log($"現在のレベルは{lv}");
                Debug.Log($"次のレベルに必要な経験値は{next_xp}");
            }
            
            // レベルアップしたらHPを全回復
            if (lvUpFlag)
            {
                heal(job.max_hp * lv + min_max_hp);
            }
        }
    }

    public class Battle
    {
        public Entity player;
        public Entity enemy;
        public Cson.SkillData skill;
        public int battle_speed;

        System.Random random = new System.Random();

        public Battle(Entity player, Entity enemy)
        {
            (this.player, this.enemy, this.battle_speed) = (player, enemy, 0);
        }

        // 状態確認
        public void Check_effect(Entity target)
        {
            int i = 0;
            // 状態異常の数だけループ（状態異常の数は変化する）
            while (i < target.effect.Count())
            {
                // 確認する状態異常
                Effect effect = target.effect[i];

                // 状態異常の処理を実行
                switch (effect.type)
                {
                    case "poison":
                        if (1 > (target.current_hp / 100) * effect.strength)
                        {
                            target.current_hp -= 1;
                        }
                        else
                        {
                            double castNum = (target.current_hp / 100) * effect.strength;
                            target.current_hp -= (int)castNum;
                        }
                        break;

                    // ここに足していく
                    default:
                        break;
                }

                //表示処理
                Debug.Log($"{target.name}は{effect.type}状態です");
                Debug.Log($"現在HPは{target.current_hp}");

                effect.turn --;

                // エフェクト削除処理
                if (effect.turn <= 0)
                {
                    Debug.Log($"{i}番目のスキルの{effect.type}状態は解消されました");
                    target.effect.RemoveAt(i);
                }
                else
                {
                    Debug.Log($"{i}番目の状態異常の残りのターン数は{effect.turn}です");
                }

                i++;
            }
        }

        public void Main()
        {
            // スピードを初期化
            this.player.current_speed += this.player.job.speed * this.player.lv / 100;
            this.enemy.current_speed += this.enemy.job.speed * this.enemy.lv / 100;
            this.battle_speed += (this.player.job.speed * this.player.lv + this.enemy.job.speed * this.enemy.lv) / 4;

            // ステータスを表示
            Debug.Log($"{this.enemy.name}のHP {this.enemy.current_hp} ATK {min_attack + (this.enemy.job.attack * this.enemy.lv)} DEF {min_defense + (this.enemy.job.defense * this.enemy.lv) / DEFENSE_RATE}");
            Debug.Log($"{this.player.name}のHP {this.player.current_hp} ATK {min_attack + (this.player.job.attack * this.player.lv)} DEF {min_defense + (this.player.job.defense * this.player.lv) / DEFENSE_RATE} LV {this.player.lv}");

            int player_atk_count = 0;
            int enemy_atk_count = 0;

            Debug.Log($"レベル{this.enemy.lv}の{this.enemy.name}が現れた！");

            int order = 0;
            while (true)
            {
                // スピード更新
                this.enemy.current_speed++;
                this.player.current_speed++;
                this.battle_speed++;
                
                // 状態確認ターン
                if (this.battle_speed >= 50)
                {
                    this.Check_effect(this.enemy);
                    this.Check_effect(this.player);
                    this.battle_speed = 0;
                }

                // 勝敗が確定したら終了
                if (this.player.current_hp <= 0 || this.enemy.current_hp <= 0)
                {
                    break;
                }

                // 敵のターン
                if (this.enemy.current_speed >= 100)
                {
                    enemy_atk_count++;
                    Debug.Log($"{this.enemy.name}のターン");

                    this.Calculate_Damage(this.player, this.Calculate_ATK(this.enemy, order, this.player));

                    if (order < this.enemy.skills.Count() - 1)
                    {
                        order++;
                    }
                    else
                    {
                        order = random.Next(0, this.enemy.skills.Count());
                    }
                }

                // プレイヤーのターン
                if (this.player.current_speed >= 100)
                {
                    player_atk_count++;
                    Debug.Log($"{this.player.name}のターン");

                    Debug.Log($"使いたいスキルのindex番号を入力して下さい{this.player.skills.Count()}個が現在使えるスキルの数です。");
                    // TODO 使いたいスキルのindex番号の入力受付
                    int skillIndex = 1;
                    this.Calculate_Damage(this.enemy, this.Calculate_ATK(this.player, skillIndex, this.enemy));
                }
            }

            if (this.player.current_hp <= 0)
            {
                Debug.Log($"{this.player.name}の負け");
            }
            else if (this.enemy.current_hp <= 0)
            {
                Debug.Log($"{this.enemy.name}の負け");
                // プレイヤーのlvを倒した敵のlvまで引き上げる
                this.player.lv_update(this.enemy.lv);
            }
        }

        public void Calculate_Damage(Entity user, int attack)
        {
            if (attack > 0)
            {
                // ダメージ計算式(ダメージ = 攻撃÷攻撃軽減率 - 防御力÷防御軽減率)
                double castNum = (attack / ATK_RATE) - ((min_defense + (user.job.defense * user.lv)) / DEFENSE_RATE);
                int damage = (int)castNum;
                if (damage > 0)
                {
                    user.current_hp -= damage;
                    Debug.Log($"{user.name}に{damage}ダメージ！！");
                }
                else
                {
                    Debug.Log($"{user.name}に0ダメージ");
                }
                Debug.Log($"{user.name}のHPは{user.current_hp}");
            }
        }

        public int Calculate_ATK(Entity user, int index, Entity target)
        {
            skill = user.skills[index];
            Debug.Log(skill.skillName);
            user.current_speed -= skill.cool;

            float attack = 0;
            float adjust = min_attack;

            switch (skill.skillType)
            {
                case "skill_damage":
                    attack = user.job.attack * user.lv * user.skills[index].atk;
                    break;

                case "skill_change":
                    Debug.Log("変更したい役職の名前を入力して下さい");
                    string job_name = System.Console.ReadLine();
                    user.setJob(job_name);
                    break;

                case "skill_effect":
                    switch (skill.type)
                    {
                        case "poison":
                            //なんか処理
                            break;
                        default:
                            Effect effect = new Effect(skill.type, skill.strength, skill.turn);
                            target.appendEffect(effect);
                            break;
                    }
                    break;
                    
                case "skill_self":
                    int power = user.current_hp;

                    if (user.current_hp >= 50)
                    {
                        user.current_hp = skill.leave;
                        power -= user.current_hp;
                    }
                    else
                    {
                        user.current_hp = 0;
                    }

                    attack = user.job.attack * user.lv * user.skills[index].atk * (1 + (power / 1000) ^ 3);
                    adjust += skill.adjust;

                    Debug.Log($"消費したHP{power}");

                    break; 
            }

            // クリティカル
            if (skill.rate >= random.Next(1, 101))
            {
                Debug.Log("Critical！\n");
                attack *= user.skills[index].critical;
            }

            return (int)(attack + adjust);
        }
    }

    public void Main()
    {
        //スキルのインスタンス作成
        Cson.SkillData s = new Cson.SkillData("クラスチェンジ");
        Cson.SkillData s_1 = new Cson.SkillData("スラッシュ");
        Cson.SkillData s_2 = new Cson.SkillData("スイフトスラッシュ");
        Cson.SkillData es_1 = new Cson.SkillData("突進");
        Cson.SkillData es_2 = new Cson.SkillData("切り裂く");
        Cson.SkillData es_3 = new Cson.SkillData("シャドウファング");
        Cson.SkillData f_1 = new Cson.SkillData("ラストリゾート");

        List<Cson.SkillData> playerSkills = new List<Cson.SkillData>() { s, s_1, s_2, es_1, es_2, es_3, f_1 };
        List<Cson.JobData> playerJobs = new List<Cson.JobData>() { new Cson.JobData("ルーキー"), new Cson.JobData("猪"), new Cson.JobData("熊"), new Cson.JobData("蛇") };
        List<Cson.EnemyData> enemyList = new List<Cson.EnemyData>() { new Cson.EnemyData("猪", 1, 1, 1), new Cson.EnemyData("熊", 1, 1, 2), new Cson.EnemyData("蛇", 1, 1, 3) };

        Entity player = new Entity("もも", 1, 5);
        player.appendSkills(playerSkills);
        player.setJob("ルーキー");
        player.lv_update();

        // プレイヤーのlvを中心に乱数を返す
        int CalculateEnemyLv()
        {
            // 乱数の幅
            int range = 2;

            int r = random.Next(player.lv - range, player.lv + range);
            return Mathf.Max(1, r);
        }

        int GetXpFromLevel(int lv)
        {
            // 経験値 = 0からn-1までの数^2 の和
            // = n(n - 1)(2n - 1)/6 より
            int xp = (lv - 1) * lv * (2 * lv - 1) * 5 / 6;
            return xp;
        }

        void StartBattle()
        {
            Entity CreateEnemy()
            {
                int e_xp = GetXpFromLevel(CalculateEnemyLv());
                int e_index = random.Next(0, 3);
                Cson.EnemyData enemyData = enemyList[e_index];
                

                Entity e = new Entity(enemyData.enemyName, enemyData.hp, enemyData.xp);
                Job job = new Job(enemyData.jobData.name, enemyData.jobData.maxHP, enemyData.jobData.speed, enemyData.jobData.atk, enemyData.jobData.def);
                e.job = job;
                e.appendSkills(enemyData.jobData.skills);
                e.lv_update();
                return e;
            }

            Entity enemy = CreateEnemy();
            Battle b = new Battle(player, enemy);
            b.Main();

            if (player.current_hp > 0)
            {
                //StartBattle();
            }
        }
        StartBattle();
    }
}