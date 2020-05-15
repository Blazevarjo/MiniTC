using MiniTC.Properties;
using System;
using System.IO;
using System.Linq;


namespace MiniTC.Model
{
    class CopyingModel
    {
        public void Copy(string source, string target)
        {
            var attribute = File.GetAttributes(source);
            if (attribute.HasFlag(FileAttributes.Directory))
            {
                target = Path.Combine(target, Path.GetFileName(source));
                DirectoryCopy(source, target);
            }
            else
            {
                FileCopy(source, target);
            }
        }

        #region Auxiliary functions
        private void FileCopy(string source, string target)
        {
            if (Directory.GetFiles(target).Select(x => Path.GetFileName(x)).Contains(Path.GetFileName(source)))
            {
                int count = Directory.GetFiles(target).Select(x => Path.GetFileName(x)).Where(x => x.StartsWith(Path.GetFileNameWithoutExtension(source))).Count();
                string fileName = Path.GetFileNameWithoutExtension(source) + " - " + Resources.FileCopy + count + Path.GetExtension(source);
                target = Path.Combine(target, fileName);
            }
            else
            {
                target = Path.Combine(target, Path.GetFileName(source));
            }
            try
            {
                File.Copy(source, target);
            }
            catch (UnauthorizedAccessException) { return; }
        }

        private void DirectoryCopy(string source, string target)
        {
            var dir = new DirectoryInfo(source);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
            DirectoryInfo[] dirs;
            try
            {
                dirs = dir.GetDirectories();
            }
            catch (UnauthorizedAccessException) { return; }

            if (!Directory.Exists(target))
                Directory.CreateDirectory(target);
            else
            {
                int count = Directory.GetDirectories(Directory.GetDirectoryRoot(target)).Where(x => x.StartsWith(target)).Count();
                target = Path.Combine(Path.GetDirectoryName(target), Path.GetFileNameWithoutExtension(target) + " - " + Resources.FileCopy + count);
                Directory.CreateDirectory(target);
            }
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                string path = Path.Combine(target, file.Name);
                file.CopyTo(path);
            }
            foreach (var subdir in dirs)
            {
                string path = Path.Combine(target, subdir.Name);
                DirectoryCopy(subdir.FullName, path);
            }
        }
        #endregion
    }
}
