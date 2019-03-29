namespace issuu.Client
{

    public class IssuuResult<T> where T : IIssuuData
    {
        public IssuuResult()
        {

        }

        public IssuuResult(T doc)
        {
            this.Document = doc;
        }

        public T Document { get; set; }
    }

}
