package enemyPlacer;
import java.awt.*;
import java.awt.image.CropImageFilter;
import java.awt.image.FilteredImageSource;

import javax.swing.*;

public class Parts extends JPanel {
	
	int width;
	int height;
	int xPos;
	int yPos;
	Image cropped;
	Image enemies;
	Image items;
	Image npc;
	
	public Parts(Image enemies, Image items,Image npc){
		this.setSize(16,16);
		this.enemies = enemies;
		this.items = items;
		this.npc = npc;
		
		cropped = null;
		width = 16;
		height = 16;
		this.setOpaque(false);
		
	}
	
	public void convertPart(int blockX, int blockY, Enemy en){
		
		int x = en.x;
		int y = en.y;
		
		cropped = createImage(new FilteredImageSource(enemies.getSource(),
				   new CropImageFilter(x+(blockX*16),y+(blockY*16),16,16)));
		
		this.update(getGraphics());
		this.repaint(this.getLocation().x,this.getLocation().y,this.width,this.height);
		this.update(getGraphics());
		

	}
	
	public void convertPart(int blockX, int blockY, Item it){
		
		int x = it.x;
		int y = it.y;
		
		cropped = createImage(new FilteredImageSource(items.getSource(),
				   new CropImageFilter(x+(blockX*16),y+(blockY*16),16,16)));
		
		this.update(getGraphics());
		this.repaint(this.getLocation().x,this.getLocation().y,this.width,this.height);
		this.update(getGraphics());
		

	}
	
	public void convertPart(int blockX, int blockY, Npc np){
		
		int x = np.x;
		int y = np.y;
		
		cropped = createImage(new FilteredImageSource(npc.getSource(),
				   new CropImageFilter(x+(blockX*16),y+(blockY*16),16,16)));
		
		this.update(getGraphics());
		this.repaint(this.getLocation().x,this.getLocation().y,this.width,this.height);
		this.update(getGraphics());
		

	}
	
	public void setNull(){
		
		cropped = null;
		this.update(getGraphics());
		
		this.repaint(this.getLocation().x,this.getLocation().y,this.width,this.height);
		this.update(getGraphics());
	
	
	}
	public void paintComponent(Graphics g){
		
		g.drawImage(cropped,0,0,width,height,null);
	
		
	}
}
