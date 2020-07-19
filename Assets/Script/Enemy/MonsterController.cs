using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [Header("몬스터 정보")]
    public string name;
    public string objectKind_string;
    Stage stage;
    MonsterType monsterType;
    int atk;
    int hp; int current_hp;
    float speed;
    float def;
    int exp;

    [Header("플레이어 정보")]
    public Transform player_transform;
    [Header("스파인 네임 정보")]
    SkeletonAnimation skeletonAnimation;
    string currentSpineName;
    public string move;
    public string hit;
    public string attack;

    [Header("오디오")]
    public AudioSource hit_nomal_atk_AudioSource;
    [Header("게임오버")]
    public GameEnd gameEnd;

    Rigidbody2D rigidbody2D;

    private void OnEnable()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        stage = GameManager.instance.monsterManager.GetMonster(name).stage;
        monsterType = GameManager.instance.monsterManager.GetMonster(name).monsterType;
        atk = GameManager.instance.monsterManager.GetMonster(name).atk;
        hp = GameManager.instance.monsterManager.GetMonster(name).hp;
        speed = GameManager.instance.monsterManager.GetMonster(name).speed;
        def = GameManager.instance.monsterManager.GetMonster(name).def;
        exp = GameManager.instance.monsterManager.GetMonster(name).exp;

        rigidbody2D = this.GetComponent<Rigidbody2D>();
        current_hp = hp;
    }



    private void OnDisable()
    {
        hit_flag = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    bool hit_flag;

    private void FixedUpdate()
    {
        if (TimeManager.instance.GetTime())
           return;

        if (!hit_flag && !attack_flag)
        {
            if (!obstacle_flag)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, player_transform.position, speed / 120f);
            }
            else
            {
                if (obstacle_left_move)
                    this.transform.position = new Vector2(this.transform.position.x - (Time.deltaTime * speed / 2), this.transform.position.y);
                if (obstacle_right_move)
                    this.transform.position = new Vector2(this.transform.position.x + (Time.deltaTime * speed / 2), this.transform.position.y);
                if (obstacle_up_move)
                    this.transform.position = new Vector2(this.transform.position.x , this.transform.position.y + (Time.deltaTime * speed / 2));
                if (obstacle_down_move)
                    this.transform.position = new Vector2(this.transform.position.x , this.transform.position.y - (Time.deltaTime * speed / 2));
            }


            if (player_transform.position.x - this.transform.position.x > 0)
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
            else
                this.transform.rotation = Quaternion.Euler(0, 0, 0);

            if (currentSpineName != move)
            {
                currentSpineName = move;
                skeletonAnimation.AnimationState.SetAnimation(0, move, true);
                skeletonAnimation.timeScale = 2;
            }
        }
    }


    public void Hit(int damage, bool sound_flag = false)
    {

        if (TimeManager.instance.GetTime())
            return;

        GameObject damage_obj = ObjectPoolingManager.instance.GetQueue(ObjectKind.nomal_damage);
        damage_obj.transform.position = this.transform.position;
        damage_obj.GetComponent<Damage>().DamageSet((int)(damage - def));

        if (damage - def < 0)
            current_hp -= 1;
        else current_hp -= ((int)(damage -def));

        if (current_hp <= 0)
        {
            gameEnd.monsterDeadCount++;
            int point = GameManager.instance.monsterManager.GetMonster(name).point;
            player_transform.GetComponent<PlayerController>().PointUp(point);
            player_transform.GetComponent<PlayerController>().Exp_Up(exp);
            GameObject smoke = ObjectPoolingManager.instance.GetQueue(ObjectKind.smoke);
            smoke.transform.position = this.transform.position;
            ObjectPoolingManager.instance.InsertQueue(this.gameObject, (ObjectKind)Enum.Parse(typeof(ObjectKind), objectKind_string));
            player_transform.GetComponent<PlayerController>().deadMonsterNum++;
        }

        else
        {
            StartCoroutine(Hit_Coroutine());

            if (currentSpineName != hit) // 스파인
            {
                currentSpineName = hit;
                GetComponent<Animator>().SetTrigger("hit");
                skeletonAnimation.AnimationState.SetAnimation(0, hit, false);
                skeletonAnimation.timeScale = 1;
            }

            if (sound_flag)
            {
                hit_nomal_atk_AudioSource.Play();
            }
        }
    }
    IEnumerator Hit_Coroutine()
    {
        hit_flag = true;
        yield return new WaitForSeconds(0.3f);
        hit_flag = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        rigidbody2D.velocity = Vector2.zero;
        obstacle_flag = false;
    }

    bool attack_flag;
    bool obstacle_flag;
    bool obstacle_left_move;
    bool obstacle_right_move;
    bool obstacle_up_move;
    bool obstacle_down_move;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 공격
        if (collision.transform.tag.Contains("Player") && !attack_flag)
        {
            if (TimeManager.instance.GetTime())
                return;

            
            collision.transform.GetComponent<PlayerController>().Hit(atk, gameObject);
            rigidbody2D.velocity = Vector2.zero;
            StartCoroutine(Attack_Coroutine());

            if (currentSpineName != attack)
            {
                currentSpineName = attack;
                skeletonAnimation.AnimationState.SetAnimation(0, attack, false);
                skeletonAnimation.timeScale = 1;
            }
        }

        if (collision.transform.tag.Contains("Obstacle") && !obstacle_flag )
        {
      
            obstacle_flag = true;
            obstacle_left_move = false;
            obstacle_right_move = false;
            obstacle_up_move = false;
            obstacle_down_move = false;

            float left_distance = Vector2.Distance(new Vector2(this.transform.position.x - 0.2f, this.transform.position.y), collision.transform.position);
            float right_distance = Vector2.Distance(new Vector2(this.transform.position.x + 0.2f, this.transform.position.y), collision.transform.position);
            float up_distance = Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.y + .2f), collision.transform.position);
            float down_distance = Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.y - .2f), collision.transform.position);

            List<float> distance_list = new List<float>();
            distance_list.Add(left_distance); distance_list.Add(right_distance); distance_list.Add(up_distance); distance_list.Add(down_distance);
            distance_list.Sort();

            if (distance_list[0] == left_distance)
            {
                if (player_transform.position.y - this.transform.position.y > 0)
                    obstacle_up_move = true;
                else
                    obstacle_down_move = true;
            }
            if (distance_list[0] == right_distance)
            {
                if (player_transform.position.y - this.transform.position.y > 0)
                    obstacle_up_move = true;
                else
                    obstacle_down_move = true;
            }
            if (distance_list[0] == up_distance)
            {
                if (player_transform.position.x - this.transform.position.x > 0)
                    obstacle_right_move = true;
                else
                    obstacle_left_move = true;
            }
            if (distance_list[0] == down_distance)
            {
                if (player_transform.position.x - this.transform.position.x > 0)
                    obstacle_right_move = true;
                else
                    obstacle_left_move = true;
            }
        }
    }
  
    IEnumerator Attack_Coroutine()
    {
        attack_flag = true;
        yield return new WaitForSeconds(0.3f);
        attack_flag = false;
    }
}
