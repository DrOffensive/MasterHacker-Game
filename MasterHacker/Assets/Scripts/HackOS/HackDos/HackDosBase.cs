using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackDosBase : MonoBehaviour
{
    public List<DosPanel> screens;
    KeyCheck keyCheck;
    public bool loggedIn;
    HackDosPanel currentScreen;

    public HackDosPanel CurrentScreen
    {
        get => currentScreen;
    }

    [System.Serializable]
    public struct DosPanel
    {
        public string name;
        public HackDosPanel screen;
        public HackDosScreenBase panel;
    }

    public enum HackDosPanel
    {
        LoginPanel, PromptPanel, LoadScreen, ImageViewer, TextEditor 
    }

    public HackDosScreenBase GetScreen (HackDosPanel screen)
    {
        foreach (DosPanel d in screens)
        {
            if (d.screen.Equals(screen))
                return d.panel;
        }

        Debug.LogWarning(screen + " not set");
        return null;
    }

    public void Enable (bool on)
    {
        if(on)
        {
            if(loggedIn)
            {
                GetScreen(currentScreen).OnResume();
            } else
            {
                OpenScreen(HackDosPanel.LoginPanel);
            }
        } else
        {
            CloseAll();
        }
    }

    public void OpenScreen (HackDosPanel screen)
    {
        CloseAll();
        HackDosScreenBase t = GetScreen(screen);
        t.gameObject.SetActive(true);
        t.SetKeyCheck = keyCheck;
        t.OnOpen();
        currentScreen = screen;
    }

    void CloseAll ()
    {
        foreach(DosPanel d in screens)
        {
            d.panel.gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        keyCheck = FindObjectOfType<KeyCheck>();
        //Enable(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
