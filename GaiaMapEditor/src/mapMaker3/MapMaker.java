package mapMaker3;

import javax.swing.*;
import javax.swing.filechooser.FileNameExtensionFilter;

import java.awt.*;
import java.awt.event.*;
import java.io.FileNotFoundException;

//This is the map maker class which designs and displays the
//functioning map maker. 
//Next Update
//   Add 4 more collision types
//   Add the ability to increase the size of the map
//   Add a help button that displays a text explaining
//                   how this map editor is used
public class MapMaker extends JFrame{

	
	private static final long serialVersionUID = 1L;

	//this is just the background color
	JPanel background;
	JPanel newBack;

	int i = 0;
	
	JFileChooser fileChooser;
	FileOptions options = new FileOptions();
	
	//this'll be the working area sprites
	EditTile [][] tableTiles = new EditTile[38][48];
	CollisionTiles [][] colTiles = new CollisionTiles[38][48];
	//This is where the preloaded sprite sheet will be
	TileSprite[][] tilesToChoose = new TileSprite[40][25];
	
	//Radio Buttons
	JRadioButton [] config = new JRadioButton[3];
	
	//The tile we have selected
	SelectedTile selected;
	
	//Create instance of the map with the given name
	public static void main(String [] args) throws Exception{
		
		
		new MapMaker("Gaia Map Editor v1.4");

		
	}

	public MapMaker(String title) throws Exception{
		//set the name of the frame
		super(title);
			
		
		
		UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName());
		//this sets the size of the window
		this.setSize(1220,720);
		//we will not allow the user to resize this
		this.setResizable(false);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		
		//not to setup the background
		background = new JPanel();
		background.setBounds(this.getBounds());
		background.setBackground(new Color(0,0,240));
		//set the layout to null to move place panels wherever we 
		//want to
		background.setLayout(null);
	
		newBack = new JPanel();
		newBack.setBackground(new Color(0,0,0));
		newBack.setLayout(null);
		newBack.setLocation(5,5);
		//initiate the filechooser 
		
		
		
		fileChooser = new JFileChooser();
		fileChooser.setFileFilter(new FileNameExtensionFilter("Gaia Maps", "gmaps"));
	
		//building the working area and the feild
		buildSpriteSheet();
		
		//set up the buttons and add functionality to them
		addButtonsAndFunctionality();
		
		//add custom logo
		TileSprite logo = new TileSprite();
		logo.logoOnly(new ImageIcon("AlexIcon.png").getImage());
		logo.setLocation(5,630);
		logo.setSize(50,50);
		background.add(logo);
		
		//after everything is done we will make this visible to the user
		this.add(background);
		
		//a double check to make sure eveything is ready and then show it
		background.validate();
		background.repaint();
		this.validate();
		//update the images and everything for first time use
		this.update(getGraphics());
		this.setVisible(true);
		
		
		
	}
	
	//this creates simple buttons at the given location
	//and then add it to the screen for the user to see
	//and interact with
	public void addButtonsAndFunctionality(){
		
		//These next statements just adds buttons and 
		//it then gives the location and sets the size of them
		//before giving them functionality
		StandardButton remove = new StandardButton("Remove");
		remove.setLocation(new Point(185,660));
		remove.setSize(100,25);
		
		StandardButton clearAll = new StandardButton("Clear All");
		clearAll.setLocation(new Point(300,660));
		clearAll.setSize(100,25);
		
		StandardButton printStatement = new StandardButton("Tester");
		printStatement.setLocation(new Point(70,660));
		printStatement.setSize(100,25);
		
		StandardButton newFile = new StandardButton("New File");
		newFile.setLocation(new Point(70,630));
		newFile.setSize(100,25);
		
		StandardButton save = new StandardButton("Save Map");
		save.setLocation(new Point(185,630));
		save.setSize(100,25);
		
		StandardButton load = new StandardButton("Load Map");
		load.setLocation(new Point(300,630));
		load.setSize(100,25);
		
		//These buttons are for changing the collionTiles
		//StandardButton wall = new StandardButton("Wall");
		StandardButton wall = new StandardButton(new Color(255,0,0),new Color(175,0,0),
												new Color(255,100,100),new Color(100,0,0));
		wall.setLocation(480,641);
		wall.setSize(20,20);
		
		StandardButton land = new StandardButton(new Color(0,0,255),new Color(0,0,175),
												new Color(100,100,255), new Color(0,0,100));
		land.setLocation(510, 641);
		land.setSize(20,20);
		
		StandardButton water = new StandardButton(new Color(255,0,255),new Color(175,0,175),
												  new Color(200,100,200), new Color(125,0,125));
		water.setLocation(540,641);
		water.setSize(20,20);
		
		StandardButton ladder =new StandardButton(new Color(0,255,0),new Color(0,175,0),
								new Color(100,255,100),new Color(0,100,0));
		ladder.setLocation(570,641);
		ladder.setSize(20,20);
		
		StandardButton noColl = new StandardButton(new Color(40,40,40),new Color(0,0,0),
												   new Color(10,10,10),new Color(0,0,0));
		noColl.setLocation(600,641);
		noColl.setSize(20,20);

		
		StandardButton leftStair = new StandardButton(new Color(190,186,20), new Color(100,94,10),
													  new Color(120,116,18), new Color(80,76,10));
		leftStair.setLocation(520, 670);
		leftStair.setSize(20,20);
		
		StandardButton rightStair = new StandardButton(new Color(20,150,150), new Color(20,0,0),
		 											   new Color(20,100,100), new Color(20,0,0));
		rightStair.setLocation(480,670);
		rightStair.setSize(20,20);
	
		StandardButton topLeftStair = new StandardButton(new Color(240,244,44), new Color(200,204,24),
													  new Color(180,184,44), new Color(100,104,44));
		topLeftStair.setLocation(600, 670);
		topLeftStair.setSize(20,20);
		
		StandardButton topRightStair = new StandardButton(new Color(57,240,240), new Color(37,210,210),
		 											   new Color(57,180,180), new Color(57,20,20));
		topRightStair.setLocation(560,670);
		topRightStair.setSize(20,20);
		
		StandardButton jumpTop = new StandardButton(new Color(232,158,23),new Color(200,134,23)
															  ,new Color(200,140,23), new Color(190,130,23));
		jumpTop.setLocation(630,641);
		jumpTop.setSize(20,20);
		
		StandardButton jumpBottom = new StandardButton(new Color(154,106,16),new Color(134,86,16)
		                                              ,new Color(120,80,16), new Color(110,70,16));
		jumpBottom.setLocation(660,641);
		jumpBottom.setSize(20,20);
		

		
		//Creates radioButtons for what the user would like to use..as in change
		//this would result in collisions, images or both
		final JRadioButton imageOnly = new JRadioButton("Image Only", true);
		imageOnly.setLocation(690,650);
		imageOnly.setSize(90,15);
		imageOnly.setVisible(true);
		imageOnly.setBackground(background.getBackground());
		imageOnly.setForeground(Color.white);
		
		final JRadioButton collOnly = new JRadioButton("Collision Only", false);
		collOnly.setLocation(690,635);
		collOnly.setSize(90,15);
		collOnly.setVisible(true);
		collOnly.setBackground(background.getBackground());
		collOnly.setForeground(Color.white);
		
		final JRadioButton bothOnly = new JRadioButton("Both", false);
		bothOnly.setLocation(690,665);
		bothOnly.setSize(90,15);
		bothOnly.setVisible(true);
		bothOnly.setBackground(background.getBackground());
		bothOnly.setForeground(Color.white);
		
		
		//add all the buttons
		background.add(newBack);
		background.add(topLeftStair);
		background.add(topRightStair);
		background.add(rightStair);
		background.add(leftStair);
		background.add(imageOnly); config[0] = collOnly;
		background.add(collOnly);  config[1] = imageOnly;
		background.add(bothOnly);  config[2] = bothOnly;
		background.add(printStatement);
		background.add(newFile);
		background.add(save);
		background.add(load);
		background.add(clearAll);
		background.add(noColl);
		background.add(remove);
		background.add(wall);
		background.add(ladder);
		background.add(land);
		background.add(water);
		background.add(jumpTop);
		background.add(jumpBottom);

		
		
		//changes the selected tile to a null tile which will allow to 
		//remove tiles off the map.
		remove.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				selected.setToNull();
				selected.updateColl('n');
				background.repaint(selected.getLocation().x,selected.getLocation().y,
						           selected.getWidth(),selected.getHeight());
				
		}});
		
		//Resets all tiles to the null tile
		clearAll.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				for(int i = 0; i < tableTiles.length;i++)
					for(int j = 0; j < tableTiles[i].length;j++){
						if(!tableTiles[i][j].isNull)
							tableTiles[i][j].setNull();
						if(colTiles[i][j].collisionType != 'n')
							colTiles[i][j].updateTile('n');
					}
				repaintAll();
			
		}});
		
		
		//this is a tester that prints which tiles are not null.
		printStatement.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				for(int y = 0; y < tableTiles.length; y++){
					for(int x = 0; x < tableTiles[y].length; x++)
						if(!tableTiles[y][x].isNull)
							System.out.print(tableTiles[y][x].tile + " ");
					System.out.println();
					}
		}});
		
		//creates the saving function
		save.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
			int returnValue = fileChooser.showSaveDialog(null);
				if(returnValue == JFileChooser.APPROVE_OPTION){
					if(fileChooser.getSelectedFile()!= null)
						try{
						options.saveFile(fileChooser.getCurrentDirectory().getAbsolutePath() +"/"+ 
											fileChooser.getSelectedFile().getName(),tableTiles,colTiles);
						}catch(FileNotFoundException fnfe){
						}
		}}});
		
		//creates the loading function
		load.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				int returnValue = fileChooser.showOpenDialog(null);
				if(returnValue == JFileChooser.APPROVE_OPTION){
					try{
					
					options.loadFile(fileChooser.getSelectedFile().getAbsolutePath(), tableTiles,colTiles);
				
		
					newBack.update(newBack.getGraphics());
					newBack.repaint();
					
					}catch(FileNotFoundException fnfe){
					}
					
		}}});
		
		//this changes the other radios to false and to the image only true
		imageOnly.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				collOnly.setSelected(false);
				bothOnly.setSelected(false);
				imageOnly.setSelected(true);
				selected.isImageOnly = true;
				selected.isCollOnly = false;
				
		}});
		
		//this changes the other radios to false and to the both only true
		bothOnly.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				collOnly.setSelected(false);
				bothOnly.setSelected(true);
				imageOnly.setSelected(false);
				selected.isImageOnly = true;
				selected.isCollOnly = true;
		}});
		
		//this changes the other radios to false and to the coll only true
		collOnly.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				collOnly.setSelected(true);
				bothOnly.setSelected(false);
				imageOnly.setSelected(false);
				selected.isImageOnly = false;
				selected.isCollOnly = true;
		}});
		
		//The next few lines of codes updates the collision type
		//when the according buttons are pressed
		topRightStair.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				selected.updateColl('i');
		}});
		topLeftStair.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				selected.updateColl('u');
		}});
		rightStair.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				selected.updateColl('r');
		}});
		leftStair.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				selected.updateColl('s');
		}});
		
		wall.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				selected.updateColl('w');
		}});
		
		land.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				selected.updateColl('l');
		}});
		
		water.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				selected.updateColl('p');
		}});
		
		ladder.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				selected.updateColl('e');
		}});
		
		noColl.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				selected.updateColl('n');
			
		}});
		
		jumpTop.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				selected.updateColl('j');
			
		}});
		jumpBottom.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				selected.updateColl('m');
			
		}});
		
	}
	
	//Repaints the whole map from the 1st tile to the last one
	public void repaintAll(){
		
		background.repaint(tableTiles[0][0].getLocation().x,tableTiles[0][0].getLocation().y,
				   tableTiles[tableTiles.length-1][tableTiles[0].length-1].getLocation().x + 16,
				   tableTiles[tableTiles.length-1][tableTiles[0].length-1].getLocation().y+ 16);
		
	}
	
	public void buildSpriteSheet(){
	
		//Makes an area showing the selected tile
		JLabel selLabel = new JLabel("Selected");
		selLabel.setForeground(new Color(255,255,255));
		selLabel.setSize(100,25);
		selLabel.setFont(new Font("Calibri", 0, 16));
		selLabel.setLocation(410, 625);
		background.add(selLabel);
		
		JLabel collisionLabel = new JLabel("Collision Type");
		collisionLabel.setForeground(new Color(200,50,50));
		collisionLabel.setSize(100,25);
		collisionLabel.setFont(new Font("Calibri",1,16));
		collisionLabel.setLocation(508,612);
		background.add(collisionLabel);
		
		JLabel collLabels = new JLabel("Wall     Land    Water   Ladder   None   JumpT   JumpB");
		collLabels.setForeground(selLabel.getForeground());
		collLabels.setSize(220,25);
		collLabels.setFont(new Font("Calibri",0,10));
		collLabels.setLocation(481,625);
		background.add(collLabels);
		
		JLabel collLabels2 = new JLabel("Stair.BR    Stair.BL     Stair.UR     Stair.UL");
		collLabels2.setForeground(selLabel.getForeground());
		collLabels2.setSize(200,25);
		collLabels2.setFont(new Font("Calibri",0,10));
		collLabels2.setLocation(481,653);
		background.add(collLabels2);
		//creates selected sprite on first sprite in the spritesheet map
		//and setting it up
		selected = new SelectedTile(new ImageIcon("GTILES.png").getImage(),new Point(420,650),32,0,0,background);
		selected.setBorder();
		selected.setNullImage(new ImageIcon("null.png").getImage());
		selected.setToNull();
		
		
		//Collision tile for the selectedTile sprite
		SelectCollTile selectedColl = new SelectCollTile(selected.location,selected.displaySize,background);
		selected.addCollBackground(selectedColl);
		background.add(selectedColl);
		background.add(selected);
		
		
		
		
		//add sprite sheet tiles 40 on Y axis by 25 on x axis
		//and now add functionability to the mouse
		for(int y = 0; y < 40; ++y){
			for(int x = 0; x < 25; x++){
				tilesToChoose[y][x] = new TileSprite(new ImageIcon("GTILES.png").getImage(),new Point(785,5),
											   x,y);
				tilesToChoose[y][x].addMouseFunction(selected);
				
				background.add(tilesToChoose[y][x]);
			}
		}
		
		//create the editable map tiles
		
		
		//create collision tiles that will be above the editable map tiles
		for(int y= 0; y < 38; y++){
			for(int x = 0; x < 48; x++){
				tableTiles[y][x] = new EditTile(new ImageIcon("GTILES.png").getImage(),new Point(0,0),x,y);
				tableTiles[y][x].setNullImage(new ImageIcon("null.png").getImage());
				tableTiles[y][x].setNull();
				colTiles[y][x] = new CollisionTiles(16,tableTiles[y][x].getLocation());
				colTiles[y][x].addMouseFunction(selected,tableTiles[y][x],newBack);
				colTiles[y][x].setBackground(new Color(0,0,0,0));
			
				newBack.add(colTiles[y][x]);
				newBack.add(tableTiles[y][x]);
				
			}
			
		}
		newBack.setSize((48*16),(38*16));
	
	}	
}
