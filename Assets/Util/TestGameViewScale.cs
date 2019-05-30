using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameViewScale : MonoBehaviour
{
    void Start()
    {
		this.SetGameViewScale();
	}

	private void SetGameViewScale()
	{
#if UNITY_EDITOR
		System.Reflection.Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;
		System.Type type = assembly.GetType("UnityEditor.GameView");
		UnityEditor.EditorWindow v = UnityEditor.EditorWindow.GetWindow(type);
		
		var defScaleField = type.GetField("m_defaultScale", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
		
		//whatever scale you want when you click on play
		float defaultScale = 0.1f;
		
		var areaField = type.GetField("m_ZoomArea", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
		var areaObj = areaField.GetValue(v);
		
		var scaleField = areaObj.GetType().GetField("m_Scale", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
		scaleField.SetValue(areaObj, new Vector2(defaultScale, defaultScale));
#endif
	}
}
