using UnityEngine;
using System;

public class Cell
{
    public bool hasMine;
    public bool isRevealed;
    public bool isFlagged;
    public int adjacentMines;

    public event Action OnRevealed;

    public override string ToString() {
        if (hasMine)
            return "M";
        return adjacentMines.ToString();
    }

    //v podstatě ToString pro výpis buňky tak jak je ve hře, neboli jestli je skrytá apod.
    public string ToStringForGame()
    {
        if(isFlagged)
            return "F";
        if(!isRevealed) 
            return "H";
        if(hasMine)
            return "M";
        return adjacentMines.ToString();
    }

    public void Reveal()
    {
        isFlagged = false;
        isRevealed = true;
        OnRevealed?.Invoke();
    }
}
