using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float time;
    private bool counting;
    public Action OnZero;
    public Action<string> UpdateGUI;

    public void StartTimer(float start)
    {
        Debug.Log("started timer");
        time = start;
        counting = true;
    }

    public void EndTimer(bool withAction)
    {
        Debug.Log("end timer");
        time = 0f;
        counting = false;
        if (withAction) OnZero?.Invoke();
    }

    public string GetTime()
    {
        return Mathf.RoundToInt(time) + "";
    }

    private void Update()
    {
        if (!counting) return;

        time -= Time.unscaledDeltaTime;
        if (time <= 0f) EndTimer(true);
        UpdateGUI.Invoke(GetTime());
    }
}

