using GithubVisualizer.Helpers;
using GithubVisualizer.Interfaces;
using GithubVisualizer.Models.DataModels;
using GithubVisualizer.Models.RequestModels;
using GithubVisualizer.Models.ResponseModels;

namespace GithubVisualizer.Services
{
	public class VisualizerService : IVisualizerService
	{
		public GenerateVisualizerResponseModel GenerateVisualizerLink(GenerateVisualizerRequestModel model)
		{
			if (!Uri.TryCreate(model.RepoUrl, UriKind.Absolute, out Uri result))
			{
				return new GenerateVisualizerResponseModel
				{
					Url = null,
					Error = "Could not understand URL, format URL properly",
					IsError = true
				};

			}

			var url = result.ToString();

			//Get Repo Username and Name from Github

			// Temporary short link 
			var entry = new Repository
			{
				VisualizerUrl = url,
				UserName = "khelechy",
				RepoName = "GithubVisualizer"
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

		public Repository GetVisualizedRepository(HttpContext context)
		{
			var path = context.Request.Path.ToUriComponent().Trim('/');
			if (path.Length == 6)
			{
				var id = Repository.GetId(path);
				var entry = LiteDbHelper.GetById(id);
				return entry;
			}
			else
			{
				return null;
			}
		}
	}
}
