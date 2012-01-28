package enemyPlacer;
import javax.swing.*;
import javax.swing.event.*;
import javax.swing.filechooser.FileNameExtensionFilter;


import java.util.*;
import java.awt.*;
import java.awt.event.*;
import java.io.FileNotFoundException;

public class EnemyPlacerMain extends JFrame{

	
	private static final long serialVersionUID = 1L;

	public static void main(String [] args){
		
		try{
		new EnemyPlacerMain();
		}catch(Exception e){
		System.err.println("Error broken, something is not working");	
		
		}
	}
	
	//Global Variables
	JPanel background;
	JPanel listHolding;
	JPanel enemies;
	JPanel items;
	JPanel npc;
	FileOptions fo = new FileOptions();
	JFileChooser fileChooser = new JFileChooser();
	StandardButton remove;
	StandardButton highlight;
	StandardButton loadMapOnly;
	StandardButton loadMapWithEnemies;
	StandardButton saveMapWithEnemies;
	JList list;
	ArrayList<String> data;
	DefaultListModel listData;
	MapTiles [][] mapTiles = new MapTiles[38][48];
	CollisionTiles[][] colTiles = new CollisionTiles[38][48];
	JPanel itemEnemyNpc;
	JTabbedPane tabs;
	
	public EnemyPlacerMain() throws Exception{
		super("Enemy Placer v 1.0");

		//Setting up the default operations of the this JFrame
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName());
		this.setSize(1340,720);
		this.setResizable(false);
		
		fileChooser = new JFileChooser();
		fileChooser.setFileFilter(new FileNameExtensionFilter("Gaia Maps", "gmaps"));
		
		//Setting up the panel
		background = new JPanel();
		background.setSize(this.getSize());
		background.setBackground(new Color(0,0,240));
		background.setLayout(null);
	
		listHolding = new JPanel();
		listHolding.setBackground(background.getBackground());
		listHolding.setLocation(new Point(4,400));
		listHolding.setSize(270,212);
		
		itemEnemyNpc = new JPanel(new GridLayout(1,1)); 
		itemEnemyNpc.setBackground(background.getBackground());
		itemEnemyNpc.setLocation(new Point(5,5));
		itemEnemyNpc.setSize(550,385);
		
		
		//Setting up the buttons
		remove = new StandardButton("Remove");
		remove.setLocation(new Point(20,630));
		remove.setSize(110,25);
		
		highlight = new StandardButton("Highlight");
		highlight.setLocation(new Point(145,630));
		highlight.setSize(110,25);
		
		loadMapOnly = new StandardButton("Load Map Only");
		loadMapOnly.setLocation(305,630);
		loadMapOnly.setSize(110,25);
		
		loadMapWithEnemies = new StandardButton("Load Enemies");
		loadMapWithEnemies.setLocation(430,630);
		loadMapWithEnemies.setSize(110,25);
		
		saveMapWithEnemies = new StandardButton("Save Enemies"); 
		saveMapWithEnemies.setLocation(555,630);
		saveMapWithEnemies.setSize(110,25);
		
		enemies = new JPanel();
		enemies.setSize(itemEnemyNpc.getSize());
		
		
				
		npc = new JPanel();
		npc.setSize(itemEnemyNpc.getSize());
		
		items = new JPanel();
		items.setSize(itemEnemyNpc.getSize());
		
		//We need to design the "Map Tiles" to show the map
		//we will also show the collision if the collision is checked
		setUpWindows();
		buttonConfiguration();
		tabsFixUp();
		
		//This designs the tabs with the certain
		//JPanels and everything
		tabs = new JTabbedPane();
		tabs.addTab("Items",items);
		tabs.addTab("Enemies",enemies);
		tabs.addTab("NPC",npc);
		
		tabs.addChangeListener(new ChangeListener(){
			public void stateChanged(ChangeEvent arg0) {
				items.update(items.getGraphics());
				enemies.update(enemies.getGraphics());
				npc.update(npc.getGraphics());
				
		}});
		
		
		
		itemEnemyNpc.add(tabs);
		
		
		//Add everything to the Main Panel
		background.add(remove);
		background.add(highlight);
		background.add(loadMapOnly);
		background.add(loadMapWithEnemies);
		background.add(saveMapWithEnemies);
		background.add(itemEnemyNpc);
		background.add(listHolding);
	
		//Add everything to the main window
		//then update and set it visible that way 
		//it doesn't have to buffer while being shown
		this.add(background);
		background.validate();
		background.repaint();
		this.validate();
		this.validateTree();
		this.update(getGraphics());
		this.repaint();
		this.setVisible(true);
		
	}
	
	public void tabsFixUp(){
	
		
		

		
	}
	public void setUpWindows(){
		
		//Initiate the Variables
		listData = new DefaultListModel();
		data = new ArrayList<String>();
		list = new JList(listData);
		list.setBackground(new Color(240,240,240));
		
		//Setting up the JList
		list.setVisibleRowCount(14);
		
		//Add Scrollable Area
		JScrollPane scrollBar = new JScrollPane(list, JScrollPane.VERTICAL_SCROLLBAR_AS_NEEDED,
													JScrollPane.HORIZONTAL_SCROLLBAR_AS_NEEDED);
		listHolding.add(scrollBar);
		
		
		//Setting up the main window and set everything as the null panels
		Image img = new ImageIcon("null.png").getImage();
		Image spriteSheet = new ImageIcon("GTILES.png").getImage();
		
		//Rebuilding map area
		for(int y = 0; y < 38; y++ ){
			for(int x = 0; x < 48; x++){
				mapTiles[y][x] = new MapTiles(spriteSheet,new Point(560 + (16*x),5 + (16*y)),-1,-1,'n',img);
				mapTiles[y][x].setNull();
				colTiles[y][x] = new CollisionTiles(16,new Point((560 + (16*x)),5 + (16*y)));
				mapTiles[y][x].connect(colTiles[y][x]);
				background.add(colTiles[y][x]);
				background.add(mapTiles[y][x]);
			
			}
		}
		
	}
	public void buttonConfiguration(){
		highlight.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				//highlight enemy
				
		}});

		remove.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				//remove selected item in the list
		}});
		
		
		loadMapOnly.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
			
				int returnValue = fileChooser.showOpenDialog(null);
				if(returnValue == JFileChooser.APPROVE_OPTION){
					try{
						fo.loadFile(fileChooser.getSelectedFile().getAbsolutePath(), mapTiles, colTiles);		
						background.revalidate();
						background.update(getGraphics());
						background.repaint();
					}catch(FileNotFoundException fnfe){
						System.err.println("Error Occured Loading Map");
					}
				
					
		}}});
		
		//loadWithEnemies;
		loadMapWithEnemies.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				
		}});
		
		//saveWithEnemies;
		saveMapWithEnemies.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				
		}});
		
	}
	
	public void repaintAll(){
		background.repaint(mapTiles[0][0].getLocation().x,mapTiles[0][0].getLocation().y,
				   mapTiles[mapTiles.length-1][mapTiles[0].length-1].getLocation().x + 16,
				   mapTiles[mapTiles.length-1][mapTiles[0].length-1].getLocation().y+ 16);
		
		
	}
	
	
}

