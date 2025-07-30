//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MapGridObject {

//    public enum Type {
//        Empty,
//        Mine,
//        MineNum_1,
//        MineNum_2,
//        MineNum_3,
//        MineNum_4,
//        MineNum_5,
//        MineNum_6,
//        MineNum_7,
//        MineNum_8,
//    }
//    private Grid<MapGridObject> grid;
//    private int x;
//    private int y;
//    private Type type;

//    public MapGridObject(Grid<MapGridObject> grid, int x, int y) {
//        this.grid = grid;
//        this.x = x;
//        this.y = y;
//        type = (Type)Random.Range(0, System.Enum.GetValues(typeof(Type)).Length);
//    }

//    public override string ToString(){
//return type.ToString();
//    }
//}
