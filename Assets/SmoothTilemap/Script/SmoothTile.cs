using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SmoothTilemap
{

    public class SmoothTile : Tile
    {

#pragma warning disable 649
        [SerializeField] TileSpriteSet m_tileSpriteSet;
        [SerializeField] TileConditionSet[] m_tileConditionSets = CreateDefaultTileConditionSets();
#pragma warning restore 649
        public TileSpriteSet TileSpriteSet => m_tileSpriteSet;
        public TileConditionSet[] TileConditionSets => m_tileConditionSets;

        private static TileConditionSet[] CreateDefaultTileConditionSets()
        {
            var list = new List<TileConditionSet>();
            foreach(var a in (TileSpriteType[])Enum.GetValues(typeof(TileSpriteType)))
            {
                var tileCondSet = new TileConditionSet();
                tileCondSet.TileSpriteType = a;
                list.Add(tileCondSet);
            }
            return list.ToArray();
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            bool ret = base.StartUp(position, tilemap, go);
            return ret;
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            tileData.sprite = m_tileSpriteSet[TileSpriteType.Fill];
            foreach(var cond in m_tileConditionSets)
            {
                if(MatchTileCondition(cond, position, tilemap))
                {
                    tileData.sprite = m_tileSpriteSet[cond.TileSpriteType];
                    break;
                }
            }
        }

        private bool MatchTileCondition(TileConditionSet set, Vector3Int position, ITilemap tilemap)
        {
            foreach(var dir in (TileDirectionType[])Enum.GetValues(typeof(TileDirectionType)))
            {
                TileConditionType condType = set[dir];
                if(condType == TileConditionType.Optional)
                {
                    continue;
                }
                var newPos = position;
                var offset = dir.ToVector2();
                newPos.x += offset.x;
                newPos.y += offset.y;
                var tile = tilemap.GetTile(newPos);
                if(!((tile == null && condType == TileConditionType.Missing) ||
                    (tile != null && condType == TileConditionType.Exist)))
                {
                    return false;
                }
            }
            return true;
        }

        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
        {
            base.RefreshTile(position, tilemap);
            foreach(var dir in (TileDirectionType[])Enum.GetValues(typeof(TileDirectionType)))
            {
                var v3 = Vector3Int.zero;
                var vDir = dir.ToVector2();
                v3.x = vDir.x;
                v3.y = vDir.y;
                TryRefreshTile(position + v3, tilemap);
            }
        }

        private void TryRefreshTile(Vector3Int position, ITilemap tilemap)
        {
            var tile = tilemap.GetTile(position);
            if(tile == null || !(tile is SmoothTile))
            {
                return;
            }
            tilemap.RefreshTile(position);
        }


#if UNITY_EDITOR
        [MenuItem("Assets/Create/2D/SmoothTile")]
        private static void Create()
        {
            // 選択中のフォルダを簡単に取得する方法がないので...
            var myTile = ScriptableObject.CreateInstance<SmoothTile>();
            var path = AssetDatabase.GenerateUniqueAssetPath("Assets/Application/Tile/SmoothTile.asset");
            AssetDatabase.CreateAsset(myTile, path);
        }
#endif
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(SmoothTile))]
    public class SmoothTileEditor : Editor
    {
        public SmoothTile SmoothTile => target as SmoothTile;

        private string m_file = "TileSpriteTable";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("ファイル名");
                m_file = EditorGUILayout.TextField(m_file);
                EditorGUILayout.EndHorizontal();
            }
            if(GUILayout.Button("Save"))
            {
                SaveMatrix();
            }
            if(GUILayout.Button("Load"))
            {
                LoadMatrix();
            }
            EditorGUILayout.EndVertical();
        }

        private void SaveMatrix()
        {
            var sb = new StringBuilder();
            sb.AppendLine("名前,左上,上,右上,左,中央,右,左下,下,右下");
            foreach(var tileCond in SmoothTile.TileConditionSets)
            {
                var tileSpriteTile = tileCond.TileSpriteType;
                sb.Append(tileSpriteTile.ToString()).Append(",");
                foreach(var dir in (TileDirectionType[])Enum.GetValues(typeof(TileDirectionType)))
                {
                    var tileCondType = tileCond[dir];
                    var tileCondTypeStr = tileCondType switch
                    {
                        TileConditionType.Missing => "M",
                        TileConditionType.Optional => "O",
                        TileConditionType.Exist => "E",
                        _ => "",
                    };
                    sb.Append(tileCondTypeStr).Append(",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.AppendLine();
            }
            File.WriteAllText(GetPath(), sb.ToString());
        }

        private void LoadMatrix()
        {
            var lines = File.ReadAllLines(GetPath());
            //var spriteTypes = (TileSpriteType[])Enum.GetValues(typeof(TileSpriteType));
            foreach(var line in lines.Skip(1))
            {
                var table = line.Split(',');
                var spriteTypeStr = table[0];
                if(!Enum.TryParse<TileSpriteType>(spriteTypeStr, out var spriteType))
                {
                    Debug.LogError($"不明なスプライトタイプです。: {spriteTypeStr}");
                    continue;
                }
                var spriteSet = SmoothTile.TileSpriteSet[spriteType];
                int spriteTypeN = 0;
                foreach(var e in (TileDirectionType[])Enum.GetValues(typeof(TileDirectionType)))
                {
                    int index = spriteTypeN + 1;
                    var omnStr = table[index];
                    var omn = omnStr switch
                    {
                        "O" => TileConditionType.Optional,
                        "M" => TileConditionType.Missing,
                        "E" => TileConditionType.Exist,
                    };
                    var tileCondSet = SmoothTile.TileConditionSets.FirstOrDefault((x) => x.TileSpriteType == spriteType);
                    if(tileCondSet != null)
                    {
                        tileCondSet[e] = omn;
                    }
                    spriteTypeN++;
                }
            }
        }

        private string GetPath()
        {
            string path = m_file;
            if(!path.EndsWith(".csv"))
            {
                path += ".csv";
            }
            return path;
        }
    }
#endif

}