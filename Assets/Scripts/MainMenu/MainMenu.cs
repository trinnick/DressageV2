using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	Texture btnTexture;
	Texture btnView;
		
	void  OnGUI (){
		GUI.depth = 10;
		GUI.Box (new Rect (50,50,300,150), "Start Menu");
		GUI.Label(new Rect(70, 75, 100, 20), "Select a Test");
 	  	if(GUI.Button( new Rect(250,100,50,20),"Start")) 
 	  		Application.LoadLevel(1);
 	  	if(GUI.Button( new Rect(300,100,50,20),"Quit")) 
 	  		Application.Quit();
		
		
	}
}