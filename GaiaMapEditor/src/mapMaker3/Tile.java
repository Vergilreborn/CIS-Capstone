package mapMaker3;

//The general tile class
public class Tile {

	//Numbered tile (x,y)
	public int x;
	public int y;
	public int size;
	//number to make movement less
	int lessMovementBy = 0;

	//is this tile passable
	boolean isPassableObject = false;
	
	
	//This is the tile information hold which hols onto the
	//x position, y position and the size of the tile
	public Tile(int xPos, int yPos, int size){
		x = xPos;
		y = yPos;
		this.size = size;
	}
	
	//This will be used for the water sprite since we will have
	//the water to slow down the character
	public void setLessMovement(int lessMovement){
		this.lessMovementBy = lessMovement;
	}
	//the tostring to print out when printing it
	public String toString(){
	return ""+ x + "," + y + "";	
	
	}
}
