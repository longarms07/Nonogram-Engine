using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ridd.NonogramEngine
{
    [System.Serializable]
    public class NonogramHintTile : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_numberDisplay;
        [SerializeField] private Image m_backingImage;
        [Tooltip("Parent object of any extra bells and whisltes that need to disappear when a hint box is hidden. " +
            "Should not be the base game object for the hint tile.")]
        [SerializeField] private GameObject m_extraContentToShowOrHide;

        private int m_number;
        private bool m_active = true;

        public int GetNumber() { return m_number; }
        public void SetNumber(int number) 
        { 
            m_number = number;
            m_numberDisplay.text = number.ToString();
        }

        public void SetActive(bool active)
        {
            Color backingImageColor = m_backingImage.color;
            backingImageColor.a = active ? 1f : 0f;
            m_backingImage.color = m_backingImage.color;
            m_numberDisplay.gameObject.SetActive(active);
            m_extraContentToShowOrHide?.SetActive(active);
            m_active = active;
        }

        public bool GetActive() { return m_active; }


    }
}
