using UnityEngine;
using System.Xml;
using System;
using System.Collections;

public class HorseMovement : MonoBehaviour {
	
	//Reference to the class where all variables are stored
	public GlobalSettings gs = new GlobalSettings();
	//Reference to the class that allows for the toggle of the camera view
	public CameraToggle tg = new CameraToggle();
	
	// Use this for initialization
	IEnumerator Start () {
		
		//Load XML data from external file (In this case from a url web address)
		//This is collected from the Global Settings script
		string url = gs.getFileAddress();
		WWW www = new WWW(url);
		
		//Load the data and yeild (wait) till it is ready before executing
		//Using yield it continues to perform the required task, and the return function
		//allows for the continuation of the program script to wait until this process path
		//is finished
		yield return www;
		if (www.error == null){
			
			//Create new XML document from loaded data
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(www.text);
						
			//Point to the Movement nodes and process them
			gs.setTestID(xmlDoc.SelectSingleNode("Test/TestId").InnerText);
			
			yield return StartCoroutine(ProcessMovement(xmlDoc.SelectNodes("Test/Movement")));	
		}
		else{
			
			//Create error nessage based on opening the xml file
			Debug.Log("Error: " + www.error);
		}	
	}
	
	//Converts an XmlNodeList into Movement objects
	IEnumerator ProcessMovement(XmlNodeList nodes){
		Movement movement;
		
		//Move through each tagged item within the xml document
		foreach (XmlNode node in nodes){
			movement = new Movement();
			
			//Extract and assign tagged items within the xmlDoc to object variables within Movement class
			movement.MovementID = node.SelectSingleNode("MovementId").InnerText;
			movement.MovementDesc = node.SelectSingleNode("MovementDesc").InnerText;
			
			//Set Global Variables so that this information can be accessed anywhere
			gs.setMovementID(movement.MovementID);
			gs.setMovementDesc(movement.MovementDesc.Split('.'));
			
			//Assign MovementDetails to Movement Class variables
			foreach (XmlNode detail in node.SelectNodes("MovementDetail")){
				movement.Sequence = detail.SelectSingleNode("SequenceId").InnerText;
				movement.Path = detail.SelectSingleNode("Path").InnerText;
				movement.PathDesc = detail.SelectSingleNode("PathDesc").InnerText;
				movement.Gait = detail.SelectSingleNode("Gait").InnerText;
				
				gs.setSequence(movement.Sequence);
				gs.setPathDesc(movement.PathDesc);
			
				//Call LoadMovement method and pass movement object
				yield return StartCoroutine(LoadMovement(movement));
			}
		}
	}
	
	//Process movements based on collected variable information
	IEnumerator LoadMovement(Movement movement){
		
		Vector3 firstPosition = new Vector3();
		Vector3 secondPosition = new Vector3();
		Vector3 thirdPosition = new Vector3();
		Vector3 fourthPosition = new Vector3();
		Vector3 fifthPosition = new Vector3();
		
		//Split the string of path and place in array elements based on seperator character -
		string[] path = movement.Path.Split('-');
				
		//Assign the firstPosition based on the first element of the array path
		firstPosition = GameObject.Find(path[0]).transform.position;
		
		//Assign the secondPosition based on the second elemnt of the array path
		secondPosition = GameObject.Find(path[1]).transform.position;
		
		//If there is a third element in path, assign it to the thirdPosition variable
		if(path.Length >= 3){
			thirdPosition = GameObject.Find(path[2]).transform.position;
		}
		
		//If there is a fourth element in path, assign it to the fourthPosition variable
		if(path.Length >= 4){
			fourthPosition = GameObject.Find(path[3]).transform.position;
		}
		
		//If there is a fifth element in path, assign it to the fifthPosition variable
		if(path.Length >= 5){
			fifthPosition = GameObject.Find(path[4]).transform.position;
		}
	
		if(movement.PathDesc == "Line" || movement.PathDesc =="Diagonal"){
			
			//Call movement based on Coroutine and yield commands
	
			if(path.Length>=2){
				yield return StartCoroutine(move(firstPosition,secondPosition));
			}
			
			if(path.Length >= 3){
				yield return StartCoroutine(move(secondPosition,thirdPosition));
			}
			
			if(path.Length >= 4){
				yield return StartCoroutine(move(thirdPosition,fourthPosition));
			}
			
			if(path.Length >= 5){
				yield return StartCoroutine(move(fourthPosition,fifthPosition));
			}
		}
		else{
			string[] desc = movement.PathDesc.Split('_');
						
			if(desc[0] == "HalfCircle"){
				int change = 0;
				if(desc[3] == "Down"){
					change = 1;
				}
				
				if(desc[3] == "Up"){
					change = -1;
				}
				yield return StartCoroutine(halfCircleMovement(firstPosition,secondPosition,change, path[0], path[1], float.Parse(desc[2])));
			}
			if(desc[0] == "FullCircle"){
				float diameter = float.Parse(desc[2]);
				yield return StartCoroutine(fullCircleMovement(firstPosition,diameter,path[0], desc[1]));
			}
			
			if(desc[0] == "Serpentine"){
				yield return StartCoroutine(serpentineMovement(firstPosition,secondPosition,desc[1]));
			}
		}				
	}

	void OnGUI(){		
		String movementDescFormatted = null;
		
		String[] movementDescUnformatted = gs.getMovementDesc();
		for(int i=0;i<movementDescUnformatted.Length;i++){
			movementDescFormatted += "\n" + movementDescUnformatted[i];
		}
		
		GUI.Box (new Rect (0,0,250,100), "Movement Description:\n" + movementDescFormatted);
		GUI.Box (new Rect (Screen.width/2 + 0,0,100,50), "Test ID: \n" + gs.getTestID());
		GUI.Box (new Rect (Screen.width - 100,0,100,50), "Movement ID: \n" + gs.getMovementID());
		GUI.Box (new Rect (Screen.width - 100,55,100,50), "Sequence: \n" + gs.getSequence());
		GUI.Box (new Rect (Screen.width - 120,110,120,50), "Path Description: \n" + gs.getPathDesc());
	}
	
	IEnumerator move(Vector3 first, Vector3 second){
	    float dist = Vector3.Distance(first, second);
	
	    for (float i = 0.0f; i < 1.0f; i += ((gs.getRateOfMovement() * Time.deltaTime)/dist)) {
	        transform.position = Vector3.Lerp(first, second, i);
			yield return null;	
			
			pauseMovement();
			tg.toggle();		
	    }
	}
	
	IEnumerator halfCircleMovement(Vector3 first, Vector3 second, int change, string markerOne, string markerTwo, float diameter){
		float distance = Vector3.Distance(first,second);
		Vector3 radius =  new Vector3(change * distance,0.0f,0.0f)/2;
		
		if((markerOne == "H" && markerTwo == "K") || (markerOne == "K" && markerTwo == "H")){
			distance = 20.0f;
			radius =  new Vector3(0.0f,0.0f,change * distance)/2;
		}
		
		Vector3 center = (first + second)/2;
		Vector3 tempOne = first + radius;
		Vector3 tempTwo = second + radius;
		Vector3 midPoint = center + radius;
	    float dist = (float)Math.PI * (distance)/2;
	
	    for (float i = 0.0f; i < 1.0f; i += ((gs.getRateOfMovement() * Time.deltaTime)/dist)*2) {
	        Vector3 bp1 = Vector3.Lerp(first, tempOne, i);
	        Vector3 bp2 = Vector3.Lerp(tempOne, midPoint, i);
	
	        transform.position = Vector3.Lerp(bp1, bp2, i);
			yield return null;
			
			pauseMovement();
			tg.toggle();
	    }
	
	    for (float i = 0.0f; i < 1.0f; i += ((gs.getRateOfMovement() * Time.deltaTime)/dist)*2) {
	        Vector3 bp1 = Vector3.Lerp(midPoint, tempTwo, i);
	        Vector3 bp2 = Vector3.Lerp(tempTwo, second, i);
	
	        transform.position = Vector3.Lerp(bp1, bp2, i);
			yield return null;
			
			pauseMovement();
			tg.toggle();
	    }
	}
	
	IEnumerator fullCircleMovement(Vector3 first, float diameter, string marker, string circleDirection){
		int circleDir = 0;
		if(diameter == 20){
			diameter = 18;
		}
		
		if(marker == "G" || marker == "I" || marker == "X" || marker == "L" || marker == "D"){
			diameter -= 1.0f;
		}
		
		float radius = (diameter)/2;
	    float dist = (float)Math.PI * (diameter);
		Vector3 second = new Vector3();
		int change = 1;

		if(marker == "A"){
			change = -1;
			if(circleDirection == "Left"){
				circleDir = -1;
			}
			else{
				circleDir = 1;
			}
			second = new Vector3((change * diameter) + first.x,first.y,first.z);
		}
		
		if(marker == "C"){
			if(circleDirection == "Left"){
				circleDir = -1;
			}
			else{
				circleDir = 1;
			}
			second = new Vector3((change * diameter) + first.x,first.y,first.z);
		}
			
		if(marker == "H" || marker == "S" || marker == "E" || marker == "V" || marker == "K"){
			if(circleDirection == "Left"){
				circleDir = 1;
			}
			else{
				circleDir = -1;
			}
			second = new Vector3(first.x,first.y,(change * diameter) + first.z);
		}
			
		if(marker == "M" || marker == "R" || marker == "B" || marker == "P" || marker == "F"){
			change = -1;
			if(circleDirection == "Left"){
				circleDir = 1;
			}
			else{
				circleDir = -1;
			}
			second = new Vector3(first.x,first.y,(change * diameter) + first.z);
		}
			
		if(marker == "G" || marker == "I" || marker == "X" || marker == "L" || marker == "D"){
			
			if(circleDirection == "Left"){
				circleDir = 1;
				change = -1;
			}
			else{
				circleDir = -1;
				change = 1;
			}
			second = new Vector3(first.x,first.y,(change * diameter) + first.z);
		}
		
		float distance = Vector3.Distance(first,second);
		
		radius *= change;
		
		for (int x = 0; x < 2; x++){
			Vector3 tempOne = new Vector3();
			Vector3 tempTwo = new Vector3();
			Vector3 midPoint = new Vector3();
			
			if(marker == "A" || marker == "C"){
				Vector3 center = new Vector3(radius + first.x,first.y,first.z);
				tempOne = new Vector3(first.x,first.y,(circleDir * radius) + first.z);
				tempTwo = new Vector3(second.x,second.y,(circleDir * radius) + second.z);
				midPoint = new Vector3(center.x,center.y,(circleDir * radius) + center.z);
			}
			
			if(marker == "H" || marker == "S" || marker == "E" || marker == "V" || marker == "K" || 
				marker == "M" || marker == "R" || marker == "B" || marker == "P" || marker == "F" ||
				marker == "G" || marker == "I" || marker == "X" || marker == "L" || marker == "D"){
				Vector3 center = new Vector3(first.x,first.y,radius + first.z);
				tempOne = new Vector3((circleDir * radius) + first.x,first.y,first.z);
				tempTwo = new Vector3((circleDir * radius) + second.x,second.y,second.z);
				midPoint = new Vector3((circleDir * radius) + center.x,center.y,center.z);
			}
		
		    for (float i = 0.0f; i < 1.0f; i += ((gs.getRateOfMovement() * Time.deltaTime)/dist)*4) {
		        Vector3 bp1 = Vector3.Lerp(first, tempOne, i);
		        Vector3 bp2 = Vector3.Lerp(tempOne, midPoint, i);
		
		        transform.position = Vector3.Lerp(bp1, bp2, i);
				yield return null;
				
				pauseMovement();
				tg.toggle();
		    }
		
		    for (float i = 0.0f; i < 1.0f; i += ((gs.getRateOfMovement() * Time.deltaTime)/dist)*4) {
		        Vector3 bp1 = Vector3.Lerp(midPoint, tempTwo, i);
		        Vector3 bp2 = Vector3.Lerp(tempTwo, second, i);
		
		        transform.position = Vector3.Lerp(bp1, bp2, i);
				yield return null;
				
				pauseMovement();
				tg.toggle();
		    }
			
			radius *= -1;
			Vector3 reversal = first;
			first = second;
			second = reversal;
		}
	}
	
	IEnumerator serpentineMovement(Vector3 first, Vector3 second, string side){
		int change = -1;
		int direction = 1;
		
		if(side == "Right"){
			direction = -1;
		}
		if(side == "Left"){
			direction = 1;
		}
		
		float distance = Vector3.Distance(first,second);
		Vector3 diameter = new Vector3(0.0f,0.0f,18.0f); //Width of arena
		Vector3 radius =  change * (diameter/2);
		Vector3 oneThird = new Vector3(direction * distance/3.0f,0.0f,0.0f);
		
		
		Vector3 center = first + (oneThird/2);
		Vector3 tempOne = first + radius;
		second = first + oneThird;
		Vector3 tempTwo = second + radius;
		Vector3 midPoint = center + radius;
	    float dist = (float)Math.PI * (18.0f/2.0f) * 3.0f;
	
		for (int x = 0; x < 3; x++){
		    for (float i = 0.0f; i < 1.0f; i += ((gs.getRateOfMovement() * Time.deltaTime)/dist)*6) {
		        Vector3 bp1 = Vector3.Lerp(first, tempOne, i);
		        Vector3 bp2 = Vector3.Lerp(tempOne, midPoint, i);
		
		        transform.position = Vector3.Lerp(bp1, bp2, i);
				yield return null;
				
				pauseMovement();
				tg.toggle();
		    }
		
		    for (float i = 0.0f; i < 1.0f; i += ((gs.getRateOfMovement() * Time.deltaTime)/dist)*6) {
		        Vector3 bp1 = Vector3.Lerp(midPoint, tempTwo, i);
		        Vector3 bp2 = Vector3.Lerp(tempTwo, second, i);
		
		        transform.position = Vector3.Lerp(bp1, bp2, i);
				yield return null;
				
				pauseMovement();
				tg.toggle();
		    }
			change *= -1;
			first += oneThird;
			tempOne += oneThird + (change * diameter);
			midPoint += oneThird + (change * diameter);
			tempTwo += oneThird + (change * diameter);
			second += oneThird;
		}
	}
	
	public void pauseMovement(){
			if(Input.GetKeyDown(KeyCode.Space) && gs.getPause() == false){
				gs.setPause(true);
			}
			else if(Input.GetKeyDown(KeyCode.Space) && gs.getPause() == true){
				gs.setPause(false);
			}
			
			if(gs.getPause() == true){
				Time.timeScale = 0;
			}
			
			else if(gs.getPause() == false){
				Time.timeScale = 1;
			}		
	}
}