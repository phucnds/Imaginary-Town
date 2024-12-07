using System;
using ImaginaryTown.Core;
using ImaginaryTown.Ultis;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateChunk : MonoBehaviour
{
    private enum ChunkShape
    {
        None,
        TopRight,
        BottomRight,
        BottomLeft,
        TopLeft,
        Top,
        Right,
        Bottom,
        Left,
        Four
    }


    [SerializeField] private Tilemap tilemap;
    [SerializeField] private int cellSize = 3;
    [SerializeField] private int gridSize;

    [SerializeField] private GameObject[] chunkShapes;
    private TileBase[,] grid;

    private void Start()
    {
        IntializeGrid();
    }

    private void IntializeGrid()
    {
        grid = new TileBase[gridSize, gridSize];


        BoundsInt bounds = tilemap.cellBounds;


        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(position);

                if (tile != null)
                {
                    grid[x, y] = tile;
                }
            }
        }
    }

    [NaughtyAttributes.Button]
    private void UpdateChunk()
    {
        ChunkManager.Instance.ClearChild();

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                TileBase tile = grid[x, y];
                if (tile == null) continue;

                TileBase fTile = IsValidGridPosition(x, y + 1) ? grid[x, y + 1] : null;
                TileBase rTile = IsValidGridPosition(x + 1, y) ? grid[x + 1, y] : null;
                TileBase bTile = IsValidGridPosition(x, y - 1) ? grid[x, y - 1] : null;
                TileBase lTile = IsValidGridPosition(x - 1, y) ? grid[x - 1, y] : null;

                int configuration = 0;
                if (fTile != null) configuration = configuration + 1;
                if (rTile != null) configuration = configuration + 2;
                if (bTile != null) configuration = configuration + 4;
                if (lTile != null) configuration = configuration + 8;

                Vector3Int position = new Vector3Int(x, y, 0);
                Vector3 worldPosition = tilemap.CellToWorld(position) + new Vector3(1, 0, 1) * cellSize / 2;

                GameObject go = Instantiate(SetChunkRenderer(configuration), worldPosition, Quaternion.identity);
                go.transform.SetParent(ChunkManager.Instance.ChunkParent);

            }
        }
    }

    private GameObject SetChunkRenderer(int configuration)
    {
        switch (configuration)
        {
            case 0: return chunkShapes[(int)ChunkShape.Four];
            case 1: return chunkShapes[(int)ChunkShape.Bottom];
            case 2: return chunkShapes[(int)ChunkShape.Left];
            case 3: return chunkShapes[(int)ChunkShape.BottomLeft];
            case 4: return chunkShapes[(int)ChunkShape.Top];
            case 5: return chunkShapes[(int)ChunkShape.None];
            case 6: return chunkShapes[(int)ChunkShape.TopLeft];
            case 7: return chunkShapes[(int)ChunkShape.Left];
            case 8: return chunkShapes[(int)ChunkShape.Right];
            case 9: return chunkShapes[(int)ChunkShape.BottomRight];
            case 10: return chunkShapes[(int)ChunkShape.None];
            case 11: return chunkShapes[(int)ChunkShape.Bottom];
            case 12: return chunkShapes[(int)ChunkShape.TopRight];
            case 13: return chunkShapes[(int)ChunkShape.Right];
            case 14: return chunkShapes[(int)ChunkShape.Top];
            case 15: return chunkShapes[(int)ChunkShape.None];
        }

        return chunkShapes[9];
    }

    private bool IsValidGridPosition(int x, int y)
    {
        if (x < 0 || x >= gridSize || y < 0 || y >= gridSize) return false;
        return true;
    }

}
