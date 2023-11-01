using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenMove : MonoBehaviour
{
    public void Move(Vector3 moveTo, Action action = null)
    {
        if (action == null)
        {
            LeanTween.moveLocal(gameObject, moveTo, 0.2f).setIgnoreTimeScale(true);
        }
        else
        {
            LeanTween.moveLocal(gameObject, moveTo, 0.2f).setOnComplete(action).setIgnoreTimeScale(true);
        }
    }
}
