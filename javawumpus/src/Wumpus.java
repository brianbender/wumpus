import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.Random;


public class Wumpus {

	char i$ = '\0';
	private Map map = new Map();
	protected int[] mapItemLocations = new int[7];
	private int[] copyOfMapItemlocations = new int[7];
	private int availableArrows = 5;
	protected UserInteraction ui = new UserInteraction();

	public static Random random = new Random();
	/**
	 * @param args
	 */
	public static void main(String[] args) {
		Wumpus game = new Wumpus();
		try {
			game.run();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	public void run() throws IOException {
		int f = 0;
		if (needInstructions()) 										// 25 if (i$ = "N") or (i$ = "n") then 35
				giveInstructions();																	// 30 gosub 375
		placeItemsOnMap();
		do { 
			availableArrows = 5;																		// 230 a = 5
			ui.println("HUNT THE WUMPUS");											// 245 print "HUNT THE WUMPUS"
			do {
				printPlayerStatus();																// 255 gosub 585
				if (1 == getMoveShootChoiceFromPlayer())
					f = shoot();
				else
					f = movePlayerToLocation(getNewPlayerLocation()); 
			} while (f == 0);
			if (f < 0)
				ui.println("HA HA HA - YOU LOSE!");
			else
				ui.println("HEE HEE HEE - THE WUMPUS'LL GET YOU NEXT TIME!!");			// 335 print "HEE HEE HEE - THE WUMPUS'LL GET YOU NEXT TIME!!"

			for (int j = 1; j <= 6; ++j) {
				mapItemLocations[j] = copyOfMapItemlocations[j];
			}
		
			print("SAME SETUP (Y-N)");
			i$ = (char) readChar(); readChar(); 
			if (i$ != 'Y' && i$ != 'y') 
				placeItemsOnMap();
		} while (true);
	}
	public int movePlayerToLocation(int newLocation) {
		mapItemLocations[1] = newLocation;
		
		if (newLocation == getWumpus()) {
			ui.println("... OOPS! BUMPED A WUMPUS!");
			int f = moveWumpus();
			if (f != 0) 
				return f;
		}
		
		if (newLocation == mapItemLocations[3] || newLocation == mapItemLocations[4]) {
			ui.println("YYYYIIIIEEEE . . . FELL IN PIT");
			return -1;
		}
		
		if (newLocation == mapItemLocations[5] || newLocation == mapItemLocations[6]) {
			ui.println("ZAP--SUPER BAT SNATCH! ELSEWHEREVILLE FOR YOU!");
			return movePlayerToLocation(fnA());
		}
		return 0;
	}
	public int getNewPlayerLocation() {
		boolean validMove;
		int newLocation;
		do {
			newLocation = getMoveDirection();
			validMove = isValidPlayerMove(newLocation);
			if (!validMove)
				print("NOT POSSIBLE - ");											// 1030 print "NOT POSSIBLE -";
		} while (!validMove);
		return newLocation;
	}

	private boolean isValidPlayerMove(int newLocation) {
		if (map.isValidMove(playerLocation(), newLocation))
			return true;
		if (newLocation == playerLocation())
			return true;
		return false;
	}
	public int getMoveDirection() {
		int move;
		do {
			print("WHERE TO ");													// 985 print "WHERE TO";
			move = readInt();																// 990 input l
		} while (move < 1 || move > 20);
		return move;
	}
	private int shoot() {
		int j9 = getShotDistanceFromPlayer();
		int[] p = getIntendedFlightPathFromPlayer(j9);
																					// 795 rem *** SHOOT ARROW ***
		int f = shootArrow(j9, p);
		boolean gameEnded = (f != 0);
		if (gameEnded)
			return f;
		ui.println("MISSED");													// 845 print "MISSED"
		f = moveWumpus();																// 860 gosub 935
																					// 865 rem *** AMMO CHECK ***
		availableArrows = availableArrows - 1;																	// 870 a = a-1
		if (availableArrows <= 0)													// 875 if a > 0 then 885
			f = -1;																		// 880 f = -1
		return f;
	}
	public int moveWumpus() {
		int k = fnC();																		// 940 k = fnc(0)
		if (k < 4) {													// 945 if k = 4 then 955
			setWumpus(map.getRoomExits(getWumpus()).room(k));
		}																// 950 l(2) = s(l(2),k)
		if (getWumpus() == playerLocation()) {												// 955 if l(2) <> l then 970
			ui.println("TSK TSK TSK - WUMPUS GOT YOU!");							// 960 print "TSK TSK TSK - WUMPUS GOT YOU!"
			return -1;																		// 965 f = -1
		}
		return 0;
	}
	private int setWumpus(int newLocation) {
		return mapItemLocations[2] = newLocation;
	}
	private int getWumpus() {
		return mapItemLocations[2];
	}
	public int shootArrow(int shotDistance, int[] arrowPath) {
		int ll = playerLocation();																		// 800 l = l(1)
		for (int k2 = 1; k2 <= shotDistance; ++k2) {
			ll = map.isValidMove(ll, arrowPath[k2]) ? arrowPath[k2] : map.getRoomExits(ll).room(fnB());																// 830 l = s(l,fnb(1))
			if (ll == getWumpus()) {												// 900 if l <> l(2) then 920
				ui.println("AHA! YOU GOT THE WUMPUS!");								// 905 print "AHA! YOU GOT THE WUMPUS!"
				return 1;																			// 910 f = 1
			}
																			// 915 return
			if (ll == playerLocation()) {												// 920 if l <> l(1) then 840
				ui.println("OUCH! ARROW GOT YOU!");									// 925 print "OUCH! ARROW GOT YOU!"
				return -1;
			}
		}
		return 0;
	}
	public int[] getIntendedFlightPathFromPlayer(int numberOfRooms) {
		int[] p = new int[6];
		for (int k = 1; k <= numberOfRooms; ++k) {
			do {
				print("ROOM # ");													// 760 print "ROOM #";
				p[k] = readInt();																// 765 input p(k)
				if (did180(p, k))
					ui.println("ARROWS AREN'T THAT CROOKED - TRY ANOTHER ROOM");			// 780 print "ARROWS AREN'T THAT CROOKED - TRY ANOTHER ROOM"
			} while(did180(p, k));
		}
		return p;
	}
	public boolean did180(int[] path, int roomCount) {
		return roomCount > 2 && path[roomCount] == path[roomCount-2];
	}
	public int getShotDistanceFromPlayer() {
		int numberOfRoomsToShoot;
		do {
			print("NO. OF ROOMS (1-5) ");										// 735 print "NO. OF ROOMS (1-5)";
			numberOfRoomsToShoot = readInt();																// 740 input j9
		} while (outOfArrowRange(numberOfRoomsToShoot));
		return numberOfRoomsToShoot;
	}
	private boolean outOfArrowRange(int numberOfRoomsToShoot) {
		return numberOfRoomsToShoot < 1 || numberOfRoomsToShoot > 5;
	}

	public int getMoveShootChoiceFromPlayer() throws IOException {
		while (true) {
			print("SHOOT OR MOVE (S-M) ");
			i$ = (char) readChar();
			readChar(); 
			if (i$ == 'S' || i$ == 's')
				return 1;
			else if (i$ == 'M' || i$ == 'm')
				return 2;
		}
	}

	public void placeItemsOnMap() {
		do {
					randomizeMapItemLocations();
			}  while (crossover());
	}
	public void printPlayerStatus() {
		ui.println("");														// 590 print
		printNearbyItemHints();
		printPlayerLocation();
		printTunnelOptions();
		ui.println("");														// 660 print
	}
	public void printTunnelOptions() {
		Paths room = map.getRoomExits(playerLocation());
		print("TUNNELS LEAD TO "); ui.print(room.room(1));
					print(" "); ui.print(room.room(2)); 
					print(" "); ui.println(room.room(3));
	}
	public void printPlayerLocation() {
		print("YOUR ARE IN ROOM "); ui.println(playerLocation());				// 650 print "YOU ARE IN ROOM ";l(1)
	}

	public void printNearbyItemHints() {
		for (int j = 2; j <= 6; ++j) {
			Paths roomExits = map.getRoomExits(playerLocation());
				if (roomExits.canGetToRoom(mapItemLocations[j]))
					printItemNearbyPlayerHint(j - 1);

		}
	}

	public void printItemNearbyPlayerHint(int itemType) {
		switch(itemType) {																			// 610 on j-1 goto 615,625,x,635,635
				case 1: ui.println("I SMELL A WUMPUS!"); break;
				case 2:
				case 3: ui.println("I FEEL A DRAFT"); break;
				case 4:
				case 5: ui.println("BATS NEARBY!"); break;
				};
	}
	public boolean crossover() {
		for (int j = 1; j <= 6; ++j) {
			for (int k = 1; k <= 6; ++k) {
				if (j == k) continue;
				if (mapItemLocations[j] == mapItemLocations[k])
					return true;
			}
		}
		return false;
	}
	
	private void randomizeMapItemLocations() {
		for (int j = 1; j <= 6; ++j) {
			mapItemLocations[j] = fnA();																	// 175 l(j) = fna(0)
			copyOfMapItemlocations[j] = mapItemLocations[j];																	// 180 m(j) = l(j)
		}
	}
	private int playerLocation() {
		return mapItemLocations[1];
	}
	private boolean needInstructions() throws IOException {
		print("INSTRUCTIONS (Y-N) "); 
		char answer = (char) readChar();
		readChar();
		return answer != 'N' && answer != 'n';
	}
	
	private void giveInstructions() throws IOException {
		ui.println("WELCOME TO 'HUNT THE WUMPUS'");					 		// 380 print "WELCOME TO 'HUNT THE WUMPUS'"
		ui.println("  THE WUMPUS LIVES IN A CAVE OF 20 ROOMS. EACH ROOM");							// "  THE WUMPUS LIVES IN A CAVE OF 20 ROOMS. EACH ROOM"
		ui.println("HAS 3 TUNNELS LEADING TO OTHER ROOMS. (LOOK AT A");		// 390 print "HAS 3 TUNNELS LEADING TO OTHER ROOMS. (LOOK AT A"
		ui.println("DODECAHEDRON TO SEE HOW THIS WORKS-IF YOU DON'T KNOW");							// "DODECAHEDRON TO SEE HOW THIS WORKS-IF YOU DON'T KNOW"
		ui.println("WHAT A DODECAHEDRON IS, ASK SOMEONE)");					// 400 print "WHAT A DODECAHEDRON IS, ASK SOMEONE)"
		ui.println("");														// 405 print
		ui.println("     HAZARDS:");											// 410 print "     HAZARDS:"
		ui.println(" BOTTOMLESS PITS - TWO ROOMS HAVE BOTTOMLESS PITS IN THEM");					// " BOTTOMLESS PITS - TWO ROOMS HAVE BOTTOMLESS PITS IN THEM"
		ui.println("     IF YOU GO THERE, YOU FALL INTO THE PIT (& LOSE!)");						// "     IF YOU GO THERE, YOU FALL INTO THE PIT (& LOSE!)"
		ui.println(" SUPER BATS - TWO OTHER ROOMS HAVE SUPER BATS. IF YOU");						// " SUPER BATS - TWO OTHER ROOMS HAVE SUPER BATS. IF YOU"
		ui.println("     GO THERE, A BAT GRABS YOU AND TAKES YOU TO SOME OTHER");					// "     GO THERE, A BAT GRABS YOU AND TAKES YOU TO SOME OTHER"
		ui.println("     ROOM AT RANDOM. (WHICH MAY BE TROUBLESOME)");			// 435 print "     ROOM AT RANDOM. (WHICH MAY BE TROUBLESOME)"
		ui.println("HIT RETURN TO CONTINUE"); readChar();				// 440 input "HIT RETURN TO CONTINUE";a$
		ui.println("     WUMPUS:");											// 445 print "     WUMPUS:"
		ui.println(" THE WUMPUS IS NOT BOTHERED BY HAZARDS (HE HAS SUCKER");						// " THE WUMPUS IS NOT BOTHERED BY HAZARDS (HE HAS SUCKER"
		ui.println(" FEET AND IS TOO BIG FOR A BAT TO LIFT).  USUALLY"); 		// 455 print " FEET AND IS TOO BIG FOR A BAT TO LIFT).  USUALLY"
		ui.println(" HE IS ASLEEP.  TWO THINGS WAKE HIM UP: YOU SHOOTING AN");						// " HE IS ASLEEP.  TWO THINGS WAKE HIM UP: YOU SHOOTING AN"
		ui.println("ARROW OR YOU ENTERING HIS ROOM.");							// 465 print "ARROW OR YOU ENTERING HIS ROOM."
		ui.println("     IF THE WUMPUS WAKES HE MOVES (P=.75) ONE ROOM");							// "     IF THE WUMPUS WAKES HE MOVES (P=.75) ONE ROOM"
		ui.println(" OR STAYS STILL (P=.25).  AFTER THAT, IF HE IS WHERE YOU");						// " OR STAYS STILL (P=.25).  AFTER THAT, IF HE IS WHERE YOU"
		ui.println(" ARE, HE EATS YOU UP AND YOU LOSE!");						// 480 print " ARE, HE EATS YOU UP AND YOU LOSE!"
		ui.println("");														// 485 print
		ui.println("     YOU:");												// 490 print "     YOU:"
		ui.println(" EACH TURN YOU MAY MOVE OR SHOOT A CROOKED ARROW"); 		// 495 print " EACH TURN YOU MAY MOVE OR SHOOT A CROOKED ARROW"
		ui.println("   MOVING:  YOU CAN MOVE ONE ROOM (THRU ONE TUNNEL)");							// "   MOVING:  YOU CAN MOVE ONE ROOM (THRU ONE TUNNEL)"
		ui.println("   ARROWS:  YOU HAVE 5 ARROWS.  YOU LOSE WHEN YOU RUN OUT");					// "   ARROWS:  YOU HAVE 5 ARROWS.  YOU LOSE WHEN YOU RUN OUT"
		ui.println("   EACH ARROW CAN GO FROM 1 TO 5 ROOMS. YOU AIM BY TELLING");					// "   EACH ARROW CAN GO FROM 1 TO 5 ROOMS. YOU AIM BY TELLING"
		ui.println("   THE COMPUTER THE ROOM#S YOU WANT THE ARROW TO GO TO.");						// "   THE COMPUTER THE ROOM#S YOU WANT THE ARROW TO GO TO."
		ui.println("   IF THE ARROW CAN'T GO THAT WAY (IF NO TUNNEL) IT MOVES"); 					// "   IF THE ARROW CAN'T GO THAT WAY (IF NO TUNNEL) IT MOVES"
		ui.println("   AT RANDOM TO THE NEXT ROOM.");							// 525 print "   AT RANDOM TO THE NEXT ROOM."
		ui.println("     IF THE ARROW HITS THE WUMPUS, YOU WIN.");				// 530 print "     IF THE ARROW HITS THE WUMPUS, YOU WIN."
		ui.println("     IF THE ARROW HITS YOU, YOU LOSE.");					// 535 print "     IF THE ARROW HITS YOU, YOU LOSE."
		ui.println("HIT RETURN TO CONTINUE"); readChar();				// 540 input "HIT RETURN TO CONTINUE";a$
		ui.println("    WARNINGS:");											// 545 print "    WARNINGS:"
		ui.println("     WHEN YOU ARE ONE ROOM AWAY FROM A WUMPUS OR HAZARD,");						// "     WHEN YOU ARE ONE ROOM AWAY FROM A WUMPUS OR HAZARD,"
		ui.println("     THE COMPUTER SAYS:");									// 555 print "     THE COMPUTER SAYS:"
		ui.println(" WUMPUS:  'I SMELL A WUMPUS'");							// 560 print " WUMPUS:  'I SMELL A WUMPUS'"
		ui.println(" BAT   :  'BATS NEARBY'");									// 565 print " BAT   :  'BATS NEARBY'"
		ui.println(" PIT   :  'I FEEL A DRAFT'");								// 570 print " PIT   :  'I FEEL A DRAFT'"
		ui.println("");														// 575 print
	}
	public void print(String data) {
		System.out.print(data);
	}
	public int readChar() throws IOException {
		return System.in.read();
	}
	public static int fnA() {
		return random.nextInt(20) + 1;
	}
	public static int fnB() {
		return random.nextInt(3) + 1;
	}
	public static int fnC() {
		return random.nextInt(4) + 1;
	}

	public int readInt() {
		String line = "";
		BufferedReader is = new BufferedReader(new InputStreamReader(System.in));
		try {
			line = is.readLine();
		} catch (IOException e) {
			return 0;
		}
		return Integer.parseInt(line);
	}
}
