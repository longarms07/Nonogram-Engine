using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ridd.NonogramEngine
{

    public class NonogramList : ScriptableObject
    {
        [SerializeField]
        private List<NonogramData> m_nonograms;

        public List<NonogramData> GetNonogramData() { return m_nonograms; }

        public void SetNonogramData(List<NonogramData> nonogramData)
        {
            m_nonograms = nonogramData;
        }

    }
}
