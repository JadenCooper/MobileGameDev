using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenScale : MonoBehaviour
{
    public void ScaleUp(Vector3? scale)
    {
        AlterState();
        if (scale == null)
        {
            LeanTween.scale(gameObject, Vector3.one, 0.2f);
        }
        else
        {
            LeanTween.scale(gameObject, scale.Value, 0.2f);
        }
    }

    public void ScaleDown(Vector3? scale)
    {
        if (scale == null)
        {
            LeanTween.scale(gameObject, Vector3.zero, 0.2f).setOnComplete(AlterState);
        }
        else
        {
            LeanTween.scale(gameObject, scale.Value, 0.2f).setOnComplete(AlterState);
        }
    }

    private void AlterState()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
