using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ridd.NonogramEngine
{
    [System.Serializable]
    public class NonogramTile : MonoBehaviour
    {

        public enum FillState
        {
            FILLED,
            EMPTY,
            CROSSED
        }

        [SerializeField] private Image m_backgroundImage;
        [SerializeField] private Image m_tileImage;
        [SerializeField] private Sprite m_emptySprite;
        [SerializeField] private Sprite m_filledSprite;
        [SerializeField] private Sprite m_crossedSprite;

        [SerializeField, HideInInspector] private Vector2Int m_id; // column, row format
        private NonogramGrid m_parentGrid;

        private bool m_shouldBeFilled = false;
        private FillState m_currentState = FillState.EMPTY;
        private Color m_color;
        

        public void SetData(NonogramSquare squareData)
        {
            m_shouldBeFilled = squareData.IsFilled();
        }

        public bool ShouldBeFilled() { return m_shouldBeFilled; }
        public bool MarkedCorrectly()
        {
            if (m_shouldBeFilled && m_currentState == FillState.FILLED) return true;
            else if (!m_shouldBeFilled && m_currentState != FillState.FILLED) return true;
            return false;
        }

        public void MarkFilled()
        {
            Mark(FillState.FILLED);
        }

        public void MarkCrossed()
        {
            Mark(FillState.CROSSED);
        }

        public void MarkEmpty()
        {
            Mark(FillState.EMPTY);
        }

        public void MarkCorrectly()
        {
            FillState correctState = m_shouldBeFilled ? FillState.FILLED : FillState.CROSSED;
            Mark(correctState);
        }

        private void Mark(FillState mark)
        {
            m_currentState = mark;
            Sprite stateSprite;
            switch (m_currentState)
            {
                case FillState.FILLED: 
                    stateSprite = m_filledSprite;
                    break;
                case FillState.CROSSED:
                    stateSprite = m_crossedSprite;
                    break;
                default:
                    stateSprite = m_emptySprite;
                    break;
            }
            m_tileImage.sprite = stateSprite;
            m_parentGrid.TileStateChanged(this);
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
