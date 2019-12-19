module Map
type Pos={
    x:int
    y:int
}

type EntityID = int
type EnemyType = int
type AggressionLevel = 
|ActivelyPursueStayClose
|ActivelyPursue
|AttackInRange
|Ignore

type Tactic = int

type BasicCommands=
    |Move of Pos
    |Stop
    |Deploy
    |Attack of EntityID
    |ChangeTactic of Tactic
    |AdjustAgression of EnemyType * AggressionLevel

type Unit ={
    hp:int
    team:int
    pos:Pos
    direction:float
    width:int
    length:int
    mindControllable:bool
    //navigate: GridSquare
}
 
type GridType =
|Normal
|Water
|Ore
|Cliff
|Impassable

type ImageIndex = int

type GridPoint ={
    elevation:int
    movementType:GridType
}
type GridChunk ={
    points :GridPoint [][]
    width : int
    height : int
    //nav : Pos 
} 



type Map ={
    points :GridPoint [][]
    units : Unit List
    width : int
    height : int
}


