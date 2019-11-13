using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HackOSSystem : MonoBehaviour
{
    public CommandParser.StringCommand entryPoint;
    bool complete;
    public bool Complete { get { bool c = complete; complete = false; return c; } }

    public virtual IEnumerator ParseCommandLine(string[] commandLine, PromptScreen screen)
    {
        complete = true;
        yield return null;
    }

    public virtual void CompleteJob () { complete = true; }
}
