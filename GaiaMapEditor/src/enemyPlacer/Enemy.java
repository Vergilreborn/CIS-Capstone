package enemyPlacer;
import javax.swing.*;
import java.awt.*;
import java.awt.image.CropImageFilter;
import java.awt.image.FilteredImageSource;
import java.awt.event.*;


public class Enemy extends JPanel {

	private static final long serialVersionUID = 1L;

	String type;
	
	String name;
	int health;
	int strength;
	int defense;
	Image croppedImage;
	boolean selected;
	Enemy []  enemyList;
	SelectingTiles selector;
	int x;
	int y;
	int width;
	int height;
	int itemNumber;
	
	public Enemy(String name,Image img,int posX, int posY, int width, int height ){
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
				for(int i = 0; i < enemyList.length; i++){
					if(i == itemNumber){
						setBorder(BorderFactory.createLineBorder(new Color(230,0,0), 2));		
					}else
						enemyList[i].setBorder(BorderFactory.createLineBorder(new Color(0,0,0), 1));
					repaint(enemyList[i].x,enemyList[i].x,enemyList[i].width,enemyList[i].health);
				}
				
				
				selector.changeSelected('e',itemNumber);
				
				
		}});
	}
	
	public void connectArray(Enemy [] list){
		enemyList = list;
	}
	public void paintComponent(Graphics g){
		
		g.drawImage(croppedImage, 0,0,null);
	}

}
