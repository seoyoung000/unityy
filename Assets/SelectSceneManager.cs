using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectScreenManager : MonoBehaviour
{
    public void GoToSongTest()
    {
        Debug.Log("Test 화면으로 이동!");
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitToSelectScene()
    {
        Debug.Log("Title 화면으로 이동!");
        SceneManager.LoadScene("TitleScene");
    }
    public void GoToSongSinchan()
    {
        Debug.Log("ShinchanBGM 화면으로 이동!");
        SceneManager.LoadScene("SampleScene2");
    }


}
