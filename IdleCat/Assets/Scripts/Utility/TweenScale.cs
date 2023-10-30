using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenScale : MonoBehaviour
{
    public void ScaleUp(Vector3? scale, Action action = null)
    {
        AlterState();
        if (scale == null)
        {
            if (action == null)
            {
                LeanTween.scale(gameObject, Vector3.one, 0.2f).setIgnoreTimeScale(true);
            }
            else
            {
                LeanTween.scale(gameObject, Vector3.one, 0.2f).setOnComplete(action).setIgnoreTimeScale(true);
            }
        }
        else
        {
            if (action == null)
            {
                LeanTween.scale(gameObject, scale.Value, 0.2f).setIgnoreTimeScale(true);
            }
            else
            {
                LeanTween.scale(gameObject, scale.Value, 0.2f).setOnComplete(action).setIgnoreTimeScale(true);
            }
        }
    }

    public void ScaleDown(Vector3? scale, Action action = null)
    {
        if (scale == null)
        {
            if (action == null)
            {
                LeanTween.scale(gameObject, Vector3.zero, 0.2f).setOnComplete(AlterState).setIgnoreTimeScale(true);
            }
            else
            {
                LeanTween.scale(gameObject, Vector3.zero, 0.2f).setOnComplete(AlterState).setOnComplete(action).setIgnoreTimeScale(true);
            }
        }
        else
        {
            if (action == null)
            {
                LeanTween.scale(gameObject, scale.Value, 0.2f).setOnComplete(AlterState).setIgnoreTimeScale(true);
            }
            else
            {
                LeanTween.scale(gameObject, scale.Value, 0.2f).setOnComplete(AlterState).setOnComplete(action).setIgnoreTimeScale(true);
            }
        }
    }

    private void AlterState()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
