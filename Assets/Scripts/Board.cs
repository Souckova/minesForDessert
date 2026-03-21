using UnityEngine;

public class Board : MonoBehaviour
{
    public int width = 9;
    public int height = 9;
    public int minesCount = 9;

    private Cell[,] grid;

    public GameObject cellPrefab; // přetáhneš prefab v inspectoru
    public float cellMargin = 1f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateGrid();
        Debug.Log(GridToString());
        PutMines();
        Debug.Log(GridToString());
        CalculateAdjacents();
        Debug.Log(GridToString());
        SpawnCells();
    }

    void SpawnCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x * cellMargin, 0, y * cellMargin); // XZ plane
                
                GameObject cellGO = Instantiate(cellPrefab, this.transform);
                cellGO.transform.localPosition = pos;
                cellGO.transform.localRotation = Quaternion.identity;

                cellGO.name = $"Cell_{x}_{y}";

                CellView view = cellGO.GetComponent<CellView>();
                if (view != null)
                {
                    view.SetData(grid[x, y]);
                }
            }
        }
    }


    void CreateGrid() {
        grid = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new Cell();
            }
        }

        Debug.Log("Grid vytvořen!");
    }

    void PutMines()
    {
        int minesPlaced = 0;

        while(minesPlaced < minesCount)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            if(!grid[x, y].hasMine)
            {
                grid[x, y].hasMine = true;
                minesPlaced++;
            }
        }
    }

    void CalculateAdjacents()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if(grid[x, y].hasMine)
                {
                    continue;
                }

                grid[x, y].adjacentMines = CountMinesAround(x, y);
            }
        }
    }

    int CountMinesAround(int x, int y)
    {
        int count = 0;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                int nx = x + dx;
                int ny = y + dy;

                if (IsInsideGrid(nx, ny) && grid[nx, ny].hasMine)
                {
                    count++;
                }
            }
        }

        return count;
    }

    bool IsInsideGrid(int x, int y)
    {
        return 
        x >= 0 && x < width &&
        y >= 0 && y < height; 
    }

    string GridToString() {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        for(int y = 0; y < height; y++) {
            for(int x = 0; x < width; x++) {
                sb.Append(grid[x, y].ToString());
                sb.Append(" ");
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

}