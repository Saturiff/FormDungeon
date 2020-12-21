using DungeonUtility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace DungeonServer
{
    /// <summary>
    /// 伺服器地圖類，生成地圖以供隨機生成物品時使用
    /// </summary>
    public class Map
    {
        public Map() => GenerateRoomData();

        private string[] ReadMapFromFile()
        {
            string path = @".\Maps\Map.csv";
            string rawData = "";
            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                byte[] b = new byte[473];
                UTF8Encoding temp = new UTF8Encoding(true);

                while (fs.Read(b, 0, b.Length) > 0)
                    rawData += temp.GetString(b);
            }

            string[] mapData = rawData.Split(',', '\n');

            return mapData;
        }

        public void GenerateRoomData()
        {
            tilesData.Clear();
            string[] mapData = ReadMapFromFile();

            for (int i = 0; i < col * row; i++)
            {
                TileType tileType = EnumEx.GetEnumByOrder<TileType>(Convert.ToUInt16(mapData[i]));
                tilesData.Add(new Point(i % col, i / col), tileType);
            }
        }

        private TileType? GetTileType((int x, int y) p)
        {
            p = (p.x / tileSize.Width, p.y / tileSize.Height);

            if (p.x > -1 && p.x < col && p.y > -1 && p.y < row)
                return tilesData[new Point(p.x, p.y)];

            return null;
        }

        public bool IsWalkable(Rect objRect)
        {
            if ((GetTileType(objRect.x0y0) == TileType.Floor)
                && (GetTileType(objRect.x1y0) == TileType.Floor)
                && (GetTileType(objRect.x0y1) == TileType.Floor)
                && (GetTileType(objRect.x1y1) == TileType.Floor))
                return true;

            return false;
        }

        public (int x, int y) GetRandomFitPointInPlayGround(int w, int h)
        {
            (int x, int y) loc;

            do
            {
                loc = Rand.GetRandPointInRect(playGround);
            }
            while (!IsWalkable(new Rect(loc.x, loc.y, w, h)));

            return loc;
        }

        private static readonly Rect playGround = new Rect(800, 440);
        private static readonly Rect tileSize = new Rect(40, 40);
        private static readonly int row = playGround.Height / tileSize.Width;
        private static readonly int col = playGround.Width / tileSize.Height;
        private readonly Dictionary<Point, TileType> tilesData = new Dictionary<Point, TileType>(row * col);

        public enum TileType
        {
            None,
            Wall,
            WallTop,
            Floor
        }
    }
}
