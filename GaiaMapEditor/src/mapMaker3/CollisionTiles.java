package mapMaker3;
import java.awt.*;

import javax.swing.*;
import java.awt.event.*;


public class CollisionTiles extends JPanel{
	
	
	
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
		//'j' = jumpTop
		//'m' = jumpBottom
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
		case 'j': this.setBackground(new Color(232,158,23,100)); break;
		case 'm': this.setBackground(new Color(154,106,16,100)); break;
		}
	}
	
	//This adds the functionality of when the mouse is clicked. It will test to see if the mouse is dragged
	//and if it is it will check to see what the collision type is and according to that will update
	//the sprites
	public void addMouseFunction(final SelectedTile select, final EditTile edit,final JPanel background){
		addMouseListener(new MouseAdapter(){
			//Tests to see if the mouse button is pressed down
			public void mousePressed(MouseEvent me){
				select.readyDrag = true;
				if((select.isCollOnly)){
					
					updateTile(select.collChar);
					thisSprite.validate();
					thisSprite.repaint();
				}
				if(select.isImageOnly){
					edit.changeTile(select.croppedImage,select.tile);
					edit.repaint();
				}
				background.repaint(thisSprite.getLocation().x, thisSprite.getLocation().y,
						thisSprite.getWidth(), thisSprite.getHeight());
			
			}
			//The drag is null because nothing is being clicked/held in
			public void mouseReleased(MouseEvent me){
				select.readyDrag = false;
			}
			//checks to see if the mouse is entered the field of this current sprite
			//then it edits it to whatever the right drawing spec is.
			public void mouseEntered(MouseEvent me){
				if(select.readyDrag){
					if((select.isCollOnly)){					
						updateTile(select.collChar);
						thisSprite.validate();
						thisSprite.repaint();
					}
					if(select.isImageOnly){
						edit.changeTile(select.croppedImage,select.tile);
						edit.repaint();
					}
					background.repaint(thisSprite.getLocation().x, thisSprite.getLocation().y,
							thisSprite.getWidth(), thisSprite.getHeight());
				}
			}
		});
	}
}
