using System;
using System.Linq;

namespace DungeonUtility
{
    public static class EnumEx
    {
        /// <summary>
        /// 由索引取得目標枚舉項目
        /// </summary>
        /// <param name="index">欲取得枚舉項目之索引</param>
        /// <returns>索引</returns>
        public static T GetEnumByOrder<T>(int index)
                => Enum.GetValues(typeof(T)).Cast<T>().Select((x, i)
                => new { item = x, index = i }).Single(x => x.index == index).item;

        /// <summary>
        /// 由枚舉項目取得索引
        /// </summary>
        /// <param name="item">欲取得索引之枚舉項目</param>
        /// <returns>枚舉項目</returns>
        public static int GetOrderByEnum<T>(T item)
                => Enum.GetValues(typeof(T)).Cast<T>().Select((x, i)
                => new { item = x, index = i }).Single(x => x.item.Equals(item)).index;
    }
}
