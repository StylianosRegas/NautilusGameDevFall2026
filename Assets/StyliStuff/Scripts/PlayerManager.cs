using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerManager : MonoBehaviour
{
    public int health = 5;
    public int ammo = 5;
    public float speedValue = 5f;
    private float speed;
    private bool isRotate = true;
    private bool canHide = false;
    private bool isHiding = false;

    [SerializeField]
    private GameObject Torpedo;

    enum Facing { Horizontal, Vertical }
    Facing activeAxis = Facing.Horizontal;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = speedValue;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0 && v == 0) activeAxis = Facing.Horizontal;
        else if (v != 0 && h == 0) activeAxis = Facing.Vertical;

        if (isRotate)
        {
            if (activeAxis == Facing.Horizontal && h != 0)
            {
                sr.flipX = h < 0;
                rb.rotation = 0;
            }
            else if (activeAxis == Facing.Vertical && v != 0)
            {
                sr.flipX = false;               
                rb.rotation = v > 0 ? 90 : -90;
            }
          
        }

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RotateToggle(isRotate);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            hide();
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

        if (collision.gameObject.tag == "HidingSpot")
        {
            canHide = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HidingSpot")
        {
            canHide = false;
        }
    }

    private void hide()
    {
        if (canHide && !isHiding)
        {
            Debug.Log("HIDING");
            isHiding = true;
            speed = 0;
            sr.enabled = false;
        }

        else if (!canHide)
        {
            Debug.Log("nowhere to hide");
        }

        else if (isHiding)
        {
            Debug.Log("NO LONGER HIDING");
            isHiding = false;
            speed = speedValue;
            sr.enabled = true;
        }
    }

    void Shoot()
    {
        if (ammo == 0)
        {
            Debug.Log("Out of ammo!");

        }
        else
        {

            GameObject clone = Torpedo;
            float angle = sr.flipX ? 180f : rb.rotation;
            Quaternion rot = Quaternion.Euler(0, 0, angle);
            
            Instantiate(clone, transform.position, rot);
            

            --ammo;
        }
    }

}

