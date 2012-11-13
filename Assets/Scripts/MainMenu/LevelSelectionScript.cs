using UnityEngine;
using System.Collections;

public class LevelSelectionScript : MonoBehaviour {
	
	GUISkin newSkin = new GUISkin();
	private bool showList= false;
	private int listEntry= 0;
	private GUIStyle listStyle;
	private Texture2D DropDownMenuBox;
	private GUIContent[] comboBoxList = new GUIContent[2];
	private Popup cb = new Popup();
	
	private void Start(){
	    comboBoxList = new GUIContent[5];
	    comboBoxList[0] = new GUIContent("Thing 1");
	    comboBoxList[1] = new GUIContent("Thing 2");
	    comboBoxList[2] = new GUIContent("Thing 3");
	    comboBoxList[3] = new GUIContent("Thing 4");
	    comboBoxList[4] = new GUIContent("Thing 5");

	}
	
	void levelSelectMenu() {
		
		GlobalSettings gs = new GlobalSettings();
		
		//layout start
	    GUI.BeginGroup(new Rect(Screen.width / 2 - 200, 50, 400, 300));
	    
	    //boxes
	    GUI.Box(new Rect(0, 0, 400, 300), "");
		
	    
	    //level preview/icon
	    //GUI.Label(new Rect(100, 20, 198, 198), "");
	    
		int selectedItemIndex = cb.GetSelectedItemIndex();
	    selectedItemIndex = cb.List(new Rect(50, 100, 100, 20), comboBoxList[selectedItemIndex].text, comboBoxList, listStyle );
	    if(cb.isClickedComboButton) {
	    	gs.setFileAddress(selectedItemIndex);
			Application.LoadLevel("main");
	    }
	    if(GUI.Button(new Rect(205, 250, 180, 40), "go back")) {
			MainMenuScript script = GetComponent<MainMenuScript>(); 
		    script.enabled = true;
		    LevelSelectionScript script2 = GetComponent<LevelSelectionScript>(); 
		    script2.enabled = false;
	    }
	    //layout end
	    GUI.EndGroup(); 
	}
	
	void OnGUI () {
	    //load GUI skin 
	    GUI.skin = newSkin;
		
	    //execute theMapMenu function
	    levelSelectMenu();
	}
}
