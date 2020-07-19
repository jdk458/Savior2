using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfoManager : MonoBehaviour
{
    public float point = 100;
    public int research_level = 1;

    public bool hp_research;
    public bool atk_research;
    public bool atkspeed_research;
    public bool speed_research;
    public bool item_research;
    public bool shield_research;
    public bool recovery_research;
    public bool skilldamage_research;
    public bool exp_research;
    public bool point_research;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("point"))
        {
            point = PlayerPrefs.GetFloat("point");
            research_level = PlayerPrefs.GetInt("research_level");

            hp_research = bool.Parse(PlayerPrefs.GetString("hp_research"));
            atk_research = bool.Parse(PlayerPrefs.GetString("atk_research"));
            atkspeed_research = bool.Parse(PlayerPrefs.GetString("atkspeed_research"));
            speed_research = bool.Parse(PlayerPrefs.GetString("speed_research"));
            item_research = bool.Parse(PlayerPrefs.GetString("item_research"));
            shield_research = bool.Parse(PlayerPrefs.GetString("shield_research"));
            recovery_research = bool.Parse(PlayerPrefs.GetString("recovery_research"));
            skilldamage_research = bool.Parse(PlayerPrefs.GetString("skilldamage_research"));
            exp_research = bool.Parse(PlayerPrefs.GetString("exp_research"));
            point_research = bool.Parse(PlayerPrefs.GetString("point_research"));
        }

    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("point", point);
        PlayerPrefs.SetInt("research_level", research_level);

        PlayerPrefs.SetString("hp_research", hp_research.ToString());
        PlayerPrefs.SetString("atk_research", atk_research.ToString());
        PlayerPrefs.SetString("atkspeed_research", atkspeed_research.ToString());
        PlayerPrefs.SetString("speed_research", speed_research.ToString());
        PlayerPrefs.SetString("item_research", item_research.ToString());
        PlayerPrefs.SetString("shield_research", shield_research.ToString());
        PlayerPrefs.SetString("recovery_research", recovery_research.ToString());
        PlayerPrefs.SetString("skilldamage_research", skilldamage_research.ToString());
        PlayerPrefs.SetString("exp_research", exp_research.ToString());
        PlayerPrefs.SetString("point_research", point_research.ToString());
    }


    public int GetMasicMaxHP()
    {
        if (hp_research)
        {
            return research_level;
        }
        else
        {
            return research_level - 1;
        }
    }
    public int GetMasicAtk()
    {
        if (atk_research)
        {
            return research_level;
        }
        else
        {
            return research_level - 1;
        }
    }

    public int GetMasicAtkspeed()
    {
        if (atkspeed_research)
        {
            return research_level;
        }
        else
        {
            return research_level - 1;
        }
    }

    public int GetMasicSpeed()
    {
        if (speed_research)
        {
            return research_level;
        }
        else
        {
            return research_level - 1;
        }
    }

    public int GetMasicItemDistance()
    {
        if (item_research)
        {
            return research_level;
        }
        else
        {
            return research_level - 1;
        }
    }

    public int GetMasicShield()
    {
        if (shield_research)
        {
            return research_level;
        }
        else
        {
            return research_level - 1;
        }
    }

    public int GetMasicRecovery()
    {
        if (recovery_research)
        {
            return research_level;
        }
        else
        {
            return research_level - 1;
        }
    }

    public int GetMasicSkillDamage()
    {
        if (skilldamage_research)
        {
            return research_level;
        }
        else
        {
            return research_level - 1;
        }
    }

    public int GetMasicExp()
    {
        if (exp_research)
        {
            return research_level;
        }
        else
        {
            return research_level - 1;
        }
    }

    public int GetMasicPoint()
    {
        if (point_research)
        {
            return research_level;
        }
        else
        {
            return research_level - 1;
        }
    }
}
