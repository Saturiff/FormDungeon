using System.Drawing;

namespace DungeonUtility
{
    public static class GraphicsEx
    {
        /// <summary>
        /// 繪製空心的圓形
        /// </summary>
        /// <param name="g">Graphics物件</param>
        /// <param name="pen">Pen物件</param>
        /// <param name="centerX">圓心座標X</param>
        /// <param name="centerY">圓心座標Y</param>
        /// <param name="radius">半徑</param>
        public static void DrawCircle(this Graphics g, Pen pen,
                                      float centerX, float centerY, float radius)
        {
            g.DrawEllipse(pen, centerX - radius, centerY - radius,
                          radius + radius, radius + radius);
        }

        /// <summary>
        /// 繪製實心的圓形
        /// </summary>
        /// <param name="g">Graphics物件</param>
        /// <param name="brush">Brush物件</param>
        /// <param name="centerX">圓心座標X</param>
        /// <param name="centerY">圓心座標Y</param>
        /// <param name="radius">半徑</param>
        public static void FillCircle(this Graphics g, Brush brush,
                                      float centerX, float centerY, float radius)
        {
            g.FillEllipse(brush, centerX - radius, centerY - radius,
                          radius + radius, radius + radius);
        }
    }
}
