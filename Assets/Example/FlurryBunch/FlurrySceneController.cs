using UnityEngine;
using System.Collections.Generic;

public class FlurrySceneController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FlurryBunch.GetInstance().LogEvent("sceneStarted", new Dictionary<string, string>{{"name", "FlurryScene"}});
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
