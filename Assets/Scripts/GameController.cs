using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private SudokuMatrix sudoku;

    private List<List<Tile>> sudokuTiles = new List<List<Tile>>();

    private Tile selectedTile = null;

    private float tileSizeX;

    private float tileSizeY;

    [SerializeField] private loadRewardedAds rewardAds;

    [SerializeField] private GameObject canvas;

    [SerializeField] private Sprite texture;

    [SerializeField] private Text timer;

    [SerializeField] private Text tokens;

    [SerializeField] private Font font;

    [SerializeField] private Image grid;

    [SerializeField] private GameObject gameOver;
    private void Start()
    {
        Input.simulateMouseWithTouches = true;

        gameOver.SetActive(false);

        sudoku = SaveSystem.LoadGame();

        if (sudoku.GetGameType() == 0)
        {
            for (int indexX = 0; indexX < 9; indexX++)
            {
                sudokuTiles.Add(new List<Tile>());

                for (int indexY = 0; indexY < 9; indexY++)
                {
                    bool isChangeable = false;

                    if (sudoku.initialSudoku[indexX][indexY] == -1)
                    {
                        isChangeable = true;
                    }

                    Tile tile = new Tile(indexX, indexY, sudoku.sudoku[indexX][indexY], isChangeable, texture, font, canvas);

                    sudokuTiles[indexX].Add(tile);
                }
            }
        }

        tileSizeX = sudokuTiles[1][0].tileObject.transform.position.x - sudokuTiles[0][0].tileObject.transform.position.x;

        tileSizeY = sudokuTiles[0][1].tileObject.transform.position.y - sudokuTiles[0][0].tileObject.transform.position.y;

        for (int indexX = 0; indexX < 9; indexX++)
        {
            CheckValidity(indexX);
        }

        timer.text = sudoku.timer.GetTimer();

        tokens.text = "Tokens: " + sudoku.GetTokens().ToString();

        Suggest();

        InvokeRepeating("TimerTick", 1f, 1f);
    }

    private void TimerTick()
    {
        sudoku.timer.Tick();

        timer.text = sudoku.timer.GetTimer();
    }

    public void MakeGameEasier()
    {
        if (!sudoku.easyGameMode)
        {
            if (sudoku.GetTokens() >= 1)
            {
                sudoku.easyGameMode = true;

                sudoku.AddTokens(-1);

                tokens.text = "Tokens: " + sudoku.GetTokens().ToString();

                Suggest();
            }
            else
            {
                rewardAds.ShowAds();

                sudoku.easyGameMode = true;

                Suggest();
            }
        }
    }

    public void GetToken(int tokens)
    {
        sudoku.AddTokens(tokens);

        this.tokens.text = "Tokens: " + sudoku.GetTokens().ToString();
    }

    private void Suggest()
    {
        if (sudoku.easyGameMode)
        {
            for (int indexX = 0; indexX < 9; indexX++)
            {
                for (int indexY = 0; indexY < 9; indexY++)
                {
                    List<int> notSuggested = new List<int>();

                    foreach ((int, int) position in sudoku.GetBlock(indexX, indexY))
                    {
                        if (!notSuggested.Contains(sudoku.sudoku[position.Item1][position.Item2]))
                        {
                            notSuggested.Add(sudoku.sudoku[position.Item1][position.Item2]);
                        }
                    }

                    foreach ((int, int) position in sudoku.GetLine(indexX, indexY))
                    {
                        if (!notSuggested.Contains(sudoku.sudoku[position.Item1][position.Item2]))
                        {
                            notSuggested.Add(sudoku.sudoku[position.Item1][position.Item2]);
                        }
                    }

                    foreach ((int, int) position in sudoku.GetColumn(indexX, indexY))
                    {
                        if (!notSuggested.Contains(sudoku.sudoku[position.Item1][position.Item2]))
                        {
                            notSuggested.Add(sudoku.sudoku[position.Item1][position.Item2]);
                        }
                    }

                    if (sudoku.sudoku[indexX][indexY] == -1)
                    {
                        for (int index = 0; index < 9; index++)
                        {
                            if (!notSuggested.Contains(index + 1))
                            {
                                if (!sudokuTiles[indexX][indexY].suggestionText[index].gameObject.activeSelf)
                                {
                                    sudokuTiles[indexX][indexY].suggestionText[index].gameObject.SetActive(true);
                                }
                            }
                            else
                            {
                                if (sudokuTiles[indexX][indexY].suggestionText[index].gameObject.activeSelf)
                                {
                                    sudokuTiles[indexX][indexY].suggestionText[index].gameObject.SetActive(false);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int index = 0; index < 9; index++)
                        {
                            if (sudokuTiles[indexX][indexY].suggestionText[index].gameObject.activeSelf)
                            {
                                sudokuTiles[indexX][indexY].suggestionText[index].gameObject.SetActive(false);
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Quit");

        SaveSystem.SaveGame(sudoku);
    }

    public void CheckValidity(int indexX)
    {
        for (int indexY = 0; indexY < 9; indexY++)
        {
            sudokuTiles[indexX][indexY].SetError(false);

            if (sudoku.sudoku[indexX][indexY] != -1)
            {
                if (sudoku.CheckLineValidity(indexX, indexY).Count > 1)
                {
                    foreach ((int, int) tile in sudoku.CheckLineValidity(indexX, indexY))
                    {
                        if (sudokuTiles[tile.Item1][tile.Item2].GetError() == false)
                        {
                            sudokuTiles[tile.Item1][tile.Item2].SetError(true);
                        }
                    }
                }

                if (sudoku.CheckColumnValidity(indexX, indexY).Count > 1)
                {
                    foreach ((int, int) tile in sudoku.CheckColumnValidity(indexX, indexY))
                    {
                        if (sudokuTiles[tile.Item1][tile.Item2].GetError() == false)
                        {
                            sudokuTiles[tile.Item1][tile.Item2].SetError(true);
                        }
                    }
                }

                if (sudoku.CheckBlockValidity(indexX, indexY).Count > 1)
                {
                    foreach ((int, int) tile in sudoku.CheckBlockValidity(indexX, indexY))
                    {
                        if (sudokuTiles[tile.Item1][tile.Item2].GetError() == false)
                        {
                            sudokuTiles[tile.Item1][tile.Item2].SetError(true);
                        }
                    }
                }
            }
        }
    }

    public void KeyboardButton(int button)
    {
        int posX = selectedTile.GetPosX();
        int posY = selectedTile.GetPosY();

        if (selectedTile.GetIsChangeable())
        {
            sudoku.sudoku[posX][posY] = button;

            sudoku.undoSudoku.Add(((posX, posY), button));

            selectedTile.SetValue(button);

            Suggest();

            for (int indexX = 0; indexX < 9; indexX++)
            {
                for (int indexY = 0; indexY < 9; indexY++)
                {
                    if (sudoku.sudoku[indexX][indexY] == sudoku.sudoku[posX][posY] && sudoku.sudoku[posX][posY] != -1)
                    {
                        sudokuTiles[indexX][indexY].SetHighlighted(2);
                    }
                    else
                    {
                        sudokuTiles[indexX][indexY].SetHighlighted(0);
                    }
                }
            }

            for (int indexX = 0; indexX < 9; indexX++)
            {
                CheckValidity(indexX);
            }

            bool boardIsComplete = true;

            foreach (List<int> row in sudoku.sudoku)
            {
                if (row.Contains(-1))
                {
                    boardIsComplete = false;
                }
            }

            if (boardIsComplete)
            {
                bool gameIsOver = true;

                for (int indexX = 0; indexX < 9; indexX++)
                {
                    for (int indexY = 0; indexY < 9; indexY++)
                    {
                        if (sudokuTiles[indexX][indexY].GetError())
                        {
                            gameIsOver = false;
                        }
                    }
                }

                Debug.Log(gameIsOver);

                if (gameIsOver)
                {
                    gameOver.SetActive(true);
                }
            }
        }
    }

    public void Undo()
    {
        List<((int, int), int)> auxUndo = sudoku.undoSudoku.FindAll(x => x.Item1.Item1 == sudoku.undoSudoku.ElementAt(sudoku.undoSudoku.Count - 1).Item1.Item1 && x.Item1.Item2 == sudoku.undoSudoku.ElementAt(sudoku.undoSudoku.Count - 1).Item1.Item2);

        if (auxUndo.Count > 1)
        {
            ((int, int), int) position = auxUndo.ElementAt(auxUndo.Count - 2);

            sudoku.sudoku[position.Item1.Item1][position.Item1.Item2] = position.Item2;

            sudokuTiles[position.Item1.Item1][position.Item1.Item2].SetValue(position.Item2);

            for (int indexX = 0; indexX < 9; indexX++)
            {
                CheckValidity(indexX);
            }

            sudoku.undoSudoku.Remove(auxUndo[auxUndo.Count - 1]);

            SelectTile(position.Item1.Item1, position.Item1.Item2);
        }
        else 
        if (auxUndo.Count == 1)
        {
            ((int, int), int) position = auxUndo.ElementAt(auxUndo.Count - 1);

            sudoku.sudoku[position.Item1.Item1][position.Item1.Item2] = -1;

            sudokuTiles[position.Item1.Item1][position.Item1.Item2].SetValue(-1);

            for (int indexX = 0; indexX < 9; indexX++)
            {
                CheckValidity(indexX);
            }

            SelectTile(position.Item1.Item1, position.Item1.Item2);

            sudoku.undoSudoku.Remove(auxUndo[auxUndo.Count - 1]);
        }

        Suggest();
    }

    private void SelectTile(int posX, int posY)
    {
        if (selectedTile != null)
        {
            selectedTile.SetSelected(false);
        }

        for (int indexX = 0; indexX < 9; indexX++)
        {
            for (int indexY = 0; indexY < 9; indexY++)
            {
                if (sudoku.sudoku[indexX][indexY] == sudoku.sudoku[posX][posY] && sudoku.sudoku[posX][posY] != -1)
                {
                    sudokuTiles[indexX][indexY].SetHighlighted(2);
                }
                else
                {
                    sudokuTiles[indexX][indexY].SetHighlighted(0);
                }
            }
        }

        selectedTile = sudokuTiles[posX][posY];

        selectedTile.SetSelected(true);

        foreach ((int, int) tile in sudoku.GetLine(posX, posY))
        {
            sudokuTiles[tile.Item1][tile.Item2].SetHighlighted(1);
        }

        foreach ((int, int) tile in sudoku.GetColumn(posX, posY))
        {
            sudokuTiles[tile.Item1][tile.Item2].SetHighlighted(1);
        }

        foreach ((int, int) tile in sudoku.GetBlock(posX, posY))
        {
            sudokuTiles[tile.Item1][tile.Item2].SetHighlighted(1);
        }

        sudokuTiles[posX][posY].SetHighlighted(2);
    }

    public void NewGame()
    {
        sudoku = new SudokuMatrix(0);

        selectedTile = null;

        sudokuTiles = new List<List<Tile>>();

        CancelInvoke("TimerTick");

        InvokeRepeating("TimerTick", 1f, 1f);

        if (sudoku.GetGameType() == 0)
        {   
            for (int indexX = 0; indexX < 9; indexX++)
            {
                sudokuTiles.Add(new List<Tile>());

                for (int indexY = 0; indexY < 9; indexY++)
                {
                    bool isChangeable = false;

                    if (sudoku.initialSudoku[indexX][indexY] == -1)
                    {
                        isChangeable = true;
                    }

                    Tile tile = new Tile(indexX, indexY, sudoku.sudoku[indexX][indexY], isChangeable, texture, font, canvas);

                    sudokuTiles[indexX].Add(tile);
                }
            }
        }

        timer.text = sudoku.timer.GetTimer();
    }

    private void Tap()
    {
        if (Input.touchCount > 0)
        {
            UnityEngine.Touch touch = Input.GetTouch(0);

            if (gameOver.activeSelf)
            {
                NewGame();

                gameOver.SetActive(false);
            }
            else
            {
                for (int indexX = 0; indexX < 9; indexX++)
                {
                    for (int indexY = 0; indexY < 9; indexY++)
                    {
                        if (sudokuTiles[indexX][indexY].tileObject.transform.position.x + (tileSizeX / 2) > touch.position.x &&
                            sudokuTiles[indexX][indexY].tileObject.transform.position.x - (tileSizeX / 2) < touch.position.x &&
                            sudokuTiles[indexX][indexY].tileObject.transform.position.y + (tileSizeY / 2) > touch.position.y &&
                            sudokuTiles[indexX][indexY].tileObject.transform.position.y - (tileSizeY / 2) < touch.position.y)
                        {
                            SelectTile(indexX, indexY);

                            break;
                        }
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Tap();
    }

    private void AddTokens(int tokens)
    {
        sudoku.AddTokens(tokens);

        this.tokens.text = "Tokens: " + sudoku.GetTokens().ToString();
    }
}
