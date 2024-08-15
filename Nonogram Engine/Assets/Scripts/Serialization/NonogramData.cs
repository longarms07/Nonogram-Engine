using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ridd.NonogramEngine
{
    [System.Serializable]
    public class NonogramData
    {
        [SerializeField] private string name;
        [SerializeField] private string completed;
        [SerializeField] private NonogramRow[] rows;
        private Vector2Int dimensions = Vector2Int.zero;
        private bool passedValidityCheck;


        public string GetName() { return name; }
        public string GetCompletedFilename() { return completed; }
        public NonogramRow[] GetRows() { return rows; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(name) && rows.Length > 0;
        }

        public Vector2Int GetDimensions()
        {
            if (dimensions == Vector2Int.zero && rows.Length > 0)
            {
                dimensions = new Vector2Int(rows[0].GetRow().Length, rows.Length);
            }
            return dimensions;
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
}
