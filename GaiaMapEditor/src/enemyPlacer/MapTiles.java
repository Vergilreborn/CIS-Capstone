package enemyPlacer;
import javax.swing.*;

import java.awt.*;
import java.awt.image.*;

public class MapTiles extends JPanel{

	

	private static final long serialVersionUID = 1L;
	Image croppedImage = null;
	Image tile;
	Image nullImage;
	CollisionTiles tileCol;
	char collType = 'n';
	boolean isNull;
	
	public MapTiles(Image tile, Point location, int tileX, int tileY, char collisionType,Image nullImage){
		this.nullImage = nullImage;
		this.setSize(16,16);
		this.setLocation(location);
		this.tile = tile;
		
		int tilePositionX = (tileX * 16);
		int tilePositionY = (tileY * 16);
		
		croppedImage = createImage(new FilteredImageSource(tile.getSource(),
								   new CropImageFilter(tilePositionX,tilePositionY,16,16)));
		
	}
	
	public void setNull(){
		croppedImage = nullImage;
		
	}
	
	public void updateTile(int tileX, int tileY, char collisionType){
		int tilePositionX = (tileX * 16);
		int tilePositionY = (tileY * 16);
		
		croppedImage = createImage(new FilteredImageSource(tile.getSource(),
				   new CropImageFilter(tilePositionX,tilePositionY,16,16)));
		
		this.repaint(this.getLocation().x,this.getLocation().y,16,16);
		update(getGraphics());
		

	}
	public void connect(CollisionTiles t){
	this.tileCol = t;	
	
	}
	
	
	public void paintComponent(Graphics g){
		
		g.drawImage(croppedImage,0,0,null);
	
	}
	
}
