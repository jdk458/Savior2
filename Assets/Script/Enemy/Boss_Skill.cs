using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Skill : MonoBehaviour
{
    public static Boss_Skill instance;
    [HideInInspector] public int attack;
    [HideInInspector] public bool phase_01;

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

    private void Awake()
    {
        instance = this;
    }

    public void Breath()
    {
        //브레스
        GameObject breath = Instantiate(phase1[0], mouth.position, Quaternion.identity);
    }
    
    public void Flame()
    {
        //플레임
        GameObject flame = Instantiate(phase1[1], player.position, Quaternion.identity);
    }

    public void Claw()
    {
        //할퀴기
        //부채꼴 범위 추가
        GameObject breath = Instantiate(phase1[0], hand.position, Quaternion.identity);
    }

    public void RandomShot()
    {
        //화염탄 난사
        for (int i = 0; i < 5; i++)
        {
            GameObject randomskill = Instantiate(phase1[0], mouth.position, Quaternion.identity);
            randomskill.GetComponent<Boss_Skillinfo>().randomshot = true;
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
        GameObject randomshot01 = Instantiate(phase2[5], pos.position, Quaternion.identity);
    }

    public void BreathBall()
    {
        //브레스탄
        GameObject breathball01 = Instantiate(phase2[0], mouth.position, Quaternion.identity);
        GameObject breathball02 = Instantiate(phase2[0], new Vector3(mouth.position.x,mouth.position.y-1), Quaternion.identity);
        GameObject breathball03 = Instantiate(phase2[0], new Vector3(mouth.position.x, mouth.position.y + 1), Quaternion.identity);
    }
    public void FlameBomb()
    {
        //화염폭발
        GameObject flamebomb = Instantiate(phase2[1], player.position, Quaternion.identity);
    }

    public void Spin()
    {
        //꼬리치기
        GameObject spin = Instantiate(phase2[2], tail.position, Quaternion.identity);
    }

    public void FlameStrong()
    {
        //플레임 강화
        GameObject flamestrong = Instantiate(phase3[0], player.position, Quaternion.identity);
    }

    public void BurningGround()
    {
        //불타는 대지
        GameObject burning01 = Instantiate(phase3[1], hand.position, Quaternion.identity);
        GameObject burning02 = Instantiate(phase3[1], new Vector3(hand.position.x, hand.position.y - 1), Quaternion.identity);
        GameObject burning03 = Instantiate(phase3[1], new Vector3(hand.position.x, hand.position.y + 1), Quaternion.identity);
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
    }

}
