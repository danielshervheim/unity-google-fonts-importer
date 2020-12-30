// Copyright © Daniel Shervheim, 2020
// www.danielshervheim.com

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

using EditorCoroutineUtility = Unity.EditorCoroutines.Editor.EditorCoroutineUtility;

namespace DSS.GoogleFonts
{
    public class GoogleFontWindow : EditorWindow
    {
        // The font associated with this window.
        GoogleFont font = null;

        // The scroll position within the window.
        Vector2 scrollPos = Vector2.zero;

        // Returns a new window for the given font asset.
        public static void Window(GoogleFont font)
        {

            GoogleFontWindow window = (GoogleFontWindow)EditorWindow.GetWindow(typeof(GoogleFontWindow), true, font.family);
            window.Setup(font);
        }

        void Setup(GoogleFont font)
        {
            this.font = font;
        }

        void OnGUI()
        {
            if (font == null)
            {
                return;
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            {
                GUILayout.Label(font.family, EditorStyles.boldLabel);
                EditorGUILayout.Space();

                Dictionary<string, string> styles = font.GetStyles();

                // Show download buttons for each specific style.
                foreach (string style in styles.Keys)
                {
                    if (GUILayout.Button(style))
                    {
                        string path = EditorUtility.SaveFilePanelInProject("Download " + font.family + " (" + style + ")", font.family + " (" + style + ").ttf", "ttf", "Save font style as...");
                        if (path.Length != 0)
                        {
                            EditorCoroutineUtility.StartCoroutine(DownloadFontStyle(styles[style], path), this);
                        }
                    }
                }

                // And show a download all button, to download all styles at once.
                EditorGUILayout.Space();
                if (GUILayout.Button("Download All Styles"))
                {
                    string path = EditorUtility.SaveFolderPanel("Save \"" + font.family + "\" to folder", "", "");
                    if (path.Length != 0)
                    {
                        EditorCoroutineUtility.StartCoroutine(DownloadFont(path), this);
                    }
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        // Downloads the specified font style (from the given url) to the given
        // file path.
        IEnumerator DownloadFontStyle(string url, string filePath)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            if(www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                System.IO.File.WriteAllBytes(filePath, www.downloadHandler.data);
                AssetDatabase.Refresh();
            }
        }

        // Downloads all font styles to the given folder.
        IEnumerator DownloadFont(string folderPath)
        {
            Dictionary<string, string> styles = font.GetStyles();
            foreach (string style in styles.Keys)
            {
                string url = styles[style];
                string path = folderPath + System.IO.Path.DirectorySeparatorChar + font.family + " (" + style + ").ttf";
                yield return DownloadFontStyle(url, path);
            }
        }
    }
}
