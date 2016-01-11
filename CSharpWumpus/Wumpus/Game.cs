using System;
using System.Collections.Generic;

namespace Wumpus
{
    public class Game
    {
        private int _currentLine;
        private readonly Stack<int> ReturnLine = new Stack<int>();
        private int _nextLine;
        private IO _io;
        private readonly BoardPieces _boardPieces;

        int[,] exits =
                {
                    {0, 0, 0, 0},
                    {0, 2, 5, 8}, {0, 1, 3, 10}, {0, 2, 4, 12}, {0, 3, 5, 14}, {0, 1, 4, 6},
                    {0, 5, 7, 15}, {0, 6, 8, 17}, {0, 1, 7, 9}, {0, 8, 10, 18}, {0, 2, 9, 11},
                    {0, 10, 12, 19}, {0, 3, 11, 13}, {0, 12, 14, 20}, {0, 4, 13, 15}, {0, 6, 14, 16},
                    {0, 15, 17, 20}, {0, 7, 16, 18}, {0, 9, 17, 19}, {0, 11, 18, 20}, {0, 13, 16, 19}
                };

        public Game(IO io)
        {
            this._io = io;
            EarlyExit = 1150;
            Dice = new Dice();
            _boardPieces = new BoardPieces();

        }



        public int EarlyExit { get; set; }

        public Dice Dice { get; set; }

//TODO remove after refactoring so that this isn't needed by tests

        public void Play()
        {
            try
            {
                _currentLine = 5;
                char istr = '\0';
              
              
                int[] p = new int[6];
                int aa = 5;
                int ll = aa;
                int o = 1;
                int f = 0;

                int k = 0;
                int k1 = 0;
                int j9 = 0;
                while (_currentLine <= 1150 && EarlyExit != _currentLine)
                {
                    _nextLine = _currentLine + 1;
                    switch (_currentLine)
                    {
                        case 15:
                            istr = GiveIntroduction(istr);
                            break; // 25 if (i$ = "N") or (i$ = "n") then 35
                        case 170:
                            _boardPieces.GenerateBoardPieces(Dice);
                            break;
                        case 230:
                            aa = 5;
                            break; // 230 a = 5
                        case 235:
                            ll = _boardPieces._pieces[1];
                            break; // 235 l = l(1)
                        case 245:
                            _io.WriteLine("HUNT THE WUMPUS");
                            break; // 245 print "HUNT THE WUMPUS"
                        case 255:
                            gosub(585, 260);
                            break; // 255 gosub 585
                        case 265:
                            gosub(670, 270);
                            break; // 265 gosub 670
                        case 270:
                            switch (o)
                            {
                                case 1:
                                    _nextLine = 280;
                                    break;
                                case 2:
                                    _nextLine = 300;
                                    break;
                            }
                            break; // 270 on o goto 280,300
                        case 280:
                            gosub(715, 285);
                            break; // 280 gosub 715
                        case 285:
                            if (f == 0) _nextLine = 255;
                            break; // 285 if f = 0 then 255
                        case 290:
                            _nextLine = 310;
                            break; // 290 goto 310
                        case 300:
                            gosub(975, 305);
                            break; // 300 gosub 975
                        case 305:
                            if (f == 0) _nextLine = 255;
                            break; // 305 if f = 0 then 255
                        case 310:
                            if (f > 0) _nextLine = 335;
                            break; // 310 if f > 0 then 335
                        case 315:
                            break; // 315 rem *** LOSE ***
                        case 320:
                            _io.WriteLine("HA HA HA - YOU LOSE!");
                            break; // 320 print "HA HA HA - YOU LOSE!"
                        case 325:
                            _nextLine = 340;
                            break; // 325 goto 340
                        case 335:
                            _io.WriteLine("HEE HEE HEE - THE WUMPUS'LL GET YOU NEXT TIME!!");
                            break; // 335 print "HEE HEE HEE - THE WUMPUS'LL GET YOU NEXT TIME!!"
                        case 340:
                            _boardPieces.ResetToLastPieces();
                            break; // 350 next j
                        case 355:
                            _io.Prompt("SAME SETUP (Y-N)");
                            break; // 355 print "SAME SETUP (Y-N)";
                        case 360:
                            istr = _io.ReadChar();
                            break; // 360 input i$
                        case 365:
                            if (istr != 'Y' && istr != 'y') _nextLine = 170;
                            break; // 365 if (i$ <> "Y") and (i$ <> "y") then 170
                        case 370:
                            _nextLine = 230;
                            break; // 370 goto 230
                        case 590:
                            _io.WriteLine("");
                            break; // 590 print
                      case 605:
                            var neighboringRooms = getNeighboringRooms();

                            for(int j = 2; j<=6; ++j)
                                if (neighboringRooms.Contains(_boardPieces._pieces[j]))
                                    PrintNearHazard(j);
                                
                            break; // 605 if s(l(1),k) <> l(j) then 640
                        case 650:
                            _io.Prompt("YOUR ARE IN ROOM ");
                            _io.WriteLine(_boardPieces._pieces[1].ToString());
                            break; // 650 print "YOU ARE IN ROOM ";l(1)
                        case 655:
                            _io.Prompt("TUNNELS LEAD TO ");
                            _io.Prompt(exits[ll, 1].ToString()); // 655 print "TUNNELS LEAD TO ";s(l,1);" ";s(l,2);" ";s(l,3)
                            _io.Prompt(" ");
                            _io.Prompt(exits[ll, 2].ToString());
                            _io.Prompt(" ");
                            _io.WriteLine(exits[ll, 3].ToString());
                            break;
                        case 660:
                            _io.WriteLine("");
                            break; // 660 print
                        case 665:
                            returnFromGosub();
                            break; // 665 return
                        case 675:
                            _io.Prompt("SHOOT OR MOVE (S-M) ");
                            break; // 675 print "SHOOT OR MOVE (S-M)";
                        case 680:
                            istr = _io.ReadChar();
                            break; // 680 input i$
                        case 685:
                            if (istr != 'S' && istr != 's') _nextLine = 700;
                            break; // 685 if (i$ <> "S") and (i$ <> "s") then 700
                        case 690:
                            o = 1;
                            break; // 690 o = 1
                        case 695:
                            returnFromGosub();
                            break; // 695 return
                        case 700:
                            if (istr != 'M' && istr != 'm') _nextLine = 675;
                            break; // 700 if (i$ <> "M") and (i$ <> "m") then 675
                        case 705:
                            o = 2;
                            break; // 705 o = 2
                        case 710:
                            returnFromGosub();
                            break; // 710 return
                        case 720:
                            f = 0;
                            break; // 720 f = 0
                        case 735:
                            _io.Prompt("NO. OF ROOMS (1-5) ");
                            break; // 735 print "NO. OF ROOMS (1-5)";
                        case 740:
                            j9 = _io.readInt();
                            break; // 740 input j9
                        case 745:
                            if (j9 < 1) _nextLine = 735;
                            break; // 745 if j9 < 1 then 735
                        case 750:
                            if (j9 > 5) _nextLine = 735;
                            break; // 750 if j9 > 5 then 735
                        case 755:
                            k = 1;
                            break; // 755 for k = 1 to j9
                        case 760:
                            _io.Prompt("ROOM # ");
                            break; // 760 print "ROOM #";
                        case 765:
                            p[k] = _io.readInt();
                            break; // 765 input p(k)
                        case 770:
                            if (k <= 2) _nextLine = 790;
                            break; // 770 if k <= 2 then 790
                        case 775:
                            if (p[k] != p[k - 2]) _nextLine = 790;
                            break; // 775 if p(k) <> p(k-2) then 790
                        case 780:
                            _io.WriteLine("ARROWS AREN'T THAT CROOKED - TRY ANOTHER ROOM");
                            break; // 780 print "ARROWS AREN'T THAT CROOKED - TRY ANOTHER ROOM"
                        case 785:
                            _nextLine = 760;
                            break; // 785 goto 760
                        case 790:
                            ++k;
                            if (k <= j9) _nextLine = 760;
                            break; // 790 next k
                        case 800:
                            ll = _boardPieces._pieces[1];
                            break; // 800 l = l(1)
                        case 805:
                            k = 1;
                            break; // 805 for k = 1 to j9
                        case 810:
                            k1 = 1;
                            break; // 810 for k1 = 1 to 3
                        case 815:
                            if (exits[ll, k1] == p[k]) _nextLine = 895;
                            break; // 815 if s(l,k1) = p(k) then 895
                        case 820:
                            ++k1;
                            if (k1 <= 3) _nextLine = 815;
                            break; // 820 next k1
                        case 830:
                            ll = exits[ll, Dice.RollD3()];
                            break; // 830 l = s(l,fnb(1))
                        case 835:
                            _nextLine = 900;
                            break; // 835 goto 900
                        case 840:
                            ++k;
                            if (k <= j9) _nextLine = 810;
                            break; // 840 next k
                        case 845:
                            _io.WriteLine("MISSED");
                            break; // 845 print "MISSED"
                        case 850:
                            ll = _boardPieces._pieces[1];
                            break; // 850 l = l(1)
                        case 860:
                            gosub(935, 865);
                            break; // 860 gosub 935
                        case 870:
                            aa = aa - 1;
                            break; // 870 a = a-1
                        case 875:
                            if (aa > 0) _nextLine = 885;
                            break; // 875 if a > 0 then 885
                        case 880:
                            f = -1;
                            break; // 880 f = -1
                        case 885:
                            returnFromGosub();
                            break; // 885 return
                        case 895:
                            ll = p[k];
                            break; // 895 l = p(k)
                        case 900:
                            if (ll != _boardPieces._pieces[2]) _nextLine = 920;
                            break; // 900 if l <> l(2) then 920
                        case 905:
                            _io.WriteLine("AHA! YOU GOT THE WUMPUS!");
                            break; // 905 print "AHA! YOU GOT THE WUMPUS!"
                        case 910:
                            f = 1;
                            break; // 910 f = 1
                        case 915:
                            returnFromGosub();
                            break; // 915 return
                        case 920:
                            if (ll != _boardPieces._pieces[1]) _nextLine = 840;
                            break; // 920 if l <> l(1) then 840
                        case 925:
                            _io.WriteLine("OUCH! ARROW GOT YOU!");
                            break; // 925 print "OUCH! ARROW GOT YOU!"
                        case 930:
                            _nextLine = 880;
                            break; // 930 goto 880
                        case 940:
                            k = Dice.RollD4();
                            break; // 940 k = fnc(0)
                        case 945:
                            if (k == 4) _nextLine = 955;
                            break; // 945 if k = 4 then 955
                        case 950:
                            _boardPieces._pieces[2] = exits[_boardPieces._pieces[2], k];
                            break; // 950 l(2) = s(l(2),k)
                        case 955:
                            if (_boardPieces._pieces[2] != ll) _nextLine = 970;
                            break; // 955 if l(2) <> l then 970
                        case 960:
                            _io.WriteLine("TSK TSK TSK - WUMPUS GOT YOU!");
                            break; // 960 print "TSK TSK TSK - WUMPUS GOT YOU!"
                        case 965:
                            f = -1;
                            break; // 965 f = -1
                        case 970:
                            returnFromGosub();
                            break; // 970 return
                        case 980:
                            f = 0;
                            break; // 980 f = 0
                        case 985:
                            _io.Prompt("WHERE TO ");
                            break; // 985 print "WHERE TO";
                        case 990:
                            ll = _io.readInt();
                            break; // 990 input l
                        case 995:
                            if (ll < 1) _nextLine = 985;
                            break; // 995 if l < 1 then 985
                        case 1000:
                            if (ll > 20) _nextLine = 985;
                            break; // 1000 if l > 20 then 985
                        case 1005:
                            k = 1;
                            break; // 1005 for k = 1 to 3
                        case 1015:
                            if (exits[_boardPieces._pieces[1], k] == ll) _nextLine = 1045;
                            break; // 1015 if s(l(1),k) = l then 1045
                        case 1020:
                            ++k;
                            if (k <= 3) _nextLine = 1010;
                            break; // 1020 next k
                        case 1025:
                            if (ll == _boardPieces._pieces[1]) _nextLine = 1045;
                            break; // 1025 if l = l(1) then 1045
                        case 1030:
                            _io.Prompt("NOT POSSIBLE - ");
                            break; // 1030 print "NOT POSSIBLE -";
                        case 1035:
                            _nextLine = 985;
                            break; // 1035 goto 985
                        case 1045:
                            _boardPieces._pieces[1] = ll;
                            break; // 1045 l(1) = l
                        case 1055:
                            if (ll != _boardPieces._pieces[2]) _nextLine = 1090;
                            break; // 1055 if l <> l(2) then 1090
                        case 1060:
                            _io.WriteLine("... OOPS! BUMPED A WUMPUS!");
                            break; // 1060 print "... OOPS! BUMPED A WUMPUS!"
                        case 1070:
                            gosub(940, 1075);
                            break; // 1070 gosub 940
                        case 1075:
                            if (f == 0) _nextLine = 1090;
                            break; // 1075 if f = 0 then 1090
                        case 1080:
                            returnFromGosub();
                            break; // 1080 return
                        case 1090:
                            if (ll == _boardPieces._pieces[3]) _nextLine = 1100;
                            break; // 1090 if l = l(3) then 1100
                        case 1095:
                            if (ll != _boardPieces._pieces[4]) _nextLine = 1120;
                            break; // 1095 if l <> l(4) then 1120
                        case 1100:
                            _io.WriteLine("YYYYIIIIEEEE . . . FELL IN PIT");
                            break; // 1100 print "YYYYIIIIEEEE . . . FELL IN PIT"
                        case 1105:
                            f = -1;
                            break; // 1105 f = -1
                        case 1110:
                            returnFromGosub();
                            break; // 1110 return
                        case 1120:
                            if (ll == _boardPieces._pieces[5]) _nextLine = 1130;
                            break; // 1120 if l = l(5) then 1130
                        case 1125:
                            if (ll != _boardPieces._pieces[6]) _nextLine = 1145;
                            break; // 1125 if l <> l(6) then 1145
                        case 1130:
                            _io.WriteLine("ZAP--SUPER BAT SNATCH! ELSEWHEREVILLE FOR YOU!");
                            break; // 1130 print "ZAP--SUPER BAT SNATCH! ELSEWHEREVILLE FOR YOU!"
                        case 1135:
                            ll = Dice.RollD20();
                            break; // 1135 l = fna(1)
                        case 1140:
                            _nextLine = 1045;
                            break; // 1140 goto 1045
                        case 1145:
                            returnFromGosub();
                            break; // 1145 return
                    }
                    _currentLine = _nextLine;
                }
            }
            catch (Exception e)
            {
                // TODO Auto-generated catch block
                _io.WriteLine(e.StackTrace);
            }
        }

        private List<int> getNeighboringRooms()
        {
            List<int> neighboringRooms = new List<int>();
            for (int i = 1; i <= 3; i++)
                neighboringRooms.Add(exits[GetPlayerLocation(), i]);
            return neighboringRooms;
        }

        private int GetPlayerLocation()
        {
            return _boardPieces._pieces[1];
        }

        private void PrintNearHazard(int j)
        {
            switch (j - 1)
            {
                // 610 on j-1 goto 615,625,625,635,635
                case 1:
                    NearAWumpus();
                    break;
                case 2:
                case 3:
                    NearAPit();
                    break;
                case 4:
                case 5:
                    NearBats();
                    break;
            }
        }

        private void NearBats()
        {
            _io.WriteLine("BATS NEARBY!");
        }

        private void NearAPit()
        {
            _io.WriteLine("I FEEL A DRAFT");
        }

        private void NearAWumpus()
        {
            _io.WriteLine("I SMELL A WUMPUS!");
        }

        private char GiveIntroduction(char istr)
        {
            _io.Prompt("INSTRUCTIONS (Y-N) ");
            istr = _io.ReadChar();
            if (!(istr == 'N' || istr == 'n'))
                _io.GiveInstructions();
            return istr;
        }

        private void gosub(int gosubLine, int lineToReturnTo) {
            _nextLine = gosubLine;
            ReturnLine.Push(lineToReturnTo);
        }

        private void returnFromGosub() {
            if (ReturnLine.Count == 0)
                _nextLine = 1151;
            else
                _nextLine = ReturnLine.Pop();
        }
    }
}