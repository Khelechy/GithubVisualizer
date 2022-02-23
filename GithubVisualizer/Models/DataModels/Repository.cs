using Microsoft.AspNetCore.WebUtilities;

namespace GithubVisualizer.Models.DataModels
{
	public class Repository
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string RepoName { get; set; }
		public string VisualizerUrl { get; set; }

		public string GetUrlChunk()
        {
            // Transform the "Id" property on this object into a short piece of text
            return WebEncoders.Base64UrlEncode(BitConverter.GetBytes(Id));
        }

        public static int GetId(string urlChunk)
        {
            // Reverse our short url text back into an interger Id
            return BitConverter.ToInt32(WebEncoders.Base64UrlDecode(urlChunk));
        }
    }
}
