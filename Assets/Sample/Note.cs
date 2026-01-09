using UnityEngine;

public class Note : MonoBehaviour
{
    public float speed = 5f;

    public NoteDirection direction; 

    public float expectedTime;

    void Start()
    {
    }

    void Update()
    {

        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -6f) 
        {
            Destroy(gameObject); // 노트 오브젝트 파괴!
        }
    }
}
