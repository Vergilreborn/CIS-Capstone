package enemyPlacer;
import javax.swing.*;
import java.awt.*;
import java.util.ArrayList;

public class SelectingTiles extends JPanel{

	
	
	private static final long serialVersionUID = 1L;
	
	Parts [][] display = new Parts[4][4];
	Image enemies;
	Image items;
	Image objects;
	Image npc;
	char currentSelection;
	
	Enemies eList;
	Items iList;
	Npcs npcList;
	Enemy currEn;
	Npc currNpc;
	Item currItem;
	GObjects oList;
	GObject currObj;
	// Obj currObj;
	boolean isEnemy = false;
	boolean isNpc = false;
	boolean isItem = false;
	boolean isObject = false;
	String name;
	
	DefaultListModel list;
	ArrayList <String> data;
	SetInfo stats;
	EnemyPlacerMain epm;
	
	
	
	public SelectingTiles(char imageType, Image items, Image enemies, Image NPC, Image objects, Enemies enm, Items itms,Npcs npcs,GObjects g, EnemyPlacerMain epm){
		this.epm = epm;
		iList = itms;
		eList = enm;
		npcList = npcs;
		oList = g;
		this.items = items;
		this.enemies = enemies;
		this.objects = objects;
		this.npc = NPC;
		this.setSize(64,64);
		this.setLayout(null);
		
		this.currentSelection = imageType;
		this.setOpaque(false);
		
		
		for(int y = 0; y < display.length; y++){
			for(int x = 0; x < display[y].length;x++){
				display[y][x] = new Parts(enemies,items,NPC);
				display[y][x].setLocation(16*x,16*y);
				this.add(display[y][x]);
			}
		}
		 validate();
		
	}
	public void changeSelected(char type, int itemNumber){
		currentSelection = type;
		switch(type){
		
			case 'e': isEnemy = true; 
					  isItem = isObject = isNpc = false; 
					  currEn = eList.enemies[itemNumber]; 
					  for(int y = 0; y < display.length;y++)
						  for(int x = 0; x < display[y].length;x++)
							  display[y][x].setNull();
					  for(int y = 0; y < currEn.height/16;y++)
						  for(int x = 0; x < currEn.width/16;x++){
							  display[y][x].convertPart(x,y , currEn);	
							  validate();
						  }
					  break;
			case 'i' : isItem = true;
					   isEnemy = isObject = isNpc = false;
					   currItem = iList.item[itemNumber];
					   for(int y = 0; y < display.length;y++)
							  for(int x = 0; x < display[y].length;x++)
								  display[y][x].setNull();
						  for(int y = 0; y < currItem.height/16;y++)
							  for(int x = 0; x < currItem.width/16;x++){
								  display[y][x].convertPart(x,y , currItem);	
								  validate();
							  }
						  break;
			case 'n' : isNpc= true;
					   isEnemy = isObject = isItem = false;
					   currNpc = npcList.npcPeople[itemNumber];
					   for(int y = 0; y < display.length;y++)
							  for(int x = 0; x < display[y].length;x++)
								  display[y][x].setNull();
						  for(int y = 0; y < currNpc.height/16;y++)
							  for(int x = 0; x < currNpc.width/16;x++){
								  display[y][x].convertPart(x,y , currNpc);	
								  validate();
							  }
						  break;
						  
			case 'o' : isObject= true;
			   isEnemy = isNpc = isItem = false;
			   currObj = oList.objs[itemNumber];
			   for(int y = 0; y < display.length;y++)
					  for(int x = 0; x < display[y].length;x++)
						  display[y][x].setNull();
				  for(int y = 0; y < currObj.height/16;y++)
					  for(int x = 0; x < currObj.width/16;x++){
						  display[y][x].convertPart(x,y , currObj);	
						  validate();
					  }
				  break;
					  
		
		}
	
		repaint(display[0][0].getLocation().x, display[0][0].getLocation().y,16*4,16*4);	
	}
	
	
}
