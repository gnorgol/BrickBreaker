using UnityEngine;

public class Brick : MonoBehaviour
{
    public int hitsToBreak = 1;
    public GameObject bonusItem;
    public GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        hitsToBreak--;
        if (hitsToBreak <= 0)
        {
            if (bonusItem != null)
            {
                Instantiate(bonusItem, transform.position, Quaternion.identity);
            }
            gameManager.RemoveBrick(gameObject);
        }
    }
}