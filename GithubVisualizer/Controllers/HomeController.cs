using GithubVisualizer.Interfaces;
using GithubVisualizer.Models;
using GithubVisualizer.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GithubVisualizer.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IVisualizerService _visualizerService;

		public HomeController(ILogger<HomeController> logger, IVisualizerService visualizerService)
		{
			_logger = logger;
			_visualizerService = visualizerService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public Task GenerateVisualizer(GenerateVisualizerRequestModel model)
		{
			var result = _visualizerService.GenerateVisualizerLink(model);
			var fullUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{result.Url}";
			if (!result.IsError)
			{
				TempData["URL"] = fullUrl;
				HttpContext.Response.Redirect($"/#{fullUrl}");
			}
			else
			{
				HttpContext.Response.Redirect("/");
			}
			return Task.CompletedTask;
		}

		public IActionResult Visualize()
		{
			var repo = _visualizerService.GetVisualizedRepository(HttpContext);
			if(repo != null)
			{
				return View(repo);
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}