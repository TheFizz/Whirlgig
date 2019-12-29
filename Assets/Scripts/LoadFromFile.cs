using Crosstales.FB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LoadFromFile : MonoBehaviour
{
    public void Load()
    {
        SegmentsEditor editor = GameObject.FindObjectOfType<SegmentsEditor>();
        string path = FileBrowser.OpenSingleFolder();
        int i = 0;
        FileInfo[] fileInfo = null;
        try
        {
            var info = new DirectoryInfo(path);
            var fi = info.GetFiles();
            fileInfo = fi;
        }
        catch
        {
            return;
        }
        Material[] pics = new Material[Static_Data.segments];
        foreach (FileInfo file in fileInfo)
        {

            Material mat = new Material(Shader.Find("Unlit/Texture"));
            mat.name = i.ToString();
            if (file.Extension == ".jpg")
            {
                byte[] bytes = File.ReadAllBytes(file.FullName);
                Texture2D tex = new Texture2D(100, 100);
                tex.LoadImage(bytes);
                mat.mainTexture = tex;
                int picNum = Convert.ToInt32(file.Name.Replace(file.Extension, ""));
                if (picNum < Static_Data.segments - 1)
                    pics[picNum] = mat;
                i++;
            }
            if (file.Extension == ".txt")
            {
                if (file.Name.ToLower().Contains("names"))
                {
                    StreamReader reader = new StreamReader(file.FullName);
                    for (int j = 0; j < Static_Data.segments; j++)
                    {
                        if (j == 12)
                            continue;
                        string nameLine = reader.ReadLine();
                        if (nameLine != null)
                        {
                            editor.UpdateSegment(nameLine, j);
                        }
                        else break;
                    }
                    reader.Close();
                }
                else if (file.Name.ToLower().Contains("script"))
                {
                    StreamReader reader = new StreamReader(file.FullName);
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        int tmp;

                        try { tmp = Convert.ToInt32(line); }
                        catch { continue; }
                        if (tmp > 0 && tmp <= Static_Data.segments)
                        {
                            Static_Data.scriptedSectors.Add(tmp);
                        }
                    }
                    reader.Close();
                }
            }
        }
        Static_Data.pics = pics;
    }
    public void LoadColors()
    {
        string path = FileBrowser.OpenSingleFile();
        IEnumerable<string> lines;
        try
        {
            lines = File.ReadLines(path);
        }
        catch { return; }
        int linecount = 0;
        foreach (var line in lines)
        {
            if (line[0] == '#')
                continue;

            string[] rgbtext = line.Split(',');
            if (rgbtext.Length != 3)
                continue;

            byte[] rgb = new byte[3];
            for (int i = 0; i < rgbtext.Length; i++)
            {
                rgb[i] = Convert.ToByte(rgbtext[i]);
            }

            Color tmp = new Color32(rgb[0], rgb[1], rgb[2], 255);
            switch (linecount)
            {
                case 0:
                    Static_Data.primaryColor = tmp;
                    break;
                case 1:
                    Static_Data.secondaryColor = tmp;
                    break;
                case 2:
                    Static_Data.thirteenColor = tmp;
                    break;
                case 3:
                    Static_Data.bordersColor = tmp;
                    break;
                case 4:
                    Static_Data.arrowColor = tmp;
                    break;
                case 5:
                    Static_Data.spinColor = tmp;
                    break;
                case 6:
                    Static_Data.domeColor = tmp;
                    break;
            }
            linecount++;
        }
    }

    public void LoadCustomLetter()
    {
        string path = FileBrowser.OpenSingleFile();
        byte[] bytes = null;
        try
        {
            bytes = File.ReadAllBytes(path);
        }
        catch { return; }
        Texture2D tex = new Texture2D(800, 450);
        tex.LoadImage(bytes);
        Static_Data.customLetter = tex;
    }

    public void LoadCustomDummy()
    {
        string path = FileBrowser.OpenSingleFile();
        byte[] bytes = null;
        try
        {
            bytes = File.ReadAllBytes(path);
        }
        catch { return; }
        Texture2D tex = new Texture2D(100, 100);
        tex.LoadImage(bytes);
        Static_Data.customDummy = tex;
    }
}
