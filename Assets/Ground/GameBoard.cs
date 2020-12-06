using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard
{
    private int turns, length;
    private int[,] board;
    private System.Random random = new System.Random();

    public const int PATH = 0, FULL_OBSTACLE = 1, JUMP_OBSTACLE = 2, SLIDE_OBSTACLE = 3, COLLECTIBLE = 4,
        NOTHING = 5, LONG_FULL_OBSTACLE = 6, LONG_JUMP_OBSTACLE = 7, COVERED = 8;
    public const int LEFT = 6, RIGHT = 7, UP = 8;

    public GameBoard (int turns, int length) {
        this.turns = turns;
        this.length = length;

        board = new int[3, length];

        Debug.Log(board.Length);
        Debug.Log(board.GetLength(0));

       for (int i=0; i<3; i++) {
 //           Debug.Log(i);
  //          board[i, 0] = PATH;
    //        board[i, length - 1] = PATH;
        }

        for (int i=0; i<3; i++) {
            for (int j = 0; j < length; j++)
                board[i, j] = NOTHING;
        }
        AssignBoard();
        AssignJumpAndSlide();
        AssignCollectibles();
    }

    public int[,] GetBoard() {
        return board;
    }

    public void AssignBoard() {
        int x = random.Next(0, 3), y = 1;
        board[x, y] = PATH;
        while (y < length - 1) {
            int dir = GetRandomValidDir(x, y);
            switch (dir) {
                case LEFT:
                    board[x - 1, y] = PATH;
                    PutUpObstacle(x, y);
                    x = x - 1;
                    break;
                case RIGHT:
                    board[x + 1, y] = PATH;
                    PutUpObstacle(x, y);
                    x = x + 1;
                    break;
                case UP:
                    board[x, y + 1] = PATH;
                    y = y + 1;
                    break;
            }
        }
    }

    public void AssignJumpAndSlide() {
        for (int i=0; i<3; i++) {
            for (int j=1; j<length -1; j++) {
                if ((board[i, j+1] == PATH || board[i, j+1] == NOTHING) &&
                    (board[i, j- 1] == PATH || board[i, j - 1] == NOTHING)) {
                    switch (random.Next(0, 4)) {
                        case 0:
                        case 1:
                            break;
                        case 2:
                            if (random.Next(0, 2) < 1)
                                board[i, j] = JUMP_OBSTACLE;
                            else
                                board[i, j] = LONG_JUMP_OBSTACLE;
                            break;
                        case 3:
                            board[i, j] = SLIDE_OBSTACLE;
                            if (board[i, j+1] == NOTHING || board[i, j+1] == PATH) {
                                board[i, j + 1] = COVERED;
                            }
                            break;
                    }
                }
            }
        }
    }

    public void AssignCollectibles() {
        for (int i = 0; i < 3; i++) {
            for (int j = 1; j < length - 1; j++) {
                if (board[i, j] == PATH || board[i, j] == NOTHING) {
                    switch (random.Next(0, 4)) {
                        case 0:
                        case 1:
                            board[i, j] = COLLECTIBLE;
                            break;
                    }
                
                }
            }
        }
    }

    public int GetRandomValidDir(int x, int y) {
        List<int> validDirs = new List<int>();
        if (x > 0) { if (board[x - 1, y] == NOTHING) validDirs.Add(LEFT); }
        if (x < 2) { if (board[x + 1, y] == NOTHING) validDirs.Add(RIGHT); }
        if (board[x, y + 1] == NOTHING) validDirs.Add(UP);
        if (validDirs.Count > 0) return validDirs[random.Next(0, validDirs.Count)];
        else return UP;
    }

    public void PutUpObstacle(int x, int y) {
        Boolean assigned = false;
        if (y + 2 < length) {
            if (y + 3 < length) {
                if (random.Next(0, 10) < 5) {
                    board[x, y + 1] = COVERED;
                    board[x, y + 2] = LONG_FULL_OBSTACLE;
                    board[x, y + 3] = COVERED;
                    assigned = true;
                }

            }

            if (!assigned) {
                int a = random.Next(0, 2);
                if (a == 0) board[x, y + 1] = FULL_OBSTACLE;
                else board[x, y + 2] = FULL_OBSTACLE;
            }
        }
        else {
            board[x, y + 1] = FULL_OBSTACLE;
        }
       
    }


}
