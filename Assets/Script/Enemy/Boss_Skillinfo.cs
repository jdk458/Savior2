using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [HideInInspector] public bool breathball;
    [HideInInspector] public bool direction;//브레스탄과 플레이어의 x방향구분
    [HideInInspector] public bool spin;
    [HideInInspector] public bool spinstrong;

    [HideInInspector] public Vector3 pos; //랜덤샷 떨어질 곳

    [Header("꼬리치기 지속시간")]
    public int spintime;
    public int spinstrongtime;

    [Header("꼬리치기 속도")]
    public float spinspeed;
    public float spinstrongspeed;

    public void Destroy()
    {
        Destroy(this.gameObject);
        if (phase1 && randomshot)
            Boss_Skill.instance.RandomShot01(this.transform);
        else if (!phase1 && randomshot)
            Boss_Skill.instance.RandomShot02(this.transform);
        randomshot = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (TimeManager.instance.GetTime())
                return;

            collision.GetComponent<PlayerController>().Hit(skillDamage,this.gameObject);
            
        }
    }

    private void Update()
    {
        if (TimeManager.instance.GetTime())
            return;

        if (randomshot)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, pos, 0.3f);
            if (target.position.x - this.transform.position.x > 0)
                this.transform.rotation = Quaternion.Euler(0,180,60);
            else
                this.transform.rotation = Quaternion.Euler(0, 0, 60);
        }
        if (breathstrong)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, target.position, 3.0f*Time.deltaTime);
        }
        if (breathball)
        {
            if (direction)
                this.transform.position = Vector3.MoveTowards(transform.position, new Vector3(this.transform.position.x + 40, this.transform.position.y), 7.2f*Time.deltaTime);
            else
                this.transform.position = Vector3.MoveTowards(transform.position, new Vector3(this.transform.position.x - 40, this.transform.position.y), 7.2f*Time.deltaTime);
        }
        if (spin)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, target.position, spinspeed * Time.deltaTime);
        }
        if (spinstrong)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, target.position, spinstrongspeed * Time.deltaTime);
        }
    }

    public IEnumerator SpinCoroutine()
    {
        yield return new WaitForSeconds(spintime);
        Destroy(this.gameObject);
    }

    public IEnumerator SpinStrongCoroutine()
    {
        yield return new WaitForSeconds(spinstrongtime);
        Destroy(this.gameObject);
    }
}
