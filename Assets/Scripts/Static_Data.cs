using System.Collections.Generic;
using UnityEngine;

public class Static_Data : MonoBehaviour
{
    public static int segments = 10;
    public static Dictionary<int, string> segmentOptions = null;
    public static GameObject[] letters;
    public static GameObject[] nexts;
    public static Material[] pics;
    public static int selectedSector = -1;

    public static Texture2D customLetter;
    public static Texture2D customDummy;

    public static Color primaryColor = new Color32(87, 76, 90, 255);
    public static Color secondaryColor = new Color32 (73, 58, 77, 255);
    public static Color thirteenColor = new Color32(70, 102, 71, 255);

    public static bool filesLoaded = false;
    public static bool whiteFont = false;

    public static int pickerMode;
}
