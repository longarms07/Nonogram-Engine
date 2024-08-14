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

        [SerializeField] private NonogramTile[][] m_nonogramTiles;
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
            m_nonogramTiles = new NonogramTile[m_rowCount][];

            for (int x = 0; x < m_rowCount; x++)
            {
                m_nonogramTiles[x] = new NonogramTile[m_columnCount];
                for (int y = 0; y < m_columnCount; y++)
                {
                    GameObject prefabInstance = InstantiatePrefab(m_tilePrefab, m_gridLayoutGroup.transform);
                    if (prefabInstance == null)
                    {
                        Debug.LogError("Cannot put together grid, the tile prefab returned a null instance when instantiated!");
                    }
                    NonogramTile tileInstance = prefabInstance.GetComponent<NonogramTile>();
                    m_nonogramTiles[x][y] = tileInstance;
                    m_nonogramTiles[x][y].SetInGrid(this, new Vector2Int(x, y));
                }
            }
            m_gridInstantiated = true;
        }

        private GameObject InstantiatePrefab(GameObject prefab, Transform parent)
        {
            if (Application.isPlaying)
            {
                return Instantiate(prefab, parent);
            }
            else
            {
#if UNITY_EDITOR
                GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab as GameObject);
                prefabInstance.transform.SetParent(parent);
                prefabInstance.transform.localScale = Vector3.one;
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
