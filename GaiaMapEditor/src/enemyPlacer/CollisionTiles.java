package enemyPlacer;
import java.awt.Color;
import java.awt.Point;

import javax.swing.*;



public class CollisionTiles extends JPanel {

	
	private static final long serialVersionUID = 1L;
	//Collision types
	//n - nothing
	//w - wall
	//e - enemy
	//l - land
	//p - water/puddle
	char collisionType = 'n';
	CollisionTiles thisSprite;
	

	//This creates a panel of the given size and location
	public CollisionTiles(int size, Point location){
		this.setSize(size,size);
		this.setLocation(location);
		thisSprite = this;
		this.setOpaque(true);
		this.updateTile('n');
	}
	
	public void updateTile(char collisionType){
		this.collisionType = collisionType;
		//'w' = wall
		//'e' = ladder
		//'l' = land
		//'p' = water
		//'n' = null
		//'r' = rightstairs
		//'s' = leftStairs 
		//'u' = upperleftstairs(top of stairs)
		//'i' = upperrightstairs(top of sairs) 
		switch(collisionType){
		case 'w': this.setBackground(new Color(255,0,0,80));break;
		case 'e': this.setBackground(new Color(0,255,0,80));break;
		case 'l': this.setBackground(new Color(0,0,255,80));break;
		case 'p': this.setBackground(new Color(200,0,200,80));break;
		case 'n': this.setBackground(new Color(0,0,0,0));break;
		case 'r': this.setBackground(new Color(20,150,150,100)); break;
		case 's': this.setBackground(new Color(190,186,20,100)); break;
		case 'u': this.setBackground(new Color(240,244,44,100)); break;
		case 'i': this.setBackground(new Color(57,240,240,100)); break;
		}
	}
}
