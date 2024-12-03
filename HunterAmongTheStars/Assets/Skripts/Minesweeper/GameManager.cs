using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject lostGamePanel;
    [SerializeField] private GameObject wonGamePanel;
    [SerializeField] private GameObject cellPrefab;
    private bool isGameOver = false;
    public int gridSize = 10;
    public float spacing = 150f; 
    public float mineChance = 0.15f; 

    private Cell[,] grid;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        grid = new Cell[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 position = new Vector3(x * spacing, 0, z * spacing);
                GameObject cellObj = Instantiate(cellPrefab, position, Quaternion.identity);
                Cell cell = cellObj.GetComponent<Cell>();

                // Mark a cell as a mine
                cell.isMine = Random.value < mineChance;
                grid[x, z] = cell;
            }
        }

        CalculateAdjacentMines();
    }

    void CalculateAdjacentMines()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                if (grid[x, z].isMine) continue;

                int mineCount = 0;
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dz = -1; dz <= 1; dz++)
                    {
                        int nx = x + dx;
                        int nz = z + dz;

                        if (nx >= 0 && nx < gridSize && nz >= 0 && nz < gridSize)
                        {
                            if (grid[nx, nz].isMine) mineCount++;
                        }
                    }
                }

                grid[x, z].adjacentMines = mineCount;
            }
        }
    }

    public void RevealCell(int x, int z)
    {
        if (x < 0 || x >= gridSize || z < 0 || z >= gridSize) return;

        Cell cell = grid[x, z];
        if (cell.isRevealed) return;

        cell.Reveal();

        if (cell.isMine)
        {
            EndGame(false); // lose
        }
        else if (CheckWinCondition())
        {
            EndGame(true); // win
        }

        if (cell.adjacentMines == 0 && !cell.isMine)
        {
            // automatically open neihbour cells 
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dz = -1; dz <= 1; dz++)
                {
                    RevealCell(x + dx, z + dz);
                }
            }
        }
    }

    public void EndGame(bool isWin)
    {
        isGameOver = true;
        StartCoroutine(HandleEndGame(isWin));
    }

    private IEnumerator HandleEndGame(bool isWin)
    {
        isGameOver = true;

        if (isWin)
        {
            yield return new WaitForSeconds(2f);
            RevealAllMines();
            yield return new WaitForSeconds(2f);
            wonGamePanel.SetActive(true);
        }
        else
        {
            yield return new WaitForSeconds(2f);
            RevealAllMines();
            yield return new WaitForSeconds(2f);
            lostGamePanel.SetActive(true);
        }
    }

    private void RevealAllMines()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                if (grid[x, z].isMine)
                {
                    grid[x, z].Reveal();
                }
            }
        }
    }

    private bool CheckWinCondition()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                if (!grid[x, z].isMine && !grid[x, z].isRevealed)
                {
                    return false;
                }
            }
        }
        return true;
    }

    void Update()
    {
        if (isGameOver) return; // Block clicking if the game ended

        // Right click (open cell)
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Cell cell = hit.collider.GetComponent<Cell>();
                if (cell != null)
                {
                    for (int x = 0; x < gridSize; x++)
                    {
                        for (int z = 0; z < gridSize; z++)
                        {
                            if (grid[x, z] == cell)
                            {
                                RevealCell(x, z);
                                return;
                            }
                        }
                    }
                }
            }
        }

        // Right click (mark/cancel as flag)
        if (Input.GetMouseButtonDown(1))
        {
            AudioManager.PlaySound(SoundType.Button, 0.7f);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Cell cell = hit.collider.GetComponent<Cell>();
                if (cell != null)
                {
                    cell.ToggleFlag();
                }
            }
        }
    }
}

