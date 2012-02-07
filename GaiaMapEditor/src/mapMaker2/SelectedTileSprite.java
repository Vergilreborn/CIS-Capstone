package mapMaker2;

import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Graphics;
import java.awt.Image;
import java.awt.Point;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.image.CropImageFilter;
import java.awt.image.FilteredImageSource;

import javax.swing.BorderFactory;
import javax.swing.ImageIcon;
import javax.swing.JPanel;

public class SelectedTileSprite extends JPanel{

	//information to draw
	int size;
	int posX;
	int posY;
	int tileMoveBy;
	
	//mouse hovering over 
	static boolean isSelected;
    //	static int clicked = -1;
	static BorderLayout border = new BorderLayout();
	
	SelectedTileSprite thisSprite;
	//image and location of the image
	Point location;
	Image img;
	Image croppedImage;
	Tile tile;
	boolean isNull;
	
	//This is the constructor
	public SelectedTileSprite(String img, int tilePosX, int tilePosY, int size, Point location, int tileMoveBy){
		this(new ImageIcon(img).getImage(),tilePosX,tilePosY,size,location,tileMoveBy);
	}

	
	
	
	public SelectedTileSprite(Image img, int tilePosX, int tilePosY, int size, Point location, int tileMoveBy){
		
		
		tile = new Tile(tilePosX, tilePosY);
		
		
		this.img = img;
		this.tileMoveBy = tileMoveBy;
		this.size = size;
		this.setSize(size,size);
		this.location = location;
		posX = tileMoveBy * tilePosX;
		posY = tileMoveBy * tilePosY;
		
		this.setLocation(new Point(location.x + tileMoveBy*tilePosX,
			  location.y + tileMoveBy*tilePosY));
		
		
		//INformation to print when finding position of tiles
		//System.out.println(posX + " x,y " + posY);
	//	croppedImage = createImage(new FilteredImageSource(img.getSource(),
		//			new CropImageFilter(posX,posY,size,size)));
		croppedImage = createImage(new FilteredImageSource(img.getSource(),
				new CropImageFilter(posX,posY,tileMoveBy,tileMoveBy)));
		thisSprite = this;
	
		
	}
	
	public void setBorder(){
		thisSprite.setBorder(BorderFactory.createLineBorder(new Color(0,0,0),2));
	}
	
	
	public void setUpTile(Tile tile){
		this.tile = tile;
	}
	
	
	
	public void changeTile(int tilePosX, int tilePosY, int tileMoveBy){
		isNull = false;
		this.tile.x = tilePosX;
		this.tile.y = tilePosY;
		posX = tileMoveBy * tilePosX;
		posY = tileMoveBy * tilePosY;
		croppedImage = createImage(new FilteredImageSource(img.getSource(),
					new CropImageFilter(posX,posY,tileMoveBy,tileMoveBy)));
		
		//believe this goes first....
	//	this.repaint();
		
	}
	public void setNull(Image img){
		croppedImage = createImage(new FilteredImageSource(img.getSource(),
				new CropImageFilter(0,0,tileMoveBy,tileMoveBy)));
		isNull = true;
	}
	
	
	//this is to draw the image in the correct position
	public void paintComponent(Graphics g){
		g.drawImage(croppedImage, 0, 0, size, size, null);	     

	
	}


	
	
	
}
