using System;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class FlurryBunch: MonoBehaviour {

	private static string BUNCH = "FlurryBunch";

	private static FlurryBunch INSTANCE = null;

	public static FlurryBunch GetInstance() {
		return INSTANCE;
	}
	
	public string ApiKey = "REPLACE IT!";

	void Awake() {
		if (FlurryBunch.INSTANCE == null) {
			FlurryBunch.INSTANCE = this;
			BunchManager.registerBunch(BUNCH);
			GameObject.DontDestroyOnLoad(this);
		} else {
			GameObject.Destroy(this.gameObject);
		}
	}

	void OnEnable() {
		FlurryBunch.INSTANCE.StartSession(ApiKey);
	}

	void OnDisable() {
		FlurryBunch.INSTANCE.EndSession();
	}
	
	public void StartSession(string apiKey) {
		Debug.Log("StartSession");
		NativeGateway.dispatch(
			BUNCH,
			"startSession",
			new Dictionary<string, object> () {{"apiKey", apiKey}}
		);
	}

	public void EndSession() {
		Debug.Log("EndSession");
		NativeGateway.dispatch(
			BUNCH,
			"endSession",
			null
			);
	}

	public void SetAppVersion(string appVersion) {
		NativeGateway.dispatch(
			BUNCH,
			"setAppVersion",
			new Dictionary<string, object> () {{"appVersion", appVersion}}
		);
	}
	
	public void LogEvent(string eventId, Dictionary<string, string> parameters) {
		NativeGateway.dispatch(
			BUNCH,
			"logEvent",
			new Dictionary<string, object> () {
				{"eventId", eventId},
				{"parameters", parameters}
			}
		);
	}
	
	public void LogError(string errorId, string message, Dictionary<string, string> parameters) {
		NativeGateway.dispatch(
			BUNCH,
			"logError",
			new Dictionary<string, object> () {
				{"errorId", errorId},
				{"message", message},
				{"parameters", parameters}
			}
		);
	}
	
	public void LogPageView() {
		NativeGateway.dispatch(
			BUNCH,
			"logPageView",
			null
		);
	}
	
	public void LogTimedEventBegin(string eventId, Dictionary<string, string> parameters) {
		NativeGateway.dispatch(
			BUNCH,
			"logTimedEventBegin",
			new Dictionary<string, object> () {
				{"eventId", eventId},
				{"parameters", parameters}
			}
		);
	}
	
	public void LogTimedEventEnd(string eventId, Dictionary<string, string> parameters) {
		NativeGateway.dispatch(
			BUNCH,
			"logTimedEventEnd",
			new Dictionary<string, object> () {
				{"eventId", eventId},
				{"parameters", parameters}
			}
		);
	}
	
	public void SetContinueSessionMillis(long milliseconds) {
		NativeGateway.dispatch(
			BUNCH,
			"setContinueSessionMillis",
				new Dictionary<string, object> () {
				{"milliseconds", milliseconds}
			}
		);
	}
	
	public void SetDebugLogEnabled(bool enabled) {
		NativeGateway.dispatch(
			BUNCH,
			"setDebugLogEnabled",
				new Dictionary<string, object> () {
				{"enabled", enabled}
			}
		);
	}
	
	public void SetUserId(string userId) {
		NativeGateway.dispatch(
			BUNCH,
			"setUserId",
			new Dictionary<string, object> () {
				{"userId", userId}
			}
		);
	}
	
	public void SetAge(int age) {
		NativeGateway.dispatch(
			BUNCH,
			"setAge",
			new Dictionary<string, object> () {
				{"age", age}
			}
		);
	}
	
}
