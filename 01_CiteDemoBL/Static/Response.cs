namespace CiteDemoBL.Static
{
    public class Response<T>
    {
        public T? Data { get; set; }
        public ErrorCodes StatusCode { get; set; }
        public string? Description { get; set; }
    }
}
