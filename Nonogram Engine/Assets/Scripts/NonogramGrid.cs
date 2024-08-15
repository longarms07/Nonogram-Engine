using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Ridd.NonogramEngine
{
    public class NonogramGrid : MonoBehaviour
    {
        [SerializeField] private GameObject m_tilePrefab;
        [SerializeField] private GameObject m_hintTilePrefab;
        [SerializeField] private NonogramGridLayoutGroup m_gridLayoutGroup;
        [SerializeField] private int m_rowCount;
        [SerializeField] private int m_columnCount;

        [SerializeField] private NonogramTile[][] m_nonogramTilesByRow;
        [SerializeField] private NonogramTile[][] m_nonogramTilesByColumn;
        [SerializeField] private NonogramHintTile[][] m_columnHintTiles;
        [SerializeField] private NonogramHintTile[][] m_rowHintTiles;

        private bool m_gridInstantiated = false;
        private int m_columnHintCount;
        private int m_rowHintCount;

        public void CreateGrid()
        {
            if (!NonogramGlobals.CheckDimensionsValid(new Vector2Int(m_rowCount, m_columnCount)))
            {
                Debug.Log($"Cannot create grid, invalid dimensions of {m_rowCount} x {m_columnCount}");
                return;
            }

            // Column hints are the number of rows / 2 + 1, and rows use number of columns
            m_columnHintCount = m_rowCount / 2 + 1;
            m_rowHintCount = m_columnCount / 2 + 1;

            m_gridLayoutGroup.SetDimensions(m_rowCount, m_columnCount);
            m_nonogramTilesByRow = new NonogramTile[m_rowCount][];
            m_nonogramTilesByColumn = new NonogramTile[m_columnCount][];

            for (int row = 0; row < m_rowCount; row++)
            {
                m_nonogramTilesByRow[row] = new NonogramTile[m_columnCount];
                for (int column = 0; column < m_columnCount; column++)
                {
                    if (row == 0) // If this is the first row, we need to make the coulmn array for the first time
                    {
                        m_nonogramTilesByColumn[column] = new NonogramTile[m_rowCount];
                    }

                    GameObject prefabInstance = InstantiatePrefab(m_tilePrefab, m_gridLayoutGroup.transform, $"Tile {column},{row}");
                    if (prefabInstance == null)
                    {
                        Debug.LogError("Cannot put together grid, the tile prefab returned a null instance when instantiated!");
                    }
                    NonogramTile tileInstance = prefabInstance.GetComponent<NonogramTile>();
                    m_nonogramTilesByRow[row][column] = tileInstance;
                    m_nonogramTilesByRow[row][column].SetInGrid(this, new Vector2Int(column, row));
                }
            }
            m_gridInstantiated = true;
        }

        private GameObject InstantiatePrefab(GameObject prefab, Transform parent, string instanceName = "")
        {
            if (Application.isPlaying)
            {
                GameObject instance = Instantiate(prefab, parent);
                if (!string.IsNullOrEmpty(instanceName)) instance.name = instanceName;
                return instance;
            }
            else
            {
#if UNITY_EDITOR
                GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab as GameObject);
                prefabInstance.transform.SetParent(parent);
                prefabInstance.transform.localScale = Vector3.one;
                if (!string.IsNullOrEmpty(instanceName)) prefabInstance.name = instanceName;
                return prefabInstance;
#else
                Debug.LogError("Tried to instantiate a prefab in editor mode, despite not being in the editor!");
                return null;
#endif
            }
        }

        [ContextMenu("Test")]
        public void Test()
        {
            CreateGrid();
        }


    }
}
