using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using static DAL_Celebrity_MSSQL.DAL_Celebrity_MSSQL;

namespace ASPA007_1.Pages
{
	public class NewCelebrityModel : PageModel

	{

		public IRepository repo;

		public string PhotosRequestPath { get; set; }

		public string PhotosFolder { get; set; }

		public Celebrity? Celebrity { get; set; }

		public NewCelebrityModel(IRepository repo, IOptions<CelebritiesConfig> config)
		{
			this.repo = repo;
			this.PhotosRequestPath = config.Value.PhotosRequestPath;
			this.PhotosFolder = config.Value.PhotoPath;
		}

		public void OnGet()
		{

		}

		public IActionResult OnGetConfirm(string fullname, string nationality, string filename)

		{

			ViewData["Confirm"] = true;
			this.Celebrity = new Celebrity() { FullName = fullname, Nationality = nationality, ReqPhotoPath = filename };
			return Page();

		}

		public IActionResult OnPost([FromForm] string? fullname, [FromForm] string? nationality, IFormFile upload, string? press, string? filename)

		{

			IActionResult rc = RedirectToPage("Celebrities");

			if (string.IsNullOrEmpty(press))
			{
				var origName = Path.GetFileName(upload.FileName);
				var baseName = Path.GetFileNameWithoutExtension(origName);
				var ext = Path.GetExtension(origName);

				var targetName = origName;
				var targetPath = Path.Combine(PhotosFolder, targetName);
				if (System.IO.File.Exists(targetPath))
				{
					targetName = $"{baseName}_{Guid.NewGuid():N}{ext}";
					targetPath = Path.Combine(PhotosFolder, targetName);
				}

				using (var fs = new FileStream(targetPath, FileMode.CreateNew))
				{
					upload.CopyTo(fs);
				}

				rc = RedirectToPage("NewCelebrity", "Confirm", new { filename = targetName, fullname = fullname, nationality = nationality });

			}

			else if (press.Equals("Confirm"))
			{
				repo.AddCelebrity(new Celebrity
				{
					FullName = fullname,
					Nationality = nationality.Substring(0, 2),
					ReqPhotoPath = filename!
				});
				return RedirectToPage("Celebrities");
			}


			else rc = RedirectToPage("NewCelebrity");

			return rc;

		}
}
}
