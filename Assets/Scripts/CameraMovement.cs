using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public enum movingState { cameraEnter, cameraStartingOrbit, cameraDefault, cameraSpin, cameraToCenter, cameraToCenterOnDemand, cameraToLetterOnDemand, cameraToLetter, cameraLetterRemove, camHold };
    public static movingState currentState;

    public static Vector3 targetMovePoint;
    public static Vector3 viewAnchor = new Vector3(0, -20, 0);

    public static Quaternion targetLookPoint;

    public static GameObject letterObject;

    Vector3 prevLetter = Vector3.zero;

    public Button StartButton;
    public Button SpinButton;
    public Button SpinPlusButton;
    public Button CenterButton;
    public Button ContinueButton;

    float letterDelay;
    float centerDelay;
    float angles = 0;

    bool letterContinue = false;

    void Start()
    {
        currentState = movingState.camHold;

        Camera.main.transform.position = Vector3.MoveTowards(Static_Data.letters[0].transform.position, Vector3.zero, -0.1f);
        Camera.main.transform.position = Camera.main.transform.position + new Vector3(0, 300, 0);
        Camera.main.transform.LookAt(Vector3.zero);

        targetMovePoint = (Static_Data.letters[0].transform.position + Vector3.zero) * 1.1f;
        targetMovePoint.y += 7;

        Camera.main.transform.LookAt(Static_Data.letters[0].transform.position);
        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, targetMovePoint, 0.2f);

    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == movingState.cameraEnter)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, targetMovePoint, 0.2f);

            if (Camera.main.transform.position == targetMovePoint)
            {
                CameraStartingOrbit();
            }
        }

        if (currentState == movingState.cameraStartingOrbit)
        {
            if (angles > 360f)
            {
                CameraDefault(Static_Data.letters[0].transform.position);
            }
            Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, 0.2f);
            angles += 0.2f;
        }

        if (currentState == movingState.cameraDefault)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, targetMovePoint, 0.3f);

            targetLookPoint = Quaternion.LookRotation(viewAnchor - Camera.main.transform.position);
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, targetLookPoint, 20 * Time.deltaTime);

            if (Camera.main.transform.position == targetMovePoint)
            {
                Camera.main.transform.LookAt(viewAnchor);
                SpinButton.interactable = true;
                try { SpinPlusButton.interactable = true; }
                catch { }
                CenterButton.interactable = true;
            }
        }

        if (currentState == movingState.cameraSpin)
        {
            Camera.main.transform.RotateAround(viewAnchor, Vector3.down, 0.025f);
        }

        if (currentState == movingState.cameraLetterRemove)
        {
            letterDelay += Time.deltaTime;
            letterObject.transform.position = Vector3.MoveTowards(letterObject.transform.position, Vector3.zero, -0.5f);
            if (letterDelay > 1f)
            {
                GameObject.Destroy(letterObject);
                letterDelay = 0;
                Static_Data.nexts[Static_Data.selectedSector].SetActive(true);
                CameraDefault(prevLetter);
            }
        }


        if (currentState == movingState.cameraToCenter)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, targetMovePoint, 0.5f);
            Camera.main.transform.LookAt(viewAnchor);
        }

        if (currentState == movingState.cameraToCenterOnDemand)
        {
            centerDelay += Time.deltaTime;
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, targetMovePoint, 0.5f);
            Camera.main.transform.LookAt(viewAnchor);

            if (centerDelay > 3f)
            {
                centerDelay = 0;
                if (prevLetter != Vector3.zero)
                    CameraDefault(prevLetter);
                else
                    CameraDefault(Static_Data.letters[0].transform.position);
            }
        }

        if (currentState == movingState.cameraToLetter)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, targetMovePoint, 0.5f);
            if (Camera.main.transform.position != targetMovePoint)
            {
                targetLookPoint = Quaternion.LookRotation(viewAnchor - Camera.main.transform.position);
                Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, targetLookPoint, 10 * Time.deltaTime);
            }
            else
            {
                targetLookPoint = Quaternion.LookRotation(letterObject.transform.position - Camera.main.transform.position);
                Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, targetLookPoint, 10 * Time.deltaTime);
                ContinueButton.interactable = true;
                if (letterContinue)
                {
                    letterContinue = false;
                    ContinueButton.interactable = false;
                    //
                    CenterButton.interactable = false;
                    //
                    prevLetter = letterObject.transform.position;
                    currentState = movingState.cameraLetterRemove;
                }
            }
        }

    }
    public void CameraEnter()
    {
        StartButton.interactable = false;
        Camera.main.transform.LookAt(Static_Data.letters[0].transform.position);
        currentState = movingState.cameraEnter;
    }
    public static void CameraStartingOrbit()
    {
        currentState = movingState.cameraStartingOrbit;
    }
    public void CameraDefault(Vector3 anchor)
    {
        targetMovePoint = (anchor + Vector3.zero) * 2.5f;
        targetMovePoint.y += 25;
        currentState = movingState.cameraDefault;
    }
    public static void CameraSpin()
    {
        currentState = movingState.cameraSpin;
    }
    public static void CameraToCenter()
    {
        if (currentState != movingState.cameraToCenter)
        {
            targetMovePoint = Vector3.MoveTowards(viewAnchor, Camera.main.transform.position, 0.5f);
            targetMovePoint.y = 50;
            currentState = movingState.cameraToCenter;
        }
    }
    public static void CameraToLetter(GameObject letter)
    {
        if (currentState != movingState.cameraToLetter)
        {
            targetMovePoint = (letter.transform.position + Vector3.zero) * 1.2f;
            targetMovePoint.y = 10;
            letterObject = letter;
            currentState = movingState.cameraToLetter;
        }
    }
    public void CameraToCenterOnDemand()
    {
        if (currentState != movingState.cameraToCenterOnDemand)
        {
            CenterButton.interactable = false;
            SpinButton.interactable = false;
            try { SpinPlusButton.interactable = false; }
            catch { }

            targetMovePoint = Vector3.MoveTowards(viewAnchor, Camera.main.transform.position, 0.5f);
            targetMovePoint.y = 50;
            currentState = movingState.cameraToCenterOnDemand;
        }
    }
    public void Continue()
    {
        letterContinue = true;
    }
}

