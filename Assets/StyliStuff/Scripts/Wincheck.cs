using UnityEngine;

public class Wincheck : MonoBehaviour
{
    public PlayerManager Player;
    public int pagesNeeded = 2;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            {

            if(Player.pagesCollected == pagesNeeded)
            {
                Debug.Log("YOU WIN");
            }

        }
        
    }
}
