using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer
{
    private int seconds;
    private int minutes;
    private int hours;

    public Timer()
    {
        seconds = 0;
        minutes = 0;
        hours = 0;
    }

    public Timer(Timer timer)
    {
        this.seconds = timer.seconds;
        this.minutes = timer.minutes;
        this.hours = timer.hours;
    }
    
    public void Tick()
    {
        if (seconds == 59)
        {
            seconds = 0;
            minutes++;
        }
        else
        {
            seconds++;
        }

        if (minutes == 59)
        {
            minutes = 0;
            hours++;
        }
    }

    public string GetTimer()
    {
        string sec;
        string min;
        string hrs;

        if (seconds < 10)
        {
            sec = "0" + seconds.ToString();
        }
        else
        {
            sec = seconds.ToString();
        }

        if (minutes < 10)
        {
            min = "0" + minutes.ToString();
        }
        else
        {
            min = minutes.ToString();
        }

        if (hours < 10)
        {
            hrs = "0" + hours.ToString();
        }
        else
        {
            hrs = hours.ToString();
        }

        return hrs + ":" + min + ":" + sec; 
    }
}
