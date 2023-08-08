using System;
using System.Collections.Generic;

public static class SudokuSolver
{
    public static bool isSafe(List<List<int>> board, int row, int col, int num)
    {
        for (int d = 0; d < board.Count; d++)
        {
            if (board[row][d] == num)
            {
                return false;
            }
        }

        for (int r = 0; r < board.Count; r++)
        {
            if (board[r][col] == num)
            {
                return false;
            }
        }

        int sqrt = (int)Math.Sqrt(board.Count);
        int boxRowStart = row - row % sqrt;
        int boxColStart = col - col % sqrt;

        for (int r = boxRowStart; r < boxRowStart + sqrt; r++)
        {
            for (int d = boxColStart;
                 d < boxColStart + sqrt; d++)
            {
                if (board[r][d] == num)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static bool solveSudoku(List<List<int>> board, int n)
    {
        int row = -2;
        int col = -2;
        bool isEmpty = true;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (board[i][j] == -1)
                {
                    row = i;
                    col = j;

                    isEmpty = false;
                    break;
                }
            }
            if (!isEmpty)
            {
                break;
            }
        }

        if (isEmpty)
        {
            return true;
        }

        for (int num = 1; num <= n; num++)
        {
            if (isSafe(board, row, col, num))
            {
                board[row][col] = num;

                if (solveSudoku(board, n))
                {
                    return true;
                }
                else
                {
                    board[row][col] = -1;
                }
            }
        }
        return false;
    }
}
