using UnityEngine;
using System.Collections.Generic;

public class BunchManager {

	private static string MANAGER_BUNCH = "BunchManager";

	public static void registerBunch(string bunch) {
		NativeGateway.dispatch(
			MANAGER_BUNCH,
			"registerBunch",
			new Dictionary<string, object> () {{"bunch", bunch}}
		);
	}
}
