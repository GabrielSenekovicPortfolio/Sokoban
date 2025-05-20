using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsidianFileInfo
{
    public string FilePath { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
    public string BaseName { get; set; }

    public Dictionary<string, List<string>> Links { get; set; } = new Dictionary<string, List<string>>();
    public string BroadText { get; set; }
}