using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using ModestTree;

public static class CreateObsidianFromScripts
{
    static readonly Regex classRegex = new Regex(@"\bclass\s+(\w+)\s*:\s*(\w+)");
    static readonly Regex enumRegex = new Regex(@"\benum\s+(\w+)\s*");
    static readonly Regex fieldInjectPattern = new Regex(@"\[Inject\]s*(public|private|protected)?\s*(?:readonly\s+)?(\w+)\s+\w+");
    static readonly Regex ctorInjectPattern = new Regex(@"\[Inject\]s*public\s+\w+\s*\(([^)]*)\)");

    static readonly Dictionary<string, string> inheritanceTags = new Dictionary<string, string>()
    {
        {"MonoBehaviour", "component" },
        {"ScriptableObject", "scriptableObject" },
        {"EditorWindow", "window" },
        {"Editor", "editor" }
    };
    static readonly List<string> folderNames = new List<string>()
    {
        "Audio",
        "Meta",
        "Objects",
        "Entitites",
        "UI",
        "Tiles"
    };
    public static void Create(string outputDirectory, List<string> logLines)
    {
        logLines.Clear();
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
            logLines.Add("Created vault directory: " + outputDirectory);
        }

        string projectPath = Application.dataPath + "/Scripts";
        string[] csFiles = Directory.GetFiles(projectPath, "*.cs", SearchOption.AllDirectories);

        var inheritanceMap = new Dictionary<string, string>();
        var classFileMap = new Dictionary<string, string>();
        var tagMap = new Dictionary<string, List<string>>();

        foreach (string file in csFiles)
        {
            string content = File.ReadAllText(file);
            foreach (Match match in classRegex.Matches(content))
            {
                string className = match.Groups[1].Value;
                string baseName = match.Groups[2].Value;
                tagMap[className] = new List<string>();
                if (!inheritanceMap.ContainsKey(className))
                {
                    inheritanceMap.Add(className, baseName);
                    classFileMap.Add(className, file);
                    logLines.Add($"Found class {className} inheriting from {baseName}.");
                }
            }
            foreach (Match match in enumRegex.Matches(content))
            {
                string className = match.Groups[1].Value;
                tagMap[className] = new List<string>();
                if (!classFileMap.ContainsKey(className))
                {
                    classFileMap.Add(className, file);
                    logLines.Add($"Found enum {className}");
                    tagMap[className].Add("enum");
                }
            }
        }
        var zenjectMap = new Dictionary<string, List<string>>();
        foreach (string className in classFileMap.Keys)
        {
            zenjectMap[className] = new List<string>();
            string content = File.ReadAllText(classFileMap[className]);
            var injectedTypes = new HashSet<string>();
            foreach (Match m in fieldInjectPattern.Matches(content))
            {
                var type = m.Groups[2].Value;
                injectedTypes.Add(type);
            }
            foreach (Match m in ctorInjectPattern.Matches(content))
            {
                var paramList = m.Groups[1].Value;
                var parts = paramList.Split(',');
                foreach (var p in parts)
                {
                    var tokens = p.Trim().Split(' ');
                    if (tokens.Length >= 2)
                    {
                        var type = tokens[0];
                        injectedTypes.Add(type);
                    }
                }
            }
            zenjectMap[className].AddRange(injectedTypes);
        }
        GetReferences(classFileMap, inheritanceMap, zenjectMap, out var referencesMap);
        foreach (var kvp in inheritanceMap)
        {
            for (int i = 0; i < inheritanceTags.Count; i++)
            {
                if (AddInheritanceTag(kvp, tagMap, inheritanceMap, inheritanceTags.ElementAt(i).Key, inheritanceTags.ElementAt(i).Value))
                {
                    break;
                }
            }
        }

        CollectVaultData(inheritanceMap, referencesMap, zenjectMap, classFileMap, tagMap, outputDirectory,
            out List<ObsidianFileInfo> markdownFIles, out HashSet<string> directoriesToCreate);
        ObsidianUtilities.CreateVault(markdownFIles, directoriesToCreate);
    }
    static void GetReferences(Dictionary<string, string> classFileMap,
        Dictionary<string, string> inheritanceMap,
        Dictionary<string, List<string>> zenjectMap,
        out Dictionary<string, List<string>> referencesMap)
    {
        referencesMap = new Dictionary<string, List<string>>();
        foreach (string className in classFileMap.Keys)
        {
            referencesMap[className] = new List<string>();
            string filePath = classFileMap[className];
            string content = File.ReadAllText(filePath);
            foreach (var otherClass in classFileMap.Keys)
            {
                if (otherClass == className) continue;
                if (inheritanceMap.ContainsKey(className) &&
                    inheritanceMap[className].Contains(otherClass)) continue;
                if (zenjectMap[className].Contains(otherClass)) continue;
                var pat = $"(?<![A-Za-z0-9_]){Regex.Escape(otherClass)}(?![A-Za-z0-9_])";
                if (Regex.IsMatch(content, pat, RegexOptions.Compiled))
                {
                    referencesMap[className].Add(otherClass);
                }
            }
        }
    }
    static bool AddInheritanceTag(KeyValuePair<string, string> kvp,
        Dictionary<string, List<string>> tagMap,
        Dictionary<string, string> inheritanceMap,
        string baseClass,
        string tag)
    {
        inheritanceMap.TryGetValue(kvp.Key, out var parent);
        string endPoint = "";
        while (endPoint.IsEmpty())
        {
            if (!inheritanceMap.TryGetValue(parent, out var newParent))
            {
                endPoint = parent;
            }
            else
            {
                parent = newParent;
            }
        }
        if (endPoint == baseClass)
        {
            tagMap[kvp.Key].Add(tag);
        }
        return parent == baseClass;
    }
    static void CollectVaultData(
    Dictionary<string, string> inheritanceMap,
    Dictionary<string, List<string>> referencesMap,
    Dictionary<string, List<string>> injectionsMap,
    Dictionary<string, string> scriptPaths,
    Dictionary<string, List<string>> tagMap,
    string outputDirectory,
    out List<ObsidianFileInfo> markdownFiles,
    out HashSet<string> directoriesToCreate)
    {
        string classesDir = Path.Combine(outputDirectory, "Classes");
        markdownFiles = new List<ObsidianFileInfo>();
        directoriesToCreate = new HashSet<string> { classesDir };
        var folderCache = new Dictionary<string, string>();

        foreach (var kvp in scriptPaths)
        {
            string className = kvp.Key;
            inheritanceMap.TryGetValue(className, out string baseName);
            string targetSubDir = classesDir;

            if (File.Exists(kvp.Value))
            {
                var dirInfo = Directory.GetParent(kvp.Value);
                while (dirInfo != null && dirInfo.FullName != Application.dataPath)
                {
                    if (folderNames.Contains(dirInfo.Name))
                    {
                        if (!folderCache.TryGetValue(dirInfo.Name, out var cachedPath))
                        {
                            var newDir = Path.Combine(classesDir, dirInfo.Name);
                            directoriesToCreate.Add(newDir);
                            folderCache[dirInfo.Name] = newDir;
                            cachedPath = newDir;
                        }
                        targetSubDir = cachedPath;
                        break;
                    }
                    dirInfo = dirInfo.Parent;
                }
            }

            string mdPath = Path.Combine(targetSubDir, className + ".md");
            var markdownFile = new ObsidianFileInfo
            {
                FilePath = mdPath,
                BaseName = baseName
            };

            if (tagMap.TryGetValue(className, out var tags))
            {
                markdownFile.Tags.AddRange(tags);
            }

            if (referencesMap.TryGetValue(className, out var refs))
            {
                markdownFile.Links.Add("reference", new List<string>());
                markdownFile.Links["reference"].AddRange(refs);
            }

            if (injectionsMap.TryGetValue(className, out var injections))
            {
                markdownFile.Links.Add("injection", new List<string>());
                markdownFile.Links["injection"].AddRange(injections);
            }

            markdownFiles.Add(markdownFile);
        }
    }
}
