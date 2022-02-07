using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace ADLS_Explorer
{
    internal static class NativeMethods
    {
        private const int MAX_PATH = 260;

        private const int NAMESIZE = 80;

        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_SMALLICON = 0x01;
        private const uint SHGFI_OPENICON = 0x02;
        private const uint SHGFI_USEFILEATTRIBUTES = 0x10;
        private const uint SHGFI_TYPENAME = 0x400;

        private const uint FILE_ATTRIBUTE_DIRECTORY = 0x10;
        private const uint FILE_ATTRIBUTE_NORMAL = 0x80;



        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NAMESIZE)]
            public string szTypeName;
        };



        [DllImport("Shell32.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [DllImport("user32.dll")]
        private static extern bool DestroyIcon(IntPtr handle);

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern bool PathCompactPathEx([Out] StringBuilder pszOut, string szPath, int cchMax, int dwFlags);


        public static string CompactPath(string path, int length)
        {
            var sb = new StringBuilder(length + 1);
            PathCompactPathEx(sb, path, length + 1, 0);
            return sb.ToString();
        }


        public static Icon GetClosedFolderIcon() => GetIcon(null, FILE_ATTRIBUTE_DIRECTORY, SHGFI_ICON | SHGFI_SMALLICON);

        public static Icon GetOpenFolderIcon() => GetIcon(null, FILE_ATTRIBUTE_DIRECTORY, SHGFI_ICON | SHGFI_SMALLICON | SHGFI_OPENICON);

        public static Icon GetFileIcon(string ext) => GetIcon(ext, FILE_ATTRIBUTE_NORMAL, SHGFI_ICON | SHGFI_SMALLICON | SHGFI_USEFILEATTRIBUTES);

        private static Icon GetIcon(string ext, uint type, uint flags)
        {
            if (!string.IsNullOrWhiteSpace(ext))
                if (!ext.StartsWith("."))
                    ext = "." + ext;

            var info = new SHFILEINFO();
            SHGetFileInfo(ext, type, ref info, (uint)Marshal.SizeOf(info), flags);

            // Get a copy that doesn't use the original handle
            var ret = (Icon)Icon.FromHandle(info.hIcon).Clone();

            // Clean up native icon to prevent resource leak
            DestroyIcon(info.hIcon);

            return ret;
        }        
    
        public static string GetFileType(string ext)
        {
            if (!string.IsNullOrWhiteSpace(ext))
                if (!ext.StartsWith("."))
                    ext = "." + ext;
            
            var info = new SHFILEINFO();
            SHGetFileInfo(ext, FILE_ATTRIBUTE_NORMAL, ref info, (uint)Marshal.SizeOf(info), SHGFI_USEFILEATTRIBUTES | SHGFI_TYPENAME);

            return info.szTypeName;
        }

    }

}
