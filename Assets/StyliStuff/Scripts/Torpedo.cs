using UnityEngine;

public class Torpedo : MonoBehaviour
{
    private SpriteRenderer sr;
    public float speed = 5;
    public float time = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Destroy(gameObject, time);
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;


        transform.position += transform.right * speed * Time.deltaTime;

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
