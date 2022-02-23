using GithubVisualizer.Interfaces;
using RestSharp;

namespace GithubVisualizer.Services
{
	public class HttpService : IHttpService
	{
		private readonly IConfiguration _configuration;
		static readonly RestClient client = new RestClient();

        public HttpService(IConfiguration configuration)
        {
			_configuration = configuration;
			client.BaseUrl = new Uri(_configuration["AppConfig:BaseURL"]);
		}

		public async Task<IRestResponse> GetAsync(string endpoint, Dictionary<string, string> additionalHeaders = null)
        {
			var httpRequest = new RestRequest(endpoint, Method.GET);
			if (additionalHeaders != null)
				httpRequest.AddHeaders(additionalHeaders);
			httpRequest.AddParameter("application/json", ParameterType.RequestBody);
			var response = await client.ExecuteAsync(httpRequest);
			return response;
		}
	}
}
