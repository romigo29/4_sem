namespace ANC25_WEBAPI_DLL.Services
{
	public class CelebritiesConfig
	{
		public string PhotosRequestPath { get; set; } = string.Empty;
		public string PhotoPath { get; set; } = string.Empty;
		public string ConnectionString { get; set; } = string.Empty;

		public string ISO3166alpha2Path { get; set; } = string.Empty;

		public CelebritiesConfig()

		{

			this.PhotosRequestPath = "/Photos";
			this.PhotoPath = "./Photos";
			this.ConnectionString = "Data source=SOURCE; Initial Catalog=DBNAME; User Id=USERLOGIN;Password=PASSWORD";
			this.ISO3166alpha2Path = "/CountryCodes";

		}
	}
}


