package enemyPlacer;
import javax.swing.*;
import javax.swing.event.*;
import javax.swing.filechooser.FileNameExtensionFilter;


import java.util.*;
import java.awt.*;
import java.awt.event.*;
import java.io.FileNotFoundException;
import java.io.IOException;

public class EnemyPlacerMain extends JFrame{

	
	private static final long serialVersionUID = 1L;

	public static void main(String [] args){
		
	try{
		new EnemyPlacerMain();
		}catch(Exception e){
		System.err.println("Broken ERROR" + "\n" + e);	
		
		
		}
	}
	
	//Global Variables
	JPanel background;
	JPanel mapHolding;
	JPanel enemies;
	JPanel items;
	JPanel npc;
	JPanel objects;
	FileOptions fo = new FileOptions();
	JFileChooser fileChooser = new JFileChooser();
	StandardButton remove;
	StandardButton highlight;
	StandardButton loadMapOnly;
	StandardButton loadMapWithEnemies;
	StandardButton saveMapWithEnemies;
	StandardButton helpButton;
	SelectingTiles selector;
	JList list;
	Items itemSet;
	Npcs npcSet;
	Enemies enemySet;
	GObjects objectSets;
	ArrayList<String> data;
	DefaultListModel listData;
	MapTiles [][] mapTiles = new MapTiles[38][48];
	CollisionTiles[][] colTiles = new CollisionTiles[38][48];
	Parts [][] tilesParts = new Parts[38][48];
	JPanel itemEnemyNpc;
	JTabbedPane tabs;
	SetInfo dataBox;
	Help help;
	
	public EnemyPlacerMain() throws Exception{
		super("Enemy Placer v 1.0");

		//Setting up the default operations of the this JFrame
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName());
		this.setSize(1340,720);
		this.setResizable(false);
		
		
		help = new Help("Enemies", new ImageIcon("Help2.png").getImage());
		
		fileChooser = new JFileChooser();
		fileChooser.setFileFilter(new FileNameExtensionFilter("Gaia Maps", "gmaps"));
		
		//Setting up the panel
		background = new JPanel();
		background.setSize(this.getSize());
		background.setBackground(new Color(0,0,240));
		background.setLayout(null);
		
		mapHolding = new JPanel(true);
		mapHolding.setLayout(null);
		mapHolding.setBackground(new Color(0,0,0));
		mapHolding.setLocation(560,5);
		mapHolding.setSize(48*16,38*16);
	
	
		itemEnemyNpc = new JPanel(new GridLayout(1,1)); 
		itemEnemyNpc.setBackground(background.getBackground());
		itemEnemyNpc.setLocation(new Point(5,5));
		itemEnemyNpc.setSize(550,385);
				
		//enemies
		enemySet = new Enemies(new ImageIcon("EnemiesUpdate.png").getImage());
		itemSet = new Items(new ImageIcon("Items.png").getImage());	
		npcSet = new Npcs(new ImageIcon("NPC.png").getImage());
		objectSets = new GObjects(new ImageIcon("objs.png").getImage());
		//Selecting
				
		
		selector = new SelectingTiles('n',
										new ImageIcon("Items.png").getImage(),
										new ImageIcon("EnemiesUpdate.png").getImage(),
									    new ImageIcon("NPC.png").getImage(),
										new ImageIcon("objs.png").getImage(),
										enemySet,itemSet,npcSet,objectSets,this);
		
		
		JLabel selectLabel = new JLabel("Selected");
		selectLabel.setLocation(295,530);
		selectLabel.setSize(100,20);
		selectLabel.setFont(new Font("Calibri",1,14));
		selectLabel.setForeground(Color.red);
		
	
		
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
		
		helpButton = new StandardButton("HELP");
		helpButton.setLocation(670,630);
		helpButton.setSize(110,25);
		
		enemies = new JPanel();
		enemies.setSize(itemEnemyNpc.getSize());
		enemies.setLayout(null);
		enemies.setBackground(new Color(180,180,180));
		
		objects = new JPanel();
		objects.setSize(itemEnemyNpc.getSize());
		objects.setLayout(null);
		objects.setBackground(new Color(180,180,180));
		
		dataBox = new SetInfo(new Point(276,405),itemSet.itemList);
		dataBox.npcAction.setVisible(false);
		dataBox.objects.setVisible(false);
		dataBox.enemyStats.setVisible(false);
		
		npc = new JPanel();
		npc.setSize(itemEnemyNpc.getSize());
		npc.setLayout(null);
		npc.setBackground(new Color(180,180,180));
		
		items = new JPanel();
		items.setSize(itemEnemyNpc.getSize());
		items.setLayout(null);
		items.setBackground(new Color(180,180,180));
		
		//We need to design the "Map Tiles" to show the map
		//we will also show the collision if the collision is checked
	
		

	
		
		
		selector.setBackground(new Color(255,255,255));
		selector.setLocation(300,548);
		
		
		
		//This designs the tabs with the certain
		//JPanels and everything
		tabs = new JTabbedPane();
		tabs.addTab("Items",items);
		tabs.addTab("Enemies",enemies);
		tabs.addTab("NPC",npc);
		tabs.addTab("Objects",objects);
		
		tabs.addChangeListener(new ChangeListener(){
			public void stateChanged(ChangeEvent arg0) {
				items.update(items.getGraphics());
				items.repaint();
				enemies.update(enemies.getGraphics());
				enemies.repaint();
				enemies.update(enemies.getGraphics());
				npc.update(npc.getGraphics());
				npc.repaint();
				switch(tabs.getSelectedIndex()){
				case 0:	dataBox.npcAction.setVisible(false);
						dataBox.objects.setVisible(false);
						dataBox.enemyStats.setVisible(false);
						break;
				case 1: dataBox.npcAction.setVisible(false);
						dataBox.objects.setVisible(false);
						dataBox.enemyStats.setVisible(true);
						break;
				case 2: dataBox.objects.setVisible(false);
						dataBox.enemyStats.setVisible(false);
						dataBox.npcAction.setVisible(true);
						break;
				case 3: dataBox.npcAction.setVisible(false);
						dataBox.enemyStats.setVisible(false);
						dataBox.objects.setVisible(true);
						break;
				
			}}});
		setUpWindows();
		buttonConfiguration();
		buildEnemies();
		buildItems();
		buildNpc();
		buildObjects();
		
		itemEnemyNpc.add(tabs);
		
		
		//Add everything to the Main Panel
		background.add(selectLabel);
		background.add(mapHolding);
		background.add(remove);
		background.add(selector);
		background.add(highlight);
		background.add(loadMapOnly);
		background.add(loadMapWithEnemies);
		background.add(saveMapWithEnemies);
		background.add(helpButton);
		background.add(itemEnemyNpc);
		//background.add(list);
		//background.add(listHolding);
		background.add(dataBox.enemyStats);
		background.add(dataBox.npcAction);
		background.add(dataBox.objects);
		
	
		//Add everything to the main window
		//then update and set it visible that way 
		//it doesn't have to buffer while being shown
		enemies.repaint();
		npc.repaint();
		items.repaint();
		objects.repaint();
		this.add(background);
		background.validate();
		background.repaint();
		this.validate();
		this.validateTree();

		this.repaint();
		this.update(getGraphics());
		this.setVisible(true);
		
	}
	
	public void buildItems(){
		
		
		for(int i = 0; i < itemSet.item.length; i++){
			
			itemSet.item[i].setBorder(BorderFactory.createLineBorder(new Color(0,0,0),1));
			items.add(itemSet.item[i]);
			itemSet.item[i].addMouse();
			itemSet.item[i].connect(selector);
		}
		items.validate();
	}
	
	public void buildObjects(){
		
	for(int i = 0; i < objectSets.objs.length; i++){
			
			objectSets.objs[i].setBorder(BorderFactory.createLineBorder(new Color(0,0,0),1));
			objects.add(objectSets.objs[i]);
			objectSets.objs[i].addMouse();
			objectSets.objs[i].connect(selector);
		}
		objects.validate();
		
	
	}
	
	public void buildEnemies(){

		
		
		for(int i = 0; i < enemySet.enemies.length; i++){
			
			enemySet.enemies[i].setBorder(BorderFactory.createLineBorder(new Color(0,0,0),1));
			enemies.add(enemySet.enemies[i]);
			enemySet.enemies[i].addMouse();
			enemySet.enemies[i].connect(selector);
		}
		enemies.validate();
		
	}
	public void buildNpc(){

		
		
		for(int i = 0; i < npcSet.npcPeople.length; i++){
			
			npcSet.npcPeople[i].setBorder(BorderFactory.createLineBorder(new Color(0,0,0),1));
			npc.add(npcSet.npcPeople[i]);
			npcSet.npcPeople[i].addMouse();
			npcSet.npcPeople[i].connect(selector);
		}
		npc.validate();
		
	}
	
	
	public void setUpWindows(){
		
		//Initiate the Variables
		listData = new DefaultListModel();
		data = new ArrayList<String>();
		list = new JList(listData);
		list.setBackground(new Color(240,240,240));
		list.setLocation(4,400);
		list.setSize(270,212);
		//Setting up the JList
		list.setVisibleRowCount(14);
		//Add Scrollable Area
		JScrollPane p = new JScrollPane(list);
		p.setSize(list.getSize());
		p.setLocation(list.getLocation());
		background.add(p);
		
		
		
		//Setting up the main window and set everything as the null panels
		Image img = new ImageIcon("null.png").getImage();
		Image spriteSheet = new ImageIcon("GTILES.png").getImage();
		
		//Rebuilding map area
		for(int y = 0; y < 38; y++ ){
			for(int x = 0; x < 48; x++){
				mapTiles[y][x] = new MapTiles(spriteSheet,new Point( (16*x), (16*y)),-1,-1,'n',img);
				mapTiles[y][x].setNull();
				colTiles[y][x] = new CollisionTiles(16,new Point(((16*x)),(16*y)));
				mapTiles[y][x].connect(colTiles[y][x]);
				tilesParts[y][x] = new Parts(x,y,new ImageIcon("Items.png").getImage(),
											new ImageIcon("EnemiesUpdate.png").getImage(),
											new ImageIcon("NPC.png").getImage(),
											new ImageIcon("objs.png").getImage(),
											mapHolding,data,listData,dataBox);
				tilesParts[y][x].connect(selector,tilesParts);
				tilesParts[y][x].setLocation(16*x,16*y);
				tilesParts[y][x].mouseFunction();
				mapHolding.add(tilesParts[y][x]);
				mapHolding.add(colTiles[y][x]);
				mapHolding.add(mapTiles[y][x]);
				
			
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
						//background.revalidate();
						mapHolding.repaint();
						mapHolding.update(mapHolding.getGraphics());
						
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
				try {
					fo.savingEnemiesObjectsItemsNpc(data);
				} catch (IOException e1) {
					e1.printStackTrace();
				}
				
		}});
		
		helpButton.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				help.setVisible(true);
			}});
		
	}
	
	public void repaintAll(){
		background.repaint(mapTiles[0][0].getLocation().x,mapTiles[0][0].getLocation().y,
				   mapTiles[mapTiles.length-1][mapTiles[0].length-1].getLocation().x + 16,
				   mapTiles[mapTiles.length-1][mapTiles[0].length-1].getLocation().y+ 16);
		
		
	}
	
	
	
}

