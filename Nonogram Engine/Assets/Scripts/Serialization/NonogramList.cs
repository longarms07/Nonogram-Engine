using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonogramList : ScriptableObject
{
    [SerializeField]
    private List<NonogramData> m_nonograms;

    public void SetNonogramData(List<NonogramData> nonogramData)
    {
        m_nonograms = nonogramData;
    }

}
