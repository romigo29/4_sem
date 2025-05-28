using ANC25_WEBAPI_DLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static ASPA008_1.Filters.CelebrityAsyncActionFilter;
using static DAL_Celebrity_MSSQL.DAL_Celebrity_MSSQL;

namespace ASPA008_1.Controllers
{
	public class CelebritiesController : Controller
	{
		IRepository repo;
		IOptions<CelebritiesConfig> config;
		public CelebritiesController(IRepository repo, IOptions<CelebritiesConfig> config)
		{

			this.repo = repo;
			this.config = config;
		}

		public record IndexModel(string PhotosRequestPath, List<Celebrity> Celebrities);
		public IActionResult Index()
		{
			return View(new IndexModel(config.Value.PhotosRequestPath, repo.GetAllCelebrities()));
		}

		public record HumanModel(string photosrequestpath, Celebrity celebrity, List<Lifeevent> lifeevents, Dictionary<string, string>? references);
		[InfoAsyncActionFilter(infotype: "Wikipedia, Facebook")]

		public IActionResult Human(int id)

		{

			IActionResult rc = NotFound();

			Celebrity? celebrity = repo.GetCelebrityById(id);

			Dictionary<string, string>? references = (Dictionary<string, string>?)HttpContext.Items[InfoAsyncActionFilter.Wikipedia];

			if (celebrity != null) rc = View(new HumanModel(config.Value.PhotosRequestPath,

			(Celebrity)celebrity, repo.GetLifeeventsByCelebrityId(id), references));

			return rc;
		}

		public IActionResult NewHumanForm()
		{
			var celebrity = new Celebrity
			{
				FullName = "",
				Nationality = "RU",
				ReqPhotoPath = "NewCelebrity.jpg"
			};

			var model = new HumanModel(
				config.Value.PhotosRequestPath,
				celebrity,
				new List<Lifeevent>(),
				null
			);

			return View("NewHumanForm", model);
		}

		[HttpPost]
		public IActionResult NewHumanForm(string fullname, string Nationality, IFormFile upload)
		{
			if (ModelState.IsValid)
			{
				try
				{
					string photoFileName = $"{Guid.NewGuid()}.jpg";

					string photoPath = Path.Combine(config.Value.PhotoPath, photoFileName);

					using (var fileStream = new FileStream(photoPath, FileMode.Create))
					{
						upload.CopyTo(fileStream);
					}

					var celebrity = new Celebrity
					{
						FullName = fullname,
						Nationality = Nationality,
						ReqPhotoPath = photoFileName
					};

					repo.AddCelebrity(celebrity);

					return RedirectToAction("Index");
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", $"Ошибка при сохранении данных: {ex.Message}");
				}
			}

			var errorCelebrity = new Celebrity
			{
				FullName = fullname ?? "",
				Nationality = Nationality ?? "RU",
				ReqPhotoPath = "NewCelebrity.jpg"
			};

			return View("NewHumanForm", new HumanModel(
				config.Value.PhotosRequestPath,
				errorCelebrity,
				new List<Lifeevent>(),
				null
			));
		}

		[HttpPost]
		public async Task<IActionResult> Confirm(string fullname, string Nationality, IFormFile upload)
		{
			if (ModelState.IsValid && upload != null)
			{
				try
				{
					string tempFileName = $"temp_{Guid.NewGuid()}.jpg";

					string filePath = Path.Combine(config.Value.PhotoPath, tempFileName);

					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await upload.CopyToAsync(fileStream);
					}

					var celebrity = new Celebrity
					{
						FullName = fullname?.Trim() ?? "",
						Nationality = Nationality ?? "RU",
						ReqPhotoPath = tempFileName      
					};

					var model = new HumanModel(
						config.Value.PhotosRequestPath,
						celebrity,
						new List<Lifeevent>(),
						null
					);

					TempData["TempPhotoFileName"] = tempFileName;

					return View("Confirm", model);
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", $"Ошибка при загрузке файла: {ex.Message}");
				}
			}

			var errorCelebrity = new Celebrity
			{
				FullName = fullname ?? "",
				Nationality = Nationality ?? "RU",
				ReqPhotoPath = "NewCelebrity.jpg"
			};

			return View("NewHumanForm", new HumanModel(
				config.Value.PhotosRequestPath,
				errorCelebrity,
				new List<Lifeevent>(),
				null
			));
		}

		[HttpPost]
		public IActionResult SaveConfirmed(HumanModel model)
		{
			try
			{
				string tempFileName = model.celebrity.ReqPhotoPath;

				string permanentFileName = $"{Guid.NewGuid()}.jpg";

				string tempFilePath = Path.Combine(config.Value.PhotoPath, tempFileName);
				string permanentFilePath = Path.Combine(config.Value.PhotoPath, permanentFileName);

				if (System.IO.File.Exists(tempFilePath))
				{
					System.IO.File.Copy(tempFilePath, permanentFilePath, true);

					System.IO.File.Delete(tempFilePath);
				}

				var celebrity = new Celebrity
				{
					FullName = model.celebrity.FullName,
					Nationality = model.celebrity.Nationality,
					ReqPhotoPath = permanentFileName
				};

				repo.AddCelebrity(celebrity);

				TempData.Remove("TempPhotoFileName");

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", $"Ошибка при сохранении данных: {ex.Message}");
				return View("Confirm", model);
			}
		}

		[HttpPost]
		public IActionResult CancelConfirm()
		{
			if (TempData.ContainsKey("TempPhotoFileName"))
			{
				string tempFileName = TempData["TempPhotoFileName"].ToString();
				string tempFilePath = Path.Combine(config.Value.PhotoPath, tempFileName);

				if (!string.IsNullOrEmpty(tempFileName) && System.IO.File.Exists(tempFilePath))
				{
					try
					{
						System.IO.File.Delete(tempFilePath);
					}
					catch (Exception)
					{
					}
				}

				TempData.Remove("TempPhotoFileName");
			}

			return RedirectToAction("NewHumanForm");
		}

		[HttpGet]
		public IActionResult Edit(int id)
		{
			var celebrity = repo.GetCelebrityById(id);
			if (celebrity == null)
				return NotFound();

			return View("NewHumanForm", new HumanModel(
				config.Value.PhotosRequestPath,
				celebrity,
				repo.GetLifeeventsByCelebrityId(id),
				null
			));
		}

		[HttpPost]
		public async Task<IActionResult> EditCelebrity(int id, string fullname, string Nationality, IFormFile upload)
		{
			var celebrity = repo.GetCelebrityById(id);
			if (celebrity == null)
				return NotFound();

			try
			{
				celebrity.FullName = fullname;
				celebrity.Nationality = Nationality;

				if (upload != null && upload.Length > 0)
				{
					string photoFileName = $"{Guid.NewGuid()}.jpg";

					string photoPath = Path.Combine(config.Value.PhotoPath, photoFileName);

					using (var fileStream = new FileStream(photoPath, FileMode.Create))
					{
						await upload.CopyToAsync(fileStream);
					}

					string oldPhotoPath = Path.Combine(config.Value.PhotoPath, celebrity.ReqPhotoPath);
					if (celebrity.ReqPhotoPath != "NewCelebrity.jpg" && System.IO.File.Exists(oldPhotoPath))
					{
						try
						{
							System.IO.File.Delete(oldPhotoPath);
						}
						catch (Exception)
						{
						}
					}

					celebrity.ReqPhotoPath = photoFileName;
				}

				repo.UpdCelebrity(celebrity);

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", $"Ошибка при сохранении данных: {ex.Message}");
			}

			return View("NewHumanForm", new HumanModel(
				config.Value.PhotosRequestPath,
				celebrity,
				repo.GetLifeeventsByCelebrityId(id),
				null
			));
		}

		[HttpGet]
		public IActionResult ConfirmDelete(int id)
		{
			var celebrity = repo.GetCelebrityById(id);
			if (celebrity == null)
			{
				return NotFound();
			}

			return View("ConfirmDelete", celebrity);
		}

		[HttpPost]
		public IActionResult Delete(int id)
		{
			var celebrity = repo.GetCelebrityById(id);
			if (celebrity == null)
			{
				return NotFound();
			}

			repo.DelCelebrity(id);

			return RedirectToAction("Index");
		}
	}
}


