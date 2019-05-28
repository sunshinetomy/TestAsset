using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class TestAssetBundleBuilder : EditorWindow
{
	[MenuItem( "AssetBundle/Build Assetbundle" )]
	public static void BuildAssetBundle()
	{
		List<AssetBundleBuild> assetBundleBuildList = new List<AssetBundleBuild>()
		{
			new AssetBundleBuild()
			{
				//string bundlePath = "Assets/Bundles/Sprites";
				assetBundleName = "Sprites",
				assetNames = new string[] {
					"Assets/Bundles/Sprites/Icon_Com_Shop_Coin_002.png",
					"Assets/Bundles/Sprites/Icon_Com_Shop_Heart_002.png",
					"Assets/Bundles/Sprites/Icon_Com_Shop_Ruby_002.png"
				}
			},
			new AssetBundleBuild()
			{
				assetBundleName = "Prefabs",
				assetNames = new string[] {
					"Assets/Bundles/Image.prefab",
				}
			}
		};

		BuildPipeline.BuildAssetBundles(@"C:\workspace_EDM\TestAsset\Assets", assetBundleBuildList.ToArray(), BuildAssetBundleOptions.CompleteAssets, BuildTarget.Android);

		EditorUtility.DisplayDialog( "alert", "complete Assetbundles Build", "ok" );
	}
}
