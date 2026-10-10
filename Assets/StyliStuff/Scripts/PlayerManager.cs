using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerManager : MonoBehaviour
{
    public int health = 5;
    public int ammo = 5;
    public float speed = 5f;
    private bool isRotate = true;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RotateToggle(isRotate);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (ammo == 0)
            {
                Debug.Log("Out of ammo!");

            }
            else
            {

                Debug.Log("SHOOT");
                --ammo;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.Log("Nowhere to hide");
        }

        if (isRotate!)
        {

            if (h <= 0)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }

            


        }
        transform.position += new Vector3(h, v, 0) * speed * Time.deltaTime;
    }

    void RotateToggle(bool rotate)
    {
        if (rotate)
        {
            isRotate = false;
            Debug.Log("Rotate off!");
            

        }

        else
        {
            isRotate = true;
            Debug.Log("Rotate on!");
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colliding");
    }
}

