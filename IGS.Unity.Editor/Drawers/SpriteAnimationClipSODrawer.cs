using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace IGS.Unity.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SpriteAnimationClipSO))]
    internal class SpriteAnimationClipSODrawer : UnityEditor.Editor
    {
        SpriteAnimationClipSO _target;

        const int IMG_SIZE = 75;

        void OnEnable()
        {
            _target = (SpriteAnimationClipSO)target;
        }

        public override void OnInspectorGUI()
        {
            // get the prview texture
            Texture2D texPreview = AssetPreview.GetAssetPreview(_target[0]);

            // set preview texture size
            GUILayout.Label("", GUILayout.Width(IMG_SIZE), GUILayout.Height(IMG_SIZE));

            // draw texture on inspector
            if(texPreview != null)
            {
                texPreview = Texture2D.whiteTexture;
            }

            GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texPreview);

            DrawDefaultInspector();
            DrawClipInfo();
            DrawControlButtons();
        }

        private void DrawClipInfo()
        {
            GUI.enabled = false;
            EditorGUILayout.FloatField("Length", _target.Length);
            GUI.enabled = true;
        }

        private void DrawControlButtons()
        {
            GUILayout.Space(20);

            if(GUILayout.Button("Load Sprites"))
            {
                LoadSprites();
            }
        }

        private void LoadSprites()
        {
            string path = EditorUtility.OpenFolderPanel("Load Sprites", Cache_Folder_Path, "");
            if(string.IsNullOrEmpty(path)) return;
                
            DirectoryInfo dir = new DirectoryInfo(path);
            if(!dir.Exists) return;

            FileInfo[] files = Sprite_Filters.SelectMany(f => dir.GetFiles(f)).ToArray();
            if(files == null || files.Length == 0) return;

            Array.Sort(files, (a, b)=> a.FullName.CompareTo(b.FullName));

            // load assets
            List<Sprite> sprites = new List<Sprite>();

            foreach(var file in files)
            {
                sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(file.FullName));
            }

            // set fields
            _target.GetType().GetField("frames", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_target, sprites.ToArray());

            // save changed
            EditorUtility.SetDirty(_target);
            AssetDatabase.SaveAssets();

            Cache_Folder_Path = path;
        }

        static string Cache_Folder_Path = "";
        static string[] Sprite_Filters = new string[] { "*.png", "*.jpg", "*.jpeg" };
    }
}
