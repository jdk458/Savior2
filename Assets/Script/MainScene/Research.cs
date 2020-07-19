using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Research : MonoBehaviour
{
    public Reserch_kind reserch_Kind;
    [Header("Line")] public Image[] fill_image;
    [Header("If_Line")] public Image if_fill_image;
    [Header("연구레벨_포인트")] public GameObject Research_Level_Point;
    [Header("설명")] public GameObject explanation;
    public ResearchManager researchManager;

    void OnEnable()
    {
        string name = gameObject.name;
        switch (name)
        {
            case "최대체력":
                if (GameManager.instance.userinfo.hp_research)
                {
                   
                    for (int i = 0; i < fill_image.Length; i++)
                    {
                        fill_image[i].fillAmount = 1;
                    }
                    this.gameObject.GetComponent<Image>().fillAmount = 1;
                    circle_flag = true;
                }
                break;
            case "공격력":
                if (GameManager.instance.userinfo.atk_research)
                {
                    for (int i = 0; i < fill_image.Length; i++)
                    {
                        fill_image[i].fillAmount = 1;

                    }
                    if_fill_image.GetComponent<Image>().fillAmount = 1;
                    this.gameObject.GetComponent<Image>().fillAmount = 1;
                    circle_flag = true;
                }
                break;
            case "공격속도":
                if (GameManager.instance.userinfo.atkspeed_research)
                {
                    for (int i = 0; i < fill_image.Length; i++)
                    {
                        fill_image[i].fillAmount = 1;
                    }
                    if_fill_image.GetComponent<Image>().fillAmount = 1;
                    this.gameObject.GetComponent<Image>().fillAmount = 1;
                    circle_flag = true;
                }
                break;
            case "이동속도":
                if (GameManager.instance.userinfo.speed_research)
                {
                    for (int i = 0; i < fill_image.Length; i++)
                    {
                        fill_image[i].fillAmount = 1;
                    }
                    if_fill_image.GetComponent<Image>().fillAmount = 1;
                    this.gameObject.GetComponent<Image>().fillAmount = 1;
                    circle_flag = true;
                }
                break;
            case "아이템":
                if (GameManager.instance.userinfo.item_research)
                {
                    for (int i = 0; i < fill_image.Length; i++)
                    {
                        fill_image[i].fillAmount = 1;
                    }
                    if_fill_image.GetComponent<Image>().fillAmount = 1;
                    this.gameObject.GetComponent<Image>().fillAmount = 1;
                    circle_flag = true;
                }
                break;
            case "방어력":
                if (GameManager.instance.userinfo.shield_research)
                {
                    for (int i = 0; i < fill_image.Length; i++)
                    {
                        fill_image[i].fillAmount = 1;
                    }
                    if_fill_image.GetComponent<Image>().fillAmount = 1;
                    this.gameObject.GetComponent<Image>().fillAmount = 1;
                    circle_flag = true;
                }
                break;
            case "체력회복":
                if (GameManager.instance.userinfo.recovery_research)
                {
                    for (int i = 0; i < fill_image.Length; i++)
                    {
                        fill_image[i].fillAmount = 1;
                    }
                    if_fill_image.GetComponent<Image>().fillAmount = 1;
                    this.gameObject.GetComponent<Image>().fillAmount = 1;
                    circle_flag = true;
                }
                break;
            case "스킬데미지":
                if (GameManager.instance.userinfo.skilldamage_research)
                {
                    for (int i = 0; i < fill_image.Length; i++)
                    {
                        fill_image[i].fillAmount = 1;
                    }
                    if_fill_image.GetComponent<Image>().fillAmount = 1;
                    this.gameObject.GetComponent<Image>().fillAmount = 1;
                    circle_flag = true;
                }
                break;
            case "경험치":
                if (GameManager.instance.userinfo.exp_research)
                {
                    for (int i = 0; i < fill_image.Length; i++)
                    {
                        fill_image[i].fillAmount = 1;
                    }
                    if_fill_image.GetComponent<Image>().fillAmount = 1;
                    this.gameObject.GetComponent<Image>().fillAmount = 1;
                    circle_flag = true;
                }
                break;
            case "포인트":
                if (GameManager.instance.userinfo.point_research)
                {
                    for (int i = 0; i < fill_image.Length; i++)
                    {
                        fill_image[i].fillAmount = 1;
                    }
                    if_fill_image.GetComponent<Image>().fillAmount = 1;
                    this.gameObject.GetComponent<Image>().fillAmount = 1;
                    circle_flag = true;
                }
                break;
        }
        researchManager.CheckFillAmount();
    }


    public bool circle_flag;
    public bool if_fill_flag;

    public void PointDownCircle()
    {
        explanation.SetActive(true);
        explanation.transform.localPosition = new Vector2(this.transform.localPosition.x, this.transform.localPosition.y + 150);
        explanation.transform.GetChild(0).GetComponent<Text>().text = reserch_Kind.ToString() + " 10";
        explanation.transform.GetChild(1).GetComponent<Text>().text = "포인트 " + level_point(GameManager.instance.userinfo.research_level);

        Explanation();
        Research_Level_Point.transform.GetChild(0).GetComponent<Text>().DOFade(1, .5F);
        Research_Level_Point.transform.GetChild(1).GetComponent<Text>().DOFade(1, .5F);

        if (!if_fill_flag || GameManager.instance.userinfo.point < 0.02f)
            return;

        if (this.GetComponent<Image>().fillAmount == 1)
            return;
        circle_flag = true;
    }
    public void PointUpCircle()
    {
        explanation.SetActive(false);

        Research_Level_Point.transform.GetChild(0).GetComponent<Text>().DOFade(0, .5F);
        Research_Level_Point.transform.GetChild(1).GetComponent<Text>().DOFade(0, .5F);

        if (!if_fill_flag)
            return;

        if ((this.GetComponent<Image>().fillAmount == 0 || this.GetComponent<Image>().fillAmount == 1))
            return;
        circle_flag = false;
    }

    private void FixedUpdate()
    {
        if (circle_flag && GameManager.instance.userinfo.point > 0 && if_fill_flag)
        {
            this.GetComponent<Image>().fillAmount += 0.02f;
            if (this.GetComponent<Image>().fillAmount < 1)
            {
                GameManager.instance.userinfo.point -= (Time.deltaTime * level_point(GameManager.instance.userinfo.research_level));
                Explanation();
            }
        }
        if (!circle_flag)
        {
            if (this.GetComponent<Image>().fillAmount > 0)
            {
                this.GetComponent<Image>().fillAmount -= 0.02f;
                GameManager.instance.userinfo.point += (Time.deltaTime * level_point(GameManager.instance.userinfo.research_level));
                Explanation();
            }
        }

        if (this.GetComponent<Image>().fillAmount == 1)
        {
            switch (reserch_Kind)
            {
                case Reserch_kind.Null:
                    break;
                case Reserch_kind.최대체력:
                    GameManager.instance.userinfo.hp_research = true;
                    break;
                case Reserch_kind.공격력:
                    GameManager.instance.userinfo.atk_research = true;
                    break;
                case Reserch_kind.공격속도:
                    GameManager.instance.userinfo.speed_research = true;
                    break;
                case Reserch_kind.이동속도:
                    GameManager.instance.userinfo.atkspeed_research = true;
                    break;
                case Reserch_kind.아이템획득거리:
                    GameManager.instance.userinfo.item_research = true;
                    break;
                case Reserch_kind.방어력:
                    GameManager.instance.userinfo.shield_research = true;
                    break;
                case Reserch_kind.체력회복:
                    GameManager.instance.userinfo.recovery_research = true;
                    break;
                case Reserch_kind.스킬데미지:
                    GameManager.instance.userinfo.skilldamage_research = true;
                    break;
                case Reserch_kind.경험치:
                    GameManager.instance.userinfo.exp_research = true;
                    break;
                case Reserch_kind.포인트:
                    GameManager.instance.userinfo.point_research = true;
                    break;
                default:
                    break;
            }

            for (int i = 0; i < fill_image.Length; i++)
            {
                if (fill_image[i].fillAmount < 1)
                    fill_image[i].fillAmount += 0.02f;
            }

            researchManager.CheckFillAmount();
        }

        if (if_fill_image != null && if_fill_image.fillAmount == 1 && !if_fill_flag)
        {
            if_fill_flag = true;
            Visible();
        }
    }

    void Visible()
    {
        gameObject.transform.GetChild(0).GetComponent<Image>().DOFade(1, 2f);
    }

    int level_point(int level)
    {
        return 10 * level;
    }

    void Explanation()
    {
        Research_Level_Point.transform.GetChild(0).GetComponent<Text>().text = "연구레벨 : " + GameManager.instance.userinfo.research_level + "레벨";
        Research_Level_Point.transform.GetChild(1).GetComponent<Text>().text = "포인트 : " + (int)GameManager.instance.userinfo.point;
    }
}

public enum Reserch_kind
{
    Null,
    최대체력,
    공격력,
    공격속도,
    이동속도,
    아이템획득거리,
    방어력,
    체력회복,
    스킬데미지,
    경험치,
    포인트
}
