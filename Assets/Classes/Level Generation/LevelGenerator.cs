using UnityEngine;
using System;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

	public Transform player;
	public Transform prefab;
	public Vector3 startPos;
	private Vector3 nextPos;

	private int xRecycleCutoff=20, yRecycleCutoff=10;
	private readonly int totalInstanciatedPlatforms = 20;
	//information about the player's mobility used to determine if one platform can be reached from another
	private readonly float platformWidthGap = 10;
	private readonly float platformUpwardsGap = 3;
	private readonly float platformDownwardsGap = 15;
	//private readonly float playerSpeed = 10;
	//private readonly float playerJump = 5;
	
	//stores the minimally necessary information on all platforms in the entire level
	private List<Platform> allPlatforms;
	//each list stores the farthest point of all platforms for a particular extremity in sorted order - this is used to identify when they should become active
	private List<PlatformReference> rightExtremities, leftExtremities, upperExtremities, lowerExtremities;
	//used to keep track of index locations in the extremity lists
	private int rightExIndex, leftExIndex, upperExIndex, lowerExIndex;
	//used to keep track of the farthest active platform with respect to each border
	private float minXPosition, maxXPosition, minYPosition, maxYPosition;
	//used to keep track of the nearest inactive platform in each direction
	private float nextLeftPos, nextRightPos, nextUpperPos, nextLowerPos;
	//used to keep track of replacement platforms when the borders are pulled by player movement
	private float nextLeftPosRep, nextRightPosRep, nextUpperPosRep, nextLowerPosRep;
	
	//a directed graph with platforms as nodes and edges to represent reachable platforms
	private Graph platformGraph;
	
	private Queue<Transform> inactivePlatforms;
	private List<Transform> activePlatforms;
	
	//ADDRESS CASES WHERE MULTIPLE PLATFORMS HAVE EXTREMITIES WHICH SHARE THE SAME POSITION
	//IMPROVE LEVEL GENERATION RULES
	//IMPROVE GRAPH CONSTRUCTION AND CHECKING WITH NEW MOBILITY RULES
	//ADD CODE TO CORRECT THE GRAPH WHEN IT IS NOT VALID
	//ADD CODE TO MARK THE START AND END NODES OF THE GRAPH
	
	//VERIFY CASES WHEN LIST ENDS ARE REACHED
	//VERIFY THE CORECTNESS FOR ALL EVENTS
	//VERIFY GRAPH CONSTRUCTION CORRECTNESS
	
	
	void Start(){
		activePlatforms = new List<Transform>();
		inactivePlatforms = new Queue<Transform>();
		allPlatforms = new List<Platform>();
		rightExtremities = new List<PlatformReference>();
		leftExtremities = new List<PlatformReference>();
		upperExtremities = new List<PlatformReference>();
		lowerExtremities = new List<PlatformReference>();
		platformGraph = new Graph();

		//instantiate up to the number of allowed platforms
		for (int a=0; a<totalInstanciatedPlatforms; a++) {
			Transform platform = (Transform)Instantiate(prefab);
			inactivePlatforms.Enqueue(platform);
		}

		//REPLACE WITH IMPROVED LEVEL GENERATION
		//fill data structure with all platforms for the level
		nextPos = startPos;
		nextPos.x -= 4.1f;
		Vector3 startScale = new Vector3(4, 0.36f, 1);
		
		for (int a=0; a<50; a++){
			nextPos.x += 4.1f;
			Platform tempPlat = new Platform(nextPos, startScale);
			
			allPlatforms.Add(tempPlat);
			platformGraph.addVertex(new Vertex());
			
			//store information about each extremity of the platform
			rightExtremities.Add(new PlatformReference(tempPlat.getPosition().x + (tempPlat.getScale().x/2), a));
			leftExtremities.Add(new PlatformReference(tempPlat.getPosition().x - (tempPlat.getScale().x/2), a));
			upperExtremities.Add(new PlatformReference(tempPlat.getPosition().y + (tempPlat.getScale().y/2), a));
			lowerExtremities.Add(new PlatformReference(tempPlat.getPosition().y - (tempPlat.getScale().y/2), a));
			
			tempPlat.setPosition(nextPos);
		}

		//sort the lists of references
		rightExtremities.Sort();
		leftExtremities.Sort();
		upperExtremities.Sort();
		lowerExtremities.Sort();
		
		
		//construct a directed graph with edges between plaforms that are sufficiently reachable from eachother
		buildGraphEdges();
		
		//check if the graph represents a level which can be traversed based on the mobility of the player
		//UPDATE THIS TO CORRECT THE GRAPH WHEN IT IS NOT VALID
		//ADD THE CORRECT INDICATION OF THE START AND END NODES
		if (!platformGraph.isValid(0, 1))
			print ("LEVEL CANNOT BE COMPLETED BY CURRENT PLAYER");
		
		
		float rightExCutoff = player.localPosition.x - xRecycleCutoff;
		float leftExCutoff = player.localPosition.x + xRecycleCutoff;
		float upperExCutoff = player.localPosition.y - yRecycleCutoff;
		float lowerExCutoff = player.localPosition.y + yRecycleCutoff;
		float tempPos, tempScale;
		
		//find the smallest active right extremity
		for (int a=0; a<allPlatforms.Count; a++){
		
			//when the first right extremity within the active x range is found
			if (rightExtremities[a].getExtremity() > rightExCutoff){
			
				//mark the position of the next right extremity that would appear when pushing the borders left
				if (a == 0)
					nextLeftPos = float.MinValue;
				else
					nextLeftPos = rightExtremities[a-1].getExtremity();
					
				//mark the extremity index
				rightExIndex = a;
					
				//mark the position of the next right extremity that would appear when pulling the borders to the right
				nextLeftPosRep = rightExtremities[a].getExtremity();
				
				//continue checking platforms until the first which is also within the active y range is found
				for (int b=a+1; b<allPlatforms.Count; b++){
					tempPos = allPlatforms[rightExtremities[b].getIndex()].getPosition().y;
					tempScale = allPlatforms[rightExtremities[b].getIndex()].getScale().y;
					
					//if the platform is within the allowable y range
					if (((tempPos + (tempScale/2)) > upperExCutoff) && ((tempPos - (tempScale/2)) < lowerExCutoff)){
						//mark the extremity position
						minXPosition = rightExtremities[b].getExtremity();
						
						break;
					}
				}
				
				break;
			}
		}
		
		//find the largest active left extremity
		for (int a=allPlatforms.Count-1; a>=0; a--){
			if (leftExtremities[a].getExtremity() < leftExCutoff){
				if (a == (leftExtremities.Count-1))
					nextRightPos = float.MaxValue;
				else
					nextRightPos = leftExtremities[a+1].getExtremity();
					
				leftExIndex = a;
				nextRightPosRep = leftExtremities[a].getExtremity();
			
				for (int b=a-1; b>=0; b--){
					tempPos = allPlatforms[leftExtremities[b].getIndex()].getPosition().y;
					tempScale = allPlatforms[leftExtremities[b].getIndex()].getScale().y;
					if (((tempPos + (tempScale/2)) > upperExCutoff) && ((tempPos - (tempScale/2)) < lowerExCutoff)){
						maxXPosition = leftExtremities[b].getExtremity();
						break;
					}
				}
				break;
			}
		}
		
		//find the smallest active upper extremity
		for (int a=0; a<allPlatforms.Count; a++){
			if (upperExtremities[a].getExtremity() > upperExCutoff){
				if (a == 0)
					nextLowerPos = float.MinValue;
				else
					nextLowerPos = upperExtremities[a-1].getExtremity();
					
				upperExIndex = a;
				nextLowerPosRep = upperExtremities[a].getExtremity();
				
				for (int b=a+1; b<allPlatforms.Count; b++){
					tempPos = allPlatforms[upperExtremities[b].getIndex()].getPosition().x;
					tempScale = allPlatforms[upperExtremities[b].getIndex()].getScale().x;
					if (((tempPos + (tempScale/2)) > rightExCutoff) && ((tempPos - (tempScale/2)) < leftExCutoff)){
						minYPosition = upperExtremities[b].getExtremity();
						break;
					}
				}
				break;
			}
		}
		
		//find the largest active lower extremity
		for (int a=allPlatforms.Count-1; a>=0; a--){
			if (lowerExtremities[a].getExtremity() < lowerExCutoff){
				if (a == (lowerExtremities.Count-1))
					nextUpperPos = float.MaxValue;
				else
					nextUpperPos = lowerExtremities[a+1].getExtremity();
					
				lowerExIndex = a;
				nextUpperPosRep = lowerExtremities[a].getExtremity();
					
				for (int b=a-1; b>=0; b--){	
					tempPos = allPlatforms[lowerExtremities[b].getIndex()].getPosition().x;
					tempScale = allPlatforms[lowerExtremities[b].getIndex()].getScale().x;
					if (((tempPos + (tempScale/2)) > rightExCutoff) && ((tempPos - (tempScale/2)) < leftExCutoff)){
						maxYPosition = lowerExtremities[b].getExtremity();
						break;
					}
				}
				break;
			}
		}
		
		
		//activate the platform with the farthest allowable right extremity
		activate(rightExtremities[rightExIndex].getIndex());

		//check all platforms in order of their right extremity location for allowable activation until the farthest allowable left extremity is reached
		int xIndex = rightExIndex+1;
		while (rightExtremities[xIndex].getIndex() != leftExtremities[leftExIndex].getIndex()){
			tempPos = allPlatforms[rightExtremities[xIndex].getIndex()].getPosition().y;
			tempScale = allPlatforms[rightExtremities[xIndex].getIndex()].getScale().y;
			//if the platform is within the allowable y range then activate it
			if (((tempPos + (tempScale/2)) > upperExCutoff) && ((tempPos - (tempScale/2)) < lowerExCutoff))
				activate(rightExtremities[xIndex].getIndex());
			xIndex++;
		}
		
		//activate the platform with the farthest allowable left extremity
		activate(leftExtremities[leftExIndex].getIndex());
	}
	
	//take an inactive platform and activate it with the characteristics of the platform at the specified index
	private void activate(int index){
		prefab = inactivePlatforms.Dequeue();
		prefab.localPosition = allPlatforms[index].getPosition();
		prefab.localScale = allPlatforms[index].getScale();
		activePlatforms.Add(prefab);
	}


	void Update(){
		float rightExCutoff = player.localPosition.x - xRecycleCutoff;
		float leftExCutoff = player.localPosition.x + xRecycleCutoff;
		float upperExCutoff = player.localPosition.y - yRecycleCutoff;
		float lowerExCutoff = player.localPosition.y + yRecycleCutoff;
		float tempPosition;
		int index;

		//if the next platform off the left border is reached via border push
		if (nextLeftPos >= rightExCutoff){
			rightExIndex--;
			index = rightExtremities[rightExIndex].getIndex();
			//check if the upper extremity of the next platform is in range
			tempPosition = allPlatforms[index].getPosition().y + (allPlatforms[index].getScale().y /2);
			if (tempPosition >= (player.localPosition.y - yRecycleCutoff)){
				//check if the lower extremity of the next platform is in range
				tempPosition -= allPlatforms[index].getScale().y;
				if (tempPosition <= (player.localPosition.y + yRecycleCutoff)){
					//activate the platform
					activate (index);
					//update the global extremity
					minXPosition = rightExtremities[rightExIndex].getExtremity();
				}
			}
			//update the next platform positions
			nextLeftPosRep = nextLeftPos;
			
			if (rightExIndex == 0)
				nextLeftPos = float.MinValue;
			else
				nextLeftPos = rightExtremities[(rightExIndex-1)].getExtremity();
		}
		
		//if the next platform off the right border is reached via border push
		if (nextRightPos <= leftExCutoff){
			leftExIndex++;
			index = leftExtremities[leftExIndex].getIndex();
			tempPosition = allPlatforms[index].getPosition().y + (allPlatforms[index].getScale().y /2);
			if (tempPosition >= (player.localPosition.y - yRecycleCutoff)){
				tempPosition -= allPlatforms[index].getScale().y;
				if (tempPosition <= (player.localPosition.y + yRecycleCutoff)){
					activate (index);
					maxXPosition = leftExtremities[leftExIndex].getExtremity();
				}
			}
			nextRightPosRep = nextRightPos;
			if (leftExIndex == (leftExtremities.Count-1))
				nextRightPos = float.MaxValue;
			else
				nextRightPos = leftExtremities[(leftExIndex+1)].getExtremity();
		}
		
		//if the next platform off the upper border is reached via border push
		if (nextUpperPos <= lowerExCutoff){
			lowerExIndex++;
			index = lowerExtremities[lowerExIndex].getIndex();
			tempPosition = allPlatforms[index].getPosition().x + (allPlatforms[index].getScale().x /2);
			if (tempPosition >= (player.localPosition.x - xRecycleCutoff)){
				tempPosition -= allPlatforms[index].getScale().x;
				if (tempPosition <= (player.localPosition.x + xRecycleCutoff)){
					activate (index);
					maxYPosition = lowerExtremities[lowerExIndex].getExtremity();
				}
			}
			nextUpperPosRep = nextUpperPos;
			if (lowerExIndex == (leftExtremities.Count-1))
				nextUpperPos = float.MaxValue;
			else
				nextUpperPos = lowerExtremities[(lowerExIndex+1)].getExtremity();
		}
		
		//if the next platform off the bottom border is reached via border push
		if (nextLowerPos >= upperExCutoff){
			upperExIndex++;
			index = upperExtremities[upperExIndex].getIndex();
			tempPosition = allPlatforms[index].getPosition().x + (allPlatforms[index].getScale().x /2);
			if (tempPosition >= (player.localPosition.x - xRecycleCutoff)){
				tempPosition -= allPlatforms[index].getScale().x;
				if (tempPosition <= (player.localPosition.x + xRecycleCutoff)){
					activate (index);
					minYPosition = upperExtremities[upperExIndex].getExtremity();
				}
			}
			nextLowerPosRep = nextLowerPos;
			if (upperExIndex == 0)
				nextLowerPos = float.MinValue;
			else
				nextLowerPos = upperExtremities[(upperExIndex-1)].getExtremity();
		}
		
		
		//if the left borders are pulled such that the next platform off the left border must be updated
		if (nextLeftPosRep <= rightExCutoff){
			nextLeftPos = nextLeftPosRep;
			rightExIndex++;
			if (rightExIndex == (allPlatforms.Count-1))
				nextLeftPosRep = float.MaxValue;
			else
				nextLeftPosRep = rightExtremities[rightExIndex].getExtremity();
		}
		
		//if the borders are pulled such that the next platform off the right border must be updated
		if (nextRightPosRep >= leftExCutoff){
			nextRightPos = nextRightPosRep;
			leftExIndex--;
			if (upperExIndex == 0)
				nextRightPosRep = float.MinValue;
			else
				nextRightPosRep = leftExtremities[leftExIndex].getExtremity();
		}
		
		//if the borders ares pulled such that the next platform off the upper border must be updated
		if (nextUpperPosRep >= lowerExCutoff){
			nextUpperPos = nextUpperPosRep;
			lowerExIndex--;
			if (lowerExIndex == 0)
				nextUpperPosRep = float.MinValue;
			else
				nextUpperPosRep = lowerExtremities[lowerExIndex].getExtremity();
		}
		
		//if the borders are pulled such that the next platform off the lower border must be updated
		if (nextLowerPosRep <= upperExCutoff){
			nextLowerPos = nextLowerPosRep;
			upperExIndex++;
			if (upperExIndex == (allPlatforms.Count-1))
				nextLowerPosRep = float.MaxValue;
			else
				nextLowerPosRep = upperExtremities[upperExIndex].getExtremity();
		}


		//index set to max value in order to throw an exception if the indicated platform cannot be found
		index = int.MaxValue;

		//check if a platform should be deactivated off the left border
		if (minXPosition < rightExCutoff){
			float newPosition = float.MaxValue;
			float newYMin = float.MaxValue, newYMax = float.MinValue;
			
			//loop through all active platforms
			for (int a=0; a<activePlatforms.Count; a++){
				//store the position of the right extremity current platform
				tempPosition = (activePlatforms[a].localPosition.x + (activePlatforms[a].localScale.x / 2));
				//if the platform to deactivate has been found then store its index
				if (tempPosition == minXPosition)
					index = a;
				//update the new plaform farthest to the left border
				else if (tempPosition < newPosition)
					newPosition = tempPosition;
					
				//store the position of the upper extremity current platform
				tempPosition = (activePlatforms[a].localPosition.y + (activePlatforms[a].localScale.y / 2));
				if (tempPosition > minYPosition && tempPosition < newYMin)
					newYMin = tempPosition;
				//store the position of the lower extremity current platform
				tempPosition -= activePlatforms[a].localScale.y;
				if (tempPosition < maxYPosition && tempPosition > newYMax)
					newYMax = tempPosition;
			}
			
			//update the global min active
			minXPosition = newPosition;
			
			//check if the platform to deactive was also the farthest platform for the other borders
			if ((activePlatforms[index].localPosition.y + (activePlatforms[index].localScale.y / 2)) == minYPosition)
				minYPosition = newYMin;
			if ((activePlatforms[index].localPosition.y - (activePlatforms[index].localScale.y / 2)) == maxYPosition)
				maxYPosition = newYMax;

			//move the platform to be deactivated
			inactivePlatforms.Enqueue(activePlatforms[index]);
			activePlatforms.RemoveAt(index);

			index = int.MaxValue;
		}

		//check if a platform should be deactivated off the right border
		if (maxXPosition > leftExCutoff){
			float newPosition = float.MinValue;
			float newYMin = float.MaxValue, newYMax = float.MinValue;
			
			for (int a=0; a<activePlatforms.Count; a++){
				tempPosition = (activePlatforms[a].localPosition.x - (activePlatforms[a].localScale.x / 2));
				if (tempPosition == maxXPosition)
					index = a;
				else if (tempPosition > newPosition)
					newPosition = tempPosition;
					
				tempPosition = (activePlatforms[a].localPosition.y + (activePlatforms[a].localScale.y / 2));
				if (tempPosition > minYPosition && tempPosition < newYMin)
					newYMin = tempPosition;
				tempPosition -= activePlatforms[a].localScale.y;
				if (tempPosition < maxYPosition && tempPosition > newYMax)
					newYMax = tempPosition;
			}
			
			maxXPosition = newPosition;
			
			if ((activePlatforms[index].localPosition.y + (activePlatforms[index].localScale.y / 2)) == minYPosition)
				minYPosition = newYMin;
			if ((activePlatforms[index].localPosition.y - (activePlatforms[index].localScale.y / 2)) == maxYPosition)
				maxYPosition = newYMax;
			
			inactivePlatforms.Enqueue(activePlatforms[index]);
			activePlatforms.RemoveAt(index);

			index = int.MaxValue;
		}

		//check if a platform should be deactivated off the bottom border
		if (minYPosition < upperExCutoff){
			float newPosition = float.MaxValue;
			float newXMin = float.MaxValue, newXMax = float.MinValue;
			
			for (int a=0; a<activePlatforms.Count; a++){
				tempPosition = (activePlatforms[a].localPosition.y + (activePlatforms[a].localScale.y / 2));
				if (tempPosition == minYPosition)
					index = a;
				else if (tempPosition < newPosition)
					newPosition = tempPosition;
					
				tempPosition = (activePlatforms[a].localPosition.x + (activePlatforms[a].localScale.x / 2));
				if (tempPosition > minXPosition && tempPosition < newXMin)
					newXMin = tempPosition;
				tempPosition -= activePlatforms[a].localScale.x;
				if (tempPosition < maxXPosition && tempPosition > newXMax)
					newXMax = tempPosition;
			}
			
			minYPosition = newPosition;
			
			if ((activePlatforms[index].localPosition.x + (activePlatforms[index].localScale.x / 2)) == minXPosition)
				minXPosition = newXMin;
			if ((activePlatforms[index].localPosition.x - (activePlatforms[index].localScale.x / 2)) == maxXPosition)
				maxXPosition = newXMax;
			
			inactivePlatforms.Enqueue(activePlatforms[index]);
			activePlatforms.RemoveAt(index);

			index = int.MaxValue;
		}

		//check if a platform should be deactivated off the top border
		if (maxYPosition > lowerExCutoff){
			float newPosition = float.MinValue;
			float newXMin = float.MaxValue, newXMax = float.MinValue;
			
			for (int a=0; a<activePlatforms.Count; a++){
				tempPosition = (activePlatforms[a].localPosition.y - (activePlatforms[a].localScale.y / 2));
				if (tempPosition == maxYPosition)
					index = a;
				else if (tempPosition > newPosition)
					newPosition = tempPosition;
					
				tempPosition = (activePlatforms[a].localPosition.x + (activePlatforms[a].localScale.x / 2));
				if (tempPosition > minXPosition && tempPosition < newXMin)
					newXMin = tempPosition;
				tempPosition -= activePlatforms[a].localScale.x;
				if (tempPosition < maxXPosition && tempPosition > newXMax)
					newXMax = tempPosition;
			}
			
			maxYPosition = newPosition;
			
			if ((activePlatforms[index].localPosition.x + (activePlatforms[index].localScale.x / 2)) == minXPosition)
				minXPosition = newXMin;
			if ((activePlatforms[index].localPosition.x - (activePlatforms[index].localScale.x / 2)) == maxXPosition)
				maxXPosition = newXMax;
			
			inactivePlatforms.Enqueue(activePlatforms[index]);
			activePlatforms.RemoveAt(index);
		}
		
		
		//check if any extremity lists have reached their end
		if (leftExIndex == allPlatforms.Count-1)
			nextRightPos = float.MaxValue;
		if (rightExIndex == 0)
			nextLeftPos = float.MinValue;
		if (upperExIndex == 0)
			nextLowerPos = float.MinValue;
		if (lowerExIndex == allPlatforms.Count-1)
			nextUpperPos = float.MaxValue;

	}
	
	
	private void buildGraphEdges(){
		float rightExPos, leftExPos, topExPos, otherTopExPos;
		int index, otherIndex = 0, otherIndexWalk, leftExIndexLookup;
		float xGap, yGap;
		
		//walk through platforms in order of their right extremities
		for (int a=0; a<allPlatforms.Count; a++){
			//get the right extremity platform info
			index = rightExtremities[a].getIndex();
			rightExPos = allPlatforms[index].getPosition().x + (allPlatforms[index].getScale().x / 2);
			topExPos = allPlatforms[index].getPosition().y + (allPlatforms[index].getScale().y / 2);
			
			//walk through platforms in order of their left extremities starting from the last platform selected in this way
			//walk until the left extremity is to the right of the chosen right extremity
			leftExIndexLookup = leftExtremities[otherIndex].getIndex();
			leftExPos = allPlatforms[leftExIndexLookup].getPosition().x - (allPlatforms[leftExIndexLookup].getScale().x / 2);
			while ((leftExPos < rightExPos) && otherIndex < (allPlatforms.Count-1)){
				//get the left extremity platform info
				otherIndex++;
				leftExIndexLookup = leftExtremities[otherIndex].getIndex();
				leftExPos = allPlatforms[leftExIndexLookup].getPosition().x - (allPlatforms[leftExIndexLookup].getScale().x / 2);
			}
			
			//walk forward from the chosen left extremity until the gap on the x axe between the current left extremity platorm and the right extremity platform becomes too great
			otherIndexWalk = otherIndex;
			do{
				//get the left extremity platform info
				leftExIndexLookup = leftExtremities[otherIndexWalk].getIndex();
				leftExPos = allPlatforms[otherIndexWalk].getPosition().x - (allPlatforms[otherIndexWalk].getScale().x / 2);
				otherTopExPos = allPlatforms[otherIndexWalk].getPosition().y + (allPlatforms[otherIndexWalk].getScale().y / 2);
				xGap = leftExPos - rightExPos;
				
				//if the left extremity platform is above the right extremity platform
				if (otherTopExPos > topExPos){
					yGap = otherTopExPos - topExPos;
					//check if the left extremity platform can be reached from the right extremity platform
					if (yGap < platformUpwardsGap)
						//add an edge to the graph to indicate that the platform can be reached
						platformGraph.addEdge(index, new Edge(xGap, yGap, leftExIndexLookup));
					//check if the right extremity platform can be reached from the left extremity platform
					if (yGap < platformDownwardsGap)
						//add an edge to the graph to indicate that the platform can be reached
						platformGraph.addEdge(leftExIndexLookup, new Edge(xGap, yGap, index));
				}
				//if the left extremity platform is below the right extremity platform
				else{
					yGap = topExPos - otherTopExPos;
					//check if the left extremity platform can be reached from the right extremity platform
					if (yGap < platformDownwardsGap)
						//add an edge to the graph to indicate that the platform can be reached
						platformGraph.addEdge(index, new Edge(xGap, yGap, leftExIndexLookup));
					//check if the right extremity platform can be reached from the left extremity platform
					if (yGap < platformUpwardsGap)
						//add an edge to the graph to indicate that the platform can be reached
						platformGraph.addEdge(leftExIndexLookup, new Edge(xGap, yGap, index));
				}
				
				otherIndexWalk++;
			} while (((leftExPos - rightExPos) < platformWidthGap) && otherIndexWalk < allPlatforms.Count);
		}
	}
	

}