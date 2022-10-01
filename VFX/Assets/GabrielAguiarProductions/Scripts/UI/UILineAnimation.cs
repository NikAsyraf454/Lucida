using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UILineAnimation : MonoBehaviour
{
    public UILineRenderer[] lines;

    public float time = 1f;

    private void OnEnable()
    {
        AnimateLines();
    }

    public void AnimateLines()
    {
        foreach(UILineRenderer line in lines)
        {
            AnimateLine(line);
        }
    }

    void AnimateLine(UILineRenderer line)
    {
        List<Vector2> points = line.points;

        Animate(line, points);
    }

    public void Animate(UILineRenderer line, List<Vector2> points)
    {
        line.points = new List<Vector2>();

        for(int i=0; i<points.Count; i++)
        {
            int index = i;
            AnimatePoint(line, index, new Vector2(0, 4), points[index]);
        }
    }

    void AnimatePoint(UILineRenderer line, int index, Vector2 start, Vector2 end)
    {
        LeanTween.delayedCall(time * index, () =>
        {
            if(index > 0)
            {
                start = line.points[index - 1];
                line.points.Add(start);
            }
            else
            {
                line.points.Add(start);
            }

            LeanTween.value(gameObject, (value) =>
            {
                line.points[index] = value;
                line.SetVerticesDirty();
            }, start, end, time);
        });

    }


}
