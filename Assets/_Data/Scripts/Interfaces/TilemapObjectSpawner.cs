using ImaginaryTown.Core;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapObjectSpawner : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private int cellSize = 4;

    private int gridSize = 39;

    [NaughtyAttributes.Button]
    void GenerateObjects()
    {
        EffectManager.Instance.ClearChild();

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(position);

                if (tile != null)
                {
                    Vector3 worldPosition = tilemap.CellToWorld(position) + new Vector3(1, 0, 1) * cellSize / 2;
                    GameObject go = Instantiate(objectPrefab, worldPosition, Quaternion.identity);
                    go.transform.SetParent(EffectManager.Instance.EffectParent);
                }
            }
        }
    }
}
