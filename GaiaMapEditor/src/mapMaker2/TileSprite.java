package mapMaker2;
import javax.swing.*;

import java.awt.*;
import java.awt.event.*;
import java.awt.image.*;



public class TileSprite extends JPanel{

	
	//Testing purposes. 
	/*public static void main(String [] args){
		JFrame t = new JFrame("test");
		t.setVisible(true);
		t.setSize(500,500);
		t.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		TileSprite j = new TileSprite("FireEmblemMapTiles.png",0,0,18,new Point(200,200),17);
		
		t.validate();
		t.add(j);
		
		j.setStartLocation(new Point(100,100));
		j.validate();
		j.changeTile(1, 1, 17);
		j.validate();
		
	}*/
	
	//information to draw
	int size;
	int posX;
	int posY;
	int tileMoveBy;
	
	//mouse hovering over 
	static boolean isSelected;
    //	static int clicked = -1;
	static BorderLayout border = new BorderLayout();
	
	TileSprite thisSprite;
	//image and location of the image
	Point location;
	Image img;
	Image croppedImage;
	Tile tile;
	
	
	//This is the constructor
	public TileSprite(String img, int tilePosX, int tilePosY, int size, Point location, int tileMoveBy){
		this(new ImageIcon(img).getImage(),tilePosX,tilePosY,size,location,tileMoveBy);
	}

	
	
	
	public TileSprite(Image img, int tilePosX, int tilePosY, int size, Point location, int tileMoveBy){
		
		
		tile = new Tile(tilePosX, tilePosY);
		
		
		this.img = img;
		this.tileMoveBy = tileMoveBy;
		this.size = size;
		this.setSize(size,size);
						
		posX = tileMoveBy * tilePosX;
		posY = tileMoveBy * tilePosY;
		
		this.setLocation(new Point(location.x + tileMoveBy*tilePosX,
			  location.y + tileMoveBy*tilePosY));
		
		
		//INformation to print when finding position of tiles
		//System.out.println(posX + " x,y " + posY);
		croppedImage = createImage(new FilteredImageSource(img.getSource(),
					new CropImageFilter(posX,posY,size,size)));
	
		thisSprite = this;
	
		
	}
	
	public void setBorder(){
		thisSprite.setBorder(BorderFactory.createLineBorder(new Color(0,0,0),2));
	}
	
	public void addMouseS(final SelectedTileSprite selected, final MapMaker map){
	
		addMouseListener(new MouseAdapter(){
			public void mouseEntered(MouseEvent e){
				isSelected = true;
								
			}
			public void mouseExited(MouseEvent e){
				isSelected = false;
				
			}
			public void mouseClicked(MouseEvent e){
				
				if(isSelected){
		 
					selected.changeTile(thisSprite.tile.x,thisSprite.tile.y, thisSprite.tileMoveBy);
				
					
					
					selected.repaint();
					selected.paint(selected.getGraphics());
					map.background.repaint();
				//	map.paint(map.getGraphics());
				
										
				}
			}});
		
		
	}
	public void setUpTile(Tile tile){
		this.tile = tile;
	}
	
	
	
	public void changeTile(int tilePosX, int tilePosY, int tileMoveBy){
		posX = tileMoveBy * tilePosX;
		posY = tileMoveBy * tilePosY;
		croppedImage = createImage(new FilteredImageSource(img.getSource(),
					new CropImageFilter(posX,posY,size,size)));
		
		//believe this goes first....
		this.repaint();
		
	}
	
	
	//this is to draw the image in the correct position
	public void paintComponent(Graphics g){
	//g.drawImage(croppedImage,location.x,location.y,location.x+tileMoveBy,location.y+tileMoveBy,
		//	0,0,size,size,null);
	g.drawImage(croppedImage,0,0,null);
	}

}
