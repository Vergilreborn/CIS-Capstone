package enemyPlacer;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.Image;
import java.awt.Point;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.image.CropImageFilter;
import java.awt.image.FilteredImageSource;

import javax.swing.BorderFactory;
import javax.swing.JPanel;

public class GObject extends JPanel {

			private static final long serialVersionUID = 1L;

			String type;
			
			String name;
			int health;
			int strength;
			int defense;
			Image croppedImage;
			boolean selected;
			GObject []  objList;
			SelectingTiles selector;
			int x;
			int y;
			int width;
			int height;
			int itemNumber;
			
			public GObject(String name,Image img,int posX, int posY, int width, int height ){
				this.type = name;
				this.setLocation(new Point(posX,posY));
				x = posX;
				y = posY;
				this.setOpaque(false);
				
				this.width = width;
				this.height = height;
				this.setSize(width, height);
				
				
			
						
				
				
				croppedImage = createImage(new FilteredImageSource(img.getSource(),
						   new CropImageFilter(posX,posY,width,height)));
				repaint();
				
				
			}
			public void connect(SelectingTiles selector){
				this.selector = selector;
			}
			
			public void setItemNumber(int number){
				itemNumber = number;
			}
			public void addMouse(){
				this.addMouseListener(new MouseAdapter(){
					public void mouseClicked(MouseEvent e){
						for(int i = 0; i < objList.length; i++){
							if(i == itemNumber){
								setBorder(BorderFactory.createLineBorder(new Color(230,0,0), 2));		
							}else
								objList[i].setBorder(BorderFactory.createLineBorder(new Color(0,0,0), 1));
							repaint(objList[i].x,objList[i].x,objList[i].width,objList[i].health);
						}
						
						
						selector.changeSelected('o',itemNumber);
						
						
				}});
			}
			
			public void connectArray(GObject [] list){
				objList = list;
			}
			public void paintComponent(Graphics g){
				
				g.drawImage(croppedImage, 0,0,null);
			}
			public String toString(){
				return type +"," + x + "," + y;
				}
	
	
	
}
