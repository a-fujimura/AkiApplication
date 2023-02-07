using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

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
        public async Task<IActionResult> GetCashReceipt(string year)
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
                            Detail = TransactionDetails.Details.Receipt
                        };
                        rslt.Add(obj);
                    }
                    else
                    {
                        var obj = new TransactionDetails()
                        {
                            Datetime = new DateTime(int.Parse(year), i, 1),
                            Mony = 0,
                            Detail = TransactionDetails.Details.Receipt
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
        public async Task<IActionResult> GetCashReceipt(string year, string month)
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
        public async Task<IActionResult> GetCashDisbursement(string year)
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
                            Detail = TransactionDetails.Details.Disbursement
                        };
                        rslt.Add(obj);
                    }
                    else
                    {
                        var obj = new TransactionDetails()
                        {
                            Datetime = new DateTime(int.Parse(year), i, 1),
                            Mony = 0,
                            Detail = TransactionDetails.Details.Disbursement
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
        public async Task<IActionResult> GetCashDisbursement(string year, string month)
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
        public async Task<IActionResult> SetCashReceipt(string year, string month, IFormFile file)
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
        public async Task<IActionResult> SetCashDisbursement(string year, string month, IFormFile file)
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



        public class TransactionDetails
        {
            public enum Details
            {
                Receipt = 0,
                Disbursement,
            }
            public DateTime Datetime { get; set; }
            public string Memo { get; set; }
            public Details Detail { get; set; }
            public int Mony { get; set; } = -1;
        }
    }
}
