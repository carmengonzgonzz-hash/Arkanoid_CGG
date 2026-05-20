using UnityEngine;
using System.Collections;

public enum BlockType
{
    Normal,
    Hard,
    Unbreakable
}

public class Block : MonoBehaviour
{
    [Header("Configuración")]
    GameManager gameManager;

    [SerializeField] int points = 10;
    [SerializeField] int hitPoints = 1;
    int maxHitPoints;

    [SerializeField] bool countsForVictory = true;

    [Header("Sprites")]
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite hardSprite;
    [SerializeField] GameObject damageOverlay;
    [SerializeField] Sprite unbreakableSprite;

    [Header("Break Animation")]
    [SerializeField] private GameObject breakEffect;
    [SerializeField] private Animator breakAnimator;
    [SerializeField] private float destroyDelay = 0.25f;

    private bool isBreaking = false;
    private SpriteRenderer sr;
    private BlockType blockType;

    public int Points => points;
    public bool CountsForVictory => countsForVictory;

    void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Configure(BlockType newType)
    {
        blockType = newType;
        isBreaking = false;

        switch (blockType)
        {
            case BlockType.Normal:
                maxHitPoints = 1;
                hitPoints = maxHitPoints;
                points = 10;
                countsForVictory = true;
                sr.sprite = normalSprite;
                break;

            case BlockType.Hard:
                maxHitPoints = 2;
                hitPoints = maxHitPoints;
                points = 20;
                countsForVictory = true;
                sr.sprite = hardSprite;
                break;

            case BlockType.Unbreakable:
                maxHitPoints = -1;
                hitPoints = -1;
                points = 0;
                countsForVictory = false;
                sr.sprite = unbreakableSprite;
                break;
        }
    }

    public void TakeHit()
    {
        if (isBreaking) return;

        if (blockType == BlockType.Unbreakable)
        {
            // Más adelante podemos poner aquí sonido/animación de impacto.
            return;
        }

        hitPoints--;

        if (hitPoints <= 0)
        {
            BreakBlock();
        }
        else
        {
            UpdateDamageVisual();
        }
    }

    void UpdateDamageVisual()
    {
        if (damageOverlay != null)
            damageOverlay.SetActive(true);
    }

    void BreakBlock()
    {
        isBreaking = true;

        if (gameManager != null)
            gameManager.AddScore(points);

        if (breakEffect != null)
            breakEffect.SetActive(true);

        // if (sr != null)
        //     sr.enabled = false;

        if (breakAnimator != null)
            breakAnimator.SetTrigger("Break");

        StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}