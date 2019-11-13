using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginScreen :  HackDosScreenBase
{
    public KeyCheck.TextField usernameText, passwordText;
    int position = 0;
    public Text failedText;

    /*public override void CheckSpecialKeys()
    {
        if (keyListen)
        {
            if (GetKeyCheck.ReturnKey)
                OnEnter();

            if (GetKeyCheck.EscapeKey)
                OnEscape();

            int prevNext = GetKeyCheck.PrevNext;
            if (prevNext == 1)
                NextSelect();
            else if (prevNext == -1)
                PrevSelect();
        }
    }*/

    string uName, pWord;

    public override void OnEnter()
    {
        if(position == 0)
        {
            uName = GetKeyCheck.GetRawText;
            GetKeyCheck.FocusText(passwordText, "", false);
            GetKeyCheck.SetAutoFill = passwordText.AutoFill;
            position = 1;
        } else
        {
            pWord = GetKeyCheck.GetRawText;
            if(CheckLoginInformation())
            {
                FindObjectOfType<HackDosBase>().loggedIn = true;
                FindObjectOfType<HackDosBase>().OpenScreen(HackDosBase.HackDosPanel.LoadScreen);
            } else
            {
                usernameText.text.text = "";
                passwordText.text.text = "";
                GetKeyCheck.FocusText(usernameText, "", false);
                position = 0;
                failedText.enabled = true;
            }
        }
    }

    public override void NextSelect()
    {
        position = position + 1 < 2 ? 1 : 0;

        if (position == 0)
        {
            pWord = GetKeyCheck.GetRawText;
            GetKeyCheck.FocusText(usernameText, "", false);
            GetKeyCheck.SetAutoFill = usernameText.AutoFill;
        }
        else
        {
            uName = GetKeyCheck.GetRawText;
            GetKeyCheck.FocusText(passwordText, "", false);
            GetKeyCheck.SetAutoFill = passwordText.AutoFill;
        }
    }

    public override void OnClose()
    {
        //FindObjectOfType<HackDosBase>().OpenScreen(HackDosBase.HackDosPanel.PromptPanel);
    }

    public override void OnOpen()
    {
        ApplyKeyListen();
        GetKeyCheck.FocusText(usernameText, "", false);
        position = 0;
        HackOS_Base os = FindObjectOfType<HackOS_Base>();
        usernameText.AutoFill = os.profile;
        passwordText.AutoFill = os.password;
        GetKeyCheck.SetAutoFill = usernameText.AutoFill;
    }

    public override void PrevSelect()
    {
        position = position - 1 < 0 ? 1 : 0;

        if (position == 0)
        {
            pWord = GetKeyCheck.GetRawText;
            GetKeyCheck.FocusText(usernameText, "", false);
            GetKeyCheck.SetAutoFill = usernameText.AutoFill;
        }
        else
        {
            uName = GetKeyCheck.GetRawText;
            GetKeyCheck.FocusText(passwordText, "", false);
            GetKeyCheck.SetAutoFill = passwordText.AutoFill;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool CheckLoginInformation ()
    {
        HackOS_Base os = FindObjectOfType<HackOS_Base>();
        if (os.profile == uName && os.password == pWord)
            return true;

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        /*KeyCheckReturn ret = CheckSpecialKeys();
        if(ret != KeyCheckReturn.None)
        {
            Debug.Log(ret.ToString());
            switch (ret)
            {
                case KeyCheckReturn.Enter:
                    Enter();
                    break;

                case KeyCheckReturn.Escape:
                    OnEscape();
                    break;

                case KeyCheckReturn.Next:
                    NextSelect();
                    break;

                case KeyCheckReturn.Previous:
                    PrevSelect();
                    break;
            }
        }*/
        CheckSpecialKeys();
    }

    public override void OnAnyKey()
    {
        failedText.enabled = false;
    }
}
