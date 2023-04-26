
\ --------------------------------------
char # constant wall
char . constant pol
char @ constant man
char * constant star
char & constant corpse
\ -------------------------------------
\ : str,    here over char+ allot place ;
\ : maze,"  34 parse str, ;
: maze,"  34 parse here over char+ allot place ;
\ #cccccccccccc
\ #cccccccccccc
\ #cccccccccccc
\ #cccccccccccc

0 value maze
0 value /maze

here to maze
maze," ########################################################################"
maze," #...#......#....#......##...#......##...#......#....#......#....#......#"
maze," #@#.#.####.#..#.#.####.#..#.#.####....#.#.####.#..#.#.####.#..#.#.####.#"
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
\ -------------------------------------
0 value X
0 value Y
X value X0
Y value Y0
\ -------------------------------------
: find@ ( -- )
  maze#lins 0 ?do
    maze#cols 1+ I * maze + count s" @" search  ( -- addr r t|f )
    if drop addr2XY to Y to X leave else 2drop endif
  loop  X to X0  Y to Y0 ;
\ -------------------------------------
: iswall? 2dup XY2addr c@ wall = ;
: ispol?  2dup XY2addr c@ pol  = ;
: isman?  2dup XY2addr c@ man  = ;
\ -------------------------------------
\   8
\ 4-|-6
\   2
: hide@    ( -- )  pol X Y XY2addr c! ;
: show@    ( -- )  man X Y XY2addr c! ;
: go-up    ( -- )  hide@  X Y 1- ispol? if to Y drop then  show@ ;
: go-down  ( -- )  hide@  X Y 1+ ispol? if to Y drop then  show@ ;
: go-left  ( -- )  hide@  X 1- Y ispol? if drop to X then  show@ ;
: go-right ( -- )  hide@  X 1+ Y ispol? if drop to X then  show@ ;
\ -------------------------------------
0    value    #step
1000 value max#step
: #step+1 #step 1+ to #step ;
\ -------------------------------------
: printSTATE
  cr ." step: " #step . cr
  maze#lins 0 ?do
    I 1+ 3 .r ." : " maze#cols 1+ I * maze + count type cr
  loop  cr ;
\ -------------------------------------
: freedom?     X maze#cols =   Y maze#lins =   or ;
: back2start?  X X0 =   Y Y0 =  and  #step 1 > and ;
: starvation?  #step max#step > ;
: *markcorpse* corpse X Y XY2addr c! ;
: *markstar*   star   X Y XY2addr c! ;
: *markman*    man    X Y XY2addr c! ;
\ -------------------------------------
: print-help ;
\ --------------------------------------
: play-loop ( -- )
  BEGIN
    #step+1
    starvation? if *markcorpse* ." Died of starvation." exit endif
\    back2start? if ." Back to start point." exit endif
\    page printSTATE 200 ms
    freedom? if cr ." Exit. FREEDOM! ROCK'N'ROLL!" cr exit endif
    page printSTATE
\ ------------------------
    key CASE
        [char] ? OF  print-help false  ENDOF
        [char] h OF  go-left  false  ENDOF
        [char] j OF  go-down  false  ENDOF
        [char] k OF  go-up    false  ENDOF
        [char] l OF  go-right false  ENDOF
        \ vt100 cursor keys should work too
        27       OF  key? if key [char] [ = if
                    key CASE
                        [char] D OF  go-left  false  ENDOF
                        [char] B OF  go-down  false  ENDOF
                        [char] A OF  go-up    false  ENDOF
                        [char] C OF  go-right false  ENDOF
                    ENDCASE     then then  ENDOF
        [char] q OF           true   ENDOF
        false swap
    ENDCASE
  UNTIL ;
\ -------------------------------------
: play-loop2
  BEGIN
    #step+1
    starvation? if *markcorpse* ." Died of starvation." exit endif
\    back2start? if ." Back to start point." exit endif
\    page printSTATE 200 ms
    freedom? if cr ." Exit. FREEDOM! ROCK'N'ROLL!" cr exit endif
    page printSTATE
\ ------------------------
    ekey ekey>char if ( c )
        CASE
            [char] ? OF  print-help  ENDOF
            [char] h OF  go-left     ENDOF
            [char] j OF  go-down     ENDOF
            [char] k OF  go-up       ENDOF
            [char] l OF  go-right    ENDOF
\            [char] q OF  exit        ENDOF
\            [char] q OF  leave       ENDOF
        ENDCASE
    else ekey>fkey if ( key-id )
        CASE
            k-up     OF  go-up       ENDOF
            k-down   OF  go-down     ENDOF
            k-left   OF  go-left     ENDOF
            k-right  OF  go-right    ENDOF
        ENDCASE
    else ( keyboard-event )
       drop \ just ignore an unknown keyboard event type
     then then
  AGAIN ;
\ -------------------------------------
: main
  printSTATE
  find@
  play-loop
  printSTATE ;
\ --------------------------------------
main

