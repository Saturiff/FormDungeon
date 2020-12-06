﻿using DungeonUtility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace DungeonServer
{
    public class Map
    {
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
            string[] mapData = ReadMapFromFile();

            for (int i = 0; i < col * row; i++)
            {
                TileType tileType = EnumEx<TileType>.GetEnumByOrder(Convert.ToUInt16(mapData[i]));
                tilesData.Add(new Point(i % col, i / col), tileType);
            }
        }

        private TileType? GetTileType((int x, int y) p)
        {
            p = (p.x / tileSize.width, p.y / tileSize.height);

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

        public static (int x, int y) GetRandomPointInPlayGround()
            => Rand.GetRandPointInRect(playGround);

        private readonly static Rect playGround = new Rect(800, 440);
        private readonly static Rect tileSize = new Rect(40, 40);
        private readonly static int row = playGround.height / tileSize.width;
        private readonly static int col = playGround.width / tileSize.height;
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
