using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : MonoBehaviour
{
    [Range(1,10)] public float skillDamage;
    public float lvOfDamage;
    [Range(1, 10)] public float range;
    [Range(1, 10)] public int atk_num; // 타격수
    [Range(0.1f,60)] public float colltime;

    [HideInInspector] public PlayerController playerInfo;

    public void SkillCall(List<GameObject> target, int atk)
    {
        for (int i = 0; i < target.Count; i++)
        {
            StartCoroutine(SkillCall_Coroutine(target[i], atk));
        }
    }

    IEnumerator SkillCall_Coroutine(GameObject target, int atk)
    {
        for (int i = 0; i < atk_num; i++)
        {
            if (target.activeSelf == true)
            {
                target.GetComponent<MonsterController>().Hit((int)(atk * skillDamage));
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public void Destroy()
    {
        enemyList.Clear();
        skillUpdatePos_flag = false;
        Destroy(this.gameObject);
    }

    List<GameObject> enemyList = new List<GameObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (enemyList.Contains(collision.gameObject))
                return;

            enemyList.Add(collision.gameObject);
            List<GameObject> enemy = new List<GameObject>();
            enemy.Add(collision.gameObject);
            SkillCall(enemy, (int)(playerInfo.skill_lv_atk * lvOfDamage));
        }
    }

    bool skillUpdatePos_flag;
    Transform skillUpdatePos_target;
    public void UpdatePosSet(Transform skillUpdatePos_target)
    {
        this.skillUpdatePos_target = skillUpdatePos_target;
        skillUpdatePos_flag = true;
    }
    private void Update()
    {
        if (skillUpdatePos_flag)
        {
            this.transform.position = new Vector2(skillUpdatePos_target.position.x, skillUpdatePos_target.position.y);
        }
    }
}
