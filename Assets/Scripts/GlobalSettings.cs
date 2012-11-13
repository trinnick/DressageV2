using System;
using UnityEngine;
using System.Collections;

public class GlobalSettings{
	
	private float rateOfMovement = .3f;
	private int levelSelect = 1;
	private string movementID = "N/A";
	private String[] movementDesc = new String[0];
	private string pathDesc = "N/A";
	private bool pause = false;
	
	private string Camera1 = "CameraHorse";
	private string Camera2 = "CameraAbove";
	private string Camera3 = "CameraWestStand";
	private string Camera4 = "CameraEastStand";
	
	private string[] file = {
		"http://dougstewart.biz/XMLMovementDirections/FirstLevelTest1.xml",
		"http://dougstewart.biz/XMLMovementDirections/FirstLevelTest2.xml"
	};
	
	private static string fileAddress = "";
	
	public GlobalSettings(){
		fileAddress = file[0];
	}
	
	/***********************Setters and Getters******************************/
	
	public int getLevelSelect(){
		return levelSelect;
	}
	
	public void setLevelSelect(int selected){
		levelSelect = selected;
	}
	
	public float getRateOfMovement(){
		return rateOfMovement;
	}
	
	public void setRateOfMovement(float rate){
		rateOfMovement = rate;
	}
	
	public string getCamera1(){
		return Camera1;
	}
	
	public void setCamera1(string c1){
		Camera1 = c1;
	}
	
	public string getCamera2(){
		return Camera2;
	}
	
	public void setCamera2(string c2){
		Camera2 = c2;
	}
	
	public string getCamera3(){
		return Camera3;
	}
	
	public void setCamera3(string c3){
		Camera3 = c3;
	}
	
	public string getCamera4(){
		return Camera4;
	}
	
	public void setCamera4(string c4){
		Camera4 = c4;
	}
	
	public bool getPause(){
		return pause;
	}
	
	public void setPause(bool p){
		pause = p;
	}
	
	public string getPathDesc(){
		return pathDesc;
	}
	
	public void setPathDesc(string path){
		pathDesc = path;
	}
	
	public string getFileAddress(){
		return fileAddress;
	}
	
	public void setFileAddress(int i){
		i -= 1;
		fileAddress = file[i];
	}
	
	public String[] getMovementDesc(){
		return movementDesc;
	}
	
	public void setMovementDesc(String[] mDesc){
		movementDesc = mDesc;
	}
	
	public string getMovementID(){
		return movementID;
	}
	
	public void setMovementID(string id){
		movementID = id;
	}
}
