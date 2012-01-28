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
	public void addMouseFunction(final SelectedTile select, final EditTile edit,final JPanel background){
		addMouseListener(new MouseAdapter(){
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
			public void mouseReleased(MouseEvent me){
				select.readyDrag = false;
			}
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
