using UnityEngine;
using System.Collections;

public class Platform{

	private Vector3 position;
	private Vector3 scale;



	public Platform(Vector3 position, Vector3 scale){
		this.position = position;
		this.scale = scale;
	}

	public Vector3 getPosition(){
		return position;
	}

	public Vector3 getScale(){
		return scale;
	}

	public void setPosition(Vector3 position){
		this.position = position;
	}
	
	public void setScale(Vector3 scale){
		this.scale = scale;
	}
	

}
