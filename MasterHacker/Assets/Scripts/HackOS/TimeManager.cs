 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public bool realTime, UsFormat;
    public int startYear, startMonth, startDay, startHour, startMinute;
    float sec;
    int year, month, day, hour, min;

    public int[] monthLengths = new int[] { 31, 28, 31, 30, 31, 30, 31,31, 30, 31, 30, 31 };
    [Header("Time Scale")]
    public float timeScale = 1;
    // Start is called before the first frame update
    void Start()
    {
        if (!realTime)
        {

            year = startYear < 1976 ? 1976 : startYear;
            month = startMonth == 0 ? 1 : startMonth;
            day = startDay == 0 ? 1 : startDay; 
            hour = startHour;
            min = startMinute;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!realTime)
        {
            sec += Time.deltaTime * timeScale;
            if (sec > 60f)
            {
                sec -= 60f;
                min++;
                if(min > 60)
                {
                    hour++;
                    min -= 60;

                    if (hour > 23)
                    {
                        hour = 0;
                        day++;
                        if(day > monthLengths[month-1])
                        {
                            day = 1;
                            month++;
                            if(month > 12)
                            {
                                month = 1;
                                year++;
                            }
                        }
                    }
                }
            }
        }
    }

    public TimeStamp GetTimeStamp ()
    {
        return new TimeStamp(year, month, day, hour, min, (int)sec);
    }

    public string GetAsString ()
    {
        if(realTime)
        {
            if(UsFormat)
            {
                System.DateTime t = System.DateTime.Now;
                int h = t.Hour;
                int m = t.Minute;
                string mi = m.ToString();
                string ho = h.ToString();
                bool pm = false;

                if (m < 10)
                    mi = "0" + mi;
                if (h > 12)
                {
                    h -= 12;
                    ho = h.ToString();
                    pm = true;
                }
                if (h < 10)
                    ho = "0" + ho;
                

                return ho + ":" + mi + (pm ? " PM" : " AM");
            } else
            {
                System.DateTime t = System.DateTime.Now; 
                int h = t.Hour;
                int m = t.Minute;
                string mi = m.ToString();
                string ho = h.ToString();

                if (m < 10)
                    mi = "0" + mi;

                if (h < 10)
                    ho = "0" + ho;

                return ho + ":" + mi;
            }
        } else
        {
            if (UsFormat)
            {

                string mi = min.ToString();
                int h = hour;
                string ho = h.ToString();
                bool pm = false;

                if (min < 10)
                    mi = "0" + mi;

                if (h > 12)
                {
                    h -= 12;
                    pm = true;
                }
                if (h < 10)
                    ho = "0" + ho;

                return ho + ":" + mi + (pm ? " PM" : " AM");
            }
            else
            {
                string mi = min.ToString();
                string ho = hour.ToString();

                if (min < 10)
                    mi = "0" + mi;

                if (hour < 10)
                    ho = "0" + ho;

                return ho + ":" + mi;
            }

        }
    }
}
