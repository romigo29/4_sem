using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using static DAL_Celebrity_MSSQL.DAL_Celebrity_MSSQL;

namespace ASPA008_1.Filters
{
	public class CelebrityAsyncActionFilter
	{
		public class InfoAsyncActionFilter : Attribute, IAsyncActionFilter
		{

			public static readonly string Wikipedia = "WIKI";
			public static readonly string Facebook = "FACE";
			string infotype;
			public InfoAsyncActionFilter(string infotype = "")
			{
				this.infotype = infotype;
			}

			public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
			{

				IRepository? repo = context.HttpContext.RequestServices.GetService<IRepository>();
				int id = (int)(context.ActionArguments["id"] ?? -1);
				Celebrity? celebrity = repo?.GetCelebrityById(id);
				if (infotype.ToUpper().Contains(Wikipedia) && celebrity != null)
					context.HttpContext.Items.Add(Wikipedia, await WikiInfoCelebrity.GetRefereces(celebrity.FullName));

				if (infotype.ToUpper().Contains(Facebook) && celebrity != null)
					context.HttpContext.Items.Add(Facebook, getFromFace(celebrity.FullName));

				await next();
			}

			string getFromFace(string fullname)
			{
				string rc = "Info from Face";
				// FacebookClient request to Facebook
				return rc;
			}
		}

		public class WikiInfoCelebrity
		{

			HttpClient client;
			string wikiURItemplate = "https://en.wikipedia.org/w/api.php?action=opensearch&search=\"{0}\"&prop=info&format=json";
			Dictionary<string, string> wikiReferens { get; set; }
			string wikiURI;

			private WikiInfoCelebrity(string fullname)
			{
				this.client = new HttpClient();
				this.wikiReferens = new Dictionary<string, string>();
				this.wikiURItemplate = fullname;
				this.wikiURI = string.Format("https://en.wikipedia.org/w/api.php?action=opensearch&search=\"{0}\"&prop=info&format=json", fullname);
			}

			public static async Task<Dictionary<string, string>> GetRefereces(string fullname)
			{

				WikiInfoCelebrity info = new WikiInfoCelebrity(fullname);
				HttpResponseMessage message = await info.client.GetAsync(info.wikiURI);
				if (message.StatusCode == System.Net.HttpStatusCode.OK)
				{
					List<dynamic>? result = await message.Content.ReadFromJsonAsync<List<dynamic>>() ?? default(List<dynamic>);
					List<string>? ls1 = JsonSerializer.Deserialize<List<string>>(result[1]);
					List<string>? ls3 = JsonSerializer.Deserialize<List<string>>(result[3]);

					for (int i = 0; i < ls1.Count; i++)
					{
						info.wikiReferens.Add(ls1[i], ls3[i]);
					}
				}
				return info.wikiReferens;
			}
		}
	}
}
