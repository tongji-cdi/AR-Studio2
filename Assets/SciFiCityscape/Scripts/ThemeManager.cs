using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public interface MatPropertySet {}


public class MatPropertySetBase : MatPropertySet{

	public Color ColorTint;
	public float BaseSmoothnessMultiplier;
	public float BaseMetallic;


}




public class SciFiTheme {

	public static int NUM_BASEMAT = 3;
	public MatPropertySetBase[] MatBaseProperties = new MatPropertySetBase[NUM_BASEMAT];
	public Color DecalColor;

}

/*
 * 
 * ThemeManager class
 * 
 */
public class ThemeManager : MonoBehaviour {

	public Material[] BaseMaterial_01 = new Material[4];
	public Material[] BaseMaterial_02 = new Material[2];
	public Material[] BaseMaterial_03 = new Material[1];

	public Material DecalMaterial;

	public PostProcessingProfile[] PPProfiles;


	private SciFiTheme[] scifiThemesArray = new SciFiTheme[4];
	private Light [] streetLights;
	private Camera mainCam;


	void OnEnable () {
		
		mainCam = Camera.main;
		Initialze();

		//streetLights ; 

	}

	public void setTheme(int themeId) {

		MatPropertySetBase baseSet = scifiThemesArray[themeId].MatBaseProperties[0];

		for (int j = 0; j < BaseMaterial_01.Length; j++) {
			BaseMaterial_01[j].SetColor("_ColorTint",baseSet.ColorTint);
			BaseMaterial_01[j].SetFloat("_Metallic", baseSet.BaseMetallic);
			BaseMaterial_01[j].SetFloat("_SmoothnessMultiplier", baseSet.BaseSmoothnessMultiplier);
		}

		baseSet = scifiThemesArray[themeId].MatBaseProperties[1];

		for (int j = 0; j < BaseMaterial_02.Length; j++) {
			BaseMaterial_02[j].SetColor("_ColorTint",baseSet.ColorTint);
			BaseMaterial_02[j].SetFloat("_Metallic", baseSet.BaseMetallic);
			BaseMaterial_02[j].SetFloat("_SmoothnessMultiplier", baseSet.BaseSmoothnessMultiplier);
		}

	

		baseSet = scifiThemesArray[themeId].MatBaseProperties[2];

		for (int j = 0; j < BaseMaterial_03.Length; j++) {
			BaseMaterial_03[j].SetColor("_ColorTint",baseSet.ColorTint);
			BaseMaterial_03[j].SetFloat("_Metallic", baseSet.BaseMetallic);
			BaseMaterial_03[j].SetFloat("_SmoothnessMultiplier", baseSet.BaseSmoothnessMultiplier);
		}

		DecalMaterial.SetColor("_Color", scifiThemesArray [themeId].DecalColor);
			
}

	public void setPostProcessingProfile(int ppId) {

		PostProcessingBehaviour ppb = mainCam.GetComponent<PostProcessingBehaviour>();
		ppb.profile = PPProfiles[ppId];
	}


	/*
	 * 
	 * Initializing
	 * 
	 */


	void Initialze () {
		InitializeTheme_1();
		InitializeTheme_2();
		InitializeTheme_3();
		InitializeTheme_4();

	}

	void InitializeTheme_1 () {

		SciFiTheme scifiTheme = new SciFiTheme();

		// Color 1

		MatPropertySetBase mpsb_1 =  new MatPropertySetBase();

		mpsb_1.ColorTint = ColorConverter.HexToColor("CECECE");
		mpsb_1.BaseMetallic = 0f;
		mpsb_1.BaseSmoothnessMultiplier = 1f;

		scifiTheme.MatBaseProperties[0] = mpsb_1;

		// Color 2

		MatPropertySetBase mpsb_2 =  new MatPropertySetBase();

		mpsb_2.ColorTint = ColorConverter.HexToColor("8C8C8C");
		mpsb_2.BaseMetallic = 0f;
		mpsb_2.BaseSmoothnessMultiplier = 1f;

		scifiTheme.MatBaseProperties[1] = mpsb_2;

		// Color 3

		MatPropertySetBase mpsb_3 =  new MatPropertySetBase();

		mpsb_3.ColorTint = ColorConverter.HexToColor("FF9B37");
		mpsb_3.BaseMetallic = 0f;
		mpsb_3.BaseSmoothnessMultiplier = 1f;

		scifiTheme.MatBaseProperties[2] = mpsb_3;

		scifiTheme.DecalColor = ColorConverter.HexToColor ("636363");


		scifiThemesArray[0] = scifiTheme;

	}


	void InitializeTheme_2 () {

		SciFiTheme scifiTheme = new SciFiTheme();

		// Color 1

		MatPropertySetBase mpsb_1 =  new MatPropertySetBase();

		mpsb_1.ColorTint = ColorConverter.HexToColor("CECECE");
		mpsb_1.BaseMetallic = 1f;
		mpsb_1.BaseSmoothnessMultiplier = 1f;

		scifiTheme.MatBaseProperties[0] = mpsb_1;

		// Color 2

		MatPropertySetBase mpsb_2 =  new MatPropertySetBase();

		mpsb_2.ColorTint = ColorConverter.HexToColor("8C8C8C");
		mpsb_2.BaseMetallic = 1f;
		mpsb_2.BaseSmoothnessMultiplier = 1f;

		scifiTheme.MatBaseProperties[1] = mpsb_2;

		// Color 3

		MatPropertySetBase mpsb_3 =  new MatPropertySetBase();

		mpsb_3.ColorTint = ColorConverter.HexToColor("FF9B37");
		mpsb_3.BaseMetallic = 0f;
		mpsb_3.BaseSmoothnessMultiplier = 1f;

		scifiTheme.MatBaseProperties[2] = mpsb_3;
		scifiTheme.DecalColor = ColorConverter.HexToColor ("636363");


		scifiThemesArray[1] = scifiTheme;

	}

	void InitializeTheme_3 () {

		SciFiTheme scifiTheme = new SciFiTheme();

		// Color 1

		MatPropertySetBase mpsb_1 =  new MatPropertySetBase();



		mpsb_1.ColorTint = ColorConverter.HexToColor("FFE6C1");
		mpsb_1.BaseMetallic = 0f;
		mpsb_1.BaseSmoothnessMultiplier = 1f;

		scifiTheme.MatBaseProperties[0] = mpsb_1;

		// Color 2

		MatPropertySetBase mpsb_2 =  new MatPropertySetBase();

		mpsb_2.ColorTint = ColorConverter.HexToColor("93A3AE");
		mpsb_2.BaseMetallic = 0f;
		mpsb_2.BaseSmoothnessMultiplier = 1f;

		scifiTheme.MatBaseProperties[1] = mpsb_2;

		// Color 3

		MatPropertySetBase mpsb_3 =  new MatPropertySetBase();

		mpsb_3.ColorTint = ColorConverter.HexToColor("FF9B37");
		mpsb_3.BaseMetallic = 0f;
		mpsb_3.BaseSmoothnessMultiplier = 1f;

		scifiTheme.MatBaseProperties[2] = mpsb_3;
		scifiTheme.DecalColor = ColorConverter.HexToColor ("2881B2");

		scifiThemesArray[2] = scifiTheme;

	}

	void InitializeTheme_4 () {

		SciFiTheme scifiTheme = new SciFiTheme();

		// Color 1

		MatPropertySetBase mpsb_1 =  new MatPropertySetBase();


		mpsb_1.ColorTint = ColorConverter.HexToColor("77979E");
		mpsb_1.BaseMetallic = 0f;
		mpsb_1.BaseSmoothnessMultiplier = 1f;

		scifiTheme.MatBaseProperties[0] = mpsb_1;

		// Color 2

		MatPropertySetBase mpsb_2 =  new MatPropertySetBase();


		mpsb_2.ColorTint = ColorConverter.HexToColor("D0C9A5");
		mpsb_2.BaseMetallic = 0f;
		mpsb_2.BaseSmoothnessMultiplier = 1f;

		scifiTheme.MatBaseProperties[1] = mpsb_2;

		// Color 3

		MatPropertySetBase mpsb_3 =  new MatPropertySetBase();

		mpsb_3.ColorTint = ColorConverter.HexToColor("FF9B37");
		mpsb_3.BaseMetallic = 0f;
		mpsb_3.BaseSmoothnessMultiplier = 1f;

		scifiTheme.MatBaseProperties[2] = mpsb_3;
		scifiTheme.DecalColor = ColorConverter.HexToColor ("DDDDDD");

		scifiThemesArray[3] = scifiTheme;

	}




	

}
