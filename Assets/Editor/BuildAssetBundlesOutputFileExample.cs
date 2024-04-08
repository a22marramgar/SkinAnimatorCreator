using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class BuildAssetBundlesOutputFileExample
{
    [MenuItem("Assets/AssetBundle Output File Example")]
    static void BuildAndPrintOutputFiles()
    {
        var bundleDefinitions = new AssetBundleBuild[]
        {
            new AssetBundleBuild
            {
                assetBundleName = "mybundle",
                assetNames = Directory.EnumerateFiles("Assets/Resources").ToArray()
            }
        };
        string buildOutputDirectory = "build";
        Directory.CreateDirectory(buildOutputDirectory);
        BuildAssetBundlesParameters buildInput = new()
        {
            outputPath = buildOutputDirectory,
            bundleDefinitions = bundleDefinitions
        };
        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(buildInput);
        if (manifest != null)
        {
            var outputFiles = Directory.EnumerateFiles(buildOutputDirectory, "*", SearchOption.TopDirectoryOnly);
            //Expected output (on Windows):
            //  Output of the build:
            //      build\build
            //      build\build.manifest
            //      build\mybundle
            //      build\mybundle.manifest
            Debug.Log("Output of the build:\n\t" + string.Join("\n\t", outputFiles));
        }
    }
}