package enemyPlacer;
import javax.swing.*;
import java.awt.*;
import java.awt.image.CropImageFilter;
import java.awt.image.FilteredImageSource;

public class Enemy extends JPanel{
	
	String name;
	int health;
	int strength;
	int defense;
	Image croppedImage;
	
	public Enemy(Image img, int width, int height, int posX, int posY){
		
		
		this.setSize(width, height);
		croppedImage = createImage(new FilteredImageSource(img.getSource(),
				   new CropImageFilter(posX+1,posY+1,width,height)));
		
		
		
	}
	
	public void paintComponent(Graphics g){
		
		g.drawImage(croppedImage, 0,0,null);
	}

}
