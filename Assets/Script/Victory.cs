using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class Victory : MonoBehaviour
{
    public GameObject[] fadeOutObj;
    float fadeSpeed = 1f;

    public CountdownManager countdownManager;
    public int monsterDeadCount;
    public PlayerController playerController;
    public skillIcon[] skillIconList;
    public PlayerSkill playerSkill;
    public StageManager stageManager;

    private void OnEnable()
    {
        for (int i = 0; i < fadeOutObj.Length; i++)
        {
            if (fadeOutObj[i] != null)
            {
                if (fadeOutObj[i].GetComponent<Image>() != null)
                {
                    fadeOutObj[i].GetComponent<Image>().DOFade(0, 0);
                    fadeOutObj[i].GetComponent<Image>().DOFade(1, fadeSpeed);
                }
                if (fadeOutObj[i].GetComponent<Text>() != null)
                {
                    fadeOutObj[i].GetComponent<Text>().DOFade(0, 0);
                    fadeOutObj[i].GetComponent<Text>().DOFade(1, fadeSpeed);
                }
            }
        }

        string minuteString = ((int)countdownManager.playTime / 60).ToString("00");
        string secondString = ((int)countdownManager.playTime % 60).ToString("00");
        this.transform.Find("플레이 시간").GetChild(0).GetComponent<Text>().text = minuteString + ":" + secondString;
        this.transform.Find("몬스터 처치 수").GetChild(0).GetComponent<Text>().text = monsterDeadCount + "마리";
        this.transform.Find("획득 포인트").GetChild(0).GetComponent<Text>().text = playerController.currentPoint + "점";
        for (int i = 0; i < playerSkill.player_skill.Count; i++)
        {
            this.transform.Find("사용 액티브 스킬").GetChild(0).GetChild(i).gameObject.SetActive(true);
            this.transform.Find("사용 액티브 스킬").GetChild(0).GetChild(i).GetComponent<Image>().sprite = GetSkillIconSprite(playerSkill.player_skill[i]);
        }
    }


    public void Exit()
    {
        SceneManager.LoadScene("MainScene");
    }

    [System.Serializable]
    public struct skillIcon
    {
        public string skillName;
        public Sprite iconSprite;
    }

    public Sprite GetSkillIconSprite(string skillName)
    {
        for (int i = 0; i < skillIconList.Length; i++)
        {
            if (skillIconList[i].skillName == skillName)
            {
                return skillIconList[i].iconSprite;
            }
        }
        return null;
    }
}
