using UnityEngine;
using UnityEngine.UIElements;

public class MazeSpawner : MonoBehaviour
{
    public Cell CellPrefab;
    public Vector3 CellSize = new(1,1,0);
    public HintRenderer HintRenderer;
    public bool isMultiplayer;

    public Maze maze;

    private void Start()
    {
        MazeGenerator generator = new();
        maze = generator.GenerateMaze();

        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int y = 0; y < maze.cells.GetLength(1); y++)
            {
                var position = new Vector3(x * CellSize.x, y * CellSize.y, y * CellSize.z);
                Cell c = Instantiate(CellPrefab, position, Quaternion.identity);

             
                c.WallLeft.SetActive(maze.cells[x, y].WallLeft);
                c.WallBottom.SetActive(maze.cells[x, y].WallBottom);
                c.End.SetActive(maze.cells[x, y].End);
            }
        }
        if (!isMultiplayer) { return; }
       


        HintRenderer.DrawPath();
    }
}