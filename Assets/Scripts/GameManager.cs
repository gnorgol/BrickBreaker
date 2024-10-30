using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

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

    void Start()
    {
        SetupBricks();
        activeBalls = 1;
    }

    void Update()
    {
        livesText.text = "Vies : " + lives;
        scoreText.text = "Score : " + score;

        // V�rifier si toutes les briques sont d�truites
        if (bricks.Count == 0)
        {
            // Charger le niveau suivant ou afficher un message de victoire
            Debug.Log("Niveau termin� !");
            // Par exemple, recharger les briques
            SetupBricks();
            // R�initialiser la balle et la raquette
            ResetBallAndPaddle();
        }
    }

    void SetupBricks()
    {
        // Nettoyer les briques pr�c�dentes
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
            // R�initialiser le jeu ou afficher un �cran de fin
        }
        else
        {
            // R�initialiser la balle et la raquette
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
        // R�initialiser la position de la balle et de la raquette
        paddle.ResetPaddle();
        ball.ResetBall();
    }
    public void SpawnExtraBalls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Ball extraBall = Instantiate(ball, ball.transform.position, Quaternion.identity);
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

}