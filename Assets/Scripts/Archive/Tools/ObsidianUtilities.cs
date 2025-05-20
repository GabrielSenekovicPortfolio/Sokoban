using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ObsidianUtilities
{
    public static void CreateVault(List<ObsidianFileInfo> markdownFiles, HashSet<string> directoriesToCreate)
    {
        foreach (var dir in directoriesToCreate)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
        foreach (var fileInfo in markdownFiles)
        {
            using (StreamWriter writer = new StreamWriter(fileInfo.FilePath))
            {
                foreach (var tag in fileInfo.Tags)
                {
                    writer.WriteLine("#" + tag);
                }

                writer.WriteLine();

                if (!string.IsNullOrEmpty(fileInfo.BaseName))
                {
                    writer.WriteLine(TaggedLink("inheritance", fileInfo.BaseName));
                }
                if(fileInfo.Links.Count > 0)
                {
                    writer.WriteLine();
                    foreach(var linkList in fileInfo.Links)
                    {
                        foreach(var link in linkList.Value)
                        {
                            if(linkList.Key != "none")
                            {
                                writer.WriteLine(TaggedLink(linkList.Key, link));
                            }
                            else
                            {
                                writer.WriteLine($"[[{link}]]");
                            }
                        }
                    }
                }
                if(fileInfo.BroadText != null)
                {
                    writer.WriteLine(fileInfo.BroadText);
                }
            }
        }

        EditorUtility.DisplayDialog("Success", "Obsidian vault generated.", "Thank you!");
    }

    private static string TaggedLink(string tag, string reference)
    {
        return $"[{tag}]({reference})";
    }
}