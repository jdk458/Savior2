﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI2_Manager : MonoBehaviour
{
    public Transform mainCam;
    public GameObject blackpannel;
    public GameObject skillSelect;
    public AudioSource exitsound;
    public GameObject theCam;

    public void SkillSelect()
    {
        theCam.SetActive(true);
        theCam.transform.position = new Vector3(mainCam.position.x, mainCam.position.y, theCam.transform.position.z);
        blackpannel.SetActive(true);
        skillSelect.SetActive(true);
        skillSelect.transform.GetChild(0).gameObject.SetActive(true);
        skillSelect.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void SkillSelect_Exit()
    {
        theCam.SetActive(false);
        GameManager.instance.audioManager.EnvironVolume_Play(exitsound);
        TimeManager.instance.SetTime(false);
        blackpannel.SetActive(false);
        skillSelect.SetActive(false);
    }
}
