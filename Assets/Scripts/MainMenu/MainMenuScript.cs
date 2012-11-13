using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	GUISkin newSkin = new GUISkin();
	Texture2D logoTexture;
	
	void theFirstMenu() {
		
	    LevelSelectionScript script2 = GetComponent<LevelSelectionScript>(); 
	    script2.enabled = false;
		
	    //layout start
	    GUI.BeginGroup(new Rect(Screen.width / 2 - 150, 50, 300, 200));
	    
	    //the menu background box
	    GUI.Box(new Rect(0, 0, 300, 200), "");
	    
	    //logo picture
	    GUI.Label(new Rect(15, 10, 300, 68), logoTexture);
	    
	    ///////main menu buttons
	    //game start button
	    if(GUI.Button(new Rect(55, 100, 180, 40), "Start game")) {
	    MainMenuScript script = GetComponent<MainMenuScript>(); 
	    script.enabled = false;
	    script2.enabled = true;
	    }
	    //quit button
	    if(GUI.Button(new Rect(55, 150, 180, 40), "Quit")) {
	    Application.Quit();
	    }
	    
	    //layout end
	    GUI.EndGroup(); 
	}
	
	void OnGUI () {
	    //load GUI skin
	    GUI.skin = newSkin;
	    
	    //execute theFirstMenu function
	    theFirstMenu();
	}
}
