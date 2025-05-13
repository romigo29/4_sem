using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static DAL_Celebrity_MSSQL.DAL_Celebrity_MSSQL;
using DAL_Celebrity;
using Microsoft.Extensions.Options;

namespace ASPA007_1.Pages
{
    public class CelebrityModel : PageModel
    {

        public string PhotosRequestPath  { get; set; }
        public Celebrity? Celebrity     { get; set; }

		public IActionResult OnGet(Parms? ModelParms)
		{
			if (ModelParms == null || ModelParms.Id == null || ModelParms.repo == null || ModelParms.config == null)
			{
				return (IActionResult)Results.Problem(); //должно быть пользовательское исключение
			}

			else
			{
				this.PhotosRequestPath = ModelParms.config.Value.PhotosRequestPath;
				return ((this.Celebrity = ModelParms.repo.GetCelebrityById(ModelParms.Id)) is null)
							? NotFound() :
							(ModelParms.AcceptMIMO == "json")
							? this.RedirectToRoute("GetCelebrityById", new { Id = ModelParms.Id })
							: Page();
			}
		}

		public class Parms
		{

			[FromRoute]
			public int Id { get; set; } = -1;

			[FromQuery(Name = "id")]
			public int? queryId { get; set; } = null;

			[FromHeader(Name = "Accept")]
			public string? AcceptHeader { get; set; } = null;

			[FromServices]
			public IRepository? repo { get; set; }

			[FromServices]
			public IOptions<CelebritiesConfig>? config { get; set; }

			public string AcceptMIMO
			{
				get
				{
					return preferredAcceptMIMO(AcceptHeader, new string[] { "html", "json" }).Item1;
				}
			}

			private (string, int) preferredAcceptMIMO(string? accept, string[] parms)
			{

				(string?, int) rc = (null, -1);

				if (accept != null)
				{

					int k = 1, mink = accept.Length + 1, mini = -1;
					for (int i = 0; i < parms.Length; i++)
					{
						if ((k = accept.IndexOf(parms[i], StringComparison.OrdinalIgnoreCase)) >= 0)
							if (k < mink) { mink = k; mini = i; }
					}

					rc = ((mini > 0) ? parms[mini] : null, mini);

				}

				return rc;

			}
		}
    }
}
