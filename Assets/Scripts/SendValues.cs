using System;
using UnityEngine;
using UnityEngine.UI;

public class SendValues : MonoBehaviour
{
    public InputField segmentsInput;
    public void SendSegments()
    {
        Static_Data.segments = Convert.ToInt32(segmentsInput.text);
    }
}
