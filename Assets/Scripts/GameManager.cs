using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int lives = 3;
    public int score = 0;
    public int activeBalls = 0;
    public TMP_Text livesText;
    public TMP_Text scoreText;

    public GameObject brickPrefab;
    public int rows = 5;
    public int columns = 8;
    public float brickSpacing = 0.5f;
    private List<GameObject> bricks = new List<GameObject>();

    public Transform bricksParent;
    public Ball ball;
    public Paddle paddle;
    public List<GameObject> BonusItems = new List<GameObject>();

    public GameObject gameOverPanel;

    void Start()
    {
        gameOverPanel.SetActive(false);
        SetupBricks();
        activeBalls = 1;
    }

    void Update()
    {
        livesText.text = "Vies : " + lives;
        scoreText.text = "Score : " + score;

        // Vérifier si toutes les briques sont détruites
        if (bricks.Count == 0)
        {
            // Charger le niveau suivant ou afficher un message de victoire
            Debug.Log("Niveau terminé !");
            // Par exemple, recharger les briques
            SetupBricks();
            // Réinitialiser la balle et la raquette
            ResetBallAndPaddle();
        }
    }

    void SetupBricks()
    {
        // Nettoyer les briques précédentes
        foreach (GameObject brick in bricks)
        {
            Destroy(brick);
        }
        bricks.Clear();

        float startX = -columns / 2f + 0.5f;
        float startY = 4f;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector3 position = new Vector3(startX + x + x * brickSpacing, startY - y - y * brickSpacing, 0);
                GameObject brick = Instantiate(brickPrefab, position, Quaternion.identity);
                brick.transform.parent = bricksParent;
                bricks.Add(brick);
                //Add a random bonus item to the brick but there is a 1 in 5 chance that the brick will have a bonus item
                if (Random.Range(0, 5) == 0)
                {
                    GameObject bonusItem = BonusItems[Random.Range(0, BonusItems.Count)];
                    brick.GetComponent<Brick>().bonusItem = bonusItem;

                }
            }
        }
    }

    public void RemoveBrick(GameObject brick)
    {
        if (bricks.Contains(brick))
        {
            bricks.Remove(brick);
            Destroy(brick);
            AddScore(10);
        }
    }

    public void LoseLife()
    {
        lives--;
        if (lives <= 0)
        {
            // Fin du jeu
            Debug.Log("Game Over");
            GameOver();

        }
        else
        {
            // Réinitialiser la balle et la raquette
            ResetBallAndPaddle();
            activeBalls = 1;
        }
    }

    public void AddScore(int points)
    {
        score += points;
    }

    void ResetBallAndPaddle()
    {
        // Réinitialiser la position de la balle et de la raquette
        paddle.ResetPaddle();
        ball.ResetBall();
    }
    public void SpawnExtraBalls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Ajouter un léger décalage à la position de départ pour éviter les collisions
            Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            Ball extraBall = Instantiate(ball, ball.transform.position + offset, Quaternion.identity);
            extraBall.gameManager = this;
            extraBall.inPlay = true;
            Rigidbody2D rb = extraBall.GetComponent<Rigidbody2D>();
            rb.AddForce(Random.insideUnitCircle.normalized * ball.speed, ForceMode2D.Impulse);
        }
    }

    public void RegisterBall()
    {
        activeBalls++;
    }

    public void UnregisterBall()
    {
        activeBalls--;
        if (activeBalls <= 0)
        {
            LoseLife();
        }
    }
    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }
    public static void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}