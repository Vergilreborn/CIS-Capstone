package enemyPlacer;
import java.awt.*;


public class GObjects {
	GObject [] objs = new GObject[1];
	
	public GObjects(Image img){
		objs[0] = new GObject("lvl",img,0,0,16,16);
		objs[0].setItemNumber(0);
		objs[0].connectArray(objs);
			
	}
		
}

