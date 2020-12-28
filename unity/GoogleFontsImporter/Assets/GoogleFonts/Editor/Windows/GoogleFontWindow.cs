// Copyright © Daniel Shervheim, 2020
// www.danielshervheim.com

using UnityEditor;
using UnityEngine;

using GoogleFont = DSS.GoogleFonts.API.GoogleFont;

namespace DSS.GoogleFonts.Windows
{
    public class GoogleFontWindow : EditorWindow
    {
        GoogleFont font = null;

        bool showVariants = false;
        bool showSubsets = false;

        Vector2 scrollPos = Vector2.zero;


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

            EditorGUILayout.BeginVertical();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            {
                GUILayout.Label(font.family, EditorStyles.boldLabel);
                EditorGUILayout.Space();

                GUILayout.Label("Category: " + font.category);
                EditorGUILayout.Space();

                showVariants = EditorGUILayout.Foldout(showVariants, "Variants");
                if (showVariants)
                {
                    EditorGUI.indentLevel++;
                    foreach (string variant in font.variants)
                    {
                        GUILayout.Label(variant);
                    }
                    EditorGUI.indentLevel--;
                }


                showSubsets = EditorGUILayout.Foldout(showSubsets, "Subsets");
                if (showSubsets)
                {
                    EditorGUI.indentLevel++;
                    foreach (string subset in font.subsets)
                    {
                        GUILayout.Label(subset);
                    }
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
    }
}
