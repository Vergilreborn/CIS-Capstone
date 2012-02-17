package mapMaker3;
import java.io.*;
import java.util.*;

public class FileOptions {

	File currFile;
	
	public FileOptions(){
		
		
	}
	//This will save the information inside the file
	public void saveFile(String filePath, EditTile [][] mapTiles, CollisionTiles[][] colTiles) throws FileNotFoundException{
		currFile = new File(filePath + ".gmaps");
		PrintStream fout = new PrintStream(currFile);
		storeTiles(fout, mapTiles,colTiles);
		
	}
	//this takes the info and stores it in the file as
	//tile x, tile y, location x, location y
	public void storeTiles(PrintStream fout, EditTile [][] mapTiles,CollisionTiles[][] colTiles){
		fout.println("[MapTiles]");
	
		fout.println(mapTiles[0].length + "," + mapTiles.length);
		for(int y = 0; y < mapTiles.length; y++){
			for(int x = 0; x < mapTiles[y].length;x++){
		
				fout.println(mapTiles[y][x] + "," + colTiles[y][x].collisionType);
			}
		}
		fout.close();
	}
	//this gets the tile information of the first 2 lines, that is the declaration ["MapTiles"]
	//followed by the size of the building area if one is made
	public void loadFile(String filePath, EditTile [][] mapTiles, CollisionTiles [][]colTiles) throws FileNotFoundException{
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
	
	//This loads all the tiles since the infor is (tileX,tileY,locationX,locationY)
	//we only need the tile numbers so tileX and tile Y and rebuild that tile
	//the location we can ignore since that is for the use of the game
	public void loadTiles(Scanner sc, EditTile[][] mapTiles, int sizeX, int sizeY,CollisionTiles[][] colTiles){
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
				mapTiles[yLength][xLength].rebuild(tempX,tempY);
			
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
	
	
}
