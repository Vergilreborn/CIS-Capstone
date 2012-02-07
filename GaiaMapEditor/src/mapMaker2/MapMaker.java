package mapMaker2;

import javax.swing.*;

import java.awt.*;
import java.awt.event.*;




public class MapMaker extends JFrame{
	
	
	private static final long serialVersionUID = 1L;
	
	//This is just the background color
	JPanel background;
	
	//The working Table panel
	JPanel workTableWindow;
	Point workTableLocation;
	BuildingFieldTile[][] tableTiles = new BuildingFieldTile[42][34]; 
	
	//The tilesheet that will be already preloaded.
	Point sSheetLocation;
	TileSprite [][] sheetTiles= new TileSprite[25][40];
	SelectedTileSprite  selected;
	
	
	
	public static void main(String [] args){
		new MapMaker("MapMaker v1.2");
		
	}
	
	public MapMaker(String title){
		//initialize the Frame, 
		this.setTitle(title);
		
		//this.setVisible(true);
		
		
		//width will be 
		//697 is max height
		//16 is the extra for safety
		this.setSize(1166, 720);
		this.setResizable(false);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		
		
		
		//This is the inner main window that is the
		//same size as the frame
		background = new JPanel();
		background.setBounds(this.getBounds());
		background.setBackground(new Color(0,0,240));
		background.setLayout(null);
		
		
		//We will prepare the other frames
		loadMapSpriteSheet(background);
		buildLowerLayer(background);
		
		addButtonAndFunctionality(background);
		
		background.validate();
		this.add(background);
		
		this.validate();
		this.setVisible(true);
		
		//long prev = System.currentTimeMillis();
		while(true){
			//long t = System.currentTimeMillis();
		//	if((t - prev) > 30){
			
		//		prev = t;
				this.repaint();
				
//			}
		}
	
		
		
	}
	public void addButtonAndFunctionality(JPanel mainWindow ){
		StandardButton remove = new StandardButton("Remove");
		remove.setLocation(new Point(375,660));
		remove.setSize(100,25);
		mainWindow.add(remove);
		
		StandardButton clearAll = new StandardButton("Clear All");
		clearAll.setLocation(new Point(500,660));
		clearAll.setSize(100,25);
		mainWindow.add(clearAll);
		
		StandardButton printStatement = new StandardButton("Tester");
		printStatement.setLocation(new Point(250,660));
		printStatement.setSize(100,25);
		mainWindow.add(printStatement);
		
		StandardButton newFile = new StandardButton("New Map");
		newFile.setLocation(new Point(250,630));
		newFile.setSize(100,25);
		mainWindow.add(newFile);
		
		StandardButton save= new StandardButton("Save Map");
		save.setLocation(new Point(375,630));
		save.setSize(100,25);
		mainWindow.add(save);
		
		StandardButton load = new StandardButton("Load Map");
		load.setLocation(new Point(500,630));
		load.setSize(100,25);
		mainWindow.add(load);
		
		
		remove.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				selected.setNull(new ImageIcon("null.png").getImage());
			}});
		clearAll.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				for(int i = 0; i < tableTiles.length; i++){
					for(int j = 0; j < tableTiles[i].length; j++){
						tableTiles[i][j].setNull();
					}
			}}});
		printStatement.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				for(int i = 0; i < tableTiles.length; i++){
					for(int j = 0; j < tableTiles[i].length; j++){
						if(!tableTiles[i][j].isNull)
							System.out.println(tableTiles[i][j].tile);
					}
			}}});
		
		
	}
	//This will focus on the sprite Sheet section and filling it with images. 
	//....Or so we hope
	public void loadMapSpriteSheet(JPanel mainWindow){
		//sheetTiles[x][y] where Max(x) = 25, Max(y) = 40
		
		//Learn how to hide and show certian tiles. 
		//Then if a button is pressed, we will update the picture and move over the 
		//tileSprites. 
		//Also update tilesprite to show if it is updated/hidden/so on so forth
		JLabel selLabel = new JLabel("Selected");
		selLabel.setForeground(new Color(255,255,255));
		selLabel.setSize(100,25);
		selLabel.setFont(new Font("Arial", 1, 16));
		selLabel.setLocation(632-15, 625);
		
		selected = new SelectedTileSprite("GAIATILES.png",0,0,36,new Point(632,650),17);
		selected.setBorder();
		mainWindow.add(selected);
		mainWindow.add(selLabel);
		
		for(int y = 0; y < 40; y++){
			for(int x = 0; x < 25; x++){
				
				sheetTiles[x][y] = new TileSprite("GAIATILES.png",
												x,y,18,new Point(728,5),17);
				//if(y > 33 || x > 20)
				//	sheetTiles[x][y].setVisible(false);
				sheetTiles[x][y].addMouseS(selected,this);
				mainWindow.add(sheetTiles[x][y]);
			//	mainWindow.validate();
			}
			
		}
		
		// 882 , 576
		for(int x = 0; x < 42; x++){
			for(int y = 0; y < 34; y++){
				tableTiles[x][y] = new BuildingFieldTile("null.png",
												x,y,17,new Point(5,5),17);
				
				tableTiles[x][y].setBackground((new Color(0,0,0)));
				tableTiles[x][y].addMouseFunction(selected,this);
				mainWindow.add(tableTiles[x][y]);
			}
		}
		
		mainWindow.repaint();
		this.validate();
	
	
	
	}
	
	//This method will setup the table and the spritesheet box backgrounds
	public void buildLowerLayer(JPanel mainWindow){
		
		//This panel can hold 42 x 36 17x17 sprites
		//workTableWindow = new JPanel();
	//	workTableWindow.setLocation(5,5);
	//	workTableLocation = workTableWindow.getLocation();
	//	workTableWindow.setSize(714,576);
	//	workTableWindow.setBackground(new Color(255,255,255));
		
		
	//	mainWindow.add(workTableWindow);
	
		
	}
}
