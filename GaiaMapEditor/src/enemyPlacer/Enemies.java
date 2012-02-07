package enemyPlacer;
import java.awt.*;


//This is the class of enemies that holds all the enemies informations
public class Enemies {
	
	//The array to hold onto the enemy information
	Enemy [] enemies = new Enemy [58];
	
	//The hard coded enemies list
	public Enemies(Image img){
		enemies[0] = new Enemy("Cloak",img,1,1,16,48);
		enemies[1] = new Enemy("Skeleton Green",img,19,1,32,48);
		enemies[2] = new Enemy("Skeleton Red",img,53,1,32,48);
		enemies[3] = new Enemy("Skeleton Blue",img,87,1,32,48);
		enemies[4] = new Enemy("Bird Head",img,121,1,16,16);
		enemies[5] = new Enemy("Blue Ball",img,121,19,16,16);
		enemies[6] = new Enemy("Worm",img,139,19,16,16);
		enemies[7] = new Enemy("Silver Goop",img,157,19,16,16);
		enemies[8] = new Enemy("Fly",img,157,1,16,16);
		enemies[9] = new Enemy("SpikeBall",img,139,1,16,16);
		enemies[10] = new Enemy("Skull Stand",img,175,1,32,32);
		enemies[11] = new Enemy("Face Wheel",img,209,1,32,32);
		enemies[12] = new Enemy("Flamer",img,243,1,32,32);
		enemies[13] = new Enemy("Snake",img,277,1,32,32);
		enemies[14] = new Enemy("Spider",img,311,1,32,32);
		enemies[15] = new Enemy("Bush",img,243,35,32,32);
		enemies[16] = new Enemy("Dread",img,277,35,32,32);
		enemies[17] = new Enemy("Pyramid Head",img,311,35,32,32);
		enemies[18] = new Enemy("Bat Blue",img,1,51,32,32);
		enemies[19] = new Enemy("Bat Grey",img,35,51,32,32);
		enemies[20] = new Enemy("Bat Brown",img,69,51,32,32);
		enemies[21] = new Enemy("Bat Black",img,103,51,32,32);
		enemies[22] = new Enemy("Blue Shadow",img,173,51,16,32);
		enemies[23] = new Enemy("Ghost",img,191,51,16,32);
		enemies[24] = new Enemy("Snake Bottle",img,209,51,16,32);
		enemies[25] = new Enemy("Skull Head Gold",img,1,85,32,32);
		enemies[26] = new Enemy("Skull Head Grey",img,35,85,32,32);
		enemies[27] = new Enemy("Earring Skull Brown",img,69,85,32,32);
		enemies[28] = new Enemy("Earring Skull Purple",img,103,85,32,32);
		enemies[29] = new Enemy("Rolly Polly Blue",img,1,119,32,32);
		enemies[30] = new Enemy("Rolly Polly Green",img,35,119,32,32);
		enemies[31] = new Enemy("Alien Eye Purple",img,69,119,32,32);
		enemies[32] = new Enemy("Alien Eye Blue",img,103,119,32,32);
		enemies[33] = new Enemy("Blue Slime Ball",img,137,85,32,16);
		enemies[34] = new Enemy("Green Flat Slime",img,175,85,32,16);
		enemies[35] = new Enemy("Stone Golem",img,137,103,48,48);
		enemies[36] = new Enemy("Rock Golem",img,187,103,48,48);
		enemies[37] = new Enemy("Sand Golem",img,237,103,32,48);
		enemies[38] = new Enemy("Goblin Flute",img,275,103,32,48);
		enemies[39] = new Enemy("Samurai Statue",img,313,103,32,48);
		enemies[40] = new Enemy("Bug Dropper",img,351,103,32,48);
		enemies[41] = new Enemy("Alien Walker",img,19,153,32,64);
		enemies[42] = new Enemy("Ghoul",img, 53,153,32,64);
		enemies[43] = new Enemy("Lizard Red",img,159,153,32,64);
		enemies[44] = new Enemy("Lizard Green",img,193,153,32,64);
		enemies[45] = new Enemy("TechnoRad Red",img,227,153,32,64);
		enemies[46] = new Enemy("TechnoRad Blue",img,261,153,32,64);
		enemies[47] = new Enemy("Amazon Green",img,295,153,32,64);
		enemies[48] = new Enemy("Amazon Normal",img,329,153,32,64);
		enemies[49] = new Enemy("Mummy",img,363,153,32,64);
		enemies[50] = new Enemy("BirdFighter",img,397,153,32,64);
		enemies[51] = new Enemy("Green Worm Eye",img,1,153,16,64);
		enemies[52] = new Enemy("Worm Tail Green",img,87,153,16,64);
		enemies[53] = new Enemy("Worm Tail Purple",img,105,153,16,64);
		enemies[54] = new Enemy("Sword Red",img,123,153,16,64);
		enemies[55] = new Enemy("Sword Blue",img,141,153,16,64);
		enemies[56] = new Enemy("Ball Walker Silver",img,345,1,64,48);
		enemies[57] = new Enemy("Ball Walker Chrome",img,345,51,64,48);	
		for(int i = 0; i < enemies.length;i++){
			enemies[i].setItemNumber(i);
			enemies[i].connectArray(enemies);
		}
	}
}

