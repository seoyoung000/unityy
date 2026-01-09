// NoteSpawner.cs
using UnityEngine;
using System.Collections.Generic; // List를 사용하기 위해 필요!
using UnityEngine.SceneManagement;

public class NoteSpawner2 : MonoBehaviour
{

    public GameObject upNotePrefab;
    public GameObject leftNotePrefab;
    public GameObject downNotePrefab;
    public GameObject rightNotePrefab;


    public Transform upSpawnPoint;
    public Transform leftSpawnPoint;
    public Transform downSpawnPoint;
    public Transform rightSpawnPoint;

    public Transform judgeLineTransform;

    public List<NoteData> noteDataList = new List<NoteData>(); // <-- NoteData 리스트 사용!

    private int noteIndex = 0;

    public float noteSpeed = 5f;

    public float testDuration = 30f; // 예: 30초 동안 노트 나오게

    void Start()
    {
        noteDataList.Clear(); // 혹시 이전에 데이터가 남아있을까봐 비워줍니다.

        // GameManager 인스턴스가 준비되었는지 확인합니다.
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager 인스턴스를 찾을 수 없습니다. GameManager가 씬에 있는지, 스크립트 실행 순서가 올바른지 확인하세요.");
            return; // GameManager 없으면 함수 종료
        }

        noteDataList.Add(new NoteData { time = 0.0f, direction = NoteDirection.Up }); 
        noteDataList.Add(new NoteData { time = 0.2f, direction = NoteDirection.Down });  
        noteDataList.Add(new NoteData { time = 0.3f, direction = NoteDirection.Left }); 
        noteDataList.Add(new NoteData { time = 0.4f, direction = NoteDirection.Down });
        noteDataList.Add(new NoteData { time = 1.3f, direction = NoteDirection.Up });
        noteDataList.Add(new NoteData { time = 1.8f, direction = NoteDirection.Up });    
        noteDataList.Add(new NoteData { time = 2.0f, direction = NoteDirection.Left });
        noteDataList.Add(new NoteData { time = 2.3f, direction = NoteDirection.Right });
        noteDataList.Add(new NoteData { time = 2.6f, direction = NoteDirection.Down });
        noteDataList.Add(new NoteData { time = 3.8f, direction = NoteDirection.Up });
        noteDataList.Add(new NoteData { time = 3.0f, direction = NoteDirection.Up });
        noteDataList.Add(new NoteData { time = 3.1f, direction = NoteDirection.Left });
        noteDataList.Add(new NoteData { time = 3.2f, direction = NoteDirection.Down });
        noteDataList.Add(new NoteData { time = 3.3f, direction = NoteDirection.Right });
        noteDataList.Add(new NoteData { time = 3.4f, direction = NoteDirection.Right });


        if (noteDataList.Count == 0)
        {
            Debug.LogWarning("noteDataList에 노트 데이터가 없습니다. 게임이 진행되지 않을 수 있습니다.");
        }


        // 타이밍 순서대로 정렬 (필수!)
        // 직접 추가했더라도 시간 순서대로 정렬하는 것이 안전합니다.
        noteDataList.Sort((a, b) => a.time.CompareTo(b.time)); // 시간 순서대로 정렬!

        // Debug.Log로 생성된 타이밍 개수 확인 (선택 사항)
        Debug.Log($"총 {noteDataList.Count}개의 노트 데이터 생성 완료.");

        // noteIndex 초기화 (생성된 데이터 리스트의 첫 번째부터 시작)
        noteIndex = 0;
    }

    void Update()
    {
        if (noteIndex < noteDataList.Count && GameManager.instance != null && GameManager.instance.songTime >= noteDataList[noteIndex].time)
        {
            SpawnNote(noteDataList[noteIndex]);
            noteIndex++;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("SelectScene");
        }
    }

    // 노트 생성
    void SpawnNote(NoteData noteData)
    {
        // 사용할 프리팹과 스폰 위치를 담을 변수
        GameObject notePrefab = null;
        Transform spawnPoint = null;

        // NoteData의 방향에 따라 사용할 프리팹과 스폰 위치를 결정합니다.
        switch (noteData.direction) 
        {
            case NoteDirection.Up:
                notePrefab = upNotePrefab;
                spawnPoint = upSpawnPoint;
                break;
            case NoteDirection.Left:
                notePrefab = leftNotePrefab;
                spawnPoint = leftSpawnPoint;
                break;
            case NoteDirection.Down:
                notePrefab = downNotePrefab;
                spawnPoint = downSpawnPoint;
                break;
            case NoteDirection.Right:
                notePrefab = rightNotePrefab;
                spawnPoint = rightSpawnPoint;
                break;
        }

        if (notePrefab == null)
        {
            Debug.LogError($"Note Prefab is NOT assigned for direction: {noteData.direction}. Please assign it in the Inspector!");
            return;
        }
        if (spawnPoint == null)
        {
            Debug.LogError($"Spawn Point is NOT assigned for direction: {noteData.direction}. Please assign a Transform in the Inspector!");
            return;
        }
        if (judgeLineTransform == null)
        {
            Debug.LogError("Judge Line Transform is NOT assigned in the Inspector! Cannot calculate expected time.");
        }


        GameObject noteObject = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);

        Note note = noteObject.GetComponent<Note>();



        // 생성된 노트에 정보 설정
        note.direction = noteData.direction;
        note.speed = noteSpeed;

        if (judgeLineTransform != null)
        {
            float distance = Vector3.Distance(spawnPoint.position, judgeLineTransform.position);
            note.expectedTime = noteData.time;

        }
        else
        {
            note.expectedTime = noteData.time;
        }
    }
}
