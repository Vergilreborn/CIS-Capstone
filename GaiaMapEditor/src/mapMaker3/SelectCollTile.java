package mapMaker3;
import javax.swing.*;

import java.awt.*;

	
//This class overlies the selectedTile box. It will display the collision
//type and it will update according to the color given
public class SelectCollTile extends JPanel{

	
	private static final long serialVersionUID = 1L;
	JPanel background;
	
	//Constructor that sets the location of the box setting the initial collision
	//type to null with the null color 
	public SelectCollTile(Point location, int size,JPanel background){
		this.setLocation(location);
		this.setSize(32,32);
		
		setNullColorBackground();
		this.setOpaque(true);
		this.background = background;
	}
	
	//Sets the collision to the null color, which is fully transparent
	public void setNullColorBackground(){
	
		this.setBackground(new Color(0,0,0,0));
		this.repaint();
		
	}
	
	//This redraws this box and updates the color that it should be
	public void updateBack(Color c){
		this.setBackground(c);
		background.repaint(this.getLocation().x, this.getLocation().y,this.getWidth(),this.getHeight());
		
		
	}
	
	
	
	
	
}
