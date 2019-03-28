namespace issuu_dotnet
{
    public enum IssuuDocumentStates
    {
        /// <summary>
        /// Active documents
        /// </summary>
        A = 1,

        /// <summary>
        /// Documents that failed during conversion
        /// </summary>
        F = 2,

        /// <summary>
        /// Documents that are currently being processed
        /// </summary>
        P = 3

    }

}
