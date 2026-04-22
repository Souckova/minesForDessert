using UnityEngine;
using UnityEngine.Events;

public class Board : MonoBehaviour
{
    public int width = 9;
    public int height = 9;
    public int minesCount = 9;

    private int safeCells;
    private int revealedCells = 0;

    private Cell[,] grid;

    public GameObject cellPrefab; // přetáhneš prefab v inspectoru
    public GameObject flagPrefab;
    public float cellMargin = 1f;
    public float flagMargin = 1f;
    public float flagOffset = -10f;

    public UnityEvent onMineExplosion;
    public UnityEvent onWin;

    bool firstClick = true;
    Cell firstClickCell;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ošetření vložených atributů
        width = Mathf.Max(2, width);
        height = Mathf.Max(2, height);
        minesCount = Mathf.Clamp(minesCount, 0, width * height - 1);
        cellMargin = Mathf.Max(0.01f, cellMargin);
        flagMargin = Mathf.Max(0.01f, flagMargin);
        //inicializace atributů
        safeCells = width * height - minesCount;

        CreateGrid();
        SpawnCells();
        SpawnFlags();
    }

    void NewGame()
    {
        PutMines();
        CalculateAdjacents();
        Debug.Log(GridToString());
    }

    void FirstClick(Cell cell)
    {
        firstClick = false;
        firstClickCell = cell;

        NewGame();
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
                    view.SetBoard(this);
                    view.flagTrigger.size = new Vector3(2*cellMargin, 1f, 2*cellMargin);
                }
            }
        }
    }

    void SpawnFlags()
    {
        for(int i = 0; i < minesCount; i++)
        {
            Vector3 pos = new Vector3(0, 0, flagOffset + i * flagMargin); // XZ plane

            GameObject flagGO = Instantiate(flagPrefab, this.transform);
            flagGO.transform.localPosition = pos;
            flagGO.transform.localRotation = Quaternion.identity;
            flagGO.transform.SetParent(null);

            flagGO.name = $"Flag_{i}";
        }
    }

    public void RevealCell(Cell cell)
    {
        if(cell.isRevealed) return;
        if(cell.isFlagged) return;
        
        if(firstClick)
        {
            FirstClick(cell);
        }
        
        cell.Reveal();

        if(cell.hasMine)
        {
            cell.Explode();
            onMineExplosion?.Invoke();
        } else {
            revealedCells++;
            if(revealedCells >= safeCells)
            {
                onWin?.Invoke();
            }
        }

        if(cell.adjacentMines == 0)
        {
            RevealAround(cell);
        }
    }

    public void FlagCell(Cell cell)
    {
        if(cell.isRevealed) return;

        cell.Flag();
    }

    public void UnFlagCell(Cell cell)
    {
        if(cell.isRevealed) return;

        cell.UnFlag();
    }

    public void RevealAround(Cell cell)
    {
        int x = cell.x;
        int y = cell.y;

        for(int dx = -1; dx <= 1; dx++)
        {
            for(int dy = -1; dy <= 1; dy++)
            {
                if(dx == 0 && dy == 0) continue;

                int nx = x + dx;
                int ny = y + dy;

                if(IsInsideGrid(nx, ny))
                {
                    RevealCell(grid[nx, ny]);
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
                grid[x, y] = new Cell
                {
                    x = x,
                    y = y
                };
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

            if(!grid[x, y].hasMine && grid[x, y] != firstClickCell)
            {
                grid[x, y].hasMine = true;
                grid[x, y].adjacentMines = -1;
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