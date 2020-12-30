// Copyright © Daniel Shervheim, 2020
// www.danielshervheim.com

using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace DSS.GoogleFonts
{
    // Holds the objects returned from the API call.
    [System.Serializable]
    public class ApiResponse
    {
        // Holds the error information related to the API call.
        [System.Serializable]
        public class ApiError
        {
            // Non-zero if an error occured during the API call.
            public int code;

            // Describes the error that occured (if one occured at all).
            public string message;
        }

        const string ALL = "all";

        // The error information related to the API call. The "error.code" is 0
        // if no error occured during the API call.
        public ApiError error;

        // The list of GoogleFont objects returned.
        public List<GoogleFont> items;

        // The categories of all the fonts. This must be constructed explicitly.
        List<string> categories = new List<string>();
        List<string> categoriesHumanReadable = new List<string>();

        // Constructs a new ApiResponse object from the json returned by the API.
        public static ApiResponse FromJson(string json)
        {
            // Certain keywords in the original Json must be renamed because they
            // are not valid C# variable names, so we must swap them in the json
            // text before deserializing.
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
            foreach (string toRename in replacements.Keys)
            {
                // We must append and prepend the " character, otherwise the
                // replacement would incorrectly replace "300italic" with "regular300italic".
                json = json.Replace("\"" + toRename + "\"", "\"" + replacements[toRename] + "\"");
            }

            // Deserialize the Json into an ApiResponse object, abd explicitly
            // construct the categories list.
            ApiResponse response = JsonUtility.FromJson<ApiResponse>(json);
            response.ConstructCategoriesList();

            return response;
        }

        void ConstructCategoriesList()
        {
            categories = new List<string>();
            categoriesHumanReadable = new List<string>();

            // Find list of unique categories, sort it, and insert "ALL".
            foreach (GoogleFont font in items)
            {
                if (!categories.Contains(font.category))
                {
                    categories.Add(font.category);
                }
            }
            categories.Sort();
            categories.Insert(0, ALL);

            // Also construct a human-readable list from the found categories.
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            foreach (string category in categories)
            {
                categoriesHumanReadable.Add(
                    textInfo.ToTitleCase(category.Replace("-", " "))
                );
            }
        }

        // Returns the categories of fonts.
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

        // Returns all the fonts.
        public List<GoogleFont> GetFonts()
        {
            return items;
        }

        // Returns all the fonts in the given category.
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

        // Returns all the fonts with the given substring, and in the given category.
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
