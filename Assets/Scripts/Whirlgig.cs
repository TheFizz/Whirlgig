using System;
using UnityEngine;
using UnityEngine.UI;

public class Whirlgig : MonoBehaviour
{
    public GameObject whirlgigArrow;
    public GameObject detector;

    public float impulse = 0;
    public float targetImpulse = 0;

    public Animation pushAnimation;

    bool once = true;
    bool spinStarted = false;
    public bool SecretFeature = false;
    public bool slowable = false;

    int discardedLetters = 0;

    GameObject selectedLetter;

    public Button SpinButton;
    public Button SpinPlusButton;
    public Button CenterButton;
    public InputField targetSectorInput;

    float anglesPerSector;

    int[] pts = new int[4];

    float[,] segmentAngles = new float[Static_Data.segments, 2];

    int targetSector = 4;
    void Update()
    {
        if (impulse >= 399)
        {
            detector.gameObject.SetActive(true);
            slowable = true;
        }
        if (SecretFeature && slowable && impulse < 120 && impulse > 15)
        {
            if (whirlgigArrow.transform.eulerAngles.y >= segmentAngles[pts[3], 0] && whirlgigArrow.transform.eulerAngles.y <= segmentAngles[pts[3], 1])
            {
                impulse = 60;
            }
            if (whirlgigArrow.transform.eulerAngles.y >= segmentAngles[pts[2], 0] && whirlgigArrow.transform.eulerAngles.y <= segmentAngles[pts[2], 1])
            {
                impulse = 50;
            }
            if (whirlgigArrow.transform.eulerAngles.y >= segmentAngles[pts[1], 0] && whirlgigArrow.transform.eulerAngles.y <= segmentAngles[pts[1], 1])
            {
                impulse = 40;
            }
            if (whirlgigArrow.transform.eulerAngles.y >= segmentAngles[pts[0], 0] && whirlgigArrow.transform.eulerAngles.y <= segmentAngles[pts[0], 1])
            {
                System.Random random = new System.Random();
                impulse = random.Next(15, 20);
                SecretFeature = false;
                slowable = false;
            }
        }
        if (targetImpulse != 0 && spinStarted)
        {
            once = true;
            AddImpulse();
        }
        if (impulse > 0 && targetImpulse == 0)
        {
            once = true;
            MakeImpulse();
        }
        else if (once && spinStarted && targetImpulse == 0)
        {
            once = false;
            impulse = 0;
            ProcessSector();
        }

    }

    void MakeImpulse()
    {
        whirlgigArrow.transform.Rotate(Vector3.up, impulse * Time.deltaTime);

        if (impulse < 200)
        {
            impulse -= 0.1f;
            CameraMovement.CameraToCenter();
        }
        if (impulse < 100 && !SecretFeature)
        {
            impulse -= 0.1f;
            CameraMovement.CameraToCenter();
        }
        if (impulse < 50 && !SecretFeature)
        {
            impulse -= 0.1f;
            CameraMovement.CameraToCenter();
        }
        else
        {
            impulse -= 0.2f;
            targetImpulse = 0;
        }

    }
    void AddImpulse()
    {
        whirlgigArrow.transform.Rotate(Vector3.up, impulse * Time.deltaTime);
        if (impulse < targetImpulse)
            impulse += 4f;
        else
            targetImpulse = 0;
    }

    private void ProcessSector()
    {
        RaycastHit objectHit;
        if (Physics.Raycast(detector.transform.position, -detector.transform.up * 10, out objectHit, 50))
        {
            var index = Convert.ToInt32(objectHit.transform.name.Replace("Segment ", ""));

            if (discardedLetters < Static_Data.letters.Length)
                while (Static_Data.letters[index] == null)
                {
                    index++;
                    if (index >= Static_Data.letters.Length)
                        index = 0;
                }
            discardedLetters++;
            spinStarted = false;
            selectedLetter = Static_Data.letters[index];
            Static_Data.selectedSector = index;
            CameraMovement.CameraToLetter(selectedLetter);
        }
    }

    public void StartSpin()
    {
        if(discardedLetters == Static_Data.letters.Length-1)
        {
            for (int i = 0; i < Static_Data.letters.Length; i++)
            {
                if(Static_Data.letters[i]!=null)
                CameraMovement.CameraToLetter(Static_Data.letters[i]);
            }
            return;
        }
        System.Random random = new System.Random();
        targetImpulse = random.Next(400, 600);
        pushAnimation.Play();
        spinStarted = true;

        SpinButton.interactable = false;
        try { SpinPlusButton.interactable = false; }
        catch { }
        CenterButton.interactable = false;

        CameraMovement.CameraSpin();
    }

    public void SpecialSpin()
    {
        if (Convert.ToInt32(targetSectorInput.text) > Static_Data.segments || Convert.ToInt32(targetSectorInput.text) < 1)
        {
            targetSectorInput.text = "";
            return;
        }
        detector.gameObject.SetActive(false);
        SecretFeature = true;
        targetSector = Convert.ToInt32(targetSectorInput.text);
        for (int i = 0; i < 4; i++)
        {
            if (targetSector - i == Static_Data.segments)
            {
                pts[i] = Static_Data.segments - targetSector;
                continue;
            }
            if ((targetSector - i) < 0)
            {
                pts[i] = Static_Data.segments + (targetSector - i);
            }
            else pts[i] = targetSector - i;
        }
        anglesPerSector = 360f / Static_Data.segments;
        for (int i = 0; i < Static_Data.segments; i++)
        {
            float tmpStart = i * anglesPerSector;
            float tmpFin = i * anglesPerSector + anglesPerSector;

            segmentAngles[i, 0] = tmpStart;
            segmentAngles[i, 1] = tmpFin;
        }
        slowable = false;
        StartSpin();
    }
}
