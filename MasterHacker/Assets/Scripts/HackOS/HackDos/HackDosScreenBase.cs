using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HackDosScreenBase : MonoBehaviour
{
    public string name;
    public bool keyListen;
    KeyCheck keyCheck;

    public KeyCheck SetKeyCheck { set { keyCheck = value; } }
    public KeyCheck GetKeyCheck { get { return keyCheck; } }

    public void ApplyKeyListen ()
    {
        GetKeyCheck.listen = keyListen;
    }

    public abstract void OnOpen();

    public virtual void OnEscape()
    {
        GetKeyCheck.listen = false;
        GetKeyCheck.Defocus();
        FindObjectOfType<ZoomToPC>().ReturnToPlayer();
    }

    public abstract void OnClose();
    public abstract void NextSelect();
    public abstract void PrevSelect();
    public abstract void OnEnter();
    public abstract void OnAnyKey();

    public virtual void OnUpArrow () { }
    public virtual void OnDownArrow() { }

    public virtual void CheckSpecialKeys()
    {
        if (keyListen)
        {
            if (GetKeyCheck.AnyKey)
                OnAnyKey();

            if (GetKeyCheck.ReturnKey)
                OnEnter();

            if (GetKeyCheck.EscapeKey)
                OnEscape();

            if (GetKeyCheck.UpArrow)
                OnUpArrow();

            if (GetKeyCheck.DownArrow)
                OnDownArrow();

            int prevNext = GetKeyCheck.PrevNext;
            if (prevNext == 1)
                NextSelect();
            else if (prevNext == -1)
                PrevSelect();

        }
    }

    public virtual void OnResume ()
    {
        ApplyKeyListen();
    }

    public enum KeyCheckReturn
    {
        None, Enter, Escape, Next, Previous, UpArrow, DownArrow
    }
}
