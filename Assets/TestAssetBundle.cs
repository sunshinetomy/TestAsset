using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TestAssetBundle : MonoBehaviour
{
	[SerializeField] private Image _testImage = null;

	// Start is called before the first frame update
	void Start()
	{
		//StartCoroutine( DownloadAssetBundle_02("Icon_Com_Shop_Coin_002", typeof(Sprite), (asset) => _testImage.sprite = asset as Sprite ) );
		StartCoroutine( DownloadAssetBundle_02("Icon_Com_Shop_Coin_002", typeof(Sprite), (asset) => _testImage.sprite = asset as Sprite ) );
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
	IEnumerator DownloadAssetBundle_02 ( string assetName, Type type, Action<object> onLoaded ) {
	    while (!Caching.ready)
	        yield return null;
	    // Start a download of the given URL
	    WWW www = WWW.LoadFromCacheOrDownload (url, 0);
	
	    // Wait for download to complete
	    yield return www;
	
	    // Load and retrieve the AssetBundle
	    AssetBundle bundle = www.assetBundle;
	
	    // Load the object asynchronously
	    //AssetBundleRequest request = bundle.LoadAssetAsync ("myObject", typeof(GameObject));
		AssetBundleRequest request = bundle.LoadAssetAsync ("Icon_Com_Shop_Coin_002", typeof(Sprite));
	
	    // Wait for completion
	    yield return request;
	
	    // Get the reference to the loaded object
	    //GameObject obj = request.asset as GameObject;

		//_testImage.sprite = request.asset as Sprite;

		onLoaded( request.asset );
	
	    // Unload the AssetBundles compressed contents to conserve memory
	    //bundle.Unload(false);
	
	    // Frees the memory from the web stream
	    www.Dispose();
	}


	public string url;
	public int version;
	AssetBundle bundle;
	void OnGUI (){
	    if ( GUILayout.Button ("Download bundle") )
		{
	        bundle = AssetBundleManager.getAssetBundle (url, version);
	        if(!bundle)
	            StartCoroutine (DownloadAB());
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
