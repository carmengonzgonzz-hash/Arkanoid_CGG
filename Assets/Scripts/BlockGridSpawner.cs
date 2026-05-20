using UnityEngine;

public class BlockGridSpawner : MonoBehaviour
{
    [Header("Prefab y contenedor")]
    [SerializeField] Block blockPrefab;
    [SerializeField] Transform parent;

    [Header("Grid base")]
    [SerializeField] int baseRows = 4;
    [SerializeField] int cols = 10;
    [SerializeField] Vector2 spacing = new Vector2(1.1f, 0.6f);
    [SerializeField] Vector2 startPosition = new Vector2(-5f, 3f);

    [Header("Dificultad")]
    [SerializeField] int currentLevel = 1;
    [SerializeField] int maxRows = 7;

    [Range(0f, 1f)]
    [SerializeField] float hardBlockChance = 0.15f;

    int destructibleBlocks;

    public int CurrentLevel => currentLevel;

    [ContextMenu("Spawn Grid")]
    public void Spawn()
    {
        if (blockPrefab == null)
        {
            Debug.LogError("No hay blockPrefab asignado.");
            return;
        }

        if (parent == null) parent = transform;

        ClearParent();
        destructibleBlocks = 0;

        int rows = Mathf.Min(baseRows + currentLevel - 1, maxRows);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                BlockType type = ChooseBlockType(r, c, rows, cols);

                Vector2 pos = startPosition + new Vector2(
                    c * spacing.x,
                    -r * spacing.y
                );

                Block newBlock = Instantiate(blockPrefab, pos, Quaternion.identity, parent);
                newBlock.Configure(type);

                if (newBlock.CountsForVictory)
                {
                    destructibleBlocks++;
                }
            }
        }
    }

    BlockType ChooseBlockType(int row, int col, int rows, int cols)
    {
        // A partir del nivel 2 aparece una fila barrera con hueco central.
        bool hasBarrierRow = currentLevel >= 2;
        bool isBarrierRow = hasBarrierRow && row == 1;

        int gapLeft = (cols / 2) - 1;
        int gapRight = cols / 2;
        bool isGap = col == gapLeft || col == gapRight;

        if (isBarrierRow && !isGap)
        {
            return BlockType.Unbreakable;
        }

        // Los bloques duros siguen teniendo azar, pero controlado por nivel.
        float levelHardChance = hardBlockChance + (currentLevel - 1) * 0.05f;
        levelHardChance = Mathf.Clamp(levelHardChance, 0f, 0.45f);

        if (Random.value < levelHardChance)
        {
            return BlockType.Hard;
        }

        return BlockType.Normal;
    }

    public int GetTotalBlocks()
    {
        return destructibleBlocks;
    }

    public void NextLevel()
    {
        currentLevel++;
        Spawn();
    }

    [ContextMenu("Clear Grid")]
    void ClearParent()
    {
        if (parent == null) return;

        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}