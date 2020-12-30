using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace DSS.GoogleFonts
{
    // Holds the results of deserializing the API's json response.
    [System.Serializable]
    public class Response
    {
        const string ALL = "all";

        public Error error;
        public List<GoogleFont> items;

        List<string> categories = new List<string>();
        List<string> categoriesHumanReadable = new List<string>();

        public static Response FromJson(string json)
        {
            Dictionary<string, string> replacements = new Dictionary<string, string>()
            {
                ["files"] = "styles",

                ["regular"] = "regular400",
                ["100"] = "regular100",
                ["200"] = "regular200",
                ["300"] = "regular300",
                ["500"] = "regular500",
                ["600"] = "regular600",
                ["700"] = "regular700",
                ["800"] = "regular800",
                ["900"] = "regular900",

                ["italic"] = "italic400",
                ["100italic"] = "italic100",
                ["200italic"] = "italic200",
                ["300italic"] = "italic300",
                ["500italic"] = "italic500",
                ["600italic"] = "italic600",
                ["700italic"] = "italic700",
                ["800italic"] = "italic800",
                ["900italic"] = "italic900"
            };

            // Rename each string in the original json.
            foreach (string toRename in replacements.Keys)
            {
                json = json.Replace("\"" + toRename + "\"", "\"" + replacements[toRename] + "\"");
            }

            Response response = JsonUtility.FromJson<Response>(json);
            response.ConstructCategoriesList();

            return response;
        }

        void ConstructCategoriesList()
        {
            categories = new List<string>();
            categoriesHumanReadable = new List<string>();

            foreach (GoogleFont font in items)
            {
                if (!categories.Contains(font.category))
                {
                    categories.Add(font.category);
                }
            }
            categories.Sort();
            categories.Insert(0, ALL);

            // Modify the categories list to be more display-friendly.
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            foreach (string category in categories)
            {
                categoriesHumanReadable.Add(
                    textInfo.ToTitleCase(category.Replace("-", " "))
                );
            }
        }

        public string[] GetCategories(bool humanReadable = false)
        {
            if (humanReadable)
            {
                return categoriesHumanReadable.ToArray();
            }
            else
            {
                return categories.ToArray();
            }
        }

        public List<GoogleFont> GetFonts()
        {
            return items;
        }

        public List<GoogleFont> GetFontsByCategory(string category)
        {
            if (category.Equals(ALL))
            {
                return GetFonts();
            }
            else
            {
                List<GoogleFont> lst = new List<GoogleFont>();
                foreach (GoogleFont font in items)
                {
                    if (font.category.Equals(category))
                    {
                        lst.Add(font);
                    }
                }
                return lst;
            }
        }

        public List<GoogleFont> GetFontsBySearch(string searchTerm, string category = ALL)
        {
            List<GoogleFont> lst = new List<GoogleFont>();
            foreach (GoogleFont font in GetFontsByCategory(category))
            {
                if (font.family.Contains(searchTerm) || searchTerm.Equals(string.Empty))
                {
                    lst.Add(font);
                }
            }
            return lst;
        }
    }

    [System.Serializable]
    public class GoogleFont
    {
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

        public string family;
        public string category;
        public Styles styles;

        public Dictionary<string, string> GetStyles()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            // Regular styles.
            if (styles.regular100 != null && !styles.regular100.Equals(string.Empty))
            {
                dict["Regular 100"] = styles.regular100;
            }
            if (styles.regular200 != null && !styles.regular200.Equals(string.Empty))
            {
                dict["Regular 200"] = styles.regular200;
            }
            if (styles.regular300 != null && !styles.regular300.Equals(string.Empty))
            {
                dict["Regular 300"] = styles.regular300;
            }
            if (styles.regular400 != null && !styles.regular400.Equals(string.Empty))
            {
                dict["Regular 400"] = styles.regular400;
            }
            if (styles.regular500 != null && !styles.regular500.Equals(string.Empty))
            {
                dict["Regular 500"] = styles.regular500;
            }
            if (styles.regular600 != null && !styles.regular600.Equals(string.Empty))
            {
                dict["Regular 600"] = styles.regular600;
            }
            if (styles.regular700 != null && !styles.regular700.Equals(string.Empty))
            {
                dict["Regular 700"] = styles.regular700;
            }
            if (styles.regular800 != null && !styles.regular800.Equals(string.Empty))
            {
                dict["Regular 800"] = styles.regular800;
            }
            if (styles.regular900 != null && !styles.regular900.Equals(string.Empty))
            {
                dict["Regular 900"] = styles.regular900;
            }

            // Italic styles.
            if (styles.italic100 != null && !styles.italic100.Equals(string.Empty))
            {
                dict["Italic 100"] = styles.italic100;
            }
            if (styles.italic200 != null && !styles.italic200.Equals(string.Empty))
            {
                dict["Italic 200"] = styles.italic200;
            }
            if (styles.italic300 != null && !styles.italic300.Equals(string.Empty))
            {
                dict["Italic 300"] = styles.italic300;
            }
            if (styles.italic400 != null && !styles.italic400.Equals(string.Empty))
            {
                dict["Italic 400"] = styles.italic400;
            }
            if (styles.italic500 != null && !styles.italic500.Equals(string.Empty))
            {
                dict["Italic 500"] = styles.italic500;
            }
            if (styles.italic600 != null && !styles.italic600.Equals(string.Empty))
            {
                dict["Italic 600"] = styles.italic600;
            }
            if (styles.italic700 != null && !styles.italic700.Equals(string.Empty))
            {
                dict["Italic 700"] = styles.italic700;
            }
            if (styles.italic800 != null && !styles.italic800.Equals(string.Empty))
            {
                dict["Italic 800"] = styles.italic800;
            }
            if (styles.italic900 != null && !styles.italic900.Equals(string.Empty))
            {
                dict["Italic 900"] = styles.italic900;
            }

            return dict;
        }
    }

    [System.Serializable]
    public class Error
    {
        // Non-zero if an error occured during the API call.
        public int code;

        // Describes the error that occured (if one occured at all).
        public string message;
    }
}
