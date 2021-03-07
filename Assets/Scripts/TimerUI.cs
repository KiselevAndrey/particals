using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [Header("Стартовые данные")]
    public bool isTick;
    public bool isRunDown;

    [Header("Связывающие объекты")]
    [SerializeField] Text timerText;

    float _time;
    int _minuts;
    int _sec;
    int _millisec;

    void Update()
    {
        if (!isTick || Time.timeScale == 0) return;

        _time += isRunDown ? -Time.deltaTime : Time.deltaTime;

        FloatToTime();
    }

    #region Time
    void FloatToTime()
    {
        _minuts = (int)_time / 60;
        _sec = (int)_time % 60;
        _millisec = (int)(_time % 1 * 100);

        DrawTime();
    }

    void DrawTime()
    {
        string temp = IntToString(_minuts )+ " : ";
        temp += IntToString(_sec) + " : ";
        temp += IntToString(_millisec);

        timerText.text = temp;
    }

    string IntToString(int value)
    {
        if (value < 10) return "0" + value;
        else return value.ToString();
    }

    public float GetTime() => _time;
    public void SetTime(float time)
    {
        _time = time;
        FloatToTime();
    }
    #endregion
}
