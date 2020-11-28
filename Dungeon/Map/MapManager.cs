﻿using DungeonGame.Map;
using DungeonUtility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace DungeonGame
{
    public class MapManager
    {
        public MapManager()
        {
            GenerateRoom();
        }

        private string[] ReadMapFromFile(string mapName = default)
        {
            string path = @"D:\Desktop\不會得獎的專案\Dungeon\Maps.csv";
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

        public void GenerateRoom()
        {
            string[] mapData = ReadMapFromFile();

            Bitmap bg = new Bitmap(UI.p_Viewport.Width, UI.p_Viewport.Height);
            using (Graphics g = Graphics.FromImage(bg))
            {
                g.Clear(Color.Black);

                for (int i = 0; i < col * row; i++)
                {
                    int colPos = i % col * tileSize.Width;
                    int rowPos = i / col * tileSize.Height;
                    Point pos = new Point(colPos, rowPos);
                    Rectangle rect = new Rectangle(pos, tileSize);

                    TileType tileType = EnumEx<TileType>.GetEnumByOrder(Convert.ToUInt16(mapData[i]));
                    tilesData.Add(new Point(i % col, i / col), tileType);
                    g.FillRectangle(palette[tileType], rect);
                }
            }

            UI.p_Viewport.BackgroundImage = bg;
        }

        // todo: walkable
        private TileType? GetTileType(Point p)
        {
            Point _p = new Point(p.X / tileSize.Width, p.Y / tileSize.Height);
            
            if (_p.X > -1 && _p.X < col && _p.Y > -1 && _p.Y < row)
                return tilesData[_p];

            return null;
        }

        public bool IsWalkable(Point p)
        {
            if (GetTileType(p) == TileType.Floor || GetTileType(p) == TileType.Door)
                return true;

            return false;
        }

        private bool IsInteractable(Point p)
        {
            if (GetTileType(p) == TileType.Floor)
                return true;

            return false;
        }
        
        private void BorderlineEffect(Point p)
        {
            /*Bitmap borderline = new Bitmap(tileSize.Width, tileSize.Height);
            using (Graphics g = Graphics.FromImage(borderline))
            {
                g.Clear(Color.Transparent);

                Point _p = new Point(p.X / tileSize.Width, p.Y / tileSize.Height);

                int colPos = _p.X * tileSize.Width;
                int rowPos = _p.Y * tileSize.Height;
                Point pos = new Point(colPos, rowPos);
                Rectangle rect = new Rectangle(pos, tileSize);

                g.DrawRectangle(Pens.Azure, rect);
            }

            UI.p_Viewport.*/
        }

        public void Interact(Point p)
        {
            if(IsInteractable(p))
            {
                BorderlineEffect(p);
            }
        }

        private const int row = 11;   // 440/40
        private const int col = 20;   // 800/40
        private Size tileSize = new Size(40, 40);
        private Dictionary<Point, TileType> tilesData = new Dictionary<Point, TileType>(row * col);
        private Dictionary<TileType, Brush> palette = new Dictionary<TileType, Brush>
        {
            { TileType.None,    new SolidBrush(Color.FromArgb(70,75,82)) },
            { TileType.Wall,    new SolidBrush(Color.FromArgb(177,188,208)) },
            { TileType.WallTop, new SolidBrush(Color.FromArgb(207,218,238)) },
            { TileType.Floor,   new SolidBrush(Color.FromArgb(195,200,208)) },
            { TileType.Door,    new SolidBrush(Color.FromArgb(14,31,62)) }
        };
    }
}
