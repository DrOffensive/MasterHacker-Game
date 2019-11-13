using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScreen : HackDosScreenBase
{
    public Image loadingBar;

    public LoadInterupt[] loadStages;
    public Text jobDescription, percentOutput;
    int stage = 0;
    float next;

    [System.Serializable]
    public struct LoadInterupt
    {
        [Range(0, 1)]
        public float progress;
        public string job;
        [Range(0, 2)]
        public float loadTime;
    }

    public override void OnEnter()
    {
        //throw new System.NotImplementedException();
    }

    public override void NextSelect()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnClose()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnOpen()
    {
        ApplyKeyListen();
        stage = 0;
        next = Time.time;
    }

    public override void OnAnyKey()
    {
        //throw new System.NotImplementedException();
    }

    void Load ()
    {
       if(next <= Time.time)
        {
            if(stage+1 < loadStages.Length)
            {
                loadingBar.fillAmount = loadStages[stage].progress;
                stage++;
                next = Time.time + loadStages[stage].loadTime;

                percentOutput.text = (int)(loadStages[stage].progress * 100) + "%";

                if(jobDescription!=null)
                    jobDescription.text = loadStages[stage].job;
            }else
            {
                FindObjectOfType<HackDosBase>().OpenScreen(HackDosBase.HackDosPanel.PromptPanel);
            }
        }
    }

    public override void PrevSelect()
    {
        //throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Load();
    }
}
