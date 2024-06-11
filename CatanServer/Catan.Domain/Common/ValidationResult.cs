namespace Catan.Domain.Common
{
	public class ValidationResult
	{
		private ValidationResult(bool isSuccess, string error)
		{
			IsSuccess = isSuccess;
			Error = error;
		}

		public bool IsSuccess { get; }
		public string Error { get; }

		public static ValidationResult Success()
		{
			return new ValidationResult(true, null!);
		}
		public static ValidationResult Failure(string error)
		{
			return new ValidationResult(false, error);
		}
	}
}
