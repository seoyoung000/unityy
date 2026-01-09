using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject optionPanel;

   /* public Slider noteSpeedSlider; 
    public TextMeshProUGUI noteSpeedValueText; */

    void Start()
    {
    }

    public void ShowOptionPanel()
    {
        if (optionPanel != null)
            optionPanel.SetActive(true);
    }

    public void HideOptionPanel()
    {
        if (optionPanel != null)
            optionPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("게임 종료!");
        Application.Quit();
    }

   /* public void OnNoteSpeedSliderChanged(float value)
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.noteSpeed = value;
            UpdateNoteSpeedText(value);
        }
    }

    void UpdateNoteSpeedText(float value)
    {
        if (noteSpeedValueText != null)
            noteSpeedValueText.text = $"{value:0.0}";
    } */

    public void GoToSongSelect()
    {
        Debug.Log("선곡 화면으로 이동!");
        SceneManager.LoadScene("SelectScene");
    }
}
