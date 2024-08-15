using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ridd.NonogramEngine
{
    public static class NonogramGlobals
    {
        /// <summary>
        /// Array of valid dimensions for nonograms, as supported by this engine.
        /// If you are extending this engine and wish to have different sized nonograms, add them here.
        /// Dimensions follow a (# of Columns, # of Rows) format
        /// </summary>
        public static Vector2Int[] ValidDimensions =
        {
            new Vector2Int(5,5),
            new Vector2Int(10,10),
            new Vector2Int(15,15),
            new Vector2Int(20,15)
        };

        /// <summary>
        /// Checks whether or not a given set of dimensions for the nonograms is supported.
        /// Overload that doesn't require making a vector2Int beforehand
        /// </summary>
        /// <param name="numColumns">The number of columns in the nonogram</param>
        /// <param name="numRows">The number of rows in the nonogram</param>
        /// <returns>True if the dimensions are valid, false if not</returns>
        public static bool CheckDimensionsValid(int numColumns, int numRows)
        {
            return CheckDimensionsValid(new Vector2Int(numColumns, numRows));
        }

        /// <summary>
        /// Checks whether or not a given set of dimensions for the nonograms is supported.
        /// </summary>
        /// <param name="dimensions">A vector2Int of (Column, Row) dimensions.</param>
        /// <returns>True if the dimensions are valid, false if not</returns>
        public static bool CheckDimensionsValid(Vector2Int dimensions)
        {
            return (new List<Vector2Int>(ValidDimensions).Contains(dimensions));
        }

    }
}
