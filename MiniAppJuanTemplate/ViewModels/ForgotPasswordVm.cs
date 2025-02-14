using System.ComponentModel.DataAnnotations;

namespace MiniAppJuanTemplate.ViewModels
{
	public class ForgotPasswordVm
	{
		[DataType(DataType.EmailAddress)]
		public string? Email { get; set; }
	}
}
