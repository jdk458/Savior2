using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header("플레이어위치")]
    public Transform player;

    [Header("메인카메라")]
    public Transform main_cam;

    [Header("카운트다운")]
    public CountdownManager countdown;

    [Header("보스hp")]
    public Image boss_hp;
    public GameObject bosshpbar;

    [HideInInspector] public float Boss_hp;

    float rigidTime;
    int atk;
    int hp;
    float speed;

    string phase_num;

    bool attack_flag;
    bool hit_flag;
    bool move_flag;

    Animator Boss_ani;

    Vector3 player_pos;

    private void Start()
    {
        Boss_ani = this.GetComponent<Animator>();
        this.gameObject.transform.position = player.position;
        rigidTime = GameManager.instance.monsterManager.GetMonster(name).rigidTime;
        atk = GameManager.instance.monsterManager.GetMonster(name).atk;
        hp = GameManager.instance.monsterManager.GetMonster(name).hp;
        speed = GameManager.instance.monsterManager.GetMonster(name).speed;
        Boss_hp = 300000;
        Boss_Skill.instance.phase_01 = true;
        bosshpbar.SetActive(true);
        phase_num = "Phase_01";
        countdown.time = 300;

        player_pos = player.transform.position;
    }


    private void FixedUpdate()
    {
        //카메라 영역제한
        float cam_x = Mathf.Clamp(main_cam.position.x, player_pos.x - 6, player_pos.x + 6);
        float cam_y = Mathf.Clamp(main_cam.position.y, player_pos.y - 10, player_pos.y + 10);

        main_cam.position = new Vector3(cam_x, cam_y, main_cam.position.z);


        boss_hp.fillAmount = (float)Boss_hp / 3000;
        if (countdown.remainTime > 0 && Boss_hp <= 0)
            Victory();
        else if (countdown.remainTime <= 0)
            GameOver();
        //Move();
    }

    void GameOver()
    {

    }

    void Victory()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Move()
    {
        if (TimeManager.instance.GetTime())
            return;

        if (!hit_flag && !attack_flag)
        {
            if (move_flag)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed / 120f);

                if (player.position.x - this.transform.position.x > 0)
                    this.transform.rotation = Quaternion.Euler(0, 180, 0);
                else
                    this.transform.rotation = Quaternion.Euler(0, 0, 0);

                StartCoroutine("Attack_flag_Couroutine");
            }
            if (attack_flag)
            {
                Invoke(phase_num,0.1f);
            }
            else
            {
                StartCoroutine("Move_flag_Couroutine");
                //보스 idle 애니메이션 적용
            }

            //보스 이동 애니메이션 적용
            /*if (currentSpineName != move)
            {
                currentSpineName = move;
                skeletonAnimation.AnimationState.SetAnimation(0, move, true);
                skeletonAnimation.timeScale = 2;
            }*/
        }

    }

    IEnumerator Move_flag_Couroutine()
    {
        move_flag = true;
        yield return new WaitForSeconds(5);
        move_flag = false;
    }

    IEnumerator Attack_flag_Couroutine()
    {
        attack_flag = true;
        yield return new WaitForSeconds(5);
        attack_flag = false;
    }

    public void Phase_01()
    {
        if (TimeManager.instance.GetTime())
        {
            return;
        }
        int range = Random.Range(0, 100);
        if (range < 35)
            //보스 브레스 애니메이션 동작
            Boss_Skill.instance.Breath();
        else if (35 <= range && range < 70)
            //보스 플레임 애니메이션 동작
            Boss_Skill.instance.Flame();
        else
            //보스 할퀴기 동작
            Boss_Skill.instance.Claw();
        if(Boss_hp <= 70)
        {
            //보스 화염탄 난사 애니메이션 동작
            Boss_Skill.instance.RandomShot();
            Boss_Skill.instance.phase_01 = false;
            phase_num = "Phase_02";
        }
    }

    public void Phase_02()
    {
        if (TimeManager.instance.GetTime())
        {
            return;
        }
        int range = Random.Range(0, 100);
        if (range < 30)
            //보스 브레스탄 애니메이션 동작
            Boss_Skill.instance.BreathBall();
        else if (30 <= range && range < 60)
            //보스 화염폭발 애니메이션 동작
            Boss_Skill.instance.FlameBomb();
        else if (60 <= range && range < 80)
            //보스 꼬리치기 동작
            Boss_Skill.instance.Spin();
        else
            //플레임 동작
            Boss_Skill.instance.Flame();
        if (Boss_hp <= 30)
        {
            //보스 화염탄 난사 애니메이션 동작
            Boss_Skill.instance.RandomShot();
            Boss_Skill.instance.phase_01 = false;
            phase_num = "Phase_03";
        }

    }

    public void Phase_03()
    {
        if (TimeManager.instance.GetTime())
        {
            return;
        }
        int range = Random.Range(0, 100);
        if (range < 20)
            //보스 플레임 강화 애니메이션 동작
            Boss_Skill.instance.FlameStrong();
        else if (20 <= range && range < 45)
            //보스 불타는 대지 애니메이션 동작
            Boss_Skill.instance.BurningGround();
        else if (45 <= range && range < 70)
            //보스 브레스 강화 동작
            Boss_Skill.instance.BreathStrong();
        else if (70 <= range && range < 80)
        {
            //보스 화염탄 난사 동작
            Boss_Skill.instance.RandomShot();
            Boss_Skill.instance.phase_01 = false;
        }
        else
            //꼬리치기 강화 동작
            Boss_Skill.instance.SpinStrong();
    }

}
