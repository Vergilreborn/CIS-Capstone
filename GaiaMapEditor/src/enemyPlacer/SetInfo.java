package enemyPlacer;
import javax.swing.*;

import java.awt.event.*;
import javax.swing.event.*;
import java.awt.*;

public class SetInfo{
	
	
	String [] tester = {"please","work","just","for","Me"};
	JPanel enemyStats;
	JLabel enHP;
	JLabel enStr;
	JLabel enDef;
	JLabel enExp;
	JLabel enItem;
	JLabel enChance;
	JTextField enHPBox;
	JTextField enStrBox;
	JTextField enDefBox;
	JTextField enExpBox;
	JTextField enChanceBox;
	JComboBox enitemsList;
	
	JPanel npcAction;
	JLabel directionFacing;
	JLabel moving;
	JLabel addDialog;
	JLabel itemGive;
	JLabel dialogColor;
	JLabel npcName;
	JComboBox colorList;
	JComboBox direction;
	JComboBox typeMovement;
	JComboBox itemList;
	JTextField textForDialog;
	JTextField npcNameText;
	
	JPanel objects;
	JLabel nextMap;
	JLabel clicked;
	JLabel itemPickUp;
	JLabel chest;
	JLabel passable;
	JTextField mapName;
	JTextField position;
	JComboBox clickedTF;
	JComboBox itemPickUpList;
	JComboBox chestItemList;
	JComboBox passableTF;
	
	String [] itemListL;
	
 
	public SetInfo(Point location,String [] itemList){
		itemListL = itemList;
		
		npcAction = new JPanel();
		npcAction.setSize(278,207);
		npcAction.setBackground(new Color(230,230,230));
		npcAction.setLayout(null);
				
		enemyStats = new JPanel();
		enemyStats.setSize(278,207);
		enemyStats.setBackground(new Color(230,230,230));
		enemyStats.setLayout(null);
		
		objects = new JPanel();
		objects.setSize(278,207);
		objects.setBackground(new Color(230,230,230));
		objects.setLayout(null);
		
		
		enitemsList = new JComboBox(itemListL);
		
		enemyStats.setLocation(location);
		buildEnemyInfo();
		
		
		npcAction.setLocation(location);
		buildNpcInfo();
		
		
		objects.setLocation(location);
		buildItemInfo();
		
		
		
	}

	public void buildEnemyInfo(){
		
		
		enHP = new JLabel("HP:");
		enHP.setLocation(5,5);
		enHP.setSize(80, 15);
		
		enHPBox = new JTextField("",4);
		enHPBox.setLocation(85,5);
		enHPBox.setSize(40,15);
		enHPBox.setText("0");
		
		enStr = new JLabel("Str:");
		enStr.setLocation(5,25);
		enStr.setSize(80,15);
		
		enStrBox= new JTextField("",4);
		enStrBox.setLocation(85,25);
		enStrBox.setSize(40,15);
		enStrBox.setText("0");
		
		enDef = new JLabel("Def:");
		enDef.setLocation(5,45);
		enDef.setSize(80,15);
		
		enDefBox= new JTextField("");
		enDefBox.setLocation(85,45);
		enDefBox.setSize(40,15);
		enDefBox.setText("0");
		
		enExp = new JLabel("Exp Gain:");
		enExp.setLocation(5,65);
		enExp.setSize(80,15);
		
		enExpBox= new JTextField("",6);
		enExpBox.setLocation(85,65);
		enExpBox.setSize(40,15);
		enExpBox.setText("0");
		
		enItem = new JLabel("Item Drop");
		enItem.setLocation(5,85);
		enItem.setSize(80,15);
		
		
		
		enitemsList.setLocation(85,85);
		enitemsList.setSize(120,20);
		enitemsList.setForeground(new Color(0,0,0));
		
		
		enChance = new JLabel("Item Chance");
		enChance.setLocation(5,105);
		enChance.setSize(80,15);
	
		enChanceBox= new JTextField("",4);
		enChanceBox.setLocation(85,105);
		enChanceBox.setSize(40,15);
		enChanceBox.setText("0");
		
	
		
		
		enemyStats.add(enHP);
		enemyStats.add(enStr);
		enemyStats.add(enDef);
		enemyStats.add(enExp);
		enemyStats.add(enItem);
		enemyStats.add(enChance);
		enemyStats.add(enHPBox);
		enemyStats.add(enStrBox);
		enemyStats.add(enDefBox);
		enemyStats.add(enExpBox);
		enemyStats.add(enChanceBox);
		enemyStats.add(enitemsList);
	}
	public void buildNpcInfo(){
		

		
		
		
		
		
		directionFacing = new JLabel("Direction Facing:");
		directionFacing.setLocation(5,5);
		directionFacing.setSize(120,20);
		
		String [] dir = {"Up","Down","Left","Right"};
		direction = new JComboBox(dir);
		direction.setLocation(130,5);
		direction.setSize(120,20);
		
		moving = new JLabel("Type of Moving:");
		moving.setLocation(5,25);
		moving.setSize(120,20);
		
		String [] movType = {"none","random","Constant","Circle","Up-Down","Left-Right"};
		typeMovement = new JComboBox(movType);
		typeMovement.setLocation(130,25);
		typeMovement.setSize(120,20);
		
		addDialog = new JLabel("Add Dialog:");
		addDialog.setLocation(5,45);
		addDialog.setSize(120,20);
		
		textForDialog = new JTextField("Enter Text");
		textForDialog.setLocation(130,45);
		textForDialog.setSize(120,20);
		
		
		dialogColor = new JLabel("Dialog Color");
		dialogColor.setLocation(5,65);
		dialogColor.setSize(120,20);
		
		String [] colors = {"red","blue","orange","teal","yellow","white","green","purple","gray"};
		colorList = new JComboBox(colors);
		colorList.setLocation(130,65);
		colorList.setSize(120,20);
		
		
		itemGive = new JLabel("Item Give:");
		itemGive.setLocation(5,85);	
		itemGive.setSize(120,20);
		
	
		itemList = new JComboBox(itemListL);
		itemList.setLocation(130,85);
		itemList.setSize(120,20);
	
		
		npcName = new JLabel("Name:");
		npcName.setLocation(5,105);
		npcName.setSize(120,20);
		
		npcNameText = new JTextField("Enter Text");
		npcNameText.setLocation(130,105);
		npcNameText.setSize(120,20);
		
		
		npcAction.add(directionFacing);
		npcAction.add(moving);
		npcAction.add(addDialog);
		npcAction.add(itemGive);
		npcAction.add(direction);
		npcAction.add(typeMovement);
		npcAction.add(dialogColor);
		npcAction.add(colorList);
		npcAction.add(textForDialog);
		npcAction.add(itemList);
		npcAction.add(npcName);
		npcAction.add(npcNameText);
	}
	public void buildItemInfo(){
		
		
		nextMap = new JLabel("Map:");
		nextMap.setLocation(5,5);
		nextMap.setSize(100,20);
		
		String [] TF = {"True","False"};
		mapName  = new JTextField("Name");
		mapName.setLocation(105,5);
		mapName.setSize(80,20);
		
		position = new JTextField("0,0");
		position.setLocation(190,5);
		position.setSize(80,20);
				
		clicked = new JLabel("Checkable");
		clicked.setLocation(5,25);
		clicked.setSize(100,20);
		
		clickedTF = new JComboBox(TF);
		clickedTF.setLocation(105,25);
		clickedTF.setSize(120,20);
		
		itemPickUp = new JLabel("Item Check");
		itemPickUp.setLocation(5,45);
		itemPickUp.setSize(100,20);
		
		itemPickUpList = new JComboBox(itemListL);
		itemPickUpList.setLocation(105,45);
		itemPickUpList.setSize(120,20);
		
		chest = new JLabel("Chest");
		chest.setLocation(5,65);
		chest.setSize(100,20);
	
		chestItemList = new JComboBox(itemListL);
		chestItemList.setLocation(105,65);
		chestItemList.setSize(120,20);
	
		passable = new JLabel("Passable");
		passable.setLocation(5,85);
		passable.setSize(100,20);
		
		passableTF = new JComboBox(TF);
		passableTF.setLocation(105,85);
		passableTF.setSize(120,20);
		
		objects.add(nextMap);
		objects.add(clicked);
		objects.add(itemPickUp);
		objects.add(chest);
		objects.add(passable);
		objects.add(mapName);
		objects.add(position);
		objects.add(clickedTF);
		objects.add(itemPickUpList);
		objects.add(chestItemList);
		objects.add(passableTF);
	}
	
	public String returnObjInfo(){
		if(!checkIfNumber(mapName.getText()))
				return "NULL";
		return mapName.getText() + "," + position.getText()+ "," + (String)clickedTF.getSelectedItem() + "," + 
						(String)itemPickUpList.getSelectedItem() + "," + (String)chestItemList.getSelectedItem() + ","
						+  passableTF.getSelectedItem();
						
	
	}
	
	public String returnEnInfo(){
		
		String hp = enHPBox.getText();
		String str = enStrBox.getText();
		String def = enDefBox.getText();
		String exp = enExpBox.getText();
		String itemChance = enChanceBox.getText();
		String item = (String)enitemsList.getSelectedItem();
	
		if(!checkIfNumber(hp) || !checkIfNumber(str) || !checkIfNumber(def) ||
				!checkIfNumber(exp) || !checkIfNumber(itemChance))
			return "NULL";
		return hp + "," + str + "," + def + "," + exp + "," + itemChance + "," + item;
		
	}
	public boolean checkIfNumber(String data){
		int x = 0;
		if(data.charAt(0) == '-')
			x = 1;
		for(int i = x; i < data.length();i++){
			if(data.charAt(i) <'0' || data.charAt(i)>'9')
				return false;
		}
		return true;
	}
	
	public String returnNPCInfo(){
		//textForDialog;
		if(npcNameText.getText().equals("Enter Text"))
			return "NULL";
		return npcNameText.getText()+","+ colorList.getSelectedItem() +"," + direction.getSelectedItem() + "," + itemList.getSelectedItem();

	}
	
	
	
}

