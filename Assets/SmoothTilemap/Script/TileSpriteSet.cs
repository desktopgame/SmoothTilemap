using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using SmoothTilemap.System;

namespace SmoothTilemap
{

    [Serializable]
    public class TileSpriteSet
    {
#pragma warning disable 649
        [SerializeField] [EnumNamedArray(typeof(TileSpriteType))] Sprite[] m_sprites = new Sprite[Enum.GetValues(typeof(TileSpriteType)).Length];
#pragma warning restore 649
        public Sprite this[TileSpriteType type]
        {
            set => m_sprites[(int)type] = value;
            get => m_sprites[(int)type];
        }
    }
}