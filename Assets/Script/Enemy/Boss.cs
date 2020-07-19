using DG.Tweening;
using Spine.Unity;
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

    [Header("오디오")]
    public AudioSource hit_nomal_atk_AudioSource;

    [Header("스파인 정보")]
    SkeletonAnimation skeletonAnimation;

    [Header("몬스터 정보")]
    public string name;
    string currentSpineName;

    [Header("보스음악")]
    public AudioSource bossbgm;

    [Header("보스스킬음악")]
    public AudioSource breath_ball;
    public AudioSource breath_strong;
    public AudioSource breath;
    public AudioSource burning_ground;
    public AudioSource claw;
    public AudioSource flame;
    public AudioSource flame_bomb;
    public AudioSource flame_strong;
    public AudioSource randomshot;
    public AudioSource randomshot02;
    public AudioSource spin_strong;
    public AudioSource spin;


    [Header("스킬범위")]
    public GameObject burningground_R;
    public GameObject breathball_R;
    public GameObject breath_R;
    public GameObject claw_R;
    public GameObject flame_R;
    public GameObject flamebomb_R;
    public GameObject flamestrong_R;
    public GameObject randomshot_R;

    [Header("패널")]
    public GameObject VictoryPanel;
    public GameObject GameOverPanel;

    [Header("카메라줌")]
    public float Zoom;

    public Transform mouth;
    bool phase_01;

    //연출
    [HideInInspector] public float Boss_hp;
    [Header("보스스테이지시작정보")]
    public Transform temppos; //시작전 위에 있는곳
    public Transform startpos; //보스연출 처음 등장하는 곳
    bool boss_start;
    public GameObject smoke;//연출 연기
    bool skill_start;

    int atk;
    int hp;
    float speed;
    float def;

    string phase_num;

    bool attack_flag;
    bool hit_flag;
    bool move_flag;

    [Header("범위")]
    public float range;

    Animator Boss_ani;

    Vector3 player_pos;

    private void Start()
    {
        Boss_ani = this.GetComponent<Animator>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        this.gameObject.transform.position = new Vector2(player.position.x + 20, player.position.y + 10);
        atk = GameManager.instance.monsterManager.GetMonster(name).atk;
        //hp = GameManager.instance.monsterManager.GetMonster(name).hp;
        hp = 10000;
        speed = GameManager.instance.monsterManager.GetMonster(name).speed;
        def = GameManager.instance.monsterManager.GetMonster(name).def;

        boss_start = true;
        player.GetComponent<PlayerController>().boss_start = true;
        this.transform.position = temppos.position;
        Boss_hp = hp;
        Boss_Skill.instance.phase_01 = true;
        bosshpbar.SetActive(true);
        phase_num = "Phase_03";
        countdown.time = 300;
        skill_start = false;

        player_pos = player.transform.position;
        StartCoroutine("Boss_Start");

        Camera.main.orthographicSize = Zoom;
    }

    private void CameraShake()
    {
        Camera.main.transform.DOShakePosition(1.7f, 1.5f, 7);
    }

    IEnumerator Boss_Start()
    {
        yield return new WaitForSeconds(4f);
        skeletonAnimation.AnimationState.SetAnimation(0, "Dragon_RandomShot_E", false);
        yield return new WaitForSeconds(0.3f);
        this.transform.position = startpos.position;
        Instantiate(smoke, this.transform.position, Quaternion.identity);
        CameraShake();
        boss_start = false;
        player.GetComponent<PlayerController>().boss_start = false;
        yield return new WaitForSeconds(1f);
        skill_start = true;
    }

    private void FixedUpdate()
    {
        //카메라 영역제한
        float cam_x = Mathf.Clamp(main_cam.position.x, player_pos.x - 6, player_pos.x + 6);
        float cam_y = Mathf.Clamp(main_cam.position.y, player_pos.y - 10, player_pos.y + 10);

        main_cam.position = new Vector3(cam_x, cam_y, main_cam.position.z);


        boss_hp.fillAmount = (float)Boss_hp / hp;
        if (countdown.remainTime > 0 && Boss_hp <= 0)
            Victory();
        else if (countdown.playTime >= countdown.time)
            GameOver();
        if (!boss_start)
            Move();

    }

    void GameOver()
    {
        GameOverPanel.SetActive(true);
    }

    void Victory()
    {
        VictoryPanel.SetActive(true);
    }

    public void Move()
    {
        if (TimeManager.instance.GetTime())
            return;

        if (!hit_flag && !attack_flag)
        {
            if (move_flag)
            {
                Spine_Ani(AniKind.move);
                this.transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed / 50f);

                if (player.position.x - this.transform.position.x > 0)
                    this.transform.rotation = Quaternion.Euler(0, 180, 0);
                else
                    this.transform.rotation = Quaternion.Euler(0, 0, 0);


                RaycastHit2D[] isplayer = Physics2D.CircleCastAll(this.transform.position, range, Vector2.zero);

                for (int i = 0; i < isplayer.Length; i++)
                {
                    if (isplayer[i].transform.gameObject.tag.Contains("Player"))
                        StartCoroutine("Attack_flag_Couroutine");
                }
            }
            if (attack_flag && skill_start)
            {
                Invoke(phase_num, 0.1f);
            }
            else
            {
                StartCoroutine("Move_flag_Couroutine");
            }
        }

    }

    IEnumerator Move_flag_Couroutine()
    {
        move_flag = true;
        yield return new WaitForSeconds(3);
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
            Spine_Ani(AniKind.Breath);
        else if (35 <= range && range < 70)
            //보스 플레임 애니메이션 동작
            Spine_Ani(AniKind.Flame);
        else
            //보스 할퀴기 동작
            Spine_Ani(AniKind.Claw);
        if (Boss_hp <= hp * 0.7)
        {
            //보스 화염탄 난사 애니메이션 동작
            Spine_Ani(AniKind.RandomShot);
            Boss_Skill.instance.phase_01 = true;
            phase_01 = true;
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
            Spine_Ani(AniKind.Breath);
        else if (30 <= range && range < 60)
            //보스 화염폭발 애니메이션 동작
            Spine_Ani(AniKind.FlameBomb);
        else if (60 <= range && range < 80)
            //보스 꼬리치기 동작
            Spine_Ani(AniKind.Spin);
        else
            //플레임 동작
            Spine_Ani(AniKind.Flame);
        if (Boss_hp <= hp * 0.3)
        {
            //보스 화염탄 난사 애니메이션 동작
            Spine_Ani(AniKind.RandomShot);
            Boss_Skill.instance.phase_01 = false;
            phase_01 = false;
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
            Spine_Ani(AniKind.Breath);
        //보스 플레임 강화 애니메이션 동작
        else if (20 <= range && range < 45)
            Spine_Ani(AniKind.Breath);
        //보스 불타는 대지 애니메이션 동작
        else if (45 <= range && range < 70)
            //보스 브레스 강화 동작
            Spine_Ani(AniKind.Breath);
        else if (70 <= range && range < 80)
        {
            //보스 화염탄 난사 동작
            Spine_Ani(AniKind.RandomShot);
            Boss_Skill.instance.phase_01 = false;
            phase_01 = false;
        }
        else
            //꼬리치기 강화 동작
            Spine_Ani(AniKind.Spin);
    }

    public void Hit(int damage, bool sound_flag = false)
    {
        if (TimeManager.instance.GetTime())
            return;

        GameObject damage_obj = ObjectPoolingManager.instance.GetQueue(ObjectKind.nomal_damage);
        damage_obj.transform.position = this.transform.position;
        damage_obj.GetComponent<Damage>().DamageSet(damage);

        if (damage - def <= 0)
            Boss_hp -= 1;
        else Boss_hp -= damage;

        if (Boss_hp > 0)
        {
            if (damage > 100)
            {
                StartCoroutine(Hit_Coroutine());
                Spine_Ani(AniKind.hit);
            }
            /*else
                GetComponent<Animator>().SetTrigger("hit");*/
            /*if (sound_flag)
            {
                hit_nomal_atk_AudioSource.Play();
            }*/
        }
    }
    IEnumerator Hit_Coroutine()
    {
        hit_flag = true;
        yield return new WaitForSeconds(0.3f);
        hit_flag = false;
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (TimeManager.instance.GetTime())
            return;
        if (col.transform.tag.Contains("Player"))
            col.transform.GetComponent<PlayerController>().Hit(atk, gameObject);
    }

    void Spine_Ani(AniKind ani)
    {
        if (TimeManager.instance.GetTime())
        {
            return;
        }
        string aniName = "";
        switch (ani)
        {
            case AniKind.move:
                aniName = "Dragon_move";
                if (aniName == currentSpineName)
                    return;
                currentSpineName = aniName;
                skeletonAnimation.AnimationState.SetAnimation(0, aniName, true);
                skeletonAnimation.timeScale = 2;
                break;
            case AniKind.hit:
                aniName = "Dragon_hit";
                if (aniName == currentSpineName)
                    return;
                currentSpineName = aniName;
                GetComponent<Animator>().SetTrigger("hit");
                skeletonAnimation.AnimationState.SetAnimation(0, aniName, false);
                skeletonAnimation.timeScale = 1;
                break;
            case AniKind.Breath:
                aniName = "Dragon_Breath";
                if (aniName == currentSpineName)
                    return;
                currentSpineName = aniName;
                string[] Breath_name = { "Dragon_Breath_R", "Dragon_Breath_A", "Dragon_Breath_E" };
                float[] Breath_time = { 1, 2.5f, 1 };
                StartCoroutine(setAni_Coroutine(Breath_name, Breath_time, "Breath"));
                break;
            case AniKind.BurningGround:
                aniName = "Dragon_BurningGround";
                if (aniName == currentSpineName)
                    return;
                currentSpineName = aniName;
                string[] Burning_name = { "Dragon_BurningGround_R", "Dragon_BurningGround_A", "Dragon_BurningGround_E" };
                float[] Burning_time = { 1, 2.5f, 1 };
                StartCoroutine(setAni_Coroutine(Burning_name, Burning_time, "BurningGround"));
                break;
            case AniKind.Claw:
                aniName = "Dragon_Claw";
                if (aniName == currentSpineName)
                    return;
                currentSpineName = aniName;
                string[] Claw_name = { "Dragon_Claw_R", "Dragon_Claw_A", "Dragon_Claw_E" };
                float[] Claw_time = { 1, 1, 1 };
                StartCoroutine(setAni_Coroutine(Claw_name, Claw_time, "Claw"));
                break;
            case AniKind.FlameBomb:
                aniName = "Dragon_FlameBomb";
                if (aniName == currentSpineName)
                    return;
                currentSpineName = aniName;
                string[] FlameBomb_name = { "Dragon_FlameBomb_R", "Dragon_FlameBomb_A", "Dragon_FlameBomb_E" };
                float[] FlameBomb_time = { 1, 1, 1 };
                StartCoroutine(setAni_Coroutine(FlameBomb_name, FlameBomb_time, "FlameBomb"));
                skeletonAnimation.timeScale = 1;
                break;
            case AniKind.Flame:
                aniName = "Dragon_Flame";
                if (aniName == currentSpineName)
                    return;
                currentSpineName = aniName;
                string[] Flame_name = { "Dragon_Flame_R", "Dragon_Flame_A", "Dragon_Flame_E" };
                float[] Flame_time = { 1, 1, 1 };
                StartCoroutine(setAni_Coroutine(Flame_name, Flame_time, "Flame"));
                break;
            case AniKind.RandomShot:
                aniName = "Dragon_RandomShot";
                if (aniName == currentSpineName)
                    return;
                currentSpineName = aniName;
                string[] RandomShot_name = { "Dragon_RandomShot_R", "Dragon_RandomShot_A", "Dragon_RandomShot_E" };
                float[] RandomShot_time = { 1, 1, 1 };
                StartCoroutine(setAni_Coroutine(RandomShot_name, RandomShot_time, "RandomShot"));
                break;
            case AniKind.Spin:
                aniName = "Dragon_Spin";
                if (aniName == currentSpineName)
                    return;
                currentSpineName = aniName;
                string[] Spin_name = { "Dragon_Spin_R", "Dragon_Spin_A", "Dragon_Spin_E" };
                float[] Spin_time = { 1, 1, 0.5f };
                StartCoroutine(setAni_Coroutine(Spin_name, Spin_time, "Spin"));
                break;
            default:
                break;
        }
    }

    bool isreflect;
    Vector3 pos;
    Vector3[] randpos = new Vector3[16];
    IEnumerator setAni_Coroutine(string[] name, float[] time, string type)
    {
        for (int i = 0; i < name.Length; i++)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, name[i], false);
            skeletonAnimation.AnimationState.TimeScale = 1f;


            if (i == 0)
            {
                float randx = Random.Range(-0.7f, 0.7f);
                pos = new Vector3(player.position.x + randx, player.position.y);

                if (player.position.x - this.transform.position.x > 0)
                {
                    this.GetComponent<Boss_Skill>().isreflect = true;
                    isreflect = true;
                }
                else
                {
                    this.GetComponent<Boss_Skill>().isreflect = false;
                    isreflect = false;
                }
                StartCoroutine(SkillRangeCoroutine(type, isreflect));
            }


            if (i == 1)
            {
                switch (type)
                {
                    case "Breath":
                        if (phase_num == "Phase_01")
                        {
                            Boss_Skill.instance.Breath();
                            GameManager.instance.audioManager.EnvironVolume_Play(breath);
                        }
                        else if (phase_num == "Phase_02")
                        {
                            Boss_Skill.instance.BreathBall();
                            GameManager.instance.audioManager.EnvironVolume_Play(breath_ball);
                        }
                        else
                        {
                            Boss_Skill.instance.BreathStrong();
                            GameManager.instance.audioManager.EnvironVolume_Play(breath_strong);
                        }
                        break;
                    case "Claw":
                        Boss_Skill.instance.Claw();
                        GameManager.instance.audioManager.EnvironVolume_Play(claw);
                        break;
                    case "BurningGround":
                        Boss_Skill.instance.BurningGround();
                        GameManager.instance.audioManager.EnvironVolume_Play(burning_ground);
                        break;
                    case "FlameBomb":
                        Boss_Skill.instance.FlameBomb(pos);
                        GameManager.instance.audioManager.EnvironVolume_Play(flame_bomb);
                        break;
                    case "Flame":
                        if (phase_num == "Phase_03")
                        {
                            Boss_Skill.instance.FlameStrong(pos);
                            GameManager.instance.audioManager.EnvironVolume_Play(flame_strong);
                        }
                        else
                        {
                            Boss_Skill.instance.Flame(pos);
                            GameManager.instance.audioManager.EnvironVolume_Play(flame);
                        }
                        break;
                    case "RandomShot":
                        if (phase_01)
                        {
                            for (int k = 0; k < 10; k++)
                            {
                                float randx = Random.Range(player.position.x - 8, player.position.x + 8);
                                float randy = Random.Range(player.position.y - 8, player.position.y + 8);
                                Vector3 pos1 = new Vector3(randx, randy);
                                randpos[k] = pos1;
                                StartCoroutine(RandomRangeCoroutine(pos1));
                            }
                        }
                        else
                        {
                            for (int k = 0; k < 15; k++)
                            {
                                float randx = Random.Range(player.position.x - 8, player.position.x + 8);
                                float randy = Random.Range(player.position.y - 8, player.position.y + 8);
                                Vector3 pos1 = new Vector3(randx, randy);
                                randpos[k] = pos1;
                                StartCoroutine(RandomRangeCoroutine(pos1));
                            }
                        }
                        break;
                    case "Spin":
                        if (phase_num == "Phase_03")
                        {
                            Boss_Skill.instance.SpinStrong();
                            GameManager.instance.audioManager.EnvironVolume_Play(spin_strong);
                        }
                        else
                        {
                            Boss_Skill.instance.Spin();
                            GameManager.instance.audioManager.EnvironVolume_Play(spin);
                        }
                        break;
                    default:
                        break;
                }
            }
            if (i == 2 && type == "RandomShot")
            {
                if (phase_01)
                {
                    Boss_Skill.instance.RandomShot(randpos);
                    GameManager.instance.audioManager.EnvironVolume_Play(randomshot);
                }
                else
                {
                    Boss_Skill.instance.RandomShot(randpos);
                    GameManager.instance.audioManager.EnvironVolume_Play(randomshot02);
                }
            }
            yield return new WaitForSeconds(time[i]);
        }
    }

    IEnumerator RandomRangeCoroutine(Vector3 pos)
    {
        GameObject randomshotR = Instantiate(randomshot_R, pos, Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        Destroy(randomshotR);
    }

    public IEnumerator SkillRangeCoroutine(string name, bool isreflect)
    {
        switch (name)
        {
            case "Breath":
                if (phase_num == "Phase_01")
                {
                    if (isreflect)
                        breath_R.transform.rotation = Quaternion.Euler(0, 180, 16);
                    else
                        breath_R.transform.rotation = Quaternion.Euler(0, 0, 16);
                    breath_R.SetActive(true);
                    yield return new WaitForSeconds(1f);
                    breath_R.SetActive(false);
                }
                else if (phase_num == "Phase_02")
                {
                    if (isreflect)
                        breathball_R.transform.rotation = Quaternion.Euler(0, 180, 0);
                    else
                        breathball_R.transform.rotation = Quaternion.Euler(0, 0, 0);
                    breathball_R.SetActive(true);
                    yield return new WaitForSeconds(1f);
                    breathball_R.SetActive(false);
                }
                else
                {
                    GameObject breathstrongR = Instantiate(flamebomb_R, mouth.position, Quaternion.identity);
                    if (isreflect)
                        breathstrongR.transform.rotation = Quaternion.Euler(0, 180, 0);
                    else
                        breathstrongR.transform.rotation = Quaternion.Euler(0, 0, 0);
                    yield return new WaitForSeconds(1f);
                    Destroy(breathstrongR);
                }
                break;
            case "BurningGround":
                if (isreflect)
                    burningground_R.transform.rotation = Quaternion.Euler(0, 180, 0);
                else
                    burningground_R.transform.rotation = Quaternion.Euler(0, 0, 0);
                burningground_R.SetActive(true);
                yield return new WaitForSeconds(1f);
                burningground_R.SetActive(false);
                break;
            case "Claw":
                if (isreflect)
                    claw_R.transform.rotation = Quaternion.Euler(0, 180, 0);
                else
                    claw_R.transform.rotation = Quaternion.Euler(0, 0, 0);
                claw_R.SetActive(true);
                yield return new WaitForSeconds(1f);
                claw_R.SetActive(false);
                break;
            case "Flame":
                if (phase_num == "Phase_03")
                {
                    GameObject flamestrongR = Instantiate(flamestrong_R, pos, Quaternion.identity);
                    if (isreflect)
                        flamestrongR.transform.rotation = Quaternion.Euler(0, 180, 0);
                    else
                        flamestrongR.transform.rotation = Quaternion.Euler(0, 0, 0);
                    yield return new WaitForSeconds(1f);
                    Destroy(flamestrongR);
                }
                else
                {
                    GameObject flameR = Instantiate(flame_R, pos, Quaternion.identity);
                    if (isreflect)
                        flameR.transform.rotation = Quaternion.Euler(0, 180, 0);
                    else
                        flameR.transform.rotation = Quaternion.Euler(0, 0, 0);
                    yield return new WaitForSeconds(1f);
                    Destroy(flameR);
                }
                break;
            case "FlameBomb":
                GameObject flamebombR = Instantiate(flamebomb_R, pos, Quaternion.identity);
                if (isreflect)
                    flamebombR.transform.rotation = Quaternion.Euler(0, 180, 0);
                else
                    flamebombR.transform.rotation = Quaternion.Euler(0, 0, 0);
                yield return new WaitForSeconds(1f);
                Destroy(flamebombR);
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(0f);
    }
    enum AniKind
    {
        move,
        hit,
        Breath,
        BurningGround,
        Claw,
        FlameBomb,
        Flame,
        RandomShot,
        Spin
    }
}