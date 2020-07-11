using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningGround : MonoBehaviour
{
    SkeletonAnimation skeletonAnimation;

    private void Start()
    {
        skeletonAnimation = this.GetComponent<SkeletonAnimation>();
        string[] name = { "32_1", "32_2", "32_3" };
        int[] time = { 0, 2, 0 };
        skeletonAnimation.skeleton.SetSkin("32");
        StartCoroutine(SetAniCoroutine(name,time));
    }
    IEnumerator SetAniCoroutine(string[] name, int[] time)
    {
        for (int i = 0; i < name.Length; i++)
        {
            if (i == 1)
                skeletonAnimation.AnimationState.SetAnimation(0, name[i], true);
            else skeletonAnimation.AnimationState.SetAnimation(0, name[i], false);
            yield return new WaitForSeconds(time[i]);
        }
    }
}
