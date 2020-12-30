// Copyright © Daniel Shervheim, 2020
// www.danielshervheim.com

using System.Collections.Generic;
using UnityEngine;

namespace DSS.GoogleFonts
{
    [System.Serializable]
    public class GoogleFont
    {
        // Holds the potentially available styles for this font.
        // If a style is not available, it will be null.
        [System.Serializable]
        public class Styles
        {
            public string regular100;
            public string regular200;
            public string regular300;
            public string regular400;
            public string regular500;
            public string regular600;
            public string regular700;
            public string regular800;
            public string regular900;
            public string italic100;
            public string italic200;
            public string italic300;
            public string italic400;
            public string italic500;
            public string italic600;
            public string italic700;
            public string italic800;
            public string italic900;
        }

        // The font name.
        public string family;

        // The font category.
        public string category;

        // The styles available for this font. Note that you should call the
        // GetStyles() method rather than accessing this directly, since it weeds
        // out null styles.
        public Styles styles;

        Dictionary<string, string> stylesDict = null;

        // Returns a dictionary of the urls of available styles, indexed by the
        // style name.
        public Dictionary<string, string> GetStyles()
        {
            if (stylesDict == null)
            {
                stylesDict = new Dictionary<string, string>();
                AddStyleToDictionary("Regular 100", styles.regular100, stylesDict);
                AddStyleToDictionary("Regular 200", styles.regular200, stylesDict);
                AddStyleToDictionary("Regular 300", styles.regular300, stylesDict);
                AddStyleToDictionary("Regular 400", styles.regular400, stylesDict);
                AddStyleToDictionary("Regular 500", styles.regular500, stylesDict);
                AddStyleToDictionary("Regular 600", styles.regular600, stylesDict);
                AddStyleToDictionary("Regular 700", styles.regular700, stylesDict);
                AddStyleToDictionary("Regular 800", styles.regular800, stylesDict);
                AddStyleToDictionary("Regular 900", styles.regular900, stylesDict);
                AddStyleToDictionary("Italic 100", styles.italic100, stylesDict);
                AddStyleToDictionary("Italic 200", styles.italic200, stylesDict);
                AddStyleToDictionary("Italic 300", styles.italic300, stylesDict);
                AddStyleToDictionary("Italic 400", styles.italic400, stylesDict);
                AddStyleToDictionary("Italic 500", styles.italic500, stylesDict);
                AddStyleToDictionary("Italic 600", styles.italic600, stylesDict);
                AddStyleToDictionary("Italic 700", styles.italic700, stylesDict);
                AddStyleToDictionary("Italic 800", styles.italic800, stylesDict);
                AddStyleToDictionary("Italic 900", styles.italic900, stylesDict);
            }
            return stylesDict;
        }

        // Helper to add styles to the styles dictionary.
        void AddStyleToDictionary(string key, string value, Dictionary<string, string> dict)
        {
            if (value != null && !value.Equals(string.Empty))
            {
                dict[key] = value;
            }
        }
    }
}
