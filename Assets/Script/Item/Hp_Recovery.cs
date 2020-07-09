using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp_Recovery : MonoBehaviour
{
    [Range(10, 100)] public float recovery_amount;
    [HideInInspector] public int placenum;//보스스테이지 생성위치

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            switch (placenum)
            {
                case 1:
                    Boss_Hpmarble.instance.place1 = false;
                    break;
                case 2:
                    Boss_Hpmarble.instance.place2 = false;
                    break;
                case 3:
                    Boss_Hpmarble.instance.place3 = false;
                    break;
                case 4:
                    Boss_Hpmarble.instance.place4 = false;
                    break;
                default:
                    break;
            }
            if (StageManager.instance.currentStageInt == 6)
                Boss_Hpmarble.instance.marble_num--;
            collision.GetComponent<PlayerController>().Hp_Recovery((int)(collision.GetComponent<PlayerController>().max_hp * recovery_amount));
            if (this.gameObject.name.Contains("large"))
            {
                ObjectPoolingManager.instance.InsertQueue(this.gameObject, ObjectKind.hp_marble_large);
            }
            if (this.gameObject.name.Contains("middle"))
            {
                ObjectPoolingManager.instance.InsertQueue(this.gameObject, ObjectKind.hp_marble_middle);
            }
            if (this.gameObject.name.Contains("small"))
            {
                ObjectPoolingManager.instance.InsertQueue(this.gameObject, ObjectKind.hp_marble_small);
            }
        }
    }
}
