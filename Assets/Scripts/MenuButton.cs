using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
