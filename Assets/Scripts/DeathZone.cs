using UnityEngine;
public class DeathZone : MonoBehaviour
{
    GameManager gm;
    void Awake()
    {
        gm = FindFirstObjectByType<GameManager>();
    }
    void OnTriggerEnter2D(Collider2D other)

    {
        if (other.GetComponent<BallController>() != null)
        {
            gm.LoseLife();
        }
    }
}