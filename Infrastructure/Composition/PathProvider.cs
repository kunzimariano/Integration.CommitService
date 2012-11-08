using System;
using System.IO;
using System.Reflection;

namespace Infrastructure.Composition
{
    public static class PathProvider
    {
        public static string GetPath()
        {
            var doCopy = true;
            //doCopy = false;

            var file = Assembly.GetExecutingAssembly().Location;
            var path = new FileInfo(file).DirectoryName;

            const string sourcePath = @"C:\Users\JGough\Documents\Visual Studio 2010\Projects\CommitService\{0}\bin\Debug\{0}.dll";

            var files = new[]
                            {
                                "GitHubCommitAttemptTranslator",
                                "BitBucketCommitAttemptTranslator"
                            };
            
            if (doCopy) // Shamlesss temphackery.
            {
                Array.ForEach(files, fileName =>
                                  {
                                      var destFileName = path + @"\" + fileName + ".dll";
                                      if (!File.Exists(destFileName))
                                      {
                                          File.Copy(string.Format(sourcePath, fileName), destFileName);
                                      }
                                  }
                    );
            }

            //path = @"C:\Projects\EpixModular\FieldReporting.Ui.Mvc\bin\";
            return path;
        }
    }
}