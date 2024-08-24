using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
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

        [SerializeField] private NonogramTile[] m_nonogramTiles;
        [SerializeField] private NonogramHintTile[][] m_columnHintTiles;
        [SerializeField] private NonogramHintTile[][] m_rowHintTiles;

        [Header("Test Fields, remove later")]
        [SerializeField] private bool m_debugMode = false;
        [SerializeField] private NonogramList m_testList;

        [SerializeField, HideInInspector] private bool m_gridInstantiated = false;
        [SerializeField, HideInInspector] private int m_columnHintCount;
        [SerializeField, HideInInspector] private int m_rowHintCount;
        private NonogramData m_loadedNonogram = null;
        private bool m_nonogramLoaded = false;
        

        private void Start()
        {
            Debug.Log($"Did column hint count save? {m_columnHintCount}");
            if (m_debugMode)
            {
                SetNonogram(m_testList.GetNonogramData()[0]);
            }
        }

        public void TileStateChanged(NonogramTile changedTile)
        {
            // Stubbed
        }

        public void SetNonogram(NonogramData nonogramData)
        {
            if (!ValidateNonogramData(nonogramData)) return;
            NonogramRow[] dataRows = nonogramData.GetRows();
            
            // Copy the data rows to tiles

            // Update Hint tiles

            m_loadedNonogram = nonogramData;
            m_nonogramLoaded = true;
        }

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
            int totalTiles = m_rowCount * m_columnCount;
            m_nonogramTiles = new NonogramTile[totalTiles];
            for (int i = 0; i < totalTiles; i++)
            {
                Vector2Int tilePoint = GetTilePoint(i);
                GameObject prefabInstance = InstantiatePrefab(m_tilePrefab.gameObject, m_gridLayoutGroup.transform, $"Tile {tilePoint.x},{tilePoint.y}");
                if (prefabInstance == null)
                {
                    Debug.LogError("Cannot put together grid, the tile prefab returned a null instance when instantiated!");
                }
                NonogramTile tileInstance = prefabInstance.GetComponent<NonogramTile>();
                tileInstance.SetInGrid(this, tilePoint);
                m_nonogramTiles[i] = tileInstance;
                
            }

            // Now, we need to set up the hints
            // Horizontal hints go on all the 0 column tiles as children, so they move and grow with them
            /*m_rowHintTiles = new NonogramHintTile[m_rowCount][];
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
                    //Debug.Log(m_rowHintTiles[i] != null);
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
            }*/

            m_gridInstantiated = true;
        }

        private Vector2Int GetTilePoint(int locationInArray)
        {
            int row = locationInArray % m_rowCount;
            int column = locationInArray / m_rowCount;
            return new Vector2Int(column, row);
        }

        private int GetIndexFromTilePoint(Vector2Int point)
        {
            return (m_rowCount * point.x) + point.y;
        }

        private bool ValidateNonogramData(NonogramData nonogramData)
        {
            StringBuilder errorBuilder = new StringBuilder();
            bool passed = true;
            if (nonogramData.GetDimensions() != new Vector2Int(m_columnCount, m_rowCount))
            {
                passed = false;
                errorBuilder.AppendLine($"Nonogram Data {nonogramData.GetName()} has different dimensions than the grid! " +
                    $"Data is {nonogramData.GetDimensions().x} by {nonogramData.GetDimensions().y}" +
                    $" when the grid is {m_columnCount} by {m_rowCount}");
            }
            if (!passed) Debug.LogError(errorBuilder.ToString());
            return passed;
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
            Debug.Log("Testing that the tile point to index algorithm works...");
            for (int i = 0; i < m_nonogramTiles.Length; i++)
            {
                int retrievedIndex = GetIndexFromTilePoint(m_nonogramTiles[i].GetID());
                Debug.Assert(retrievedIndex == i,
                    $"Failure! Index is {i}, tile is {m_nonogramTiles[i].name}, got index {retrievedIndex}");
            }
            Debug.Log("Done testing tile point to index algorithm works");
#if UNITY_EDITOR
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
        }


    }
}
