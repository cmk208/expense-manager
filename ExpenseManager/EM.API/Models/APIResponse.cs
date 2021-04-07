namespace EM.API.Models
{
    public class APIResponse<T>
    {
        public int StatusCode { get; set; }
        public string StatusRemark { get; set; }
        public T Content { get; set; }
    }
}