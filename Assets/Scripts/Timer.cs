using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Timer : MonoBehaviour
{
    public float time;
    private bool counting;
    public Action OnZero;
    public Action<string> UpdateGUI;

    public void StartTimer(float start)
    {
        Time.timeScale = 1f;
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
        
        time -= Time.deltaTime;
        Debug.Log(Time.deltaTime);
        Debug.Log("please " + time);
        if (time <= 0f) EndTimer(true);
        UpdateGUI.Invoke(GetTime());
    }
}

