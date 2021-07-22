using System;
using System.IO;
using System.Collections.Generic;

namespace AutoLayout
{
    public static class KeymapModifier
    {
        private const string IncludeString = "#include \"pattrigue_danish.h\"";
        private const string ProcessInputString = "  if (!process_record_user_danish(keycode, record)) return false;";
        private const string MatrixScanString = "void matrix_scan_user(void) {\ncheck_danish_mod_tap_timers();\n}";
        private const string CFileName = "pattrigue_danish.c";

        public static void InsertCode(FileInfo file)
        {
            List<string> lines = new(File.ReadAllLines(file.FullName));

            InsertCFile(file.DirectoryName);

            InsertInclude(lines);
            InsertProcessInput(lines);
            InsertMatrixScan(lines);

            File.WriteAllLines(file.FullName, lines);
        }

        private static void InsertInclude(List<string> lines)
        {
            string previousLine = string.Empty;

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                if (line == string.Empty && previousLine.StartsWith("#include"))
                {
                    lines.Insert(i, IncludeString);
                    return;
                }

                previousLine = line;
            }

            throw new Exception();
        }

        private static void InsertProcessInput(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                if (line == "bool process_record_user(uint16_t keycode, keyrecord_t *record) {")
                {
                    lines.Insert(i + 1, ProcessInputString);
                    return;
                }
            }

            throw new Exception();
        }

        private static void InsertMatrixScan(List<string> lines)
        {
            lines.Add(MatrixScanString);
        }

        private static void InsertCFile(string directory)
        {
            string cFile = $"{AppDomain.CurrentDomain.BaseDirectory}{CFileName}";
            string destination = $"{directory}\\{CFileName}";

            File.Copy(cFile, destination);
        }
    }
}