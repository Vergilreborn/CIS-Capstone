package mapMaker3;
import javax.swing.*;
import java.awt.*;
import java.awt.image.*;

public class SelectedTile extends JPanel{


	private static final long serialVersionUID = 1L;

	//checks to see if this sprite is null
	boolean isNull;

	//add border around this tile
	BorderLayout border = new BorderLayout();
	
	//gets location of the tile
	Point location = new Point();
	
	//this is the tile information
	Tile tile;
	
	//Size to be displayed as
	int displaySize;
	//this is this sprite specifically
	SelectedTile thisSprite;
	
	//Image to be cropped and also the cropped image
	Image img;
	Image croppedImage;
	//The null image
	Image nullImage;
	
	boolean readyDrag = false;
	
	//collision character starts at nothing!
	char collChar = 'n';
	boolean isCollOnly = false;
	boolean isImageOnly = true;
	
	SelectCollTile collBackground;
	JPanel background;
	
	public SelectedTile(Image spriteSheet,Point placement,int displaySize, int tilePosX,int tilePosY,JPanel background){
		
		this.background = background;
		this.displaySize = displaySize;
		
		//creates the tile properties of X,Y coordinates and size
		tile = new Tile(tilePosX, tilePosY, 16);
		
		//Stores image
		img = spriteSheet;
		location = placement;
		
		//Set this panel with size as the image that we want
		this.setSize(displaySize,displaySize);
		
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
	
	public void addCollBackground(SelectCollTile background){
		collBackground = background;
	}

	//set a border around this sprite
	public void setBorder(){
		thisSprite.setBorder(BorderFactory.createLineBorder(new Color(255,255,255),1));
	}

	public void changeTile(Image img, Tile tile){
		croppedImage = img;
		this.tile = tile;
		background.repaint(this.getLocation().x,this.getLocation().y,this.getWidth(),this.getHeight());
		
	}
	
	public void setNullImage(Image nullImage){
		this.nullImage = nullImage;
	}
	
	//This sets this current image to null
	public void setToNull(){
		isNull = true;
		croppedImage = nullImage;	
		this.tile.x = -1;
		this.tile.y = -1;
		repaint();
		
	}
	
	public void updateColl(char collisionType){
		collChar = collisionType;
		switch(collChar){
		case 'w': collBackground.updateBack(new Color(255,0,0,80));break;
		case 'e': collBackground.updateBack(new Color(0,255,0,80));break;
		case 'l': collBackground.updateBack(new Color(0,0,255,80));break;
		case 'p': collBackground.updateBack(new Color(200,0,200,80));break;
		case 'n': collBackground.updateBack(new Color(0,0,0,0));break;
		case 'r': collBackground.updateBack(new Color(20,150,150,100)); break;
		case 's': collBackground.updateBack(new Color(190,186,20,100)); break;
		case 'u': collBackground.updateBack(new Color(240,244,44,100)); break;
		case 'i': collBackground.updateBack(new Color(57,240,240,100)); break;
		}
		collBackground.update(collBackground.getGraphics());
		
		
		
	}
	
	//This will draws the image, since it will be cropped
	//at the right size and position on the sprite sheet
	public void paintComponent(Graphics g){
	g.drawImage(croppedImage,0,0,displaySize,displaySize,null);	
	}
	
	
}


