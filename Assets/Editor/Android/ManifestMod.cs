using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Xml;
using System.Text;
using System.Linq;

public static class ManifestMod {

	[PostProcessBuild]
	public static void OnPostProcessBuild(BuildTarget target, string path) {
		if (target == BuildTarget.Android) {
			var outputFile = Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");
			
			// only copy over a fresh copy of the AndroidManifest if one does not exist
			if (!File.Exists(outputFile))
			{
				var inputFile = Path.Combine(EditorApplication.applicationContentsPath, "PlaybackEngines/androidplayer/AndroidManifest.xml");
				File.Copy(inputFile, outputFile);
			}
			UpdateManifest(outputFile);
		}
	}

	public static void UpdateManifest(string fullPath)
	{

		XmlDocument doc = new XmlDocument();
		doc.Load(fullPath);
		
		if (doc == null)
		{
			Debug.LogError("Couldn't load " + fullPath);
			return;
		}
		
		XmlNode manNode = FindChildNode(doc, "manifest");
		XmlNode dict = FindChildNode(manNode, "application");
		
		if (dict == null)
		{
			Debug.LogError("Error parsing " + fullPath);
			return;
		}
		
		string ns = dict.GetNamespaceOfPrefix("android");



		//<meta-data android:name="com.google.android.gms.version" android:value="@integer/google_play_services_version" />
		XmlElement gmsVersionElement = FindElementWithAndroidName("meta-data", "name", ns, "com.google.android.gms.version", dict);
		if (gmsVersionElement == null)
		{
			gmsVersionElement = doc.CreateElement("meta-data");
			gmsVersionElement.SetAttribute("name", ns, "com.google.android.gms.version");
			dict.AppendChild(gmsVersionElement);
		}
		gmsVersionElement.SetAttribute("value", ns, "@integer/google_play_services_version");
		
		//<uses-permission android:name="android.permission.INTERNET" />
		XmlElement internetPermissionElement = FindElementWithAndroidName("uses-permission", "name", ns, "android.permission.INTERNET", manNode);
		if (internetPermissionElement == null) {
			internetPermissionElement = doc.CreateElement("uses-permission");
			internetPermissionElement.SetAttribute("name", ns, "android.permission.INTERNET");
			manNode.AppendChild(internetPermissionElement);
		}

		//<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE"/>
		XmlElement networkStatePermissionElement = FindElementWithAndroidName("uses-permission", "name", ns, "android.permission.ACCESS_NETWORK_STATE", manNode);
		if (networkStatePermissionElement == null) {
			networkStatePermissionElement = doc.CreateElement("uses-permission");
			networkStatePermissionElement.SetAttribute("name", ns, "android.permission.ACCESS_NETWORK_STATE");
			manNode.AppendChild(networkStatePermissionElement);
		}

		doc.Save(fullPath);
	}

	private static XmlNode FindChildNode(XmlNode parent, string name)
	{
		XmlNode curr = parent.FirstChild;
		while (curr != null)
		{
			if (curr.Name.Equals(name))
			{
				return curr;
			}
			curr = curr.NextSibling;
		}
		return null;
	}
	
	private static XmlElement FindElementWithAndroidName(string name, string androidName, string ns, string value, XmlNode parent)
	{
		var curr = parent.FirstChild;
		while (curr != null)
		{
			if (curr.Name.Equals(name) && curr is XmlElement && ((XmlElement)curr).GetAttribute(androidName, ns) == value)
			{
				return curr as XmlElement;
			}
			curr = curr.NextSibling;
		}
		return null;
	}
}
