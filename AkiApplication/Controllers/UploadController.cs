using Microsoft.AspNetCore.Mvc;
using static AkiApplication.Controllers.BankController;
using System.Text.Json;
using System.Text;
using AkiTemplate.Approachable;
using static AkiTemplate.Approachable.CsvWrapper;
using System.IO;

namespace AkiApplication.Controllers
{
	public class UploadController : Controller
	{
		private readonly IWebHostEnvironment environment;
		public UploadController(IWebHostEnvironment environment)
		{
			this.environment = environment;
		}
        [Route("api/upload/{year}/{month}")]
		public async Task<IActionResult> UploadDisbursement(string year, string month, IFormFile file)
		{
			try
			{
				ICsv csv = new CsvWrapper();
				var rslt = new List<TransactionDetails>();
				var path = Path.Combine(environment.WebRootPath + @"\disbursement", $"{year}\\{month}.txt");

				if (Directory.Exists(Path.Combine(environment.WebRootPath + @"\disbursement", $"{year}")))
				{
				}
				else
				{
					Directory.CreateDirectory(Path.Combine(environment.WebRootPath + @"\disbursement", $"{year}"));
				}


				using (var memoryStream = new MemoryStream())
				{
					await file.CopyToAsync(memoryStream);
					var data2 = memoryStream.ToArray();
					var filePath = Path.Combine(environment.WebRootPath + @"\delete", file.FileName);
					using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
					{
						fs.Write(data2, 0, data2.Length);
						fs.Close();
					}

				};

				foreach (var i in csv.ReadCsvFile<CsvCulumn0>(Path.Combine(environment.WebRootPath + @"\delete", file.FileName)))
				{
					rslt.Add(new TransactionDetails()
					{
						Datetime = DateTime.Parse(i.Column0),
						Memo = i.Column1,
						Mony = int.Parse(i.Column2)
					});
				}

				System.IO.File.WriteAllText(path, JsonSerializer.Serialize(rslt));

				return StatusCode(200);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}