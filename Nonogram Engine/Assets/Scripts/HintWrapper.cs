using Ridd.NonogramEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HintWrapper
{
    [SerializeField] private NonogramTile[] m_nonogramTiles;
    [SerializeField] private NonogramHintTile[] m_hintTiles;

    public HintWrapper(NonogramTile[] nonogramTiles, NonogramHintTile[] hintTiles)
    {
        m_nonogramTiles = nonogramTiles;
        m_hintTiles = hintTiles;
    }
}
