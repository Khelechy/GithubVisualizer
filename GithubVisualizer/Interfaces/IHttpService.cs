using RestSharp;

namespace GithubVisualizer.Interfaces
{
	public interface IHttpService
	{
		Task<IRestResponse> GetAsync(string endpoint, Dictionary<string, string> additionalHeaders = null);
	}
}
