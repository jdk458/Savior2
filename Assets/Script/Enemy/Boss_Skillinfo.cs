using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Skillinfo : MonoBehaviour
{
    public int skillDamage;

    [HideInInspector] public float randx;
    [HideInInspector] public float randy;
    [HideInInspector] public Transform target;

    [HideInInspector] public bool randomshot;
    [HideInInspector] public bool breathstrong;
    [HideInInspector] public bool phase1;

    private void Start()
    {
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        float randx = Random.Range(min.x, max.x);
        float randy = Random.Range(min.y, max.y);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
        if (phase1 && randomshot)
            Boss_Skill.instance.RandomShot01(this.transform);
        else if (!phase1 && randomshot)
            Boss_Skill.instance.RandomShot02(this.transform);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().Hit(skillDamage,this.gameObject);
        }
    }

    private void Update()
    {
        if (TimeManager.instance.GetTime())
            return;

        if (randomshot)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, new Vector3(randx,randy), 3.0f);
        }
        if (breathstrong)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, target.position, 3.0f);
        }
    }
}
