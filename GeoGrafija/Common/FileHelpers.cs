using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common
{
    public static class FileHelpers
    {
        public static bool FolderHasFile(string folderPath,string fileName)
        {
            bool hasFile = false;

            var allFileNames = Directory.GetFiles(folderPath).Select(x=>Path.GetFileName(x).ToLower());

            hasFile = allFileNames.Contains(fileName.ToLower());

            return hasFile;
        }
    }
}
