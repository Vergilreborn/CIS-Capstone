package enemyPlacer;
import java.awt.*;


//This is the class of enemies that holds all the enemies informations
public class Npcs {
	
	//The array to hold onto the enemy information
	Npc [] npcPeople = new Npc [58];
	
	//The hard coded enemies list
	public Npcs(Image img){
		

		npcPeople[0] = new Npc("People0",img,1,1,16,48);
		npcPeople[1] = new Npc("People1",img,19,1,16,48);
		npcPeople[2] = new Npc("People2",img,37,1,16,48);
		npcPeople[3] = new Npc("People3",img,55,1,16,48);
		npcPeople[4] = new Npc("People4",img,73,1,16,48);
		npcPeople[5] = new Npc("People5",img,91,1,16,48);
		npcPeople[6] = new Npc("People6",img,109,1,16,48);
		npcPeople[7] = new Npc("People7",img,127,1,16,48);
		npcPeople[8] = new Npc("People8",img,145,1,16,48);
		npcPeople[9] = new Npc("People9",img,163,1,16,48);
		npcPeople[10] = new Npc("People10",img,181,1,16,48);
		npcPeople[11] = new Npc("People11",img,199,1,16,48);
		npcPeople[12] = new Npc("People12",img,217,1,16,48);
		npcPeople[13] = new Npc("People13",img,235,1,16,48);
		npcPeople[14] = new Npc("People14",img,253,1,16,48);
		npcPeople[15] = new Npc("People15",img,271,1,16,48);
		npcPeople[16] = new Npc("People16",img,289,1,16,48);
		npcPeople[17] = new Npc("People17",img,307,1,16,48);
		npcPeople[18] = new Npc("People18",img,1,51,16,48);
		npcPeople[19] = new Npc("People19",img,19,51,16,48);
		npcPeople[20] = new Npc("People20",img,37,51,16,48);
		npcPeople[21] = new Npc("People21",img,55,51,16,48);
		npcPeople[22] = new Npc("People22",img,73,51,16,48);
		npcPeople[23] = new Npc("People23",img,91,51,16,48);
		npcPeople[24] = new Npc("People24",img,109,51,16,48);
		npcPeople[25] = new Npc("People25",img,127,51,16,48);
		npcPeople[26] = new Npc("People26",img,145,51,16,48);
		npcPeople[27] = new Npc("People27",img,163,51,16,48);
		npcPeople[28] = new Npc("People28",img,181,51,16,48);
		npcPeople[29] = new Npc("People29",img,199,51,16,48);
		npcPeople[30] = new Npc("People30",img,217,51,16,48);
		npcPeople[31] = new Npc("People31",img,235,51,16,48);
		npcPeople[32] = new Npc("People48",img,253,51,16,48);
		npcPeople[33] = new Npc("People33",img,271,51,16,48);
		npcPeople[34] = new Npc("People34",img,289,51,16,48);
		npcPeople[35] = new Npc("People35",img,307,51,16,48);
		npcPeople[36] = new Npc("People36",img,1,101,16,48);
		npcPeople[37] = new Npc("People37",img,19,101,16,48);
		npcPeople[38] = new Npc("People38",img,37,101,16,48);
		npcPeople[39] = new Npc("People39",img,55,101,16,48);
		npcPeople[40] = new Npc("People40",img,73,101,16,48);
		npcPeople[41] = new Npc("People41",img,91,101,16,48);
		npcPeople[42] = new Npc("People42",img,1,151,32,48);
		npcPeople[43] = new Npc("People43",img,35,151,32,48);
		npcPeople[44] = new Npc("People44",img,69,151,32,48);
		npcPeople[45] = new Npc("People45",img,103,151,32,48);
		npcPeople[46] = new Npc("People46",img,137,151,32,48);
		npcPeople[47] = new Npc("People47",img,171,151,32,48);
		npcPeople[48] = new Npc("People48",img,205,151,32,48);
		npcPeople[49] = new Npc("People49",img,239,151,32,48);
		npcPeople[50] = new Npc("People50",img,273,151,32,48);
		npcPeople[51] = new Npc("People51",img,1,201,32,48);
		npcPeople[52] = new Npc("People52",img,69,201,32,48);
		npcPeople[53] = new Npc("People53",img,103,201,32,48);
		npcPeople[54] = new Npc("People54",img,137,201,32,48);
		npcPeople[55] = new Npc("People55",img,171,201,32,48);
		npcPeople[56] = new Npc("People56",img,205,201,32,48);
		npcPeople[57] = new Npc("People57",img,239,201,32,48);
		

		
		for(int i = 0; i < npcPeople.length;i++){
			npcPeople[i].setItemNumber(i);
			npcPeople[i].connectArray(npcPeople);
		}
	}
}

