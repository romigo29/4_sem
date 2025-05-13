using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using static DAL_Celebrity_MSSQL.DAL_Celebrity_MSSQL;
using DAL_Celebrity;

namespace ASPA007_1.Pages
{
    public class CelebritiesModel : PageModel
    {

		IRepository repo;
		public string PhotoRequestPath { get; set; }
        public List<Celebrity> Celebrities { get; set; } = new List<Celebrity>();



        public CelebritiesModel(IRepository repo, IOptions<CelebritiesConfig> config) 
        {
            this.repo = repo;
            this.PhotoRequestPath = config.Value.PhotosRequestPath;
        }

        public void OnGet()
        {
            this.Celebrities = this.repo.GetAllCelebrities();
        }
    }
}
