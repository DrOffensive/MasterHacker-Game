using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyCheck : MonoBehaviour
{
    public bool listen = false;
    public Text targetText;
    public char passwordChar = 'x', cursorChar = '_', blinkChar = ' ';
    int textIndex;
    public KeyCode specialKey = KeyCode.LeftAlt, invertKey = KeyCode.LeftShift, nextKey = KeyCode.Tab;

    string autoFill = "";
    public string SetAutoFill { set => autoFill = value; }
    [SerializeField]
    string finalText = "";
    string prefix = "";

    public string SetPrefix { set => prefix = value; }

    public int maxChars = 12;
    public bool password;

    bool holdEnter, enter;
    bool holdEsc, esc;
    bool anyKey;
    int nextPrev = 0;
    public float holdEnterTime = .25f;
    float enterPressed, escPressed, prevNextPressed;
    public bool cursorBlink;
    public float cursorBlickSpeed = .5f;
    float blink;
    bool blinkOn = true;
    public string GetRawText { get => finalText; }

    // Start is called before the first frame update
    void Start()
    {
        if (targetText != null)
            FocusText(targetText, "", 12, false, true);

        blink = Time.time + cursorBlickSpeed;
    }


    public bool AnyKey
    {
        get
        {
            bool a = anyKey;
            anyKey = false;
            return a;
        }
    }

    public bool ReturnKey
    {
        get
        {
            bool e = enter;
            enter = false;
            return e;
        }
    }

    public bool EscapeKey
    {
        get
        {
            bool e = esc;
            esc = false;
            return e;
        }
    }

    public int PrevNext
    {
        get
        {
            int p = nextPrev;
            nextPrev = 0;
            return p;
        }
    }


    public void FocusText (Text target, string pref, int maxLength, bool isPassword, bool clearOnFocus)
    {
        if (targetText != null)
        {   if(password)
            {
                string t = "";
                for(int i = 0; i < prefix.Length + finalText.Length; i++)
                {
                    t += passwordChar;
                }
                targetText.text = t;
            } else
                targetText.text = prefix + finalText;
        }

        targetText = target;
        if (clearOnFocus)
            finalText = "";
        else
            finalText = target.text;
        prefix = pref;
        textIndex = finalText.Length;

        maxChars = maxLength;
        password = isPassword;

        UpdateText();
    }

    public void Clear ()
    {
        finalText = "";
        UpdateText();
    }

    public void Defocus ()
    {
        if (targetText != null)
        {
            if (password)
            {
                string t = "";
                for (int i = 0; i < prefix.Length + finalText.Length; i++)
                {
                    t += passwordChar;
                }
                targetText.text = t;
            }
            else
                targetText.text = prefix + finalText;
        }
        targetText = null;
    }

    public void FocusText(TextField field, string pref, bool clearOnFocus)
    {
        if (targetText != null)
        {
            if (password)
            {
                string t = "";
                for (int i = 0; i < pref.Length + finalText.Length; i++)
                {
                    t += passwordChar;
                }
                targetText.text = t;
            }
            else
                targetText.text = pref + finalText;
        }

        targetText = field.text;
        if (clearOnFocus)
            finalText = "";
        else
            finalText = field.text.text;
        prefix = pref;
        textIndex = finalText.Length;

        maxChars = field.maxLength;
        password = field.isPassword;

        UpdateText();
    }



    void UpdateText ()
    {
        if (textIndex > finalText.Length)
            textIndex = Mathf.Clamp(finalText.Length, 0, Mathf.Abs(finalText.Length));
        string t = "";
        if (textIndex > 0)
        {
            for (int i = 0; i < textIndex; i++)
            {
                if (!password)
                    t += finalText[i];
                else
                    t += passwordChar;
            }
        }
        if (cursorBlink)
        {
            t += blinkOn ? "" + cursorChar : "" + blinkChar;

            if (Time.time >= blink)
            {
                blinkOn = !blinkOn;
                blink = Time.time + cursorBlickSpeed;
            }
        }else
        {
            t += "" + cursorChar;
        }
        for (int i = 0; i < /*Mathf.Clamp(*/finalText.Length - textIndex/* - 1, 0, finalText.Length)*/; i++)
        {
            if (textIndex + i > 0)
            {
                if (!password)
                    t += finalText[i + textIndex];
                else if (i != finalText.Length - 1)
                    t += passwordChar;
            }
        }

        if(holdEnter)
        {
            enter = true;
            holdEnter = false;
            enterPressed = Time.time;
        }

        if (holdEsc)
        {
            esc = true;
            holdEsc = false;
            escPressed = Time.time;
        }

        targetText.text = prefix + t;
    }

    // Update is called once per frame
    void Update()
    {
        if(enter)
        {
            if (enterPressed + holdEnterTime >= Time.time)
                enter = false;
        }
        if (esc)
        {
            if (escPressed + holdEnterTime >= Time.time)
                esc = false;
        }
        if(nextPrev != 0)
        {
            if (prevNextPressed + holdEnterTime >= Time.time)
                nextPrev = 0;
        }

        if (targetText!=null && listen)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && !password)
            {
                textIndex = Mathf.Clamp(textIndex - 1, 0, targetText.text.Length - 1);
                anyKey = true;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && !password)
            {
                textIndex = Mathf.Clamp(textIndex + 1, 0, targetText.text.Length - 1);
                anyKey = true;
            }
            ScanKeys();
        } 
        if(targetText == null && listen)
        {
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                enter = true;
                enterPressed = Time.time;
                anyKey = true;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                esc = true;
                escPressed = Time.time;
                anyKey = true;
            }

            if (Input.GetKeyDown(nextKey))
            {
                if (Input.GetKey(invertKey))
                {
                    nextPrev = -1;
                }
                else
                {
                    nextPrev = 1;
                }
                anyKey = true;
                prevNextPressed = Time.time;
            }
        }
    } 

    public void ScanKeys ()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // has backspace/delete been pressed?
            {
                anyKey = true;
                if (finalText.Length != 0)
                {
                    {
                        if (textIndex != 0)
                        {
                            string t1 = "";
                            int t1Length = textIndex - 1;
                            for (int i = 0; i < t1Length; i++)
                            {
                                t1 += finalText[i];
                            }

                            int t2Length = finalText.Length - (t1Length + 1);
                            for (int i = 0; i < t2Length; i++)
                            {
                                t1 += finalText[t1Length + 1 + i];
                            }
                            finalText = t1;
                            textIndex--;
                        }
                    }
                }
            }
            else if(c== '')
            {
                anyKey = true;
                string[] words = finalText.Split(' ');
                if (words.Length > 0)
                {
                    int soFar = 0;
                    int w = words.Length - 1;
                    for (int i = 0; i < words.Length; i++)
                    {
                        soFar += words[i].Length + 1;
                        if (textIndex < soFar)
                        {
                            w = i;
                            break;
                        }
                    }
                    string t = "";
                    for (int i = 0; i < words.Length; i++)
                    {
                        if (i != w)
                        {
                            t += words[i] + (i == words.Length - 1 ? "" : " ");
                        }
                        else
                        {
                            textIndex = t.Length - 1 > 0 ? t.Length - 1 : 0;
                        }
                    }
                    finalText = t;
                }
            }
            else if ((c == '\n') || (c == '\r')) // enter/return
            {
                anyKey = true;
                if (autoFill != "" && Input.GetKey(specialKey))
                    finalText = autoFill;
                if (!enter)
                {
                    holdEnter = true;
                }
            }
            else
            {
                anyKey = true;
                if (finalText.Length < maxChars)
                {
                    string t = "";
                    for (int i = 0; i < textIndex; i++)
                    {
                        t += finalText[i];
                    }
                    t += c;
                    for (int i = 0; i < Mathf.Clamp(finalText.Length - textIndex - 1, 0, finalText.Length); i++)
                    {
                        t += finalText[i + textIndex];
                    }

                    finalText = t;
                    textIndex++;
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Delete))
        {
            anyKey = true;
            string t1 = "";
            int t1Length = textIndex;
            for (int i = 0; i < t1Length; i++)
            {
                t1 += finalText[i];
            }

            int t2Length = finalText.Length - (t1Length + 1);
            for (int i = 0; i < t2Length; i++)
            {
                t1 += finalText[t1Length + 1 + i];
            }
            finalText = t1;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            holdEsc = true;

        if(Input.GetKeyDown(nextKey))
        {
            anyKey = true;
            if (Input.GetKey(invertKey))
            {
                nextPrev = -1;
            } else
            {
                nextPrev = 1;
            }
            prevNextPressed = Time.time;
        }

        UpdateText();
    }

    [System.Serializable]
    public struct TextField
    {
        public Text text;
        public int maxLength;
        public bool isPassword;

        public string autoFill;

        public string AutoFill { get => autoFill; set => autoFill = value; }
    }
}
