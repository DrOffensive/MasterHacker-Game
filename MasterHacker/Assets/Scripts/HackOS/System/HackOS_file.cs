using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HackOS_file : HackOs_driveData
{
    public string extension, content;
    public bool locked;
    public TimeStamp creationTime;

    public HackOS_file(string n, string e, string c, TimeStamp stamp)
    {
        name = n;
        extension = e;
        content = c;
        creationTime = stamp;
    }

    public HackOS_file(HackOS_file file, TimeStamp stamp)
    {
        name = file.name;
        extension = file.extension;
        content = file.content;
        creationTime = stamp;
    }

    public override HackOs_driveData Copy (TimeStamp stamp) {
        return new HackOS_file(this, stamp);
    }

    public override int Size ()
    {
        return content.Length * 8 + (name.Length * 8);
    }
}

public struct TimeStamp
{
    public int year, month, day, hour, minute, second;

    public TimeStamp(int year, int month, int day, int hour, int minute, int second)
    {
        this.year = year;
        this.month = month;
        this.day = day;
        this.hour = hour;
        this.minute = minute;
        this.second = second;
    }

    string GetMonth ()
    {
        switch (month)
        {
            case 1:
                return "January";
            case 2:
                return "February";
            case 3:
                return "March";
            case 4:
                return "April";
            case 5:
                return "May";
            case 6:
                return "June";
            case 7:
                return "July";
            case 8:
                return "August";
            case 9:
                return "September";
            case 10:
                return "October";
            case 11:
                return "November";
            case 12:
                return "December";

            default: return "January";
        }
    }

    public string GetFull ()
    {
        string dayPostfix = "th.";
        if (day == 1)
            dayPostfix = "st.";
        if (day == 2)
            dayPostfix = "nd.";
        if (day == 3)
            dayPostfix = "rd.";

        string h = (hour < 10 ? "0" : "") + hour.ToString();
        string m = (minute < 10 ? "0" : "") + minute.ToString();
        string s = (second < 10 ? "0" : "") + second.ToString();

        return GetMonth() + " " + day.ToString() + dayPostfix + " - " + h + ":" + m + ":" + s;  
    }
}