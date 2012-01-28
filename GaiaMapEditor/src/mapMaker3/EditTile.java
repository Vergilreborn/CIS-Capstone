package mapMaker3;

import java.awt.image.*;
import java.awt.*;

import javax.swing.*;


public class EditTile extends JPanel {

	


	private static final long serialVersionUID = 1L;

	//tests to see if this tile is selected
	boolean isSelected;
	
	//gets location of the tile
	Point location = new Point();
	
	//this is the tile information
	Tile tile;
	
	//this is this sprite specifically
	EditTile thisSprite;
	
	//Image to be cropped and also the cropped image
	Image img;
	Image croppedImage;
	Image nullImage;
	JPanel collisionColor;
	//Information is like the TileSprite class so send it to the constructor of that
	public EditTile(Image spriteSheet,Point placement,int tilePosX,int tilePosY){
		this.setLayout(null);
	
		
		tile = new Tile(tilePosX, tilePosY, 16);
		
		//Stores image
		img = spriteSheet;
		location = placement;
		
		//Set this panel with size as the image that we want
		this.setSize(tile.size,tile.size);
		
		//set location on the screen from placement given by however far its suppose to be
		this.setLocation(new Point(location.x + tilePosX*tile.size,location.y + tilePosY*tile.size));
		
		//calc where location on spritesheet is
		int x = (tilePosX * tile.size);
		int y = (tilePosY* tile.size);
			
		//Now to crop the image we want
		croppedImage = createImage(new FilteredImageSource(img.getSource(),
								   new CropImageFilter(x,y,tile.size,tile.size)));
		thisSprite = this;
		
		
		
	}
	
	//ADDITIONAL VARIABLES
	boolean isNull;
	
	
	//ADDITIONAL METHODS NEEDED SINCE WE CAN EDIT THESE SPRITES
	public void setNullImage(Image img){
		nullImage = img;
	}

	//this will allow us to set it to a null tile
	public void setNull(){
		changeTile(nullImage, new Tile(-1,-1,16));
		isNull = true;
		repaint();
	}
	
	//this will allow easy access to change the tile
	public void changeTile(Image crpImg, Tile tile){
		isNull = false;
		croppedImage = crpImg;
		this.tile = tile;
		
	}
	
	
	public String toString(){
	
		return "" + tile.toString() + "," + (this.getLocation().x -5)+ "," + (this.getLocation().y-5)+"";
	
	}
	
	public void paintComponent(Graphics g){
	
		g.drawImage(croppedImage,0,0,null);
	
	}

	public void rebuild(int tileX, int tileY) {
		
		isNull = false;
		this.tile.x = tileX;
		this.tile.y = tileY;
		int x = (tileX * tile.size);
		int y = (tileY* tile.size);
		
		
		update(getGraphics());
		
		//Now to crop the image we want
		croppedImage = createImage(new FilteredImageSource(img.getSource(),
								   new CropImageFilter(x,y,tile.size,tile.size)));
		//this.update(getGraphics());
	}
}
