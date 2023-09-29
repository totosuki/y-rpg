using System.Collections.Generic;
using UnityEngine;

public class Cson: MonoBehaviour
{
    public class JobData
    {
        public string name;
        public int maxHP, speed, atk, def;
        public List<SkillData> skills;
        public int? jobid;

        public JobData(string name = null, int? jobid = null)
        {
            switch ((name, jobid))
            {
                case var t when t.name == "ルーキー" || t.jobid == 0:
                    (this.name, this.maxHP, this.speed, this.atk, this.def) = ("ルーキー", 100, 100, 100, 100);
                    break;
                case var t when t.name == "猪" || t.jobid == 1:
                    (this.name, this.maxHP, this.speed, this.atk, this.def) = ("猪", 100, 100, 100, 65);
                    skills = new List<SkillData>() { new SkillData("突進") };
                    break;
                case var t when t.name == "熊" || t.jobid == 2:
                    (this.name, this.maxHP, this.speed, this.atk, this.def) = ("熊", 110, 105, 120, 70);
                    skills = new List<SkillData>() { new SkillData("突進"), new SkillData("切り裂く") };
                    break;
                case var t when t.name == "蛇" || t.jobid == 3:
                    (this.name, this.maxHP, this.speed, this.atk, this.def) = ("蛇", 40, 120, 60, 40);
                    skills = new List<SkillData>() { new SkillData("シャドウファング") };
                    break;
                default:
                    break;
            }
        }
    }

    public class SkillData
    {
        private List<string> skillChangeList = new List<string>() { "クラスチェンジ" };
        private List<string> skillSelfList = new List<string>() { "ラストリゾート" };
        private List<string> skillDamageList = new List<string>() { "スラッシュ", "スイフトスラッシュ", "突進", "切り裂く" };
        private List<string> skillEffectList = new List<string>() { "シャドウファング" };

        public string skillType, skillName, type;
        public float atk, critical;
        public int rate, cool, leave, adjust, strength, turn;

        public SkillData(string skillName)
        {
            if (skillChangeList.Contains(skillName)) { this.skillType = "skill_change"; }
            else if (skillSelfList.Contains(skillName)) { this.skillType = "skill_self"; }
            else if (skillDamageList.Contains(skillName)) { this.skillType = "skill_damage"; }
            else if (skillEffectList.Contains(skillName)) { this.skillType = "skill_effect"; }

            this.skillName = skillName;
            switch (this.skillType)
            {
                case "skill_change":
                    this.cool = 60;
                    break;
                case "skill_self":
                    (this.atk, this.rate, this.leave, this.adjust, this.cool, this.critical) = (1.2f, 0, 1, 51, 2000, 1.3f);
                    break;
                case "skill_damage":
                    this.critical = 1.3f;
                    switch (skillName)
                    {
                        case "スラッシュ":
                            (this.atk, this.rate, this.cool) = (1, 15, 90);
                            break;
                        case "スイフトスラッシュ":
                            (this.atk, this.rate, this.cool) = (1, 0, 77);
                            break;
                        case "突進":
                            (this.atk, this.rate, this.cool) = (1, 30, 130);
                            break;
                        case "切り裂く":
                            (this.atk, this.rate, this.cool) = (1.1f, 40, 140);
                            break;
                        default:
                            break;
                    }
                    break;
                case "skill_effect":
                    (this.type, this.strength, this.turn, this.cool) = ("poison", 3, 10, 50);
                    break;
                default:
                    break;
            }
        }
    }

    public class EnemyData
    {
        public SkillData skillData;
        public JobData jobData;

        public string enemyName, jobName;
        public int hp, xp, maxHP, defaultSpeed, atk, def;

        public EnemyData(string enemyName, int hp, int xp, int jobid) //jobid = jobDictをインデックスで指定するための数値 = [ルーキー, 猪, 熊, 蛇]
        {
            (this.enemyName, this.hp, this.xp, this.jobData) = (enemyName, hp, xp, new JobData(jobid: jobid));
        }
    }
}
