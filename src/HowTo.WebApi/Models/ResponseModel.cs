namespace HowTo.WebApi.Models
{
    public class ResponseModel<T>
    {
        public ResponseModel(bool success)
        {
            Success = success;
        }

        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }

        public T? Data { get; set; }
    }
}
