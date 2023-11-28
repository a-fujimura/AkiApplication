using AkiTemplate.Approachable;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using static AkiTemplate.Approachable.CsvWrapper;

namespace AkiApplication.Controllers
{
    public class BankController : Controller
    {
        readonly IWebHostEnvironment environment;
        public BankController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        [Route("receipt/get/{year}")]
        public async Task<IActionResult> GetReceipt(string year)
        {
            try
            {
                var rslt = new List<TransactionDetails>();
                for (var i = 1; i <= 12; i++)
                {
                    var path = Path.Combine(environment.WebRootPath + @"\receipt", $"{year}\\{i:D2}.txt");
                    if (System.IO.File.Exists(path))
                    {
                        var get = System.IO.File.ReadAllText(path);
                        var all = JsonSerializer.Deserialize<List<TransactionDetails>>(get);
                        var obj = new TransactionDetails()
                        {
                            Datetime = new DateTime(int.Parse(year), i, 1),
                            Mony = all.Sum(x => x.Mony),
                        };
                        rslt.Add(obj);
                    }
                    else
                    {
                        var obj = new TransactionDetails()
                        {
                            Datetime = new DateTime(int.Parse(year), i, 1),
                            Mony = 0,
                        };
                        rslt.Add(obj);
                    }
                }

                return Content(JsonSerializer.Serialize(rslt), "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("receipt/get/{year}/{month}")]
        public async Task<IActionResult> GetReceipt(string year, string month)
        {
            try
            {
                var rslt = new List<TransactionDetails>();
                var path = Path.Combine(environment.WebRootPath + @"\receipt", $"{year}\\{month}.txt");
                if (System.IO.File.Exists(path))
                {
                    var get = System.IO.File.ReadAllText(path);
                    rslt = JsonSerializer.Deserialize<List<TransactionDetails>>(get);
                }
                else
                {

                }
                return Content(JsonSerializer.Serialize(rslt), "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("disbursement/get/{year}")]
        public async Task<IActionResult> GetDisbursement(string year)
        {
            try
            {
                var rslt = new List<TransactionDetails>();
                for (var i = 1; i <= 12; i++)
                {
                    var path = Path.Combine(environment.WebRootPath + @"\disbursement", $"{year}\\{i:D2}.txt");
                    if (System.IO.File.Exists(path))
                    {
                        var get = System.IO.File.ReadAllText(path);
                        var all = JsonSerializer.Deserialize<List<TransactionDetails>>(get);
                        var obj = new TransactionDetails()
                        {
                            Datetime = new DateTime(int.Parse(year), i, 1),
                            Mony = all.Sum(x => x.Mony),
                        };
                        rslt.Add(obj);
                    }
                    else
                    {
                        var obj = new TransactionDetails()
                        {
                            Datetime = new DateTime(int.Parse(year), i, 1),
                            Mony = 0,
                        };
                        rslt.Add(obj);
                    }
                }

                return Content(JsonSerializer.Serialize(rslt), "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Route("disbursement/get/{year}/{month}")]
        public async Task<IActionResult> GetDisbursement(string year, string month)
        {
            try
            {
                var rslt = new List<TransactionDetails>();
                var path = Path.Combine(environment.WebRootPath + @"\disbursement", $"{year}\\{month}.txt");
                if (System.IO.File.Exists(path))
                {
                    var get = System.IO.File.ReadAllText(path);
                    rslt = JsonSerializer.Deserialize<List<TransactionDetails>>(get);
                }
                else
                {

                }
                return Content(JsonSerializer.Serialize(rslt), "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("receipt/set/{year}/{month}")]
        public async Task<IActionResult> SetReceipt(string year, string month, IFormFile file)
        {
            try
            {
                var path = Path.Combine(environment.WebRootPath + @"\receipt", $"{year}\\{month}.txt");

                if (Directory.Exists(Path.Combine(environment.WebRootPath + @"\receipt", $"{year}")))
                {
                }
                else
                {
                    Directory.CreateDirectory(Path.Combine(environment.WebRootPath + @"\receipt", $"{year}"));
                }

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var get = JsonSerializer.Deserialize<List<TransactionDetails>>(Encoding.UTF8.GetString(memoryStream.ToArray()));

                    System.IO.File.WriteAllText(path, JsonSerializer.Serialize(get));
                }
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("disbursement/set/{year}/{month}")]
        public async Task<IActionResult> SetDisbursement(string year, string month, IFormFile file)
        {
            try
            {
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
                    var get = JsonSerializer.Deserialize<List<TransactionDetails>>(Encoding.UTF8.GetString(memoryStream.ToArray()));

                    System.IO.File.WriteAllText(path, JsonSerializer.Serialize(get));

                }
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

		[Route("disbursement/upload/{year}/{month}")]
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
					using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
					fs.Write(data2, 0, data2.Length);
					fs.Close();
				};

				foreach (var i in csv.ReadCsvFile<CsvCulumn0>(path))
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

		[Route("schedule/get/{mode}")]
        public async Task<IActionResult> GetSchedule(string mode)
        {
            try
            {
                var rslt = new List<TransactionDetails>();
                var path = Path.Combine(environment.WebRootPath + @"\schedule", $"{mode}.txt");
                if (System.IO.File.Exists(path))
                {
                    var get = System.IO.File.ReadAllText(path);
                    rslt = JsonSerializer.Deserialize<List<TransactionDetails>>(get);
                }
                else
                {

                }
                return Content(JsonSerializer.Serialize(rslt), "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("schedule/set/{mode}")]
        public async Task<IActionResult> SetSchedule(string mode, IFormFile file)
        {
            try
            {
                var path = Path.Combine(environment.WebRootPath + @"\schedule", $"{mode}.txt");

                if (Directory.Exists(Path.Combine(environment.WebRootPath + @"\schedule", $"{mode}.txt")))
                {
                }
                else
                {
                    Directory.CreateDirectory(Path.Combine(environment.WebRootPath, $"schedule"));
                }

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var get = JsonSerializer.Deserialize<List<TransactionDetails>>(Encoding.UTF8.GetString(memoryStream.ToArray()));

                    System.IO.File.WriteAllText(path, JsonSerializer.Serialize(get));

                }
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        public class TransactionDetails
        {
            public DateTime Datetime { get; set; }
            public string Memo { get; set; }
            public int Mony { get; set; } = -1;
        }
    }
}
