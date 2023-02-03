using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LubricantApp.Data.Models
{
    public static class PathWrapper
    {
        public enum Extension : int
        {
            All,
            Csv,
            Txt,
            Xlsx,
            Pdf,
        }

        private static string[] PathExtensions { get; } = { "*", "csv", "txt", "xlsx", "pdf" };



        public static void CopyFile(string sourcePath, string destinationPath)
        {
            var extension = Path.GetExtension(destinationPath);
            var filename = Path.GetFileName(destinationPath).Replace(extension, "");
            var directory = destinationPath.Replace(Path.GetFileName(destinationPath), "");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.Copy(sourcePath, destinationPath);
        }

        // ディレクトリ存在チェック
        public static List<string> GetDerectories(string path, string pattern = "*")
        {
            var result = new List<string>();
            try
            {
                var _ = Directory.GetDirectories(path, pattern, SearchOption.TopDirectoryOnly).ToList();
                result = _.OrderBy(x => x).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        public static List<string> GetFileNames(string path, string pattern = "*", Extension extension = Extension.All)
        {
            var result = new List<string>();
            var files = (new DirectoryInfo(path)).GetFiles(pattern + "." + PathExtensions[Convert.ToInt32(extension)], SearchOption.TopDirectoryOnly).ToList();
            if (files.Any())
            {
                var _ = files.OrderBy(x => x.FullName);
                foreach (var i in _)
                {
                    result.Add(i.FullName);
                }
            }

            return result;
        }

        public static bool IsValidPathString(List<char> chars)
        {
            var errors = Path.GetInvalidFileNameChars();
            var getErrors = new List<char>();
            foreach (var i in errors)
            {
                getErrors.AddRange(chars.Where(x => { return x == i; }));
            }

            return !(getErrors.Any());

        }

        public static void CopyDirectory(string sourcePath, string destinationPath)
        {
            CopyDirectory(sourcePath, destinationPath, true);
        }

        private static bool CopyDirectory(string sourcePath, string destinationPath, bool isFirst)
        {
            bool result;
            try
            {
                if (isFirst)
                {
                    if (Directory.Exists(destinationPath))
                    {
                        Directory.Delete(destinationPath, true);
                    }
                }

                DirectoryInfo sourceDirectory = new DirectoryInfo(sourcePath);
                DirectoryInfo destinationDirectory = new DirectoryInfo(destinationPath);

                //コピー先のディレクトリがなければ作成する
                if (destinationDirectory.Exists == false)
                {
                    destinationDirectory.Create();
                    destinationDirectory.Attributes = sourceDirectory.Attributes;
                }

                //ファイルのコピー
                foreach (FileInfo fileInfo in sourceDirectory.GetFiles())
                {
                    //同じファイルが存在していたら、常に上書きする
                    fileInfo.CopyTo(destinationDirectory.FullName + @"\" + fileInfo.Name, true);
                }

                //ディレクトリのコピー（再帰を使用）
                foreach (DirectoryInfo directoryInfo in sourceDirectory.GetDirectories())
                {
                    CopyDirectory(directoryInfo.FullName, destinationDirectory.FullName + @"\" + directoryInfo.Name, false);
                }

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = false;
            }

            return result;
        }

        public static void TextOutput(string path, List<string> document)
        {
            var outStr = string.Empty;

            if (!Directory.Exists(path.Replace($"{Path.GetFileName(path)}", "")))
            {
                Directory.CreateDirectory(path);
            }
            foreach (var i in document)
            {
                outStr += $"{DateTime.Now:yyyyMMddHHmmss}:{i}" + Environment.NewLine;
            }

            File.AppendAllText($"{path}", outStr);

        }


    }
}
