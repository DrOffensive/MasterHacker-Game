using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandParser : MonoBehaviour
{
    public HackOS_Base OS;
    public string listDirContent, mountDir, oneDirUp;
    Queue<Command> waitingCommands;
    Coroutine currentCommand;
    public float systemVolume = 1;
    public List<HackOSSystem> installedSystems;
    public List<WordReplace> wordReplaces;

    public TextAsset SystemSounds;

    string errorAudioKey = "", welcomeJingle = "";

    public string WelcomeJingle { get => welcomeJingle; }

    [System.Serializable]
    public class WordReplace
    {
        public string word, replaceWith;
    }

    [System.Serializable]
    public class StringCommand
    {
        public string name, command;
        public bool confirm, password;
    }

    public struct SystemPath
    {
        public List<string> directories;
        public string file;
        public string fileExtention;
        public string failed;

        public SystemPath(List<string> directories, string file, string fileExtention, string failed)
        {
            this.directories = directories;
            this.file = file;
            this.fileExtention = fileExtention;
            this.failed = failed;
        }

        public SystemPath (List<string> directories)
        {
            this.directories = directories;
            this.file = "";
            this.fileExtention = "";
            this.failed = "";
        }
    }

    public struct Command
    {
        string command;
        PromptScreen screen;
        public bool refocusOnComplete;

        public Command(string command, PromptScreen screen, bool refocusOnComplete)
        {
            this.command = command;
            this.screen = screen;
            this.refocusOnComplete = refocusOnComplete;
        }

        public string CommandString { get => command; }
        public PromptScreen Screen { get => screen; }
        public bool RefocusOnComplete { get => refocusOnComplete; }
    }

    public void QueueCommand (string command, PromptScreen screen, bool refocusOnComplete)
    {
        if (waitingCommands == null)
            waitingCommands = new Queue<Command>();
            waitingCommands.Enqueue(new Command(command, screen, refocusOnComplete));
    }

    private void Start()
    {
        if (SystemSounds != null) {
            string rawText = SystemSounds.text;
            string[] sounds = rawText.Split('|');
            foreach(string s in sounds)
            {
                string[] key = s.Split(':');
                if (key[0] == "ErrorSound")
                {
                    errorAudioKey = key[1];
                } if(key[0]=="WelcomeJingle")
                {
                    welcomeJingle = key[1];
                }
            }
        }
    }

    private void Update()
    {
        if(waitingCommands != null && waitingCommands.Count > 0)
        {
            if (currentCommand == null)
            {
                Command c = waitingCommands.Dequeue();
                IEnumerator func = ParseCommand(c);
                currentCommand = StartCoroutine(func);
            }
        }
    }

    public IEnumerator ParseCommand (Command command)
    {
        string[] words = command.CommandString.ToLower().Split(' ');

        List<string> cPath = command.Screen.MountedDir;
        List<string> sPath = new List<string>();
        foreach(string dir in cPath)
        {
            sPath.Add(dir);
        }

        SystemPath targetPath = new SystemPath(sPath);

        if (words[0].Contains("/"))
        {
            string[] path = words[0].Split('/');

            int o = 0;
            while (path[o] == "")
                o++;
            if (o < path.Length)
            {
                if (path[o] == sPath[0])
                {
                    targetPath = GetSystemPath(new List<string>(), path);
                    string p = "";
                    foreach (string s in targetPath.directories)
                        p += s + "/";
                    Debug.Log(p);
                }
                else
                {
                    string[] p = new string[path.Length - o];
                    for (int i = 0; i < path.Length - o; i++)
                    {
                        p[i] = path[o + i];
                    }
                    targetPath = GetSystemPath(sPath, p);
                }
                string[] w = new string[words.Length - 1];
                for (int i = 1; i < words.Length; i++)
                {
                    w[i - 1] = words[i];
                }
                words = w;
            }
            if (OS.Root.CheckDirectory(targetPath.directories.ToArray()) == null)
            {
                string printPath = "";
                foreach (string dir in targetPath.directories)
                {
                    printPath += dir + "/";
                }
                targetPath.failed = "Unknown directory " + printPath;
                PlaySystemSound(errorAudioKey);
            }

        }
        else if (words[0].Contains("."))
        {
            string[] fileName = words[0].Split('.');
            SystemPath path = new SystemPath(sPath, fileName[0], fileName[1], "No such file " + words[0]);
            HackOS_file file = command.Screen.parser.OS.Root.GetFile(path);
            targetPath = path;
            if (file!=null)
            {
                targetPath.failed = "";
                string[] w = new string[words.Length - 1];
                for (int i = 1; i < words.Length; i++)
                {
                    w[i - 1] = words[i];
                }
                words = w;
            }
        } else
        {
            targetPath = new SystemPath(sPath);
        }
        bool failed = false;
        bool systemUsed = false;
        if (targetPath.failed != "")
        {
            command.Screen.InsertLine(targetPath.failed);
        }else
        {
            if (words.Length > 0)
            {
                foreach (HackOSSystem system in installedSystems)
                {
                    if (words[0].ToLower().Equals(system.entryPoint.command.ToLower()))
                    {
                        /*string message = system.entryMessage;
                        message = message.Replace("<file>", targetPath.file);
                        message = message.Replace("<ext>", targetPath.fileExtention);

                        command.Screen.InsertLine(message);*/
                        systemUsed = true;
                        yield return null;

                        system.ParseCommandLine(words, command.Screen, targetPath);
                        while (!system.Complete)
                        {
                            yield return null;
                        }
                        break;
                    }
                }
            }
            if (words.Length > 0 && !systemUsed) {
                if (targetPath.file == "" && targetPath.fileExtention == "")
                {
                    if (words[0].ToLower() == listDirContent.ToLower())
                    {
                        if (words.Length == 1)
                        {
                            HackOS_directory dir = OS.Root.CheckDirectory(targetPath.directories.ToArray());
                            string[] list = dir.ListContent();
                            foreach (string l in list)
                            {
                                command.Screen.InsertLine(l);
                            }
                        } else
                        {
                            command.Screen.InsertLine("Unknown Keyword " + words[1]);
                            PlaySystemSound(errorAudioKey);
                        }
                    }else if (words[0].ToLower() == mountDir.ToLower())
                    {
                        if(words.Length == 2)
                        {
                            if (words[1].ToLower() == oneDirUp.ToLower())
                            {
                                List<string> dirs = command.Screen.MountedDir;
                                if (dirs.Count > 1)
                                {
                                    dirs.RemoveAt(dirs.Count - 1);
                                    command.Screen.SetMountedDir(dirs);
                                }
                                else
                                {
                                    command.Screen.InsertLine("Can't unmount root file, try switching root");
                                    PlaySystemSound(errorAudioKey);
                                }
                            }
                            else
                            {
                                string[] path = words[1].Split('/');
                                SystemPath sys;
                                if (path[0] == command.Screen.MountedDir[0])
                                {
                                    sys = GetSystemPath(new List<string>(), path);
                                }
                                else
                                {
                                    sys = GetSystemPath(command.Screen.MountedDir, path);
                                }
                                if (sys.failed == "" && sys.file == "" && sys.fileExtention == "")
                                {
                                    if (OS.Root.CheckDirectory(sys.directories.ToArray()) != null)
                                    {
                                        command.Screen.SetMountedDir(sys.directories);
                                    } else
                                    {
                                        command.Screen.InsertLine("Unknown directory: " + words[1]);
                                        targetPath.failed = "failed";
                                        PlaySystemSound(errorAudioKey);
                                        for (int i = 0; i < path.Length; i++)
                                        {
                                            command.Screen.MountedDir.RemoveAt(command.Screen.MountedDir.Count - 1);
                                        }
                                    }
                                }
                                else
                                {
                                    if (sys.failed != "")
                                    {
                                        command.Screen.InsertLine(sys.failed);
                                        PlaySystemSound(errorAudioKey);
                                    }
                                    else
                                    {
                                        command.Screen.InsertLine("Invalid Key Word " + sys.file + sys.fileExtention);
                                        PlaySystemSound(errorAudioKey);
                                    }
                                }
                            }
                        } else
                        {
                            if(words.Length < 2)
                                command.Screen.InsertLine("Unknown Keyword");
                            else
                                command.Screen.InsertLine("Unknown Keyword " + words[2]);
                            PlaySystemSound(errorAudioKey);
                        }
                    } else
                    {
                        command.Screen.InsertLine("Unknown Keyword " + words[0]);
                        PlaySystemSound(errorAudioKey);
                    }
                } else
                {
                    command.Screen.InsertLine("Unknown Keyword " + words[0]);
                    PlaySystemSound(errorAudioKey);
                }
            } else
            {
                if(targetPath.file == "" && targetPath.fileExtention == "")
                {
                    string path = "~/";
                    foreach (string dir in targetPath.directories)
                        path += dir + "/";
                    command.Screen.InsertLine(path); 
                }
            }
        }

        /*else
        {
            command.Screen.InsertLine("Unknown keyword " + words[0]);
        }*/

        if(command.RefocusOnComplete || systemUsed)
            command.Screen.Refocus();
        yield return null;
        currentCommand = null;
    }

    SystemPath GetSystemPath (List<string> append, string[] path)
    {
        string file = "", extention = "", failed = "";
        List<string> dirs = append;
        for (int i = 0; i < path.Length; i++)
        {
            if (i != path.Length - 1)
            {
                if (path[i].Contains(" ") || path[i] == "")
                {
                    failed = "Unknown Key Word" + path[i];
                    break;
                }
                dirs.Add(path[i].ToLower());
            }
            else
            {
                if (path[i].Contains("."))
                {
                    string[] f = path[i].Split('.');
                    if (f.Length == 2)
                    {
                        file = f[0];
                        extention = f[1];
                    }
                    else
                    {
                        failed = "Unknown File " + path[i];
                        break;
                    }

                } else if (path[i].Contains(" ") || path[i] == "")
                {
                    failed = "Unknown Key Word " + path[i];
                    break;
                }
                else
                {
                    dirs.Add(path[i].ToLower());
                }
            }
        }
        return new SystemPath(dirs, file, extention, failed);
    }

    AudioPlayerSystem AudioSystem
    {
        get
        {
            foreach(HackOSSystem system in installedSystems)
            {
                if (system is AudioPlayerSystem)
                    return (AudioPlayerSystem)system;
            }

            return null;
        }
    }

    void PlaySystemSound (string key)
    {
        if(AudioSystem !=null)
        {
            AudioSystem.ParseDirectCommand(key + " -vol " + systemVolume);
        }
    }
}
