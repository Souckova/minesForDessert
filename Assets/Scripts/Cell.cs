using UnityEngine;

public class Cell
{
    public bool hasMine;
    public bool isRevealed;
    public bool isFlagged;
    public int adjacentMines;

    public override string ToString() {
        if (hasMine)
            return "M";
        return adjacentMines.ToString();
    }
}
