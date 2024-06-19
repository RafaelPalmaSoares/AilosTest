namespace Questao5.Domain.BaseEntity
{
    public class BaseResponse<T>
    {
        public T? Data { get; set; }
        public string? ErrorMessage { get;  set; }
        public bool Success { get;  set; }
        public int StatusCode { get; set; }

        public void AddData(T data)
            => Data = data;

        public void AddError(string errorMessage)
            => ErrorMessage = errorMessage;

        public void SetStatusCode(int statusCode)
            => StatusCode = statusCode;
    }
}
