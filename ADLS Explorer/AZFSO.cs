using System;
using System.Collections.Generic;
using System.IO;

namespace ADLS_Explorer
{
    public class AZFSO : IComparable
    {
        static Dictionary<string, string> _fileTypes = new Dictionary<string, string>();

        public long ContentLength { get; set; }

        public string ETag { get; set; }

        public string Group { get; set; }

        public bool IsDirectory { get; set; }

        public DateTime LastModified { get; set; }

        
        /// <summary>
        /// From AZ, this is the full path to file or directory
        /// </summary>
        public string Name { get; set; }

        public string Owner { get; set; }

        public string Permissions { get; set; }

        public int CompareTo(object obj)
        {
            var comp = obj as AZFSO;
            int ret = -IsDirectory.CompareTo(comp.IsDirectory);
            if (ret == 0)
                ret = Name.CompareTo(comp.Name);

            return ret;
        }


        public List<AZFSO> Children { get; } = new List<AZFSO>();
     
        public bool ChildrenLoaded { get; set; }
        
        /// <summary>
        /// Calculated file name (or directory name)
        /// </summary>
        public string Filename => Path.GetFileName(Name);

        public string Extension
        {
            get
            {
                var ret = (Path.GetExtension(Name) + string.Empty).ToLower();
                if (string.IsNullOrWhiteSpace(ret))
                    ret = ".file";
                return ret;
            }
        }

        public string FileType
        {
            get
            {
                if (IsDirectory)
                    return null;

                if (Extension == ".file")
                    return "file";

                if (!_fileTypes.TryGetValue(Extension, out string ret))
                    _fileTypes[Extension] = ret = NativeMethods.GetFileType(Extension);

                return ret;
            }
        }
        
        public string SizeString
        {
            get
            {
                if (IsDirectory)
                    return null;

                double size = ContentLength;
                int idx = 0;
                while (size >= 1000)
                {
                    size /= 1024;
                    idx++;
                }

                string ss = idx == 0 ? $"{size:0}" : $"{size:0.00}";

                var exts = new string[] { " B", "KB", "MB", "GB", "TB", "PB", "XB", "YB", "ZB" };

                return $"{ss} {exts[idx]}";
            }
        }

        public string SafeLocalFilename
        {
            get
            {
                string ret = Filename;

                if (IsDirectory)
                {
                    foreach (char c in Path.GetInvalidPathChars())
                        ret = ret.Replace(c, '_');

                    //Windows directories cannot end with .
                    if (ret.EndsWith('.'))
                        ret += '_';
                }
                else
                {
                    foreach (char c in Path.GetInvalidFileNameChars())
                        ret = ret.Replace(c, '_');
                }

                return ret;
            }
        }

        public override string ToString() => Name;
    }
}
