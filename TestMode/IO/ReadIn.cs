using System;
using System.Collections.Generic;
using System.IO;
using static DuszaCompetitionApplication.Utility;

namespace DuszaCompetitionApplication.IO;

public static class ReadIn
{
    public static string[] ReadFile(string path)
    {
        path = path + @"\in.txt";
        string[] lines = { };
        List<string> contents = new List<string>();

        foreach (string line in File.ReadLines(path))
        {
            if (!string.IsNullOrWhiteSpace(line)) contents.Add(line);
        }

        return contents.ToArray();
    }
}
