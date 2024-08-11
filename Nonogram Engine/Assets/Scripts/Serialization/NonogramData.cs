using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NonogramData
{
    [SerializeField] private string name;
    [SerializeField] private string completed;
    [SerializeField] private NonogramRow[] rows;

    public string GetName() { return name; }
    public string GetCompletedFilename (){ return completed; }
    public NonogramRow[] GetRows() { return rows; }

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(name) && rows.Length > 0;
    }
}

[System.Serializable]
public class NonogramRow
{
    [SerializeField] private NonogramSquare[] row;
    public NonogramSquare[] GetRow() { return row; }
}

[System.Serializable]
public class NonogramSquare
{
    [SerializeField] private bool filled;
    [SerializeField] private string color;

    public bool IsFilled() { return filled; }
    public string GetColor() { return color; }
}
