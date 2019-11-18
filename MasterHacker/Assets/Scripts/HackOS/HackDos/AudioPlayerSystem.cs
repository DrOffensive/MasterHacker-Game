using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerSystem : HackOSSystem
{
    public AudioSource speakers;
    public SoundLibrary library;
    float volume = 1;
    bool noEcho = false;

    public string noEchoModifier = "-noEcho", volumeModifier = "-vol";

    public void ParseDirectCommand (string command)
    {
        IEnumerator coroutine = ExecuteDirectCommand(command);
        StartCoroutine(coroutine);
    }
        
    IEnumerator ExecuteDirectCommand (string command)
    {
        string[] commandlines = command.Split(' ');
        string failmsg = "";
        AudioClip clip = library.GetClip(commandlines[0]);
        if (clip != null)
        {
            for (int i = 1; i < commandlines.Length; i++)
            {
                if (commandlines[i].ToLower() == noEchoModifier.ToLower())
                    noEcho = true;
                else if (commandlines[i].ToLower() == volumeModifier.ToLower())
                {
                    if (i + 1 < commandlines.Length)
                    {
                        float vol = GenericFunctions.ParseFloat(commandlines[i + 1]);
                        if (vol != float.NaN)
                        {
                            Debug.Log(vol);
                            volume = Mathf.Clamp01(vol);
                            i += 1;
                        }
                        else
                        {
                            failmsg = "Invalid volume";
                            break;
                        }
                    }
                    else
                    {
                        failmsg = "Failed to provide volume"; 
                        break;
                    }
                }
                else
                {
                    failmsg = "Invalid keyword: " + commandlines[i]; 
                    break;
                }
            }

            speakers.Stop();
            speakers.volume = volume;
            speakers.clip = clip;
            speakers.Play();
            SetComplete();
            yield return null;
            volume = 1;
            noEcho = false;
        } else
        {
            SetComplete();
            Debug.Log("Failed to play " + commandlines[0]);
            yield return null;
        }
        if(failmsg !="")
        {
            Debug.Log(failmsg);
        }
    }

    public override IEnumerator ExecuteCommand(string[] commandLine, PromptScreen screen, CommandParser.SystemPath targetPath)
    {
        bool failed = false;
        string failedMessage = "";
        HackOS_file file = screen.parser.OS.Root.GetFile(targetPath);
        if(file is HackOS_audioFile) { 
            for (int i = 1; i < commandLine.Length; i++)
            {
                if (commandLine[i].ToLower() == noEchoModifier.ToLower())
                    noEcho = true;
                else if (commandLine[i].ToLower() == volumeModifier.ToLower())
                {
                    if(i+1 < commandLine.Length)
                    {
                        float vol = GenericFunctions.ParseFloat(commandLine[i + 1]);
                        if(vol != float.NaN)
                        {
                            Debug.Log(vol);
                            volume = Mathf.Clamp01(vol);
                            i += 1;
                        } else
                        {
                            failed = true;
                            failedMessage = commandLine[i + 1] + " is not a valid value";
                            break;
                        }
                    }
                    else
                    {
                        failed = true;
                        failedMessage = "specify a valid value";
                        break;
                    }
                }
                else
                {
                    failed = true;
                    failedMessage = "Unknown keyword: " + commandLine[i];
                    break;
                }
            }
        } else
        {
            failed = true;
            failedMessage = targetPath.file + "." + targetPath.fileExtention + " is not a supported audiofile";
        }
        if (!failed)
        {
            speakers.Stop();
            speakers.volume = volume;
            speakers.clip = library.GetClip(file.content);
            speakers.Play();
            if (!noEcho)
            {
                string message = entryMessage;

                message = message.Replace("<file>", targetPath.file);
                message = message.Replace("<ext>", targetPath.fileExtention);

                screen.InsertLine(message);
                yield return new WaitForSeconds(speakers.clip.length);
                SetComplete();
            }
            else
            {
                SetComplete();
                yield return null;
            }
            volume = 1;
            noEcho = false;
        } else
        {
            screen.InsertLine(failedMessage);
            SetComplete();
            yield return null;
        }
    }
}
