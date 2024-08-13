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
                    if (Application.isPlaying)
                    {
                        GameObject nonogramTileRoot = Instantiate(m_tilePrefab, m_gridLayoutGroup.transform);
                        m_nonogramTiles[x][y] = nonogramTileRoot.GetComponent<NonogramTile>();
                    } else
                    {
#if UNITY_EDITOR
                        GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(m_tilePrefab as GameObject);
                        prefabInstance.transform.SetParent(m_gridLayoutGroup.transform);
                        prefabInstance.transform.localScale = Vector3.one;
                        NonogramTile tileInstance = prefabInstance.GetComponent<NonogramTile>();
                        m_nonogramTiles[x][y] = tileInstance;
#else
                        Debug.LogError("Tried to make the grid in editor mode, despite not being in the editor!");
                        return;
#endif
                    }
                    if (m_nonogramTiles[x][y] == null)
                    {
                        Debug.LogError($"Prefab {m_tilePrefab} did not have a retrievable nonogram tile component!");
                        return;
                    }
                    m_nonogramTiles[x][y].SetInGrid(this, new Vector2Int(x, y));
                }
            }
            m_gridInstantiated = true;
        }

        [ContextMenu("Test")]
        public void Test()
        {
            CreateGrid();
        }


    }
}
