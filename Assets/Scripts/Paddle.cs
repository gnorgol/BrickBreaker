using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float speed = 10f;
    private float screenWidthUnits;

    void Start()
    {
        screenWidthUnits = Camera.main.orthographicSize * Camera.main.aspect;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 position = transform.position;
        position.x += horizontal * speed * Time.deltaTime;
        // Limiter la raquette aux bords de l'écran
        position.x = Mathf.Clamp(position.x, -screenWidthUnits + 1, screenWidthUnits - 1);
        transform.position = position;
    }
    public void Expand()
    {
        transform.localScale += new Vector3(1f, 0f, 0f);
    }
    public void ResetPaddle()
    {
        // Réinitialiser la taille et la position de la raquette
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.position = new Vector3(0f, transform.position.y, 0f);
    }
}