package enemyPlacer;

import java.awt.Image;

public class Items {

	Item [] item = new Item [51];
	String [] itemList = new String[51];
	
	//The hard coded enemies list
	public Items(Image img){
	
		
		item[0] = new Item("letter",img,1,1,16,16);
		item[1] = new Item("chicken",img,19,1,16,16);
		item[2] = new Item("song red",img,37,1,16,16);
		item[3] = new Item("song green",img,55,1,16,16);
		item[4] = new Item("song purple",img,73,1,16,16);
		item[5] = new Item("bent key",img,91,1,16,16);
		item[6] = new Item("mushroom",img,109,1,16,16);
		item[7] = new Item("powder",img,127,1,16,16);
		item[8] = new Item("Crystal Blue",img,145,1,16,16);
		item[9] = new Item("Shield++",img,163,1,16,16);
		item[10] = new Item("Crystal Black",img,181,1,16,16);
		item[11] = new Item("Mana++",img,199,1,16,16);
		item[12] = new Item("metorite",img,1,19,16,16);
		item[13] = new Item("diamond block",img,19,19,16,16);
		item[14] = new Item("ball",img,37,19,16,16);
		item[15] = new Item("glass ball key",img,55,19,16,16);
		item[16] = new Item("silver key",img,73,19,16,16);
		item[17] = new Item("Water Statue",img,91,19,16,16);
		item[18] = new Item("Queens Key",img,109,19,16,16);
		item[19] = new Item("Sword Key",img,127,19,16,16);
		item[20] = new Item("Fish",img,145,19,16,16);
		item[21] = new Item("Health++",img,163,19,16,16);
		item[22] = new Item("Crystal Purple",img,181,19,16,16);
		item[23] = new Item("Herb",img,1,37,16,16);
		item[24] = new Item("Sunglasses",img,19,37,16,16);
		item[25] = new Item("Flame spirit",img,37,37,16,16);
		item[26] = new Item("Queens Ring",img,55,37,16,16);
		item[27] = new Item("Red Jewel",img,73,37,16,16);
		item[28] = new Item("Holy Book",img,91,37,16,16);
		item[29] = new Item("Queens Necklace",img,109,37,16,16);
		item[30] = new Item("Medal",img,127,37,16,16);
		item[31] = new Item("Crystal Green",img,145,37,16,16);
		item[32] = new Item("Strength++",img,163,37,16,16);
		item[33] = new Item("Crystal Yellow",img,181,37,16,16);
		item[34] = new Item("Flower",img,1,55,16,16);
		item[35] = new Item("Cyclo Statue",img,19,55,16,16);
		item[36] = new Item("Wind of the East",img,37,55,16,16);
		item[37] = new Item("Wind of the West",img,55,55,16,16);
		item[38] = new Item("Mana",img,73,55,16,16);
		item[39] = new Item("Heart",img,91,55,16,16);
		item[40] = new Item("Coin 20",img,109,55,16,16);
		item[41] = new Item("Coin 5",img,127,55,16,16);
		item[42] = new Item("Coin 1",img,145,55,16,16);
		item[43] = new Item("Crystal Red",img,163,55,16,16);
		item[44] = new Item("Crystal Light",img,181,55,16,16);
		item[45] = new Item("Statue1",img,1,73,16,32);
		item[46] = new Item("Statue2",img,19,73,16,32);
		item[47] = new Item("Statue3",img,37,73,16,32);
		item[48] = new Item("Statue4",img,55,73,16,32);
		item[49] = new Item("Statue5",img,73,73,16,32);
		item[50] = new Item("Statue6",img,91,73,16,32);
	
		
		
		
		
		
		for(int i = 0; i < item.length;i++){
			item[i].setItemNumber(i);
			item[i].connectArray(item);
			itemList[i] = item[i].type;
		}
	}
}
