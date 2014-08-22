using System;

public class PlatformReference  : IComparable {

	private float extremityPosition;
	private int mainListIndex;
	
	
	public PlatformReference(float extremity, int index){
		extremityPosition = extremity;
		mainListIndex = index;
	}
	
	public float getExtremity(){
		return extremityPosition;
	}
	
	public int getIndex(){
		return mainListIndex;
	}
	
	public void setExtremity(float extremity){
		extremityPosition = extremity;
	}
	
	public void setIndex(int index){
		mainListIndex = index;
	}
	
	public int CompareTo(Object other){
		return extremityPosition.CompareTo(((PlatformReference)other).getExtremity());
	}
	
	
}
