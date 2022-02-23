using GithubVisualizer.Models.DataModels;
using GithubVisualizer.Models.RequestModels;
using GithubVisualizer.Models.ResponseModels;

namespace GithubVisualizer.Interfaces
{
	public interface IVisualizerService
	{
		Repository GetVisualizedRepository(HttpContext context);
		GenerateVisualizerResponseModel GenerateVisualizerLink(GenerateVisualizerRequestModel model);
	}
}
