using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ridd.NonogramEngine
{
    public class NonogramTile : MonoBehaviour
    {

        [SerializeField] private Image m_tileBackgroundImage;
        [SerializeField] private Image m_filledImage;
        [SerializeField] private Image m_crossedImage;

        private Vector2Int m_id; // row, column format

        private bool m_shouldBeFilled = false;
        private Color m_color;

        public void SetData(NonogramSquare squareData, char column, char row)
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

        public Vector2Int GetID()
        {
            return m_id;
        }

    }
}
