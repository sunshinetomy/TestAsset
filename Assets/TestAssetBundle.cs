using System;
//using System.Diagnostics;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class TestAssetBundle : MonoBehaviour
{
	[SerializeField] private Image _testImage = null;
	[SerializeField] private Canvas _testCanvas = null;

	// Start is called before the first frame update
	void Start()
	{
		Caching.CleanCache();

		UnityEngine.Debug.Log( Application.dataPath );
		UnityEngine.Debug.Log( Application.persistentDataPath );
		UnityEngine.Debug.Log( Application.consoleLogPath );
		UnityEngine.Debug.Log( Application.streamingAssetsPath );

		UnityEngine.Debug.Log( Environment.CurrentDirectory );
	}

	private void TestPath()
	{
#if UNITY_EDITOR
		UnityEngine.Debug.Log( EditorApplication.applicationPath );
		UnityEngine.Debug.Log( EditorApplication.applicationContentsPath );
#endif
	}

	public string BundleURL;
	public string AssetName;
	IEnumerator DownloadAssetBundle()
	{
		// Download the file from the URL. It will not be saved in the Cache
		using( WWW www = new WWW( BundleURL ) )
		{
			yield return www;
			if( www.error != null )
				throw new Exception( "WWW download had an error:" + www.error );
			AssetBundle bundle = www.assetBundle;
			if( AssetName == "" )
				Instantiate( bundle.mainAsset );
			else
				Instantiate( bundle.LoadAsset( AssetName ) );
			// Unload the AssetBundles compressed contents to conserve memory
			bundle.Unload( false );

		} // memory is freed from the web stream (www.Dispose() gets called implicitly)
	}

	// Note: This example does not check for errors. Please look at the example in the DownloadingAssetBundles section for more information
	IEnumerator DownloadAssetBundle_02 ( string targetUrl, string assetName, Type type, Action<UnityEngine.Object> onLoaded ) {
	    // ready?
		while (!Caching.ready)
	        yield return null;

	    // Start a download of the given URL
	    WWW www = WWW.LoadFromCacheOrDownload(targetUrl, 0);
	
	    // Wait for download to complete
	    yield return www;
	
	    // Load and retrieve the AssetBundle
	    AssetBundle bundle = www.assetBundle;

		Debug.Log( "bundle Name-" + bundle.name );
		foreach( var asset in bundle.LoadAllAssets() )
		{
			Debug.Log( "loaded asset-" + asset.name );
		}

	    // Load the object asynchronously
	    //AssetBundleRequest request = bundle.LoadAssetAsync (assetName, typeof(GameObject));
		//AssetBundleRequest request = bundle.LoadAssetAsync (assetName, typeof(Sprite));
		AssetBundleRequest request = bundle.LoadAssetAsync (assetName, type);
		//AssetBundleRequest request = bundle.LoadAssetAsync (assetName);
	
	    // Wait for completion
	    yield return request;
	
	    // Get the reference to the loaded object
		//_testImage.sprite = request.asset as Sprite;

		UnityEngine.Debug.Log( "AssetName-"+request.asset.name );

		onLoaded( request.asset );
	
	    // Unload the AssetBundles compressed contents to conserve memory
	    bundle.Unload(false);
	
	    // Frees the memory from the web stream
	    www.Dispose();
	}

	IEnumerator DownloadAssetBundle_03 ( string targetUrl, string assetName, Action<UnityEngine.Object> onLoaded ) {
	    // ready?
		while (!Caching.ready)
	        yield return null;

	    // Start a download of the given URL
	    WWW www = WWW.LoadFromCacheOrDownload(targetUrl, 0);
	
	    // Wait for download to complete
	    yield return www;
	
	    // Load and retrieve the AssetBundle
	    AssetBundle bundle = www.assetBundle;

		Debug.Log( "bundle Name-" + bundle.name );
		foreach( var asset in bundle.LoadAllAssets() )
		{
			Debug.Log( "loaded asset-" + asset.name );
		}

	    // Load the object asynchronously
		AssetBundleRequest request = bundle.LoadAssetAsync (assetName);
	
	    // Wait for completion
	    yield return request;
	
	    // Get the reference to the loaded object
		UnityEngine.Debug.Log( "AssetName-"+request.asset.name );
		UnityEngine.Debug.Log( "AssetType-"+request.asset.GetType() );

		onLoaded( request.asset );
	
	    // Unload the AssetBundles compressed contents to conserve memory
	    bundle.Unload(false);
	
	    // Frees the memory from the web stream
	    www.Dispose();
	}


	public string url;
	public int version;
	AssetBundle bundle;
	void OnGUI (){

		GUIStyle style = new GUIStyle(GUI.skin.button);
		style.fontSize = 40;
		style.normal.textColor = Color.white;

		GUILayoutOption[] options = new GUILayoutOption[]
			{
				GUILayout.Width(400.0f),
				GUILayout.Height(150.0f)
			};

	    if(true == GUILayout.Button("Download bundle", style, options))
		{
	        bundle = AssetBundleManager.getAssetBundle (url, version);
	        if(!bundle)
	            StartCoroutine (DownloadAB());
	    }

		if(true == GUILayout.Button("Download Sprite", style, options))
		{
			Debug.Log( "click Download Sprite" );

			StartCoroutine( DownloadAssetBundle_02(
				"https://drive.google.com/uc?export=download&id=14lPKM9Ol5ancqrgNhmZ7S5-OwBsBsM5_",
				"Icon_Com_Shop_Coin_002",
				typeof(Sprite),
				(asset) => _testImage.sprite = asset as Sprite ) );
		}

		if( true == GUILayout.Button( "Download Prefab", style, options ) )
		{
			Debug.Log( "click Download Prefab" );

			StartCoroutine( DownloadAssetBundle_02(
				"https://drive.google.com/uc?export=download&id=10UvX1OX3lPRIT3XHI6UzkGAYxk14WFWW",
				"Image",
				typeof( GameObject ),
				( asset ) =>
				{
					Debug.Log( "asset type is - " + asset.GetType() );

					GameObject.Instantiate( asset as GameObject, _testCanvas.transform );
				} ) );
		}

		if( true == GUILayout.Button( "Download Sprite_03", style, options ) )
		{
			Debug.Log( "click Download Prefab" );

			StartCoroutine( DownloadAssetBundle_03(
				"https://drive.google.com/uc?export=download&id=14lPKM9Ol5ancqrgNhmZ7S5-OwBsBsM5_",
				"Icon_Com_Shop_Coin_002",
				( asset ) =>
				{
					Texture2D texture = asset as Texture2D;

					Rect rect = new Rect(0, 0, texture.width, texture.height);
					Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f)); 
					_testImage.sprite = sprite;
				} ) );
		}
	}
	IEnumerator DownloadAB (){
	    yield return StartCoroutine(AssetBundleManager.downloadAssetBundle (url, version));
	    bundle = AssetBundleManager.getAssetBundle (url, version);
	}
	void OnDisable (){
	    AssetBundleManager.Unload (url, version);
	}
}
