using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackOSSystem_imgViewer : HackOSSystem
{
    public ImageViewerScreen imageViewerScreen;
    public string bwModifier, greyModifier, colorModifier;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator ExecuteCommand(string[] commandLine, PromptScreen screen, CommandParser.SystemPath targetPath)
    {
        bool failed = false;
        HackOS_file file = screen.parser.OS.Root.GetFile(targetPath);
        ImageViewerScreen.ColorMode mode = imageViewerScreen.defaultColorMode;
        if(file!=null && commandLine.Length < 3)
        {
            screen.InsertLine("Opening " + file.name + "." + file.extension + " with ImageViewer...");
            yield return null;
            if (commandLine.Length > 1)
            {
                if (commandLine.Length > 2)
                {
                    failed = true;
                }
                else
                {
                    if (commandLine[1].Equals(bwModifier))
                        mode = ImageViewerScreen.ColorMode.BlackWhite;
                    else if (commandLine[1].Equals(greyModifier))
                        mode = ImageViewerScreen.ColorMode.GrayScale;
                    else if (commandLine[1].Equals(colorModifier))
                        mode = ImageViewerScreen.ColorMode.FullColor;
                    else
                        failed = true;
                }
            }

            if (!failed)
            {
                imageViewerScreen.gameObject.SetActive(true);
                Debug.Log("viewer: " + file.name);
                screen.keyListen = false;
                imageViewerScreen.SetKeyCheck = FindObjectOfType<KeyCheck>();
                imageViewerScreen.LoadImage(file, mode);
                imageViewerScreen.OnOpen();

                while (!imageViewerScreen.Close)
                {
                    yield return null;
                }
                imageViewerScreen.gameObject.SetActive(false);
                screen.InsertLine("Exiting ImageView");
                screen.keyListen = true;
            } else
            {
                screen.InsertLine("Unknown keyword " + commandLine[commandLine.Length-1]);

            }
        }
        else
        {
            if(commandLine.Length > 3)
            screen.InsertLine("Unknown file " + targetPath.file);
        }
        SetComplete();
    }
}
