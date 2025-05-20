using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System;

public static class CreateObsidianFromMarkup
{
    //TODO: Code not finished. The Obsidian still has some garbage in it
    static readonly Regex HeaderRegex = new Regex(@"^(#{1,6})\s+(.+)$", RegexOptions.Multiline);
    static readonly HashSet<string> incorporatedHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "Geography",
        "History",
        "Language", "Languages",
        "Demography", "Demographics",
        "Countries",
        "Major cities",
        "Culture",
        "Politics",
        "Administrative Regions", "Provinces",
        "Cuisine",
        "Adjectives", "Verbs", "Nouns", "Conjunctions", "Determiners", "Numbers", "Consonants", "Vowels", "Names", "Counters",
        "Demonstratives",
        "Dictionary",
        "Writing system",
        "Clothing",
        "Architecture",
        "IPA",
        "Orthography",
        "Eastern","Southern","Northern","Western","Central", "Outer", "Inner",
        "Life", "Lifeforms",
        "The Cities",
        "Organizations"
    };
    public static void Create(string markdown, string outputFolder)
    {
        var directoriesToCreate = new HashSet<string> { outputFolder };
        var text = File.ReadAllText(markdown);
        text = MarkdownCleaner.Clean(text);

        var matches = HeaderRegex.Matches(text)
            .Cast<Match>()
            .Select(m => new
            {
                Level = m.Groups[1].Value.Length,
                Title = m.Groups[2].Value.Trim(),
                Index = m.Index
            })
            .ToList();

        matches.Add(new { Level = 0, Title = (string)null, Index = text.Length });

        var nodes = new List<HeaderNode>();
        for (int i = 0; i < matches.Count - 1; i++)
        {
            var current = matches[i];
            var next = matches[i + 1];
            var content = text.Substring(current.Index + current.Title.Length + current.Level + 1,
                             next.Index - (current.Index + current.Title.Length + current.Level + 1))
                             .Trim();

            nodes.Add(new HeaderNode
            {
                Title = current.Title,
                Level = current.Level,
                Content = content
            });
        }

        CreateHeaderTree(nodes, out nodes);

        var result = new List<ObsidianFileInfo>();
        foreach (var node in nodes)
        {
            var info = new ObsidianFileInfo
            {
                BaseName = null,
                FilePath = Path.Combine(outputFolder, SanitizeFileName(node.Title) + ".md"),
                BroadText = node.Content
            };

            foreach (var child in node.Children)
            {
                if (!info.Links.ContainsKey("none"))
                {
                    info.Links["none"] = new List<string>();
                }

                info.Links["none"].Add(child.Title);
            }

            result.Add(info);
        }

        ObsidianUtilities.CreateVault(result, directoriesToCreate);
    }
    static void CreateHeaderTree(List<HeaderNode> nodes, out List<HeaderNode> remainingNodes)
    {
        remainingNodes = new List<HeaderNode>(nodes);
        var stack = new Stack<HeaderNode>();
        foreach (var node in nodes)
        {
            while (stack.Any() && stack.Peek().Level >= node.Level)
                stack.Pop();

            if (stack.Any())
            {
                var parent = stack.Peek();
                node.Parent = parent;

                if (incorporatedHeaders.Contains(node.Title))
                {
                    parent.Content += "\n" + node.Title;
                    parent.Content += "\n\n" + node.Content;
                    foreach(var child in node.Children)
                    {
                        child.Parent = parent;
                    }
                    parent.Children.AddRange(node.Children);
                    remainingNodes.Remove(node);
                    continue;
                }
                else
                {
                    parent.Children.Add(node);
                }
            }
            stack.Push(node);
        }
    }
    static string SanitizeFileName(string name)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');
        return name;
    }
    class HeaderNode
    {
        public string Title;
        public int Level;
        public string Content;
        public HeaderNode Parent;
        public List<HeaderNode> Children = new List<HeaderNode>();
    }
}