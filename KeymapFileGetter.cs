using System.IO;

namespace AutoLayout
{
    public sealed class KeymapFileGetter
    {
        private const string KeymapFileName = "keymap.c";

        public FileInfo GetKeymapFile(DirectoryInfo directory)
        {
            FileInfo[] files = directory.GetFiles("*.c", SearchOption.AllDirectories);

            foreach (FileInfo file in files)
            {
                if (file.Name == KeymapFileName)
                {
                    return file;
                }
            }

            return null;
        }
    }
}