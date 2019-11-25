using System.Collections;
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
}
