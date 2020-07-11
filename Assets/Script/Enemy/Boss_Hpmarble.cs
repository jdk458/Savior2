using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Hpmarble : MonoBehaviour
{
    public static Boss_Hpmarble instance;
    [Header("구슬 스폰 위치")]
    public Transform pos1;
    public Transform pos2;
    public Transform pos3;
    public Transform pos4;

    [Header("구슬 생성 최대 갯수")]
    public int max_num;
    [HideInInspector]public int marble_num;//구슬 생성된 갯수

    [Header("구슬스폰타임")]
    public float spawn_time;

    GameObject hp_marble;

    [HideInInspector] public bool place1;
    [HideInInspector] public bool place2;
    [HideInInspector] public bool place3;
    [HideInInspector] public bool place4;

    [Header("플레이어")]
    public Transform player;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        hp_marble_Spawn();
    }

    void hp_marble_Spawn()
    {
        float marble_num = Random.value;
        ObjectKind marble_type = ObjectKind.hp_marble_large;
        if (marble_num < 0.15f)
            marble_type = ObjectKind.hp_marble_large;
        else if (marble_num < 0.5f)
            marble_type = ObjectKind.hp_marble_middle;
        else
            marble_type = ObjectKind.hp_marble_small;

        hp_marble = ObjectPoolingManager.instance.GetQueue(marble_type);
        hp_marble.GetComponent<Item>().player = player;
        int randnum = Random.Range(0, 4);
        if (marble_num < max_num)
        {
            switch (randnum)
            {
                case 0:
                    if (!place1)
                    {
                        hp_marble.transform.position = pos1.position;
                        hp_marble.GetComponent<Hp_Recovery>().placenum = 1;
                        place1 = true;
                        marble_num++;
                    }
                    break;
                case 1:
                    if (!place2)
                    {
                        hp_marble.transform.position = pos2.position;
                        hp_marble.GetComponent<Hp_Recovery>().placenum = 2;
                        place2 = true;
                        marble_num++;
                    }
                    break;
                case 2:
                    if (!place3)
                    {
                        hp_marble.transform.position = pos3.position;
                        hp_marble.GetComponent<Hp_Recovery>().placenum = 3;
                        place3 = true;
                        marble_num++;
                    }
                    break;
                case 3:
                    if (!place4)
                    {
                        hp_marble.transform.position = pos4.position;
                        hp_marble.GetComponent<Hp_Recovery>().placenum = 4;
                        place4 = true;
                        marble_num++;
                    }
                    break;
                default:
                    break;
            }
        }

        Invoke("hp_marble_Spawn", spawn_time);
    }
}
