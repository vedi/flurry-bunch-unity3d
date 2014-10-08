using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MiniJSON;

public class NativeGateway {

	private static Dictionary<string, object> createDictForMethod(string bunch, string methodName) {
		return new Dictionary<string, object>() {{"bunch", bunch}, {"method", methodName}};
	}
	
	private static Dictionary<string, object> addParams(Dictionary<string, object> dict, Dictionary<string, object> parameters) {
		dict.Add("params", parameters);
		return dict;
	}
	
	private static Dictionary<string, object> dispatch(Dictionary<string, object> dict) {
		string str = serialize(dict);
		string retStr = dispatch(str);
		return deserialize(retStr);
	}

	private static string serialize(Dictionary<string, object> dict) {
		return Json.Serialize(dict);
	}

	private static Dictionary<string, object> deserialize(string str) {
		return Json.Deserialize(str) as Dictionary<string,object>;
	}
	
	public static Dictionary<string, object> dispatch(string bunch, string method, Dictionary<string, object> parameters) {
		Dictionary<string, object> dict = NativeGateway.createDictForMethod(bunch, method);
		if (parameters == null) {
			parameters = new Dictionary<string, object> ();
		}
		return dispatch(NativeGateway.addParams(dict, parameters));
	}
	
	#if UNITY_ANDROID && !UNITY_EDITOR
	private static AndroidJavaClass cls_UnityGatewayAdapter = new AndroidJavaClass("com.vedidev.nativebridge.unity3d.UnityGatewayAdapter");

	private static string dispatch(string strParams) {
		return cls_UnityGatewayAdapter.CallStatic<string>("dispatch", strParams);
	}
	#elif UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern string dispatch(string strParams);
	#else
	private static string dispatch(string strParams) {
		Debug.Log("call send with strParams: " + strParams);
		return null;
	}
	#endif

}
