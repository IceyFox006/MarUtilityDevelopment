/*
 * Marlow Greenan
 * Created: 10/25/2026
 * Last Updated: 10/27/2026
 * 
 * Creates a dialogue graph asset.
 */
using System;
using Unity.GraphToolkit.Editor;
using UnityEditor;

[Serializable]
[Graph(AssetExtension)]
public class DialogueGraph : Graph
{
    public const string AssetExtension = "dialoguegraph";

    [MenuItem("Assets/Create/Dialogue Graph", false)]
    private static void CreateAssetFile()
    {
        GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialogueGraph>();
    }
}
