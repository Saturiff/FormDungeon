using DungeonGame.Map;
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

        private bool IsInteractable(Point p)
        {
            if (GetTileType((p.X, p.Y)) == TileType.Floor)
                return true;

            return false;
        }

        private readonly static Size playGround = new Size(800, 440);
        private readonly static Size tileSize = new Size(40, 40);
        private readonly static int row = playGround.Height / tileSize.Width;
        private readonly static int col = playGround.Width / tileSize.Height;
        private Dictionary<Point, TileType> tilesData = new Dictionary<Point, TileType>(row * col);
        private readonly Dictionary<TileType, Brush> palette = new Dictionary<TileType, Brush>
        {
            { TileType.None,    new SolidBrush(Color.FromArgb(70,75,82)) },
            { TileType.Wall,    new SolidBrush(Color.FromArgb(177,188,208)) },
            { TileType.WallTop, new SolidBrush(Color.FromArgb(207,218,238)) },
            { TileType.Floor,   new SolidBrush(Color.FromArgb(195,200,208)) }
        };
    }
}
