using Crosstales.FB;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class LoadFromFile : MonoBehaviour
{
    public void Load()
    {
        SegmentsEditor editor = GameObject.FindObjectOfType<SegmentsEditor>();
        string path = FileBrowser.OpenSingleFolder();
        int i = 0;
        var info = new DirectoryInfo(path);
        var fileInfo = info.GetFiles();
        Material[] pics = new Material[Static_Data.segments];
        foreach (FileInfo file in fileInfo)
        {
            Material mat = new Material(Shader.Find("Unlit/Texture"));
            mat.name = i.ToString();
            if (file.Extension == ".jpg")
            {
                int picNum=0;
                try { picNum = Convert.ToInt32(file.Name.Replace(file.Extension, "")); }
                catch { continue; }
                byte[] bytes = File.ReadAllBytes(file.FullName);
                Texture2D tex = new Texture2D(100, 100);
                tex.LoadImage(bytes);
                mat.mainTexture = tex;
                if (picNum < Static_Data.segments - 1)
                    pics[picNum] = mat;
                i++;
            }
            if (file.Extension == ".txt")
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
        }
        Static_Data.pics = pics;
        Static_Data.filesLoaded = true;
    }
    public void LoadCustomLetter()
    {
        string path = FileBrowser.OpenSingleFile();
        byte[] bytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(800, 450);
        tex.LoadImage(bytes);
        Static_Data.customLetter = tex;
    }
    public void LoadCustomDummy()
    {
        string path = FileBrowser.OpenSingleFile();
        byte[] bytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(100, 100);
        tex.LoadImage(bytes);
        Static_Data.customDummy = tex;
    }
}
