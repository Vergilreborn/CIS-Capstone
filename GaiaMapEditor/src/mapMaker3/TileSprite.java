package mapMaker3;
import javax.swing.*;
import java.awt.*;
import java.awt.image.*;
import java.awt.event.*;

public class TileSprite extends JPanel{

	


	private static final long serialVersionUID = 1L;

	//tests to see if this tile is selected
	boolean isSelected;
	
	//gets location of the tile
	Point location = new Point();
	
	//this is the tile information
	Tile tile;
	
	//this is this sprite specifically
	TileSprite thisSprite;
	
	//Image to be cropped and also the cropped image
	Image img;
	
	
	
	Image croppedImage;
	
	public TileSprite(){
		
	}
	
	public TileSprite(Image spriteSheet,Point placement,int tilePosX,int tilePosY){
		
		//creates the tile properties of X,Y coordinates and size(16)
		tile = new Tile(tilePosX, tilePosY, 16);
		
		//Stores image
		img = spriteSheet;
		location = placement;
	
		//Set this panel with size as the image that we want
		this.setSize(tile.size,tile.size);
		
		//set location on the screen from placement given by however far its suppose to be
		this.setLocation(new Point((location.x + tilePosX)+ tilePosX*tile.size,(location.y + tilePosY)+ tilePosY*tile.size));
		
		//calc where location on spritesheet is
		int x = (tilePosX * tile.size);
		int y = (tilePosY* tile.size);
			
		//Now to crop the image we want
		croppedImage = createImage(new FilteredImageSource(img.getSource(),
								   new CropImageFilter(x,y,tile.size,tile.size)));
		thisSprite = this;
			
	}
	//This adds the mouse function to this tile checking if its pressed.
	//If it is it changes the selectTile to be the same image as this tile
	public void addMouseFunction(final SelectedTile select){
		
		addMouseListener(new MouseAdapter(){
			public void mousePressed(MouseEvent e){
				
				select.changeTile(croppedImage,tile);
				select.repaint();
			}});
		
		
	}
	public void logoOnly(Image logo){
		croppedImage = logo;
	}
	//This will draws the image, since it will be cropped
	//at the right size and position on the sprite sheet
	public void paintComponent(Graphics g){
	g.drawImage(croppedImage,0,0,null);	
	}
	
	
}
