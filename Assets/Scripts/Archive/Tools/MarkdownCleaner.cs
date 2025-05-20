using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class MarkdownCleaner
{
    static readonly Regex InlineImagePattern =
        new Regex(@"!\[.*?\]\(.*?\)", RegexOptions.Compiled);

    static readonly Regex RefImagePattern =
        new Regex(@"!\[.*?\]\[.*?\]", RegexOptions.Compiled);

    static readonly Regex LinkRefDefPattern =
        new Regex(@"^\s*\[[^\]]+\]:\s*\S+", RegexOptions.Multiline | RegexOptions.Compiled);

    static readonly Regex BoldPattern =
        new Regex(@"\*\*(.+?)\*\*|__(.+?)__", RegexOptions.Compiled);

    static readonly Regex ItalicPattern =
        new Regex(@"\*(?!\*)(.+?)\*|_(?!_)(.+?)_", RegexOptions.Compiled);

    static readonly Regex HtmlEmphasisPattern =
        new Regex(@"</?(?:strong|b|em|i)>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static string Clean(string markdown)
    {
        if (string.IsNullOrEmpty(markdown)) return markdown;

        markdown = InlineImagePattern.Replace(markdown, "");
        markdown = RefImagePattern.Replace(markdown, "");
        markdown = LinkRefDefPattern.Replace(markdown, "");

        markdown = BoldPattern.Replace(markdown, m =>
            !string.IsNullOrEmpty(m.Groups[1].Value)
                ? m.Groups[1].Value
                : m.Groups[2].Value
        );

        markdown = ItalicPattern.Replace(markdown, m =>
            !string.IsNullOrEmpty(m.Groups[1].Value)
                ? m.Groups[1].Value
                : m.Groups[2].Value
        );

        markdown = HtmlEmphasisPattern.Replace(markdown, "");

        return markdown;
    }
}
