using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Static_Data : MonoBehaviour
{
    public static int segments = 10;
    public static Dictionary<int, string> segmentOptions = null;
    public static GameObject[] letters;
    public static GameObject[] nexts;
    public static Material[] pics = null;
    public static int selectedSector = -1;
    public static List<int> scriptedSectors = new List<int>();

    public bool colorsLoaded = false;
    public static Color primaryColor = new Color32(124, 104, 130, 1);
    public static Color secondaryColor = new Color32(95, 82, 99, 1);
    public static Color thirteenColor = new Color32(6, 84, 9, 1);
    public static Color bordersColor = new Color32(78, 84, 52, 1);
    public static Color arrowColor = new Color32(255, 0, 0, 1);
    public static Color spinColor = new Color32(255, 33, 0, 1);
    public static Color domeColor = new Color32(255, 255, 255, 1);

    public static Texture2D customLetter = null;
    public static Texture2D customDummy = null;

    public static bool whiteFont = false;
}
