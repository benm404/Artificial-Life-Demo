using UnityEngine;
using System.Collections.Generic;

public class School : MonoBehaviour
{
    public GameObject fishPrefab;
    public int count = 5;
    public float radius = 100f;

    public float neighborDistance = 20f; // Make sure this is public
    public int maxNeighbors = 10;

    public float cellSize = 10f;
    public int gridSize = 10000;
    public bool partition = true;
    public Dictionary<int, List<Boid>> cells = new Dictionary<int, List<Boid>>();

    public Transform center;

    public void DoPartition()
    {
        cells.Clear();
        foreach (Boid boid in FindObjectsOfType<Boid>())
        {
            int key = PositionToCell(boid.transform.position);
            if (!cells.ContainsKey(key))
            {
                cells[key] = new List<Boid>();
            }
            cells[key].Add(boid);
        }
    }

    public int PositionToCell(Vector3 position)
    {
        Vector3 adjustedPosition = position + new Vector3(10000, 10000, 10000);
        int x = Mathf.FloorToInt(adjustedPosition.x / cellSize);
        int y = Mathf.FloorToInt(adjustedPosition.y / cellSize);
        int z = Mathf.FloorToInt(adjustedPosition.z / cellSize);

        return x + (y * gridSize) + (z * gridSize * gridSize);
    }

    public Vector3 CellToPosition(int cell)
    {
        int z = Mathf.FloorToInt(cell / (gridSize * gridSize));
        int y = Mathf.FloorToInt((cell - (z * gridSize * gridSize)) / gridSize);
        int x = cell - (y * gridSize + (z * gridSize * gridSize));

        Vector3 position = new Vector3(x, y, z) * cellSize;
        position -= new Vector3(10000, 10000, 10000);
        return position;
    }

    private void OnDrawGizmos()
    {
        float size = 200f;
        int subDivisions = Mathf.FloorToInt(size / cellSize);
        Gizmos.color = Color.cyan;
        for (int i = 0; i <= subDivisions; i++)
        {
            Gizmos.DrawLine(new Vector3(i * cellSize, 0, 0), new Vector3(i * cellSize, 0, size));
            Gizmos.DrawLine(new Vector3(0, 0, i * cellSize), new Vector3(size, 0, i * cellSize));
        }
    }
}