package enemyPlacer;
import javax.swing.*;
import java.awt.*;

import java.awt.Graphics;
import java.util.*;

public class Help extends JFrame{

	//Helper Frame that will appear showing a visual of how to use
	//the Map 
	
	public Help(String name, Image img){
		super(name);
		this.setSize(700,700);
		this.add(new Helper(img));
		this.setResizable(false);
		
		
		
		
	}
	

}

class Helper extends JPanel{

	Image img;
	public Helper(Image img){
		this.setSize(700,700);
		this.img = img;
	}
	public void paintComponent(Graphics g){
		
		g.drawImage(img, 0,0,null);
	}	
}

