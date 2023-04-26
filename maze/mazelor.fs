( -------------------------------------- )
char # constant wall
char . constant pol
char X constant man
char * constant star
char & constant corpse
( ------------------------------------- )
: str,    here over char+ allot place ;
: maze,"  34 parse str, ;
( #cccccccccccc)
( #cccccccccccc)
( #cccccccccccc)
( #cccccccccccc)
0 value maze
0 value /maze
here to maze
maze," ########################################################################"
maze," #...#......#....#......##...#......##...#......#....#......#....#......#"
maze," X.#.#.####.#..#.#.####.#..#.#.####....#.#.####.#..#.#.####.#..#.#.####.#"
maze," ###......#.####......#.####......#.####......#.####......#.####......#.#"
maze," #....###.#.......###.#..#....###.#..#....###.#..#....###.#..#....###.#.."
maze," ####.#.#.#.#####.#.#.#.#####.#.#.#.#####.#.#.#.#####.#.#.#.#####.#.#.#.#"
maze," #..#.#.#.#..#..#.#.#.#..#..#.#.#.#.....#.#.#.#.#...#.#.#.#..#..#.#.#.#.#"
maze," ##.#.#.#.#.###.#.#.#.#.###.#.#.#.#.###.#.#.#.#.###.#.#.#.#..##.#.#.#.#.#"
maze," #........#.#.........#...........#.#.........#...........#...........#.#"
maze," ######.###.#######.###.#######.###.#######.###.#######.###..######.###.#"
maze," #..................#....#......#...#...................#....#..........#"
maze," ########################################################################"
here to /maze
: maze#size  /maze maze - ;
: maze#cols  maze c@ ;
: maze#lins  maze#size maze#cols 1+ / ;
: XY2addr { X Y -- addr } 
  Y 1-  maze#cols 1+  *  X +  maze + ;
: addr2XY { addr -- X Y } 
  addr maze -  maze#cols 1+  /mod 1+ ;
( ------------------------------------- )
0 value X 
0 value Y 
X value X0
Y value Y0
( ------------------------------------- )
: findX ( -- )
  maze#lins 0 ?do 
    maze#cols 1+ I * maze + 1+ maze c@  s" X"  search 
    if drop addr2XY to Y to X else 2drop endif
  loop  X to X0  Y to Y0 ; 
: iswall? XY2addr c@ wall = ;
: ispol?  XY2addr c@ pol  = ;
: isman?  XY2addr c@ man  = ;
( ------------------------------------- )
8 value direction
( ------------------------------------- )
(   8   )
( 4-|-6 )
(   2   )
: aheadXY ( -- aX aY )
  direction case
      8 of X    Y 1- endof
      6 of X 1+ Y    endof
      2 of X    Y 1+ endof
      4 of X 1- Y    endof
  endcase ;
: rightXY ( -- rX rY )
   direction case 
      8 of X 1+ Y    endof
      6 of X    Y 1+ endof
      2 of X 1- Y    endof
      4 of X    Y 1- endof
  endcase ;
: rturn ( -- )
  direction case 
    8 of 6 endof
    6 of 2 endof
    2 of 4 endof
    4 of 8 endof
    8
  endcase  to direction ;
: lturn ( -- )
  direction case 
    6 of 8 endof
    8 of 4 endof
    4 of 2 endof
    2 of 6 endof
    8
  endcase  to direction ;
( ------------------------------------- )
0    value    #step
1000 value max#step
: #step+1 #step 1+ to #step ;
( ------------------------------------- )
: printSTATE 
  page cr
  ." step: " #step . cr
  maze#lins 0 ?do 
    I 1+ 3 .r ." : " 
    maze#cols 1+ I * maze + 1+ maze c@ type cr 
  loop cr 200 ms ;
( ------------------------------------- )
: freedom?     X maze#cols =   Y maze#lins =   or ;
: back2start?  X X0 =   Y Y0 =  and  #step 1 > and ;
: starvation?  #step max#step > ;
: *markcorpse*  corpse X Y XY2addr c! ;
: *markstar*    star   X Y XY2addr c! ;
: *markman*     man    X Y XY2addr c! ;
( ------------------------------------- )
: go recursive 
  #step+1 
  starvation? if *markcorpse* ." Died of starvation." exit endif
  back2start? if ." Back to start point." exit endif
  printSTATE 
( ------------------------------------- )
  freedom? if 
    cr ." Exit. FREEDOM! ROCK'N'ROLL!" cr
  else 
    rightXY iswall?  
    if
      aheadXY iswall?  
      if
        *markstar*  lturn  *markman*  go
      else
        *markstar*  aheadXY to Y to X  *markman*  go
      endif
    else
      *markstar*  rturn  aheadXY to Y to X  *markman*  go
    endif
  endif ;
( ------------------------------------- )
: main
  printSTATE
  findX
  go
  printSTATE ;
( ------------------------------------- )
main
( bye )
