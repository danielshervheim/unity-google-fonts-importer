using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace DSS.GoogleFonts.API
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
            // TODO: modify the JSON as needed here.
            string[] stringsToRename = {
                "regular",
                "italic",

                "100",
                "200",
                "300",
                "500",
                "600",
                "700",
                "800",
                "900",

                "100italic",
                "300italic",
                "500italic",
                "700italic",
                "800italic",
                "900italic"
            };

            // Rename each string in the original json.
            foreach (string toRename in stringsToRename)
            {
                json = json.Replace(toRename, "f_" + toRename);
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
}
