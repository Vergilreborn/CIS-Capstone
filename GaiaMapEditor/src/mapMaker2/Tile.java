package mapMaker2;

public class Tile {

	//Numbered tile (x,y)
	int x;
	int y;
	
	//number to make movement less
	int lessMovementBy = 0;
	
	//what type of movement is the tile
	boolean isNightTile = false;
	boolean isWater= false;
	boolean isMoveableWater = false;
	boolean isSand = false;
	boolean isWall = false;
	boolean isShop = false;
	boolean isWeaponStore = false;
	boolean isArena = false;
	boolean isHouse = false;
	boolean isVillage = false;
	boolean isTree = false;
	boolean isMountain = false;
	boolean isLava = false;
	boolean isWallBreakable = false;
	boolean isPassableObject = false;
	boolean isOpen = false;
	
	
	public Tile(int xPos, int yPos){
		x = xPos;
		y = yPos;
	}
	
	public void setLessMovement(int lessMovement){
		this.lessMovementBy = lessMovement;
	}
	
	public void setWater(boolean isMoveableWater, boolean isPassableObject){
		this.isWater = true;
		this.isMoveableWater = isMoveableWater;
		this.isPassableObject = isPassableObject;

	}
	public void setSand(boolean isPassableObject){
		this.isSand = true;
		this.isPassableObject = isPassableObject;
	}
	public void setWall(boolean isWallBreakable){
		this.isWall = true;
		this.isWallBreakable = isWallBreakable;
	}
	public void setBuilding(boolean isShop, boolean isWeaponStore, boolean isArena, boolean isHouse, boolean isVillage
							,boolean isOpen){
		this.isShop = isShop;
		this.isWeaponStore = isWeaponStore;
		this.isArena = isArena;
		this.isHouse = isHouse;
		this.isVillage = isVillage;
		this.isPassableObject = true;
		this.isOpen = isOpen;
	}
	public void setTreeOrMountain(boolean isTree, boolean isMountain){
		this.isTree = isTree;
		this.isMountain = isMountain;
	}
	public void setLava(boolean isPassableObject){
		this.isLava = true;
		this.isPassableObject = isPassableObject;
	}
	
	public void setNightTimeTile(){
		this.isNightTile = true;
	}
	
	public String toString(){
		String t = "Tile: " + x + "," + y + " Less Movement - " + lessMovementBy;
		String s = "";
		if(isWater== true)
			s += "\n    A water tile";
		if(isMoveableWater == true)
			s += "\n    A moveable Water tile";
		if(isSand == true)
			s += "\n    A sand tile";
		if(isWall == true)
			s += "\n    A wall tile";
		if(isShop == true)
			s += "\n    A shop tile";
		if(isWeaponStore == true)
			s += "\n    A weapon store tile";
		if(isArena == true)
			s += "\n    An arena tile";
		if(isHouse == true)
			s += "\n    A house tile";
		if(isVillage == true)
			s += "\n    A village tile";
		if(isTree == true)
			s += "\n    A tree tile";
		if(isMountain == true)
			s += "\n    A mountain tile";
		if(isLava == true)
			s += "\n    A lava tile";
		if(isWallBreakable == true)
			s += "\n    A breakable wall tile";
		if(isPassableObject == true)
			s += "\n    A passable object tile";
		if(isOpen == true)
			s += "\n    This place is open";
		if(isNightTile == true)
			s += "\n    This is a night time tile";
		return t + s;
		
	}
}
