using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageViewerScreen : HackDosScreenBase
{
    HackOS_file rawImage;
    public ColorMode defaultColorMode = ColorMode.GrayScale;
    public ColorMode imageColorMode;
    public RangeConverter imageHeightToPixelStartScale, imageHeightToChunkJump;

    public bool allowBW = true, allowGreyScale, allowFullColor;
    public RectTransform imagePanel;

    public Image pixelPrefab;
    Image[,] pixels;
    HackOSImage image;

    bool close = false;
    public bool Close { get { bool c = close; close = false; return c; } }
    float build = 0f;
    public float Build { get { float b = 0;if (build == 1f) { build = 0f; } return b; } }


    public struct HackOSImage
    {
        int width;
        int height;
        ColorMode colorMode;
        Color[] convertedColors;

        public HackOSImage(int width, int height, ColorMode colorMode, Color[] convertedColors)
        {
            this.width = width;
            this.height = height;
            this.colorMode = colorMode;
            this.convertedColors = convertedColors;
        }

        public int Width { get => width; }
        public int Height { get => height;  }
        public ColorMode ColorMode { get => colorMode; }
        public Color[] ConvertedColors { get => convertedColors; }
    }

    void DestroyImage ( )
    {
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                Destroy(pixels[x, y]);
            }
        }
    }

    

    IEnumerator BuildImage (HackOSImage image)
    {
        if (pixels != null && pixels[0, 0] != null)
            DestroyImage();

        Vector2 pixelSize = new Vector2(pixelPrefab.GetComponent<RectTransform>().sizeDelta.x, pixelPrefab.GetComponent<RectTransform>().sizeDelta.y);

        Vector2 offset = (new Vector2(pixelSize.x * image.Width, pixelSize.y * image.Height) / 2) * -1;

        imagePanel.localScale = new Vector3(imageHeightToPixelStartScale.Evaluate(image.Height), imageHeightToPixelStartScale.Evaluate(image.Height), 1f);
        int imageChunk = (int)imageHeightToChunkJump.Evaluate(image.Height);
        pixels = new Image[image.Width, image.Height];
        for (int y = image.Height-1; y >= 0; y--)
        {
            for (int x = 0; x < image.Width; x++)
            {
                int i = (y * image.Width) + x;
                Color color = image.ConvertedColors[i];
                Image pixel = Instantiate(pixelPrefab, Vector3.zero, Quaternion.identity);
                pixel.color = color;
                pixel.transform.parent = imagePanel;
                pixel.transform.localPosition = new Vector3(offset.x + (x * pixelSize.x), offset.y + (y * pixelSize.y), 0);
                pixel.transform.localRotation = Quaternion.Euler(Vector3.zero);
                pixel.transform.localScale = Vector3.one;
                pixels[x, y] = pixel;
            }

            build = Mathf.Clamp01((1f / pixelSize.y-1) * y);
            if(y%imageChunk==0)
                yield return null;
        }
    }

    public void LoadImage (HackOS_file raw)
    {
        rawImage = raw;
    }

    public void LoadImage(HackOS_file raw, ColorMode mode)
    {
        bool failed = false;
        if (mode == ColorMode.BlackWhite && !allowBW)
            failed = true;
        if (mode == ColorMode.GrayScale && !allowGreyScale)
            failed = true;
        if (mode == ColorMode.FullColor && !allowFullColor)
            failed = true;

        if (!failed)
            imageColorMode = mode;
        else
            imageColorMode = defaultColorMode;

        rawImage = raw;
    }

    public enum ColorMode
    {
        BlackWhite, GrayScale, LowRes, midRes, FullColor
    }

    public override void NextSelect()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnAnyKey()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnClose()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnEnter()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnOpen()
    {
        image = ConvertFileToImage(imageColorMode);
        IEnumerator buildFunc = BuildImage(image);
        StartCoroutine(buildFunc);
    }

    HackOSImage ConvertFileToImage (ColorMode mode)
    {
        string[] elemtents = rawImage.content.Split(';');
        if(elemtents[0].StartsWith("[")) {
            string[] sizeData = GenericFunctions.RemoveChars(new List<char> { '[', ']' }, elemtents[0]).Split('x');
            int width = int.Parse(sizeData[0]);
            int height = int.Parse(sizeData[1]);

            Color[] colors = new Color[width * height];
            for(int i = 0; i < colors.Length; i++)
            {
                colors[i] = ConvertColor(GenericFunctions.HexToRGBA(elemtents[i + 1]), imageColorMode);
            }
            return new HackOSImage(width, height, imageColorMode, colors);
        } else
        {
            return new HackOSImage(0,0, imageColorMode, new Color[0]);
        }
    }

    Color ConvertColor (Color color, ColorMode mode)
    {
        switch (mode)
        {
            case ColorMode.BlackWhite:
                float avg = (color.r + color.b + color.g) / 3;
                bool on = avg >= .5f;
                return on ? Color.white : Color.black;


            case ColorMode.GrayScale:
                float aveg = (color.r + color.b + color.g) / 3;
                return new Color(aveg, aveg, aveg, 1);

            case ColorMode.LowRes:
                return new Color((color.r * 8) / 1, (color.g * 8) / 1, (color.b * 4) / 1);

            case ColorMode.midRes:
                return new Color((color.r * 4) / 1, (color.g * 4) / 1, (color.b * 2) / 1);

            default: return color;
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
        CheckSpecialKeys();
    }

    public override void OnEscape()
    {
        DestroyImage();
        close = true;
    }
}
