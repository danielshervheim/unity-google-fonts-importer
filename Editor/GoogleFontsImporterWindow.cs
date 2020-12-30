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
    public class GoogleFontsImporterWindow : EditorWindow
    {
        // Delegate to draw contextual GUI.
        delegate void GUI();
        GUI drawGUI;

        // Error message to display if an error occured.
        string errorMessage = "";

        // API call response object.
        ApiResponse response = null;

        // The search term.
        string search = "";

        // The category index.
        int index = 0;

        // The ccroll position in the font list.
        Vector2 scrollPos = Vector2.zero;

        [MenuItem("Google Fonts/Import")]
        static void Window()
        {
            GoogleFontsImporterWindow window = (GoogleFontsImporterWindow)EditorWindow.GetWindow(typeof(GoogleFontsImporterWindow), false, "Google Fonts Importer");
        }

        void OnEnable()
        {
            drawGUI = ()=>{};

            // Try and load the API key from the keyfile.
            TextAsset keyFile = Resources.Load<TextAsset>("GoogleFontsApiKey");
            if (keyFile == null)
            {
                errorMessage = "API key not found in\nResources/GoogleFontsApiKey.txt";
                drawGUI = Error;
            }
            else
            {
                EditorCoroutineUtility.StartCoroutine(MakeApiCall(keyFile.text), this);
            }
        }

        IEnumerator MakeApiCall(string key)
        {
            drawGUI = Loading;

            UnityWebRequest www = UnityWebRequest.Get("https://www.googleapis.com/webfonts/v1/webfonts?key=" + key);
            yield return www.SendWebRequest();

            if(www.isNetworkError || www.isHttpError)
            {
                errorMessage = "UnityWebRequest failed (" + www.responseCode + ")";
                drawGUI = Error;
            }
            else
            {
                try
                {
                    response = ApiResponse.FromJson(www.downloadHandler.text);
                    if (response.error.code != 0)
                    {
                        errorMessage = response.error.message;
                        drawGUI = Error;
                    }
                    else
                    {
                        drawGUI = Success;
                    }
                }
                catch(System.Exception e)
                {
                    errorMessage = "Json deserialization failed \n" + e.Message;
                    drawGUI = Error;
                }
            }
        }

        // GUI functions.

        void OnGUI()
        {
            drawGUI();
        }

        void Error()
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
        }

        void Loading()
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Loading...", MessageType.Info);
        }

        void Success()
        {
            EditorGUILayout.Space();

            // Search box.
            search = EditorGUILayout.TextField("Search", search);
            EditorGUILayout.Space();

            // Category.
            index = EditorGUILayout.Popup(index, response.GetCategories(true));
            EditorGUILayout.Space();

            // Display list of fonts.
            EditorGUILayout.BeginVertical();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            {
                List<GoogleFont> fonts = response.GetFontsBySearch(search, response.GetCategories()[index]);
                foreach (GoogleFont font in fonts)
                {
                    // If they clicked on this font, spawn a new window to show
                    // the individual styles available.
                    if (GUILayout.Button(font.family))
                    {
                        GoogleFontWindow.Window(font);
                        EditorGUILayout.Space();
                    }
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
    }
}
