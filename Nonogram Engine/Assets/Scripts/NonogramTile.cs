using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ridd.NonogramEngine
{
    public class NonogramTile : MonoBehaviour
    {
        [SerializeField] private Image m_backgroundImage;
        [SerializeField] private Image m_tileImage;
        [SerializeField] private Sprite m_emptySprite;
        [SerializeField] private Sprite m_filledSprite;
        [SerializeField] private Sprite m_crossedSprite;

        private Vector2Int m_id; // row, column format
        private NonogramGrid m_parentGrid;

        private bool m_shouldBeFilled = false;
        private Color m_color;

        public void SetData(NonogramSquare squareData)
        {
            // Stubbed
        }

        public bool ShouldBeFilled() { return m_shouldBeFilled; }
        public bool MarkedCorrectly()
        {
            //stubbed
            return false;
        }

        public void ApplyColors()
        {
            //Stubbed
        }

        public void SetInGrid(NonogramGrid grid, Vector2Int id)
        {
            m_parentGrid = grid;
            m_id = id;
        }

        public Vector2Int GetID()
        {
            return m_id;
        }

    }
}
