namespace issuu_dotnet
{
    public class IssuuResult<T> where T : IIssuuData
    {
        public T Document { get; set; }
    }

}
