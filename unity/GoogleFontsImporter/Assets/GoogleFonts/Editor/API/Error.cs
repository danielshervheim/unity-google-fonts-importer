namespace DSS.GoogleFonts.API
{
    [System.Serializable]
    public class Error
    {
        // Non-zero if an error occured during the API call.
        public int code;

        // Describes the error that occured (if one occured at all).
        public string message;
    }
}
