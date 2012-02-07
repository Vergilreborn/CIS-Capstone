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

public class BuildingFieldTile extends JPanel {


	
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
	boolean isNull = false;
	//mouse hovering over 
	static boolean isSelected;
    //	static int clicked = -1;
	static BorderLayout border = new BorderLayout();
	
	BuildingFieldTile thisSprite;
	//image and location of the image
	Point location;
	Image img;
	Image croppedImage;
	Tile tile;
	
	
	//This is the constructor
	public BuildingFieldTile(String img, int tilePosX, int tilePosY, int size, Point location, int tileMoveBy){
		this(new ImageIcon(img).getImage(),tilePosX,tilePosY,size,location,tileMoveBy);
	}

	
	public BuildingFieldTile(Image img, int tilePosX, int tilePosY, int size, Point location, int tileMoveBy){
		this.img = img;
		tile = new Tile(tilePosX, tilePosY);
		this.tileMoveBy = tileMoveBy;
		this.size = size;
		this.setSize(size,size);
		this.setLocation(new Point(location.x + tileMoveBy * tilePosX,
				location.y + tileMoveBy*tilePosY));
		
		croppedImage = createImage(new FilteredImageSource(img.getSource(),
								new CropImageFilter(posX,posY,size,size)));
		isNull = true;
		this.tile = new Tile(0,0);
		thisSprite = this;
	}
	
	//public BuildingFieldTile(Image img, int tilePosX, int tilePosY, int size, Point location, int tileMoveBy){
		
		
	//	tile = new Tile(tilePosX, tilePosY);
		
		
	//	this.img = img;
	//	this.tileMoveBy = tileMoveBy;
	//	this.size = size;
	//	this.setSize(size,size);
						
	//	posX = tileMoveBy * tilePosX;
	//	posY = tileMoveBy * tilePosY;
		
	//	this.setLocation(new Point(location.x + tileMoveBy*tilePosX,
	//		  location.y + tileMoveBy*tilePosY));
		
		
		//INformation to print when finding position of tiles
		//System.out.println(posX + " x,y " + posY);
	//	croppedImage = createImage(new FilteredImageSource(img.getSource(),
	//				new CropImageFilter(posX,posY,size,size)));
	
		//thisSprite = this;
	
		
	//}

	
	public void setNull(){
		changeTile("null.png",0,0,tileMoveBy);
		this.tile = new Tile(0,0);
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
														
				}
			}});
	}
	
	public void addMouseFunction(final SelectedTileSprite selected, final MapMaker map){
		addMouseListener(new MouseAdapter(){
			public void mouseEntered(MouseEvent e){
				isSelected = true;
			}
			public void mouseExited(MouseEvent e){
				isSelected = false;
			}
			public void mouseClicked(MouseEvent e){
				if(isSelected){
					if(selected.isNull)
						changeTile("null.png",0,0,selected.tileMoveBy);
					else{
					changeTile("GAIATILES.png",selected.tile.x, selected.tile.y, selected.tileMoveBy);
					isNull = false;
					repaint();
					paint(getGraphics());
					map.background.repaint();
					repaint();
					}
				}
			}
		});
	
	}
	public void setUpTile(Tile tile){
		this.tile = tile;
	}
	
	
	public void changeTile(String imgName, int tilePosX, int tilePosY, int tileMoveBy){
		this.img = new ImageIcon(imgName).getImage();
		changeTile(tilePosX, tilePosY, tileMoveBy);
		this.tile = new Tile(tilePosX, tilePosY);
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
