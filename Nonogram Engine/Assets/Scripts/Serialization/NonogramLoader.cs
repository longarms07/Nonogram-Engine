using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class NonogramLoader
{
    [MenuItem("NonogramEditor/Load All of a Folder's Nonograms Into a List")]    
    
    public static void LoadNonogramList()
    {
        // Choose the folder of nonograms to load and get the list of jsons 
        string nonogramDirectory = EditorUtility.OpenFolderPanel("Select Directory", "", "");
        DirectoryInfo nonogramDirectoryInfo = new DirectoryInfo(nonogramDirectory);

        // Choose a scriptable object to save to
        string assetFilepath = EditorUtility.SaveFilePanel("Select or create an asset to save to", "Assets/Puzzles/", "Nonograms", "asset");
        if (string.IsNullOrEmpty(assetFilepath)) 
        {
            EditorUtility.DisplayDialog("Error Loading Nonograms!", "No file was selected", "Ok");
            return;
        }
        assetFilepath = assetFilepath.Substring(assetFilepath.IndexOf("Assets/")); // Need to remove everything before assets to save properly

        Debug.Log(assetFilepath);

        List<NonogramData> nonogramDatas = new List<NonogramData>();

        foreach (FileInfo jsonFile in nonogramDirectoryInfo.GetFiles("*.json"))
        {
            if (CheckForNonogram(Path.Combine(jsonFile.DirectoryName, jsonFile.Name), out NonogramData parsedNonogram))
            {
                nonogramDatas.Add(parsedNonogram);
            }
        }

        NonogramList nonogramList = ScriptableObject.CreateInstance<NonogramList>();
        nonogramList.SetNonogramData(nonogramDatas);

        AssetDatabase.CreateAsset(nonogramList, assetFilepath);
        AssetDatabase.SaveAssets();

        EditorUtility.DisplayDialog("Loading Nonograms Complete!", $"Loaded Nonograms from {nonogramDirectory} into {assetFilepath}", "Ok");
    }

    private static bool CheckForNonogram(string jsonFilename, out NonogramData nonogramData)
    {
        nonogramData = null;
        string json = File.ReadAllText(jsonFilename);
        Debug.Log(json);
        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError($"{jsonFilename} was empty!");
            return false;
        }
        NonogramData parsedData = JsonUtility.FromJson<NonogramData>(json);
        if (parsedData != null)// && parsedData.IsValid())
        {
            Debug.Log($"Parsed nonogram in {jsonFilename}: {json}");
            nonogramData = parsedData;
            return true;
        }
        Debug.LogError($"Failed to parse valid Nonogram data from {jsonFilename}");
        return false;
    }

}
