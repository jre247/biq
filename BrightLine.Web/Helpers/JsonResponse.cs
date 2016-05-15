
namespace BrightLine.Web.Helpers
{
	//TODO: JsonResponse usage should be removed in favor of WebApi calls
	public class JsonResponse
	{
		public bool Success { get; set; }
		public string Message { get; set; }
		public object Data { get; set; }
	}
}
