using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SmoothTilemap
{

    [Serializable]
    public class TileConditionPair
    {
#pragma warning disable 649
        [SerializeField] TileDirectionType m_tileDirectionType;
        [SerializeField] TileConditionType m_tileConditionType;
#pragma warning restore 649
        public TileDirectionType TileDirectionType
        {
            set => m_tileDirectionType = value;
            get => m_tileDirectionType;
        }
        public TileConditionType TileConditionType
        {
            set => m_tileConditionType = value;
            get => m_tileConditionType;
        }
    }
}