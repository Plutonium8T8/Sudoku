using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using System.Threading;

[System.Serializable]
public class SudokuMatrix
{
    private int gameType;

    public List<List<int>> initialSudoku = new List<List<int>>();

    public List<List<int>> sudoku = new List<List<int>>(); 

    public List<((int, int), int)> undoSudoku = new List<((int, int), int)>();

    public Timer timer;

    public bool easyGameMode;

    private int tokens;

    public int GetGameType()
    {
        return gameType;
    }

    public void AddTokens(int tokens)
    {
        this.tokens += tokens;
    }

    public int GetTokens()
    {
        return tokens;
    }

    private void InstantiateSudoku()
    {
        if (gameType == 0)
        {
            for (int index1 = 0; index1 < 9; index1++)
            {
                sudoku.Add(new List<int>());

                initialSudoku.Add(new List<int>());

                for (int index2 = 0; index2 < 9; index2++)
                {
                    sudoku[index1].Add(-1);

                    initialSudoku[index1].Add(-1);
                }
            }
        }
    }

    private void GenerateSudoku()
    {
        if (gameType == 0)
        {
            for (int index1 = 0; index1 < 3; index1++)
            {
                for (int index2 = 0; index2 < 3; index2++)
                {
                    for (int index3 = 0; index3 < 1; index3++)
                    {
                        int posX = UnityEngine.Random.Range(0, 3);
                        int posY = UnityEngine.Random.Range(0, 3);

                        int randomNumber = UnityEngine.Random.Range(1, 10);

                        sudoku[index1 * 3 + posX][index2 * 3 + posY] = randomNumber;

                        while ( CheckColumnValidity(index1 * 3 + posX, index2 * 3 + posY).Count > 1 ||
                               CheckLineValidity(index1 * 3 + posX, index2 * 3 + posY).Count > 1 ||
                               CheckBlockValidity(index1 * 3 + posX, index2 * 3 + posY).Count > 1 )
                        {
                            randomNumber = UnityEngine.Random.Range(1, 10);

                            sudoku[index1 * 3 + posX][index2 * 3 + posY] = randomNumber;
                        }
                    }
                }
            }

            SolveSudoku();

            for (int index1 = 0; index1 < 3; index1++)
            {
                for (int index2 = 0; index2 < 3; index2++)
                {
                    for (int index3 = 0; index3 < 6; index3++)
                    {
                        int posX = UnityEngine.Random.Range(0, 3);
                        int posY = UnityEngine.Random.Range(0, 3);

                        while (sudoku[index1 * 3 + posX][index2 * 3 + posY] == -1)
                        {
                            posX = UnityEngine.Random.Range(0, 3);
                            posY = UnityEngine.Random.Range(0, 3);
                        }

                        sudoku[index1 * 3 + posX][index2 * 3 + posY] = -1;
                    }
                }
            }

            initialSudoku = sudoku;
        }
    }

    private void SolveSudoku()
    {
        Debug.Log(SudokuSolver.solveSudoku(sudoku, 9));
    }

    public List<(int, int)> CheckLineValidity(int posX, int posY)
    {
        List<(int, int)> repeatCount = new List<(int, int)>();

        for (int index = 0; index < 9; index++)
        {
            if (sudoku[posX][posY] == sudoku[index][posY])
            {
                repeatCount.Add((index, posY));
            }
        }

        return repeatCount;
    }
    public List<(int, int)> CheckColumnValidity(int posX, int posY)
    {
        List<(int, int)> repeatCount = new List<(int, int)>();

        for (int index = 0; index < 9; index++)
        {
            if (sudoku[posX][posY] == sudoku[posX][index])
            {
                repeatCount.Add((posX, index));
            }
        }

        return repeatCount;
    }
    public List<(int, int)> CheckBlockValidity(int posX, int posY)
    {
        List<(int, int)> repeatCount = new List<(int, int)>();

        int posBX = posX / 3;
        int posBY = posY / 3;

        for (int indexX = posBX * 3; indexX < (posBX + 1) * 3; indexX++)
        {
            for (int indexY = posBY * 3; indexY < (posBY + 1) * 3; indexY++)
            {
                if (sudoku[posX][posY] == sudoku[indexX][indexY])
                {
                    repeatCount.Add((indexX, indexY));
                }
            }
        }

        return repeatCount;
    }

    public List<(int, int)> GetLine(int posX, int posY)
    {
        List<(int, int)> repeatCount = new List<(int, int)>();

        for (int index = 0; index < 9; index++)
        {
            repeatCount.Add((index, posY));
        }

        return repeatCount;
    }

    public List<(int, int)> GetColumn(int posX, int posY)
    {
        List<(int, int)> repeatCount = new List<(int, int)>();

        for (int index = 0; index < 9; index++)
        {
            repeatCount.Add((posX, index));
        }

        return repeatCount;
    }

    public List<(int, int)> GetBlock(int posX, int posY)
    {
        List<(int, int)> repeatCount = new List<(int, int)>();

        int posBX = posX / 3;
        int posBY = posY / 3;

        for (int indexX = posBX * 3; indexX < (posBX + 1) * 3; indexX++)
        {
            for (int indexY = posBY * 3; indexY < (posBY + 1) * 3; indexY++)
            {
                repeatCount.Add((indexX, indexY));
            }
        }

        return repeatCount;
    }

    public void NewGame()
    {
        sudoku = new List<List<int>>();

        initialSudoku = new List<List<int>>();

        undoSudoku = new List<((int, int), int)>();

        InstantiateSudoku();

        GenerateSudoku();

        timer = new Timer();

        easyGameMode = false;
    }

    public SudokuMatrix(int gameType)
    {
        tokens = 0;

        this.gameType = gameType;

        sudoku = new List<List<int>>();

        initialSudoku = new List<List<int>>();

        undoSudoku = new List<((int, int), int)>();

        InstantiateSudoku();

        GenerateSudoku();

        timer = new Timer();

        easyGameMode = false;
    }

    public SudokuMatrix(SudokuMatrix sudoku)
    {
        this.sudoku = sudoku.sudoku;
        this.initialSudoku = sudoku.initialSudoku;
        this.undoSudoku = sudoku.undoSudoku;
        this.gameType = sudoku.gameType;
        this.timer = sudoku.timer;
        this.tokens = sudoku.tokens;
        this.easyGameMode = sudoku.easyGameMode;
    }
}
