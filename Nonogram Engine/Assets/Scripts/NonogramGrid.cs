using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Ridd.NonogramEngine
{
    public class NonogramGrid : MonoBehaviour
    {
        [SerializeField] private NonogramTile m_tilePrefab;
        [SerializeField] private HorizontalLayoutGroup m_horizonalHintGroupPrefab;
        [SerializeField] private VerticalLayoutGroup m_verticalHintGroupPrefab;
        [SerializeField] private NonogramHintTile m_hintTilePrefab;
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
                    if (row == 0) 
                    {
                        m_nonogramTilesByColumn[column] = new NonogramTile[m_rowCount];
                    }

                    GameObject prefabInstance = InstantiatePrefab(m_tilePrefab.gameObject, m_gridLayoutGroup.transform, $"Tile {column},{row}");
                    if (prefabInstance == null)
                    {
                        Debug.LogError("Cannot put together grid, the tile prefab returned a null instance when instantiated!");
                    }
                    NonogramTile tileInstance = prefabInstance.GetComponent<NonogramTile>();
                    m_nonogramTilesByRow[row][column] = tileInstance;
                    m_nonogramTilesByColumn[column][row] = tileInstance;
                    m_nonogramTilesByRow[row][column].SetInGrid(this, new Vector2Int(column, row));
                }
            }

            // Now, we need to set up the hints
            // Horizontal hints go on all the 0 column tiles as children, so they move and grow with them
            m_rowHintTiles = new NonogramHintTile[m_rowCount][];
            for (int i = 0; i < m_rowCount; i++)
            {
                NonogramTile parentTile = m_nonogramTilesByRow[i][0];
                GameObject horizontalLayoutGroup = InstantiatePrefab(m_horizonalHintGroupPrefab.gameObject, parentTile.transform, $"Row {i} Hint Group");
                RectTransform horLGTransform = (RectTransform)horizontalLayoutGroup.transform;
                horLGTransform.sizeDelta = new Vector2(horLGTransform.sizeDelta.x, 0);
                horLGTransform.anchoredPosition = Vector2.zero;
                // Now make the hints
                m_rowHintTiles[i] = new NonogramHintTile[m_rowHintCount];
                for (int hintI = 0; hintI < m_rowHintCount; hintI++)
                {
                    GameObject hintObject = InstantiatePrefab(m_hintTilePrefab.gameObject, horizontalLayoutGroup.transform, $"Hint {i},{hintI}");
                    NonogramHintTile hintTileInstance = hintObject.GetComponent<NonogramHintTile>();
                    Debug.Log(m_rowHintTiles[i] != null);
                    m_rowHintTiles[i][hintI] = hintTileInstance;
                    // TODO: Assign the data to hint tiles
                }
            }

            m_columnHintTiles = new NonogramHintTile[m_columnCount][];
            for (int i = 0; i < m_columnCount; i++)
            {
                NonogramTile parentTile = m_nonogramTilesByColumn[i][0];
                GameObject verticalLayoutGroup = InstantiatePrefab(m_verticalHintGroupPrefab.gameObject, parentTile.transform, $"Row {i} Hint Group");
                RectTransform vertLGTransform = (RectTransform)verticalLayoutGroup.transform;
                vertLGTransform.sizeDelta = new Vector2(0, vertLGTransform.sizeDelta.y);
                vertLGTransform.anchoredPosition = Vector2.zero;
                m_columnHintTiles[i] = new NonogramHintTile[m_columnHintCount];
                for (int hintI = 0; hintI < m_columnHintCount; hintI++)
                {
                    GameObject hintObject = InstantiatePrefab(m_hintTilePrefab.gameObject, verticalLayoutGroup.transform, $"Hint {i},{hintI}");
                    NonogramHintTile hintTileInstance = hintObject.GetComponent<NonogramHintTile>();
                    m_columnHintTiles[i][hintI] = hintTileInstance;
                    // TODO: Assign the data to hint tiles
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
