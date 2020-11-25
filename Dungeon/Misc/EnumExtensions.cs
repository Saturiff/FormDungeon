using System;
using System.Linq;

namespace DungeonGame
{
    public static class EnumExtensions<T>
    {
        // 由位置取得目標枚舉
        public static T GetEnumByOrder(int index)
                => Enum.GetValues(typeof(T)).Cast<T>().Select((x, i)
                => new { item = x, index = i }).Single(x => x.index == index).item;
        
        // 由枚舉取得目標位置
        public static int GetOrderByEnum(T item)
                => Enum.GetValues(typeof(T)).Cast<T>().Select((x, i)
                => new { item = x, index = i }).Single(x => x.item.Equals(item)).index;
    }
}
