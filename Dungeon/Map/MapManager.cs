using DungeonGame.Map;
using DungeonUtility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace DungeonGame
{
    /// <summary>
    /// 地圖類，產生地圖與提供判定是否可行走的功能
    /// </summary>
    public class MapManager
    {
        public MapManager() => GenerateRoom();

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

        public void GenerateRoom()
        {
            string[] mapData = ReadMapFromFile();

            Bitmap bg = new Bitmap(Game.p_Viewport.Width, Game.p_Viewport.Height);
            using (Graphics g = Graphics.FromImage(bg))
            {
                g.Clear(Color.Black);

                for (int i = 0; i < col * row; i++)
                {
                    int colPos = i % col * tileSize.Width;
                    int rowPos = i / col * tileSize.Height;
                    Point pos = new Point(colPos, rowPos);
                    Rectangle rect = new Rectangle(pos, tileSize);

                    TileType tileType = EnumEx.GetEnumByOrder<TileType>(Convert.ToUInt16(mapData[i]));
                    _tilesData.Add(new Point(i % col, i / col), tileType);
                    g.FillRectangle(_palette[tileType], rect);
                }
            }

            Game.p_Viewport.BackgroundImage = bg;
        }

        private TileType? GetTileType((int x, int y) p)
        {
            p = (p.x / tileSize.Width, p.y / tileSize.Height);

            if (p.x > -1 && p.x < col && p.y > -1 && p.y < row)
                return _tilesData[new Point(p.x, p.y)];

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

        private static readonly Size playGround = new Size(800, 440);
        private static readonly Size tileSize = new Size(40, 40);
        private static readonly int row = playGround.Height / tileSize.Width;
        private static readonly int col = playGround.Width / tileSize.Height;
        private Dictionary<Point, TileType> _tilesData = new Dictionary<Point, TileType>(row * col);
        private readonly Dictionary<TileType, Brush> _palette = new Dictionary<TileType, Brush>
        {
            { TileType.None,    new SolidBrush(Color.FromArgb(70,75,82)) },
            { TileType.Wall,    new SolidBrush(Color.FromArgb(177,188,208)) },
            { TileType.WallTop, new SolidBrush(Color.FromArgb(207,218,238)) },
            { TileType.Floor,   new SolidBrush(Color.FromArgb(195,200,208)) }
        };
    }
}
