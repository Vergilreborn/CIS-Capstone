package enemyPlacer;
import java.awt.*;
import java.awt.event.*;
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
	SelectingTiles sT;
	Parts [][] partsList;
	int tileX;
	int tileY;
	
	
	//This holds the information that is being placed on the bottom right hand
	//of the map
	boolean isNull;
	int spriteBlockHeight;
	int spriteBlockWidth;
	char type;
	JPanel map;
	
	public Parts(int x, int y,Image items, Image enemies,Image npc,JPanel map){
		this.map = map;
		tileX = x;
		tileY = y;
		this.setSize(16,16);
		cropped = null;
		width = 16;
		height = 16;
		isNull = true;
		this.enemies = enemies;
		this.items = items;
		this.npc = npc;
		this.setOpaque(false);
	}
	
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
	
		
		this.repaint(this.getLocation().x,this.getLocation().y,16,16);

		this.update(getGraphics());
		

	}
	
	public void convertPart(int blockX, int blockY, Item it){
		
		int x = it.x;
		int y = it.y;
		
		cropped = createImage(new FilteredImageSource(items.getSource(),
				   new CropImageFilter(x+(blockX*16),y+(blockY*16),16,16)));
		//this.validate();
	//	this.update(getGraphics());
	//	this.repaint(this.getLocation().x,this.getLocation().y,this.width,this.height);
	//	this.update(getGraphics());
		this.repaint(this.getLocation().x,this.getLocation().y,16,16);

		this.update(getGraphics());

	}
	
	public void convertPart(int blockX, int blockY, Npc np){
		
		int x = np.x;
		int y = np.y;
		
		cropped = createImage(new FilteredImageSource(npc.getSource(),
				   new CropImageFilter(x+(blockX*16),y+(blockY*16),16,16)));
		
		//this.update(getGraphics());
		this.repaint(this.getLocation().x,this.getLocation().y,16,16);
		this.update(getGraphics());
		

	}
	
	public void setNull(){
		
		cropped = null;
		this.update(getGraphics());
		
		this.repaint(this.getLocation().x,this.getLocation().y,this.width,this.height);
		this.update(getGraphics());
	
	
	}
	
	public void connect(SelectingTiles st,Parts[][] partList){
		this.partsList = partList;
		this.sT = st;
	}
	public void setCropped(Image img){
		cropped = img;
	}
	
	public void setInfo(char type, int width, int height){
		spriteBlockHeight = height;
		spriteBlockWidth = width;
		this.type = type;
	}
	
	public void mouseFunction(){
		
		this.addMouseListener(new MouseAdapter(){
			public void mouseClicked(MouseEvent a){
				int tileWidth;
				int tileLength;
				
				switch(sT.currentSelection){
				case 'e': 
					
						tileLength = sT.currEn.height/16;
						tileWidth = sT.currEn.width/16;
						
						if(tileX+1 < tileWidth || tileY+1 < tileLength)
							break;
						
						for(int i = 0; i < tileWidth; i++){
							for(int j = 0; j < tileLength; j++){
								if(!partsList[tileY - j][tileX - i].isNull)
									return;
							}
						}
						
						for(int i = 0; i < tileWidth; i++){
							for(int j = 0; j < tileLength; j++){
								
					
								partsList[tileY - j][tileX - i].convertPart(tileWidth-i-1,tileLength-j-1 , sT.currEn);
							}
						}
						
						
						
						
						
						partsList[tileY][tileX].setInfo('e',tileWidth,tileLength);//Hold the information into this sprite
						sT.inputIntoList(sT.currEn,tileX,tileY);
						break;
						
						
				case 'i':
						tileLength = sT.currItem.height/16;
						tileWidth = sT.currItem.width/16;
						if(tileX < tileWidth || tileY < tileLength)
							break;	
						for(int i = 0; i < tileWidth; i++){
							for(int j = 0; j < tileLength; j++){
								if(!partsList[tileY - j][tileX - i].isNull)
									return;
							}
						}
						
						for(int i = 0; i < tileWidth; i++){
							for(int j = 0; j < tileLength; j++){
								partsList[tileY - j][tileX - i].convertPart(tileWidth-i-1,tileLength-j-1 , sT.currItem);
							}
						}
						
						partsList[tileY][tileX].setInfo('i',tileWidth,tileLength);//Hold the information into this sprite
						sT.inputIntoList(sT.currItem,tileX,tileY);
						
						break;
				case 'n':
						tileLength = sT.currNpc.height/16;
						tileWidth = sT.currNpc.width/16;
						if(tileX < tileWidth || tileY < tileLength)
							break;
						for(int i = 0; i < tileWidth; i++){
							for(int j = 0; j < tileLength; j++){
								if(!partsList[tileY - j][tileX - i].isNull)
									return;
							}
						}
						
						for(int i = 0; i < tileWidth; i++){
							for(int j = 0; j < tileLength; j++){
								partsList[tileY - j][tileX - i].convertPart(tileWidth-i-1,tileLength-j-1 , sT.currNpc);
							}
						}
						
						partsList[tileY][tileX].setInfo('i',tileWidth,tileLength);//Hold the information into this sprite
						sT.inputIntoList(sT.currNpc,tileX,tileY);
						break;
				
				}
				//--------- REFRESH  ---------//
				//Above this code is the code to process the information and to
				//redraw the map holding onto the tiles.
				//The 2 lines before this refreshes it.
				map.update(map.getGraphics());
				map.repaint();
		}});
	}
	
	public void paintComponent(Graphics g){
		
		g.drawImage(cropped,0,0,width,height,null);
	
		
	}
}
