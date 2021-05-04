using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SmoothTilemap
{

    [Serializable]
    public class TileConditionSet
    {
#pragma warning disable 649
        [SerializeField] TileSpriteType m_tileSpriteType;
        [SerializeField] TileConditionPair[] m_tileConditionTypes = CreateDefaultTileConditionPairs();
#pragma warning restore 649

        private static TileConditionPair[] CreateDefaultTileConditionPairs()
        {
            var list = new List<TileConditionPair>();
            foreach(var dir in (TileDirectionType[])Enum.GetValues(typeof(TileDirectionType)))
            {
                var tileCondPair = new TileConditionPair();
                tileCondPair.TileDirectionType = dir;
                list.Add(tileCondPair);
            }
            return list.ToArray();
        }

        public TileSpriteType TileSpriteType
        {
            set => m_tileSpriteType = value;
            get => m_tileSpriteType;
        }
        public TileConditionType this[TileDirectionType dir]
        {
            set
            {
                var td = m_tileConditionTypes.FirstOrDefault((e) => e.TileDirectionType == dir);
                Debug.Assert(td != null, $"{dir}");
                if(td != null)
                {
                    td.TileConditionType = value;
                }
            }
            get
            {
                var td = m_tileConditionTypes.FirstOrDefault((e) => e.TileDirectionType == dir);
                Debug.Assert(td != null, $"{dir}");
                return td.TileConditionType;
            }
        }
    }
}