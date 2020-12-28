using System.Collections.Generic;

namespace DSS.GoogleFonts.API
{
    [System.Serializable]
    public class GoogleFont
    {
        [System.Serializable]
        public class Files
        {
            public string f_regular;
            public string f_italic;

            public string f_100;
            public string f_200;
            public string f_300;
            public string f_500;
            public string f_600;
            public string f_700;
            public string f_800;
            public string f_900;

            public string f_100italic;
            public string f_300italic;
            public string f_500italic;
            public string f_700italic;
            public string f_800italic;
            public string f_900italic;
        }

        public string family;
        public List<string> variants;
        public List<string> subsets;
        public string version;
        public string lastModified;
        public Files files;
        public string category;
        public string kind;
    }
}
