using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SegmentsEditor : MonoBehaviour
{
    // Start is called before the first frame update
    public Dropdown dropdown;
    public InputField sectorNameInput;
    public Dictionary<int, string> segmentOptions = new Dictionary<int, string>();

    void Start()
    {
        for (int i = 0; i < Static_Data.segments; i++)
        {
            segmentOptions.Add(i, (i + 1).ToString());
            dropdown.AddOptions(new List<string>() { (i + 1).ToString() });
        }
    }

    public void UpdateSegment()
    {
        int segmentIndex = dropdown.value;

        segmentOptions.Remove(segmentIndex);
        segmentOptions.Add(segmentIndex, sectorNameInput.text);

        dropdown.options[segmentIndex].text = (segmentIndex + 1).ToString() + " (" + sectorNameInput.text + ")";
        dropdown.RefreshShownValue();

        sectorNameInput.text = "";
    }
    public void UpdateSegment(string segName, int segmentIndex)
    {
        segmentOptions.Remove(segmentIndex);
        segmentOptions.Add(segmentIndex, segName);

        dropdown.options[segmentIndex].text = (segmentIndex + 1).ToString() + " (" + segName + ")";
        dropdown.RefreshShownValue();

        sectorNameInput.text = "";
    }

    public void SendSegmentsAndStart()
    {
        Static_Data.segmentOptions = segmentOptions;
    }
}
