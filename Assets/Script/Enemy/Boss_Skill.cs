using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss_Skill : MonoBehaviour
{
    public static Boss_Skill instance;
    [HideInInspector] public int attack;
    [HideInInspector] public bool phase_01;
    [HideInInspector] public bool isreflect;

    [Header("플레이어위치")]
    public Transform player;

    [Header("보스스킬")]
    public GameObject[] phase1;
    public GameObject[] phase2;
    public GameObject[] phase3;

    [Header("생성위치")]
    public Transform mouth;
    public Transform hand;
    public Transform tail;
    public Transform randshot;
    public Transform burningground;
    public Transform breathpos;


    Vector3 min;
    Vector3 max;

    bool rangeend;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));//카메라최소값
        max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));//카메라최대값
    }

    public void Breath()
    {
        //브레스
        GameObject breath = Instantiate(phase1[0], breathpos.position, Quaternion.identity);
        if (isreflect)
            breath.transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            breath.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    
    public void Flame(Vector3 pos)
    {
        //플레임
        GameObject flame = Instantiate(phase1[1], pos, Quaternion.identity);
    }

    public void Claw()
    {
        //할퀴기
        GameObject claw = Instantiate(phase1[2], hand.position, Quaternion.identity);
        if (isreflect)
            claw.transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            claw.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void RandomShot(Vector3[] pos)
    {
        //화염탄 난사
        int num;
        if (phase_01)
            num = 10;
        else
            num = 15;
        for(int i = 0; i<num; i++)
        {
            GameObject randomskill = Instantiate(phase1[3], randshot.position, Quaternion.identity);
            randomskill.GetComponent<Boss_Skillinfo>().randomshot = true;
            randomskill.GetComponent<Boss_Skillinfo>().target = player;
            randomskill.GetComponent<Boss_Skillinfo>().pos = pos[i];
            randomskill.GetComponent<Boss_Skillinfo>().phase1 = phase_01;
        }
        
    }

    public void RandomShot01(Transform pos)
    {
        //화염탄 난사1
        GameObject randomshot01 = Instantiate(phase1[4], pos.position, Quaternion.identity);
    }

    public void RandomShot02(Transform pos)
    {
        //화염탄 난사2
        GameObject randomshot02 = Instantiate(phase2[5], pos.position, Quaternion.identity);
    }

    public void BreathBall()
    {
        //브레스탄
        GameObject breathball01 = Instantiate(phase2[0], mouth.position, Quaternion.identity);
        GameObject breathball02 = Instantiate(phase2[0], new Vector3(mouth.position.x, mouth.position.y - 3.5f), Quaternion.identity);
        GameObject breathball03 = Instantiate(phase2[0], new Vector3(mouth.position.x, mouth.position.y + 3.5f), Quaternion.identity);
        breathball01.GetComponent<Boss_Skillinfo>().breathball = true;
        breathball02.GetComponent<Boss_Skillinfo>().breathball = true;
        breathball03.GetComponent<Boss_Skillinfo>().breathball = true;
        if (isreflect)
        {
            breathball01.GetComponent<Boss_Skillinfo>().direction = true;
            breathball02.GetComponent<Boss_Skillinfo>().direction = true;
            breathball03.GetComponent<Boss_Skillinfo>().direction = true;
            breathball01.transform.rotation = Quaternion.Euler(0, 180, 0);
            breathball02.transform.rotation = Quaternion.Euler(0, 180, 0);
            breathball03.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            breathball01.GetComponent<Boss_Skillinfo>().direction = false;
            breathball02.GetComponent<Boss_Skillinfo>().direction = false;
            breathball03.GetComponent<Boss_Skillinfo>().direction = false;
            breathball01.transform.rotation = Quaternion.Euler(0, 0, 0);
            breathball02.transform.rotation = Quaternion.Euler(0, 0, 0);
            breathball03.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    public void FlameBomb(Vector3 pos)
    {
        //화염폭발
        GameObject flamebomb = Instantiate(phase2[1], pos, Quaternion.identity);
    }

    public void Spin()
    {
        //꼬리치기
        GameObject spin = Instantiate(phase2[2], tail.position, Quaternion.identity);
        spin.GetComponent<Boss_Skillinfo>().spin = true;
        spin.GetComponent<Boss_Skillinfo>().target = player;
    }

    public void FlameStrong(Vector3 pos)
    {
        //플레임 강화
        GameObject flamestrong = Instantiate(phase3[0], pos, Quaternion.identity);
    }

    public void BurningGround()
    {
        Vector2 pos;
        if (isreflect)
            pos = new Vector2(burningground.position.x + 2, burningground.position.y);
        else
            pos = new Vector2(burningground.position.x - 2, burningground.position.y);
        //불타는 대지
        GameObject burning01 = Instantiate(phase3[1], pos, Quaternion.identity);
        GameObject burning02 = Instantiate(phase3[1], new Vector3(pos.x, burningground.position.y - 3.5f), Quaternion.identity);
        GameObject burning03 = Instantiate(phase3[1], new Vector3(pos.x, burningground.position.y + 3.5f), Quaternion.identity);
    }

    public void BreathStrong()
    {
        //브레스 강화
        GameObject breathstrong = Instantiate(phase3[2], mouth.position, Quaternion.identity);
        breathstrong.GetComponent<Boss_Skillinfo>().breathstrong = true;
        breathstrong.GetComponent<Boss_Skillinfo>().target = player;
    }

    public void SpinStrong()
    {
        //꼬리치기 강화
        GameObject spin = Instantiate(phase3[5], tail.position, Quaternion.identity);
        spin.GetComponent<Boss_Skillinfo>().spinstrong = true;
        spin.GetComponent<Boss_Skillinfo>().target = player;
    }
}
