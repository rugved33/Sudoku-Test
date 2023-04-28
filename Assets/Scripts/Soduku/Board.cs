using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Zenject;
using Image = UnityEngine.UI.Image;
public class Board : MonoBehaviour
{
    private int[,] grid = new int[9,9];
    private int[,] puzzle = new int[9, 9];
    private SudokuCell[,] cells = new SudokuCell[9,9];
    private int [,,]board = new int[9,3,3];
    private int difficulty = 15;
    private ParticlePooler _particlePooler;

    [SerializeField]
    private Transform square00, square01, square02,
                    square10, square11, square12,
                    square20, square21, square22;
    [SerializeField] private GameObject SudokuCell_Prefab;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject loseText;
    [SerializeField] private Settings _settings;

    [Inject]
    private void Construct(ParticlePooler particlePooler)
    {
        _particlePooler = particlePooler;
    }

    private void Start()
    {
        winMenu.SetActive(false);
        difficulty = _settings.difficulty;
        CreateGrid();
        CreatePuzzle();
        CreateButtons();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.W))
        {
            winMenu.SetActive(true);
            _particlePooler.SpawnVFX(VFXType.Confetti,Vector3.zero);
        }
    }

    private void ConsoleOutputGrid(int [,] g)
    {
        string output = "";
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                output += g[i, j];
            }
            output += "\n";
        }
    }

    private void Make3dGrid(int [,] g)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int m = 0; m < 9; m+=3)
                    {
                        for (int n = 0; n < 9; n+=3)
                        {
                            board[i,j,k] = g[m, n];
                        }
                    }
                }
            }
        }
    }

    private bool ColumnContainsValue(int col, int value)
    {
        for (int i = 0; i < 9; i++)
        {
            if (grid[i, col] == value)
            {
                return true;
            }
        }

        return false;
    }

    private bool RowContainsValue(int row, int value)
    {
        for (int i = 0; i < 9; i++)
        {
            if (grid[row, i] == value)
            {
                return true;
            }
        }

        return false;
    }

    private bool SquareContainsValue(int row, int col, int value)
    {
        //blocks are 0-2, 3-5, 6-8
        //row / 3 is the first grid coord * 3
        //ints

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (grid[ row / 3 * 3 + i , col / 3 * 3 + j ] == value)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool CheckSquare(int x, int y, int value)
    {
        for (int i = x - x % 3; i <= x - x % 3 + 2; i++)
        {
            for (int j = y - y % 3; j <= y - y % 3 + 2; j++)
            {
                if (grid[i, j] == value)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool CheckAll(int row, int col, int value)
    {
        if (ColumnContainsValue(col,value)) {
            return false;
        }
        if (RowContainsValue(row, value))
        {
            return false;
        }
        if (SquareContainsValue(row, col, value))
        {
            return false;
        }

        return true;
    }

    private bool IsValid()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (grid[i,j] == 0)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void CreateGrid()
    {
        List<int> rowList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        List<int> colList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        int value = rowList[Random.Range(0, rowList.Count)];
        grid[0, 0] = value;
        rowList.Remove(value);
        colList.Remove(value);

        for (int i = 1; i < 9; i++)
        {
            value = rowList[Random.Range(0, rowList.Count)];
            grid[i, 0] = value;
            rowList.Remove(value);
        }

        for (int i = 1; i < 9; i++)
        {
            value = colList[Random.Range(0, colList.Count)];
            if (i < 3)
            {
                while(SquareContainsValue(0, 0, value))
                {
                    value = colList[Random.Range(0, colList.Count)]; // reroll
                }
            }
            grid[0, i] = value;
            colList.Remove(value);
        }

        for (int i = 6; i < 9; i++)
        {
            value = Random.Range(1, 10);
            while (SquareContainsValue(0, 8, value) || SquareContainsValue(8, 0, value) || SquareContainsValue(8, 8, value))
            {
                value = Random.Range(1, 10);
            }
            grid[i, i] = value;
        }

        ConsoleOutputGrid(grid);
        Make3dGrid(grid);
        SolveSudoku();
    }

    private bool SolveSudoku()
    {
        int row = 0;
        int col = 0;

        if (IsValid())
        {
            return true;
        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (grid[i, j] == 0)
                {
                    row = i;
                    col = j;
                }
            }
        }

        for (int i = 1; i <=9; i++)
        {
            if (CheckAll(row, col, i)) {
                grid[row, col] = i;

                if (SolveSudoku())
                {
                    return true;
                }
                else
                {
                    grid[row, col] = 0;
                }
            }
        }
        return false;
    }

    private void CreatePuzzle()
    {
        System.Array.Copy(grid, puzzle, grid.Length);

        for (int i = 0; i < difficulty; i++)
        {
            int row = Random.Range(0, 9);
            int col = Random.Range(0, 9);

            while (puzzle[row,col] == 0)
            {
                row = Random.Range(0, 9);
                col = Random.Range(0, 9);
            }

            puzzle[row, col] = 0;
        }

        List<int> onBoard = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        RandomizeList(onBoard);

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                for (int k = 0; k < onBoard.Count - 1; k++)
                {
                    if (onBoard[k] == puzzle[i,j])
                    {
                        onBoard.RemoveAt(k);
                    }
                }
            }
        }

        while (onBoard.Count - 1 > 1)
        {
            int row = Random.Range(0, 9);
            int col = Random.Range(0, 9);

            if (grid[row,col] == onBoard[0])
            {
                puzzle[row, col] = grid[row, col];
                onBoard.RemoveAt(0);
            }

        }

        ConsoleOutputGrid(puzzle);
        Make3dGrid(puzzle);
    }

    private void RandomizeList(List<int> l)
    {
        for (var i = 0; i < l.Count - 1; i++)
        {
            int rand = Random.Range(i, l.Count);
            int temp = l[i];
            l[i] = l[rand];
            l[rand] = temp;
        }
    }

    private void CreateButtons()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                GameObject newButton = Instantiate(SudokuCell_Prefab);
                SudokuCell sudokuCell = newButton.GetComponent<SudokuCell>();
                sudokuCell.SetValues(i, j, puzzle[i, j], i + "," + j, this);
                cells[i,j] = sudokuCell;
                newButton.name = i.ToString() + j.ToString();

                if (i < 3)
                {
                    if (j < 3)
                    {
                        newButton.transform.SetParent(square00, false);
                    }
                    if (j > 2 && j < 6)
                    {
                        newButton.transform.SetParent(square01, false);
                    }
                    if (j >= 6)
                    {
                        newButton.transform.SetParent(square02, false);
                    }
                }

                if (i >= 3 && i < 6)
                {
                    if (j < 3)
                    {
                        newButton.transform.SetParent(square10, false);
                    }
                    if (j > 2 && j < 6)
                    {
                        newButton.transform.SetParent(square11, false);
                    }
                    if (j >= 6)
                    {
                        newButton.transform.SetParent(square12, false);
                    }
                }

                if (i >= 6)
                {
                    if (j < 3)
                    {
                        newButton.transform.SetParent(square20, false);
                    }
                    if (j > 2 && j < 6)
                    {
                        newButton.transform.SetParent(square21, false);
                    }
                    if (j >= 6)
                    {
                        newButton.transform.SetParent(square22, false);
                    }
                }

            }
        }
    }

    public void UpdatePuzzle(int row, int col, int value)
    {
        puzzle[row, col] = value;
        DisableHighLights();
    }

    /// <summary>
    /// Check Puzzle
    /// </summary>
    public void CheckComplete()
    {
        if (CheckGrid())
        {
            winMenu.SetActive(true);
            _particlePooler.SpawnVFX(VFXType.Confetti,Vector3.zero);
        }
        else
        {
            loseText.SetActive(true);
        }
    }

    private bool CheckGrid()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j =  0; j < 9; j++)
            {
                if (puzzle[i,j] != grid[i,j])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void HighLightValue(int value)
    {
        if(value == 0) { return; }

        for (int i = 0; i < 9; i++)
        {
            for (int j =  0; j < 9; j++)
            {
                    SudokuCell sudokuCell = cells[i,j];

                    if(!sudokuCell.IsEmpty())
                    {   
                        if (value == sudokuCell.Value)
                        {
                            sudokuCell.gameObject.GetComponent<Image>().color = _settings.highLightColor;
                        }
                    }
				}
            }
    }

    public void DisableHighLights()
    {

        for (int i = 0; i < 9; i++)
        {
            for (int j =  0; j < 9; j++)
            {
                    SudokuCell sudokuCell = cells[i,j];
                    sudokuCell.gameObject.GetComponent<Image>().color = Color.white;
			}
        }
    }
}