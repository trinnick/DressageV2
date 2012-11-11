using UnityEngine;
using System.Collections;

public class CameraToggle{

	public void toggle() {
		
		GlobalSettings gs = new GlobalSettings();
			
		if (Input.GetKeyDown(KeyCode.Alpha1)){
			GameObject.Find(gs.getCamera1()).camera.depth = 1;
			GameObject.Find(gs.getCamera2()).camera.depth = 0;
			GameObject.Find(gs.getCamera3()).camera.depth = 0;
			GameObject.Find(gs.getCamera4()).camera.depth = 0;
			
			gs.setCameraDesc("3rd Persion View");
	    }
	
		if (Input.GetKeyDown(KeyCode.Alpha2)){
			GameObject.Find(gs.getCamera1()).camera.depth = 0;
			GameObject.Find(gs.getCamera2()).camera.depth = 1;
			GameObject.Find(gs.getCamera3()).camera.depth = 0;
			GameObject.Find(gs.getCamera4()).camera.depth = 0;
			
			gs.setCameraDesc("View from Above");
	    }
	
		if (Input.GetKeyDown(KeyCode.Alpha3)){
			GameObject.Find(gs.getCamera1()).camera.depth = 0;
			GameObject.Find(gs.getCamera2()).camera.depth = 0;
			GameObject.Find(gs.getCamera3()).camera.depth = 1;
			GameObject.Find(gs.getCamera4()).camera.depth = 0;
			
			gs.setCameraDesc("View from West Stands");
	    }
	
		if (Input.GetKeyDown(KeyCode.Alpha4)){
			GameObject.Find(gs.getCamera1()).camera.depth = 0;
			GameObject.Find(gs.getCamera2()).camera.depth = 0;
			GameObject.Find(gs.getCamera3()).camera.depth = 0;
			GameObject.Find(gs.getCamera4()).camera.depth = 1;
			
			gs.setCameraDesc("View from East Stands");
	    }
	}
}