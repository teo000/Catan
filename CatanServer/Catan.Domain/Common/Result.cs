using System.Text.Json.Serialization;

namespace Catan.Domain.Common
{
	public class Result<T>
	{
		[JsonConstructor]
		public Result(bool isSuccess, T value, string error)
		{
			IsSuccess = isSuccess;
			Error = error;
			Value = value;
		}

		public bool IsSuccess { get; private set; }
		public T Value { get; private set; }
		public string Error { get; private set; }

		public static Result<T> Success(T value)
		{
			return new Result<T>(true, value, null!);
		}
		public static Result<T> Failure(string error)
		{
			return new Result<T>(false, default!, error);
		}

	}
}
