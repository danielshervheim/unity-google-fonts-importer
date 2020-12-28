// Copyright © Daniel Shervheim, 2020
// www.danielshervheim.com

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

using Response = DSS.GoogleFonts.API.Response;
using GoogleFont = DSS.GoogleFonts.API.GoogleFont;

namespace DSS.GoogleFonts.Windows
{
    public class GoogleFontsImporterWindow : EditorWindow
    {
        // Web handler to perform the API call.
        UnityWebRequest www = null;

        // API call response object.
        Response response = null;

        // Search term.
        string search = "";

        // Category index.
        int index = 0;

        // Scroll position in list.
        Vector2 scrollPos = Vector2.zero;

        [MenuItem("Google Fonts/Import")]
        static void Window()
        {
            GoogleFontsImporterWindow window = (GoogleFontsImporterWindow)EditorWindow.GetWindow(typeof(GoogleFontsImporterWindow), false, "Google Fonts Importer");
        }

        void OnEnable()
        {
            // Try and load the API key from the keyfile.
            TextAsset keyFile = Resources.Load<TextAsset>("GoogleFontsApiKey");
            if (keyFile == null)
            {
                return;
            }

            // If that succeeded, make the API call.
            www = UnityWebRequest.Get("https://www.googleapis.com/webfonts/v1/webfonts?key=" + keyFile.text);
            www.SendWebRequest();
        }

        void OnGUI()
        {
            EditorGUILayout.Space();

            // Verify that keyfile was found.
            if (www == null)
            {
                EditorGUILayout.HelpBox("API key not found in\nAssets/GoogleFonts/Resources/GoogleFontsApiKey.txt", MessageType.Error);
                return;
            }

            // Wait until web response is received.
            if (!www.isDone)
            {
                EditorGUILayout.HelpBox("Loading API", MessageType.Info);
                return;
            }

            // Display error if the request failed.
            if (www.isNetworkError)
            {
                EditorGUILayout.HelpBox("API loading failed", MessageType.Error);
                return;
            }

            // Deserialize the JSON into a Resposne object, and display an error
            // message if it failed.
            if (response == null)
            {
                response = Response.FromJson(www.downloadHandler.text);
            }
            if (response == null)
            {
                EditorGUILayout.HelpBox("JSON deserialization failed", MessageType.Error);
                return;
            }

            // Verify that the API call itself didn't fail.
            if (response.error.code != 0)
            {
                EditorGUILayout.HelpBox(response.error.message, MessageType.Error);
                return;
            }
            
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
