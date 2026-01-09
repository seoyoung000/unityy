using UnityEngine;
using TMPro;

public class JudgeLine : MonoBehaviour
{
    public KeyCode upKey = KeyCode.UpArrow;
    public KeyCode leftKey = KeyCode.LeftArrow;
    public KeyCode downKey = KeyCode.DownArrow; 
    public KeyCode rightKey = KeyCode.RightArrow; 

    public float perfectRange = 0.15f; 
    public float goodRange = 0.4f; 

    
    public int perfectScore = 100; 
    public int goodScore = 50;

    public TextMeshProUGUI scoreText;

    
    private int currentScore = 0;

    
    void Start()
    {
        UpdateScoreText(); 
    }

    
    void Update()
    {
        if (Input.GetKeyDown(upKey))
        {
            Judge(NoteDirection.Up); 
        }
        if (Input.GetKeyDown(leftKey))
        {
            Judge(NoteDirection.Left);
        }
        if (Input.GetKeyDown(downKey))
        {
            Judge(NoteDirection.Down);
        }
        if (Input.GetKeyDown(rightKey))
        {
            Judge(NoteDirection.Right);
        }
    }



    void Judge(NoteDirection direction)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Note"))
            {
                Note note = hit.GetComponent<Note>();

                    if (note.direction == direction)
                    {
                        float timeDiff = Mathf.Abs(note.expectedTime - GameManager.instance.songTime);

                        int score = 0;

                        if (timeDiff <= perfectRange)
                        {
                            Debug.Log("Perfect!");
                            score = perfectScore;
                        }
                        else if (timeDiff <= goodRange)
                        {
                            Debug.Log("Good!");
                            score = goodScore;
                        }
                        else
                        {
                            Debug.Log("Miss!");
                        }

                        AddScore(score);
                        Destroy(hit.gameObject);
                        return;
                    }
                }
            }


        // 판정 범위 내에 해당 방향의 'Note' 태그를 가진 오브젝트가 없었을 경우 (잘못 누르거나 노트를 놓친 경우)
        Debug.Log("Miss!"); // Miss로 처리하고 콘솔 창에 출력!
        // Miss일 때는 점수를 추가하지 않습니다.
    }

    // 점수를 추가하고 UI를 업데이트하는 함수
    void AddScore(int score)
    {
        currentScore += score; // 현재 점수에 획득 점수 추가!
        UpdateScoreText();     // 점수 UI 업데이트 함수 호출!
    }

    // 점수 UI 텍스트를 업데이트하는 함수
    void UpdateScoreText()
    {
        // scoreText가 제대로 연결되어 있는지 확인 (에러 방지)
        if (scoreText != null)
        {
            // TextMeshProUGUI에 현재 점수를 표시합니다.
            scoreText.text = "Score: " + currentScore;
            // 또는 scoreText.text = $"Score: {currentScore}"; // C# 6.0 이상에서 사용 가능
        }
        else
        {
            Debug.LogWarning("Score Text UI is not assigned in the Inspector!");
        }
    }
}
