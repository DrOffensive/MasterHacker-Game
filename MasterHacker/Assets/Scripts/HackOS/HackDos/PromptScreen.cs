using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptScreen : HackDosScreenBase
{
    List<string> mountedDir = new List<string>() { "root" };
    public string bootText;
    public CommandParser parser;
    public Text[] lines;
    int currentLine = 0;

    public int CurrentLine { get => currentLine; }
    public List<string> MountedDir { get => mountedDir; }

    public void SetMountedDir (List<string> dir) { mountedDir = dir; }

    bool ynPrompt, pwPrompt;

    public int YnPrompt {
        get
        {
            string raw = GetKeyCheck.GetRawText;
            if (raw.ToLower() == "y")
            {
                LineAdvance();
                GetKeyCheck.Defocus();
                return 1;
            }
            else if (raw.ToLower() == "n")
            {
                LineAdvance();
                GetKeyCheck.Defocus();
                return -1;
            }            
            else
                GetKeyCheck.Clear();

            return 0;
        }
    }

    public bool PwPrompt
    {
        get
        {
            bool pw = pwPrompt;
            pwPrompt= false;
            return pw;
        }
    }

    public void InsertLine (string line)
    {
        lines[currentLine].text = line;
        LineAdvance();
    }

    public void LineAdvance ()
    {
        if(currentLine < lines.Length-1)
        {
            currentLine++;
        } else
        {
            Scroll();
        }
    }

    void Scroll ()
    {
        for(int i = 0; i < lines.Length - 1; i++)
        {
            lines[i].text = lines[i + 1].text;
        }
        lines[lines.Length - 1].text = "";
    }

    public void Refocus ()
    {
        GetKeyCheck.FocusText(lines[currentLine], Prefix, 40, false, true);
    }

    public override void NextSelect()
    {
    }

    public override void OnClose()
    {
    }

    public override void OnEnter()
    {
        string[] commandSplit = GetKeyCheck.GetRawText.Split(';');
        GetKeyCheck.Defocus();
        LineAdvance();
        List<string> commands = new List<string>();
        foreach(string c in commandSplit)
        {
            if (c != "")
                commands.Add(c);
            /*else
                commands.Add(" ");*/
        }
        if (commands.Count > 0)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                if (i < commands.Count - 1)
                {
                    parser.QueueCommand(commands[i], this, false);
                }
                else
                    parser.QueueCommand(commands[i], this, true);
            }
        } else
        {
            Refocus();
        }
    }

    public override void OnOpen()
    {

        InsertLine(bootText);
        Refocus();
        
        ApplyKeyListen();
    }

    public void YesNoPrompt (string description)
    {
        GetKeyCheck.FocusText(lines[currentLine], description + " [y/n]: ", 1, false, true);
    }

    string Prefix
    {
        get
        {
            string prefix = "~/";
            foreach (string s in mountedDir)
            {
                prefix += s + "/";
            }
            return prefix + ">";
        }
    }


    public override void OnAnyKey()
    {
        //throw new System.NotImplementedException();
    }

    public override void PrevSelect()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckSpecialKeys();
    }

    public static string StepIn (string baseString, int step, int margin)
    {
        string str = "";
        for(int i = 0; i < step; i++)
        {
            if (i < baseString.Length && i < step - margin)
                str += baseString[i];
            else
                str += " ";
        }
        return str;
    }
}
