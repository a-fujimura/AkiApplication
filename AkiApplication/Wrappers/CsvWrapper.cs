using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkiTemplate.Approachable
{
	public interface ICsv
	{
		List<T> ReadCsvFile<T>(string filePath);

		void WriteCsvFile<T>(string filePath, IEnumerable<T> data);
	}


	public class CsvWrapper : ICsv
	{
		public List<T> ReadCsvFile<T>(string filePath)
		{
			var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null,
				HasHeaderRecord = false,
			};

			using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
			using (var csvReader = new CsvReader(streamReader, configuration))
			{
				return csvReader.GetRecords<T>().ToList();
			}
		}

		public void WriteCsvFile<T>(string filePath, IEnumerable<T> data)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(filePath));

			using (var streamWriter = new StreamWriter(filePath, false, Encoding.UTF8))
			using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.GetCultureInfo("ja-JP")))
			{
				csvWriter.WriteRecords(data);
			}
		}

		public class CsvCulumn0
		{
			[Index(0)]
			public string Column0 { get; set; }
			[Index(1)]
			public string Column1 { get; set; }
			[Index(2)]
			public string Column2 { get; set; }

		}

	}
}



