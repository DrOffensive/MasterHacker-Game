using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HackOSSystem : MonoBehaviour
{
    public CommandParser.StringCommand entryPoint;
    public string entryMessage;
    bool complete;
    public bool Complete { get { bool c = complete; complete = false; return c; } }

    public virtual void SetComplete ()
    {
        complete = true;
    }

    public virtual void ParseCommandLine(string[] commandLine, PromptScreen screen)
    {
        IEnumerator coroutine = ExecuteCommand(commandLine, screen); 
        StartCoroutine(coroutine);
    }

    public virtual void ParseCommandLine(string[] commandLine, PromptScreen screen, CommandParser.SystemPath targetPath)
    {
        IEnumerator coroutine = ExecuteCommand(commandLine, screen, targetPath);
        StartCoroutine(coroutine);
    }

    public virtual IEnumerator ExecuteCommand(string[] commandLine, PromptScreen screen)
    {
        yield return null;
        complete = true;
    }

    public virtual IEnumerator ExecuteCommand(string[] commandLine, PromptScreen screen, CommandParser.SystemPath targetPath)
    {
        yield return null;
        complete = true;
    }

    public virtual void CompleteJob () { complete = true; }
}
