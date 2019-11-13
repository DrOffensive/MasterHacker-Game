using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandParser : MonoBehaviour
{
    public HackOS_Base OS;
    public string listDirContent, mountDir, oneDirUp;

    Queue<Command> waitingCommands;

    Coroutine currentCommand;

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
        bool refocusOnComplete;

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
        
        SystemPath targetPath = new SystemPath(command.Screen.MountedDir);

        if(words[0].Contains("/"))
        {
            string[] path = words[0].Split('/');

            int o = 0;
            while (path[o] == "")
                o++;
            if (o < path.Length)
            {
                Debug.Log(path[o]);
                if (path[o] == command.Screen.MountedDir[0])
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
                    for(int i = 0; i < path.Length - o; i++)
                    {
                        p[i] = path[o + i];
                    }
                    Debug.Log(p);
                    targetPath = GetSystemPath(command.Screen.MountedDir, p);
                }
                string[] w = new string[words.Length - 1];
                for (int i = 1; i < words.Length; i++)
                {
                    w[i - 1] = words[i];
                }
                words = w;
            }
        } else
        {
            targetPath = new SystemPath(command.Screen.MountedDir);
        }

        if(OS.Root.CheckDirectory(targetPath.directories.ToArray()) == null) {
            string path = "";
            foreach(string dir in targetPath.directories)
            {
                path += dir + "/";
            }
            targetPath.failed = "Unknown directory " + path;
        }

        if(targetPath.failed != "")
        {
            command.Screen.InsertLine(targetPath.failed);
        }else
        {
            if (words.Length > 0) {
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
                        }
                    }else if (words[0].ToLower() == mountDir.ToLower())
                    {
                        if(words.Length == 2)
                        {
                            if(words[1].ToLower() == oneDirUp.ToLower())
                            {
                                List<string> dirs = command.Screen.MountedDir;
                                if (dirs.Count > 1)
                                {
                                    dirs.RemoveAt(dirs.Count - 1);
                                    command.Screen.SetMountedDir(dirs);
                                } else
                                {
                                    command.Screen.InsertLine("Can't unmount root file, try switching root");
                                }
                            } else
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

                                if(sys.failed == "" && sys.file == "" && sys.fileExtention == "")
                                {
                                    command.Screen.SetMountedDir(sys.directories);
                                } else
                                {
                                    if(sys.failed != "")
                                    {
                                        command.Screen.InsertLine(sys.failed);
                                    } else
                                    {
                                        command.Screen.InsertLine("Invalid Key Word " + sys.file + sys.fileExtention);
                                    }
                                }
                            }
                        } else
                        {
                            if(words.Length < 2)
                                command.Screen.InsertLine("Unknown Keyword");
                            else
                                command.Screen.InsertLine("Unknown Keyword " + words[2]);
                        }
                    } else
                    {
                        command.Screen.InsertLine("Unknown Keyword " + words[0]);
                    }
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

        if(command.RefocusOnComplete)
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

    void MountDir (PromptScreen screen, List<string> dir)
    {
        if (OS.Root.CheckDirectory(dir.ToArray()) != null) {
            screen.SetMountedDir(dir);
        }
    }

    public StringCommand GetCommand (string command)
    {

        return null;
    }
}
