// GameManager.cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤 패턴

    public AudioSource bgmPlayer; 
    public AudioClip bgmClip; // 배경 음악 파일

    public float songStartDelay = 0.1f; // 게임 시작 후 음악 재생 딜레이 (초)
    public float songTime; // 현재 게임 시간 (노래 재생 시간이거나, 테스트 시 게임 경과 시간)

    // 음악 재생 없이 테스트할 때 이 변수를 true로 설정합니다.
    public bool testWithoutMusic = true; 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (!testWithoutMusic)
        {
            if (bgmPlayer != null && bgmClip != null)
            {
                bgmPlayer.clip = bgmClip;
                Invoke(nameof(StartBGM), songStartDelay);
            }
            else
            {
                Debug.LogWarning("BGM Player 또는 BGM Clip이 할당되지 않았습니다. 음악 없이 테스트 모드로 전환합니다.");
                testWithoutMusic = true;
            }
        }
        else
        {
            Debug.Log("음악 없이 테스트 모드입니다. songTime은 게임 경과 시간으로 흐릅니다.");
        }
    }

    void StartBGM()
    {
        if (bgmPlayer != null)
        {
            bgmPlayer.Play();
        }
    }

    void Update()
    {
        if (testWithoutMusic)
        {
            songTime += Time.deltaTime;
        }
        else
        {
            if (bgmPlayer != null && bgmPlayer.isPlaying)
            {
                songTime = bgmPlayer.time;
            }
        }
    }
}
