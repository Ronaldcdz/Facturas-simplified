namespace Facturas_simplified.Utils
{

  public record Result<T>
  {
    public bool IsSuccess { get; init; }
    public T Value { get; init; }
    public string ErrorMessage { get; init; }

    public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };
    public static Result<T> Failure(string errorMessage) => new() { IsSuccess = false, ErrorMessage = errorMessage };
  }
}

