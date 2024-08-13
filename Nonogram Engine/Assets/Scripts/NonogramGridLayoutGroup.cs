using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

// Heavily based on the tutorial made by GameDevGuide, found at https://www.youtube.com/watch?v=CGsEJToeXmA

namespace Ridd.NonogramEngine
{
    public class NonogramGridLayoutGroup : LayoutGroup
    {

        [SerializeField] private int m_columnCount = 1;
        [SerializeField] private int m_rowCount = 1;
        [SerializeField] private Vector2 m_spacing;

        private float m_tileSize = 0;

        public void SetDimensions(int rows, int columns)
        {
            m_rowCount = rows;
            m_columnCount = columns;
        }

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;

            float maxWidth = parentWidth / (float) m_columnCount - ((m_spacing.x / (float)m_columnCount) * (m_columnCount - 1))
                - (padding.left / (float) m_columnCount) - (padding.right / (float)m_columnCount);

            float maxHeight = parentHeight / (float) m_rowCount - ((m_spacing.y / (float) m_rowCount) * (m_rowCount - 1))
                - (padding.top / (float) m_rowCount) - (padding.bottom / (float) m_rowCount);

            // We want a square, since this is nonograms, so we'll make the dimensions the smaller of the two.
            m_tileSize = Mathf.Min(maxWidth, maxHeight);

            for (int i = 0; i < rectChildren.Count; i++)
            {
                RectTransform tileRect = rectChildren[i];

                float columnMultiplier = i % m_columnCount;
                float rowMultiplier = i / m_rowCount;

                float xPos = (m_tileSize * columnMultiplier) + (m_spacing.x * columnMultiplier) + padding.left;
                float yPos = (m_tileSize * rowMultiplier) + (m_spacing.y * rowMultiplier) + padding.top;

                SetChildAlongAxis(tileRect, 0, xPos, m_tileSize);
                SetChildAlongAxis(tileRect, 1, yPos, m_tileSize);
                tileRect.localPosition = new Vector3(tileRect.localPosition.x, tileRect.localPosition.y, 0); // Zero out the Z axis
            }

        }

        public override void CalculateLayoutInputVertical()
        {
            //Stubbed
        }

        public override void SetLayoutHorizontal()
        {
            //Stubbed
        }

        public override void SetLayoutVertical()
        {
            //Stubbed
        }

    }
}
