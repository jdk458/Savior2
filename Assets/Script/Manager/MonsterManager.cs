using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterManager : MonoBehaviour
{
    [Serializable]
    public struct MonsterStruct
    {
        public string name;
        public Stage stage;
        public MonsterType monsterType;
        public int hp;
        public int atk;
        public float def;
        public float speed;
        public int exp;
        public int point;
        public string explanation;
    }

    public MonsterStruct[] MonsterList;

    public MonsterStruct GetMonster(string name)
    {
        for (int i = 0; i < MonsterList.Length; i++)
        {
            if (MonsterList[i].name.Contains(name))
            {
                return MonsterList[i];
            }
        }
        MonsterStruct Null = new MonsterStruct();
        Null.name = "";
        return Null;
    }

    private void Start()
    {
        int monster_num = GetComponent<DatabaseManager>().monster_DB.GetLineSize();
        MonsterList = new MonsterStruct[monster_num];
        for (int i = 0; i < monster_num; i++)
        {
            List<string> monsterInfo = GetComponent<DatabaseManager>().monster_DB.GetRowData(i);
            MonsterList[i].name = monsterInfo[1];
            MonsterList[i].stage = (Stage)Enum.Parse(typeof(Stage), monsterInfo[2]);
            MonsterList[i].monsterType = (MonsterType)Enum.Parse(typeof(MonsterType), monsterInfo[3]);
            MonsterList[i].hp = int.Parse(monsterInfo[4]);
            MonsterList[i].atk = int.Parse(monsterInfo[5]);
            MonsterList[i].def = float.Parse(monsterInfo[6]);
            MonsterList[i].speed = float.Parse(monsterInfo[7]);
            MonsterList[i].exp = int.Parse(monsterInfo[8]);
            MonsterList[i].point = int.Parse(monsterInfo[9]);
            MonsterList[i].explanation = monsterInfo[11];
        }
    }
}

public enum MonsterType
{
    약한객체,
    중간객체,
    강한객체,
    보스
}

public enum Stage
{
    Stage01,
    Stage02,
    Stage03,
    Stage04,
    Stage05,
    BossStage
}
