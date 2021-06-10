using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIKeys : MonoBehaviour {


	public ReflectionProbe[] probes;

	private ThemeManager ScifiThemeManager;

	public GameObject CameraPositions;
	public Light sunLight;

	private Transform [] tPorts;
	private int portCount = 1;
	private GameObject [] emissionGOs;
	private GameObject [] nightLights;
	private string defaultFogColor = "A2C8D1";
	private bool lastLightsOn = true;





	// Use this for initialization
	void Start () {
		tPorts = CameraPositions.GetComponentsInChildren<Transform> ();
		
		ScifiThemeManager = GetComponentInChildren<ThemeManager>();

		emissionGOs = GameObject.FindGameObjectsWithTag ("Emission");
		nightLights = GameObject.FindGameObjectsWithTag ("NightLights");

		ScifiThemeManager.setTheme(0);
		ScifiThemeManager.setPostProcessingProfile(0);
		RenderSettings.fog = true;
		RenderSettings.fogDensity = 0.005f;
		RenderSettings.fogColor = ColorConverter.HexToColor (defaultFogColor);
		RenderSettings.ambientIntensity = 1f;
		RenderSettings.reflectionIntensity = 1f;
		NightLightsOn (false);
	}


	private void Teleport(int tPortIndex) {

		Transform destination = tPorts [tPortIndex];
		Camera.main.transform.position = destination.position;
		Camera.main.transform.rotation = destination.rotation;

	}

	private void NightLightsOn(bool lightsOn) {

		if (lightsOn == lastLightsOn)
			return;

		for (int i = 0; i < emissionGOs.Length; i++) {
			emissionGOs [i].SetActive (lightsOn);

		}

		for (int i = 0; i < nightLights.Length; i++) {
			nightLights [i].GetComponent<Light>().enabled = lightsOn;
		}

		sunLight.enabled = lightsOn?false:true;

		lastLightsOn = lightsOn;
	
	}

	// Update is called once per frame
	void Update () {



		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			ScifiThemeManager.setTheme(0);
			ScifiThemeManager.setPostProcessingProfile(0);
			RenderSettings.fogDensity = 0.005f;
			RenderSettings.fogColor = ColorConverter.HexToColor (defaultFogColor);
			RenderSettings.ambientIntensity = 1f;
			RenderSettings.reflectionIntensity = 1f;
			NightLightsOn (false);
		}

		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			ScifiThemeManager.setTheme(1);
			ScifiThemeManager.setPostProcessingProfile(1);
			RenderSettings.fogDensity = 0.005f;
			RenderSettings.fogColor = ColorConverter.HexToColor (defaultFogColor);
			RenderSettings.ambientIntensity = 1f;
			RenderSettings.reflectionIntensity = 1f;
			NightLightsOn (false);
		}

		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			ScifiThemeManager.setTheme(0);
			ScifiThemeManager.setPostProcessingProfile(1);
			RenderSettings.fogDensity = 0.02f;
			RenderSettings.fogColor = ColorConverter.HexToColor (defaultFogColor);
			RenderSettings.ambientIntensity = 1f;
			RenderSettings.reflectionIntensity = 1f;
			NightLightsOn (false);
		}

		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			ScifiThemeManager.setTheme(1);
			ScifiThemeManager.setPostProcessingProfile(2);
			RenderSettings.fogDensity = 0.005f;
			RenderSettings.fogColor = ColorConverter.HexToColor (defaultFogColor);
			RenderSettings.ambientIntensity = 1f;
			RenderSettings.reflectionIntensity = 1f;
			NightLightsOn (false);
		}

		if (Input.GetKeyDown(KeyCode.Alpha5)) {
			ScifiThemeManager.setTheme(1);
			ScifiThemeManager.setPostProcessingProfile(0);
			RenderSettings.fogDensity = 0.0075f;
			RenderSettings.fogColor = ColorConverter.HexToColor ("556A6F");
			RenderSettings.ambientIntensity = 1f;
			RenderSettings.reflectionIntensity = 0.6f;
			NightLightsOn (true);
		}

		if (Input.GetKeyDown(KeyCode.Alpha6) ) {
			ScifiThemeManager.setTheme(2);
			ScifiThemeManager.setPostProcessingProfile(0);
			RenderSettings.fogDensity = 0.005f;
			RenderSettings.fogColor = ColorConverter.HexToColor (defaultFogColor);
			RenderSettings.ambientIntensity = 1f;
			RenderSettings.reflectionIntensity = 1f;
			NightLightsOn (false);
		}

		if (Input.GetKeyDown(KeyCode.Alpha7)) {
			ScifiThemeManager.setTheme(3);
			ScifiThemeManager.setPostProcessingProfile(0);
			RenderSettings.fogDensity = 0.005f;
			RenderSettings.fogColor = ColorConverter.HexToColor (defaultFogColor);
			RenderSettings.ambientIntensity = 1f;
			RenderSettings.reflectionIntensity = 1f;
			NightLightsOn (false);
		}




		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			portCount--;
			if (portCount == 0)
				portCount = tPorts.Length - 1;

			Teleport (portCount);

		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			portCount++;
			if (portCount == tPorts.Length)
				portCount = 1;

			Teleport (portCount);
		}



		if (Input.GetKeyDown(KeyCode.Escape)) {
			Screen.fullScreen = false;
			Cursor.visible = true;
		}


	}
}
