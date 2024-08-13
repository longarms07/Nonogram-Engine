using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ridd.NonogramEngine
{
    public static class NonogramGlobals
    {
        public static Vector2Int[] ValidDimensions =
        {
            new Vector2Int(5,5),
            new Vector2Int(10,10),
            new Vector2Int(15,15),
            new Vector2Int(15,20)
        };

        public static bool CheckDimensionsValid(Vector2Int dimensions)
        {
            return (new List<Vector2Int>(ValidDimensions).Contains(dimensions));
        }

    }
}
