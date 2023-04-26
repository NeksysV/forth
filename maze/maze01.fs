0 value maze
0 value /maze
: >maze ( addr -- ) @ dup @ to maze cell+ @ to /maze ;
: maze,"  34 parse here over char+ allot place ;
: @strlen ( straddr -- memorylenght ) c@ 1+ ;
: maze.cols maze c@ ;
: maze.lins (   -- n ) 0 /maze maze +DO 1+ I @strlen +LOOP ;
: maze.set ( c a -- ) c! ;
: maze.get ( a -- c ) c@ ;
: maze.disp /maze maze +DO I count type cr I @strlen +LOOP ;
: pc.X  /maze    ;
: pc.X! pc.X  c! ;
: pc.X@ pc.X  c@ ;
: pc.Y  /maze 1+ ;
: pc.Y! pc.Y  c! ;
: pc.Y@ pc.Y  c@ ;
: XY>addr ( X Y -- addr )
    0     ( X Y Y' -- )
    /maze maze +DO
        1+
        2dup = if 2drop I + leave then
    I @strlen +LOOP ;
: XY.set ( c X Y -- ) XY>addr maze.set ;
: XY.get ( X Y -- c ) XY>addr maze.get ;
: XY.cur ( X Y -- vX vY ) 2 + at-xy ;
: XY.disp ( c X Y -- ) XY.cur emit ;
: array create dup , cells allot
        does> swap cells + ;
5 array dungeon
( массив из ссылок на 5 лабиринтов )
here 1 dungeon !
0 ,    ( здесь хранится maze )
       ( доступ  1 dungeon @ )
0 ,    ( здесь хранится /maze )
       ( доступ 1 dungeon @ cell+ )
here 1 dungeon @ !    ( -> maze )
maze," ##############################"
maze," #...#......#....#......##....#"
maze," #@#.#.####.#..#.#.####.#..#..#"
maze," ###......#.####......#.####..#"
maze," #....###.#.......###.#.......#"
maze," ####.#.#.#.#####.#.#.#.#####.#"
maze," ##############################"
here 1 dungeon @ cell+ !    ( -> /maze )
0 ,   ( reserv X Y <byte> PC )

1 dungeon >maze

here 2 dungeon !
0 , 0 ,
here 2 dungeon @ !
maze," #####################"
maze," #...................#"
maze," #...................#"
maze," #...................#"
maze," #...................#"
maze," #.........@.........#"
maze," #...................#"
maze," #...................#"
maze," #...................#"
maze," #...................#"
maze," #...................."
maze," #####################"
here 2 dungeon @ cell+ !
0 ,

2 dungeon >maze
