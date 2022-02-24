using GithubVisualizer.Helpers;
using GithubVisualizer.Interfaces;
using GithubVisualizer.Models.DataModels;
using GithubVisualizer.Models.RequestModels;
using GithubVisualizer.Models.ResponseModels;
using Newtonsoft.Json;

namespace GithubVisualizer.Services
{
	public class VisualizerService : IVisualizerService
	{
		private readonly IConfiguration _configuration;
        private readonly string _accept;
        private readonly IHttpService _httpService;

        public VisualizerService(IConfiguration configuration, IHttpService httpService)
		{
			_configuration = configuration;
			_accept = _configuration["AppConfig:Accept"];
			_httpService = httpService;
		}

		public async Task<GenerateVisualizerResponseModel> GenerateVisualizerLink(GenerateVisualizerRequestModel model)
		{
			var newUrl = model.RepoUrl.Replace(".git", "");
			if (!Uri.TryCreate(newUrl, UriKind.Absolute, out Uri result))
			{
				return new GenerateVisualizerResponseModel
				{
					Url = null,
					Error = "Could not understand URL, format URL properly",
					IsError = true
				};

			}

			//Validate Github Repo Link format


			var url = result.ToString();

			//Extract Path
			var pathArray = url.Split('/');
			var path = $"{pathArray[pathArray.Length - 2]}/{pathArray[pathArray.Length - 1]}";


			//Get Repo Data
			var repoData = await GetRepoData(path);
			if(repoData == null)
            {
				return new GenerateVisualizerResponseModel
				{
					Url = null,
					Error = "Github Repository does not exist",
					IsError = true
				};
			}

			// Temporary short link 
			var entry = new Repository
			{
				VisualizerUrl = url,
				Path = path
			};

			// Insert our short-link
			LiteDbHelper.Insert(entry);

			var urlChunk = entry.GetUrlChunk();
			var responseUri = urlChunk;
			return new GenerateVisualizerResponseModel
			{
				Url = responseUri,
				Error = "",
				IsError = false
			};

		}

		public async Task<GetRepositoryResponseModel> GetVisualizedRepository(HttpContext context)
		{
			var path = context.Request.Path.ToUriComponent().Trim('/');
			if (path.Length == 6)
			{
				var id = Repository.GetId(path);
				var entry = LiteDbHelper.GetById(id);
				return await GetRepoData(entry.Path);
			}
			else
			{
				return null;
			}
		}

		public async Task<GetRepositoryResponseModel> GetRepoData(string path)
        {
			var headers = new Dictionary<string, string>();
			headers.Add("Accept", _accept);
			var response = await _httpService.GetAsync($"repos/{path}", headers);
			if(response.IsSuccessful)
            {
				var responseData = JsonConvert.DeserializeObject<GetRepositoryResponseModel>(response.Content);
				//Get Event repo
				var repoRequest = await _httpService.GetAsync(responseData.events_url);
				var repoResponse = JsonConvert.DeserializeObject<List<RepoEventsResponseModel>>(repoRequest.Content);
				responseData.repo_events = repoResponse;
				return responseData;
            }
            else
            {
				return null;
            }
        }
	}
}
