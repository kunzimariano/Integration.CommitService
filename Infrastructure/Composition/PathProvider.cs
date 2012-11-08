using System;
using System.IO;
using System.Reflection;

namespace Infrastructure.Composition
{
    public static class PathProvider
    {
        public static string BinaryPath { get; set; }

        public static string GetPath()
        {

            //var file = Assembly.GetExecutingAssembly().Location;
            //var path = new FileInfo(file).DirectoryName;
            
            //var files = new[]
            //                {
            //                    "GitHubCommitAttemptTranslator",
            //                    "BitBucketCommitAttemptTranslator"
            //                };

            //var doCopy = true;
            //doCopy = false;
            //if (doCopy) // Shamlesss temphackery.
            //{
            //    Array.ForEach(files, fileName =>
            //                      {
            //                          var destFileName = path + @"\" + fileName + ".dll";
            //                          if (!File.Exists(destFileName))
            //                          {
            //                              File.Copy(string.Format(sourcePath, fileName), destFileName);
            //                          }
            //                      }
            //        );
            //}

            return BinaryPath;
        }
    }
}