using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetKeys : MonoBehaviour
{
    // Start is called before the first frame update
    public Button SpinPlusBtn;
    public InputField InputTarget;
    bool listen = true;

    private string[] cheatCode;
    private int index;
    void Start()
    {
        cheatCode = new string[] { "s", "g", "f", "e", "n" };
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && listen)
        {
            if (Input.GetKeyDown(cheatCode[index]))
            {
                index++;
            }
            else
            {
                index = 0;
            }
        }
        if (index == cheatCode.Length)
        {
            SpinPlusBtn.gameObject.SetActive(true);
            InputTarget.gameObject.SetActive(true);
            listen = false;
        }
    }
}
