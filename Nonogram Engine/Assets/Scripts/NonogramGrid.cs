using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ridd.NonogramEngine
{
    public class NonogramGrid : MonoBehaviour
    {
        [SerializeField] private NonogramTile m_tilePrefab;
        [SerializeField] private GridLayoutGroup m_gridLayoutGroup;
        [SerializeField] private Vector2Int m_dimensions;

        [SerializeField] private NonogramTile[][] m_nonogramTiles;

        private bool m_gridInstantiated = false;

        public void CreateGrid()
        {
            if (!NonogramGlobals.CheckDimensionsValid(m_dimensions))
            {
                Debug.Log($"Cannot create grid, invalid dimensions of {m_dimensions.x} x {m_dimensions.y}");
                return;
            }

            m_gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            m_gridLayoutGroup.constraintCount = m_dimensions.y;

            m_nonogramTiles = new NonogramTile[m_dimensions.x][];

            for (int x = 0; x < m_dimensions.x; x++)
            {
                m_nonogramTiles[x] = new NonogramTile[m_dimensions.y];
                for (int y = 0; y < m_dimensions.y; y++)
                {
                    m_nonogramTiles[x][y] = Instantiate(m_tilePrefab, m_gridLayoutGroup.transform);
                    m_nonogramTiles[x][y].SetInGrid(this, new Vector2Int(x, y));
                }
            }
        }

        [ContextMenu("Test")]
        public void Test()
        {
            CreateGrid();
        }


    }
}
