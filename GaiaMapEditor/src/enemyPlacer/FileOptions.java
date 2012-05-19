package enemyPlacer;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Scanner;



public class FileOptions {
	File currFile;

	public FileOptions(){
		
	}
	
	
	
	//This loads all the tiles since the infor is (tileX,tileY,locationX,locationY)
	//we only need the tile numbers so tileX and tile Y and rebuild that tile
	//the location we can ignore since that is for the use of the game
	public void loadTiles(Scanner sc, MapTiles[][] mapTiles, int sizeX, int sizeY,CollisionTiles[][] colTiles){
		//size of the arrays given from prev
		int xLength=0;
		int yLength=0;
		
		//begin to decipher and get tile X, tile y
		while(sc.hasNextLine()){
			int position = 0;
			String tileX= "";
			String tileY = "";
			String nl = sc.nextLine();
			
			// this gets tileX
			while(nl.charAt(position) != ','){
				//if(nl.charAt(position) == '('){
				//	position ++;
				//	continue;
				//}
				tileX += nl.charAt(position);
				position++;
			}
			position++;
			
			//this gets tile Y
			while(position < nl.length() && nl.charAt(position) != ','){
				tileY += nl.charAt(position);
				position ++;
			}
			
			//parse the numbers
			int tempX = Integer.parseInt(tileX);
			int tempY = Integer.parseInt(tileY);
			char collisionTile = nl.charAt(nl.length()-1);
			//if they are null set the tile to null
			//otherwise readjust image and tile info of that tile
			if(tempX != -1 && tempY != -1){
			//FIX ME
				mapTiles[yLength][xLength].updateTile(tempX,tempY,'n');
			
			}else
				mapTiles[yLength][xLength].setNull();
			
			
			colTiles[yLength][xLength].updateTile(collisionTile);
			
			    
			
			//increment length
			++xLength;
			//if we hit the end of the X array position
			//increment the y position
			if(xLength % sizeX == 0){
				++yLength;
				xLength = 0;
			}
		}
		
	}

	public void loadFile(String filePath, MapTiles[][] mapTiles, CollisionTiles[][] colTiles) throws FileNotFoundException {
		currFile = new File(filePath);
		Scanner sc= new Scanner(currFile);
		//skip declaration line
		sc.nextLine();
		String size = sc.nextLine();
		
		
		//we will get size of the tiles
		int position = 0;
		String xS = "";
		while(size.charAt(position) != ','){
	
			xS += size.charAt(position);
			position++;
		}
		position++;
		String yS = "";
		
		while(position<size.length()){
			yS += size.charAt(position);
			position ++;
		}
		int x = Integer.parseInt(xS);
		int y = Integer.parseInt(yS);
		
		position = 0;
		
		//Store back editTiles
		loadTiles(sc, mapTiles,x,y,colTiles);
		
	}
	

	public void savingEnemiesObjectsItemsNpc(ArrayList<String> data) throws IOException{
		FileWriter fW = new FileWriter(currFile,true);
		System.out.println(data.size());
		fW.write("[Objects]");
		fW.write("\r\n");
		for(int i = 0; i < data.size(); i++){
			fW.write(data.get(i));
			fW.write("\r\n");
		}
		fW.close();
	}

	
	
}
