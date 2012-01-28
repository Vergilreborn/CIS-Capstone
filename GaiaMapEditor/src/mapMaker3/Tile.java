package mapMaker3;

public class Tile {

	//Numbered tile (x,y)
	public int x;
	public int y;
	public int size;
	//number to make movement less
	int lessMovementBy = 0;

	//is this tile passable
	boolean isPassableObject = false;
	
	
	public Tile(int xPos, int yPos, int size){
		x = xPos;
		y = yPos;
		this.size = size;
	}
	
	public void setLessMovement(int lessMovement){
		this.lessMovementBy = lessMovement;
	}
	public void isPassableObject(boolean passable){
		this.isPassableObject = passable;
		
	}
	public String toString(){
	return ""+ x + "," + y + "";	
	
	}
}
