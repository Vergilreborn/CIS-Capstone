package mapMaker3;
import javax.swing.*;

import java.awt.*;

	

public class SelectCollTile extends JPanel{

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	JPanel background;
	
	public SelectCollTile(Point location, int size,JPanel background){
		this.setLocation(location);
		this.setSize(32,32);
		
		setNullColorBackground();
		this.setOpaque(true);
		this.background = background;
	}
	
	
	public void setNullColorBackground(){
	
		this.setBackground(new Color(0,0,0,0));
		this.repaint();
		
	}
	public void updateBack(Color c){
		this.setBackground(c);
		background.repaint(this.getLocation().x, this.getLocation().y,this.getWidth(),this.getHeight());
		
		
	}
	
	
	
	
	
}
