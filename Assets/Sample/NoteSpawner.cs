using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NoteSpawner : MonoBehaviour
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
        // noteDataList 리스트를 초기화합니다.
        noteDataList.Clear(); // 혹시 이전에 데이터가 남아있을까봐 비워줍니다.

        // GameManager 인스턴스가 준비되었는지 확인합니다.
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager 인스턴스를 찾을 수 없습니다. GameManager가 씬에 있는지, 스크립트 실행 순서가 올바른지 확인하세요.");
            return; // GameManager 없으면 함수 종료
        }

        float duration = 0;
        // GameManager가 음악 없이 테스트 모드이면 testDuration 사용
        if (GameManager.instance.testWithoutMusic)
        {
            duration = testDuration;
            Debug.Log($"음악 없이 테스트 모드: {testDuration}초 동안 노트 생성 타이밍 계산");
        }
        else // 음악 재생 모드이면 음악 길이 사용
        {
            if (GameManager.instance.bgmClip == null)
            {
                Debug.LogError("GameManager의 bgmClip에 배경 음악 파일이 할당되지 않았습니다.");
                return;
            }
            duration = GameManager.instance.bgmClip.length;
            Debug.Log($"음악 재생 모드: 노래 길이 {duration}초 동안 노트 생성 타이밍 계산");
        }


        float generationStartTime = GameManager.instance.songStartDelay + 0.5f;
        float noteInterval = 0.5f; // 노트 생성 간격 (이 간격마다 노트가 하나씩 나옵니다.)

       for (float time = generationStartTime; time < duration; time += noteInterval)
        {
            // 4개 방향 (0, 1, 2, 3) 중에서 랜덤으로 하나 선택!
            int randomDirectionIndex = Random.Range(0, 4); // Random.Range(min, max)에서 max는 포함 안 됨! (0, 1, 2, 3 중 랜덤)

            // 랜덤으로 선택된 인덱스에 해당하는 NoteDirection 값을 가져옵니다.
            NoteDirection randomDirection = (NoteDirection)randomDirectionIndex;

            // 랜덤 방향과 현재 시간을 가진 NoteData 객체를 만들어서 리스트에 추가!
            noteDataList.Add(new NoteData { time = time, direction = randomDirection });
        }

        // 타이밍 순서대로 정렬 (필수!)
        noteDataList.Sort((a, b) => a.time.CompareTo(b.time)); // 시간 순서대로 정렬!

        // Debug.Log로 생성된 타이밍 개수 확인 (선택 사항)
        Debug.Log($"총 {noteDataList.Count}개의 노트 데이터 (랜덤 방향) 생성 완료.");

        // noteIndex 초기화 (생성된 데이터 리스트의 첫 번째부터 시작)
        noteIndex = 0;
    }

    // 매 프레임마다 실행됩니다.
    void Update()
    {
       if (noteIndex < noteDataList.Count && GameManager.instance != null && GameManager.instance.songTime >= noteDataList[noteIndex].time)
        {
            SpawnNote(noteDataList[noteIndex]); // 노트 생성 함수 호출! (NoteData 전달)
            noteIndex++; // 다음 노트 생성을 위해 인덱스 증가!
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 씬 이름이 "SelectScene"인 씬으로 이동
            SceneManager.LoadScene("SelectScene");
        }
    }

    // 노트를 생성하는 함수입니다. NoteData 정보를 받아서 해당 노트 생성!
    void SpawnNote(NoteData noteData) // <-- NoteData 정보를 인자로 받습니다!
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

        // !!! 중요: notePrefab이나 spawnPoint가 제대로 연결되었는지 여기서 체크합니다.
        if (notePrefab == null)
        {
            Debug.LogError($"Note Prefab is NOT assigned for direction: {noteData.direction}. Please assign it in the Inspector!");
            return; // 프리팹이 없으면 생성하지 않고 함수 종료
        }
        if (spawnPoint == null)
        {
            Debug.LogError($"Spawn Point is NOT assigned for direction: {noteData.direction}. Please assign a Transform in the Inspector!");
            return; // 스폰 포인트가 없으면 생성하지 않고 함수 종료
        }
        if (judgeLineTransform == null)
        {
            Debug.LogError("Judge Line Transform is NOT assigned in the Inspector! Cannot calculate expected time.");

        }


        GameObject noteObject = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);

        // 생성된 오브젝트에서 Note 컴포넌트를 가져옵니다.
        Note note = noteObject.GetComponent<Note>();

        // 생성된 노트 오브젝트에 Note 컴포넌트가 있는지 다시 한번 확인 (안정성 강화)
        if (note == null)
        {
            Debug.LogError($"Instantiated Note object ({noteObject.name}) is missing the Note component for direction: {noteData.direction}. Check the Prefab!");
            Destroy(noteObject); // 잘못 만들어진 오브젝트 파괴
            return;
        }

        note.direction = noteData.direction; // <-- NoteData에서 가져온 방향 설정!
        note.speed = noteSpeed;     // 속도 설정

       if (judgeLineTransform != null)
        {
            float distance = Vector3.Distance(spawnPoint.position, judgeLineTransform.position);
            note.expectedTime = noteData.time + (distance / noteSpeed);
        }
        else
        {
            note.expectedTime = noteData.time; // 판정선 없어도 NoteData 시간으로 설정
        }
    }
}
