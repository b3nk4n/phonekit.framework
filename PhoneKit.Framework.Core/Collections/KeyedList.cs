using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneKit.Framework.Core.Collections
{
    /// <summary>
    /// A keyed list used for LongListSelector grouping.
    /// </summary>
    /// <typeparam name="K">The key.</typeparam>
    /// <typeparam name="V">The value item.</typeparam>
    public class KeyedList<K, V> : List<V>
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        public K Key { protected set; get; }

        /// <summary>
        /// Creates a new KeyedList instance.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="items">>The value items.</param>
        public KeyedList(K key, IEnumerable<V> items)
            : base(items)
        {
            Key = key;
        }

        /// <summary>
        /// Creates a new KeyedList instance with the given grouping.
        /// </summary>
        /// <param name="grouping">The grouping.</param>
        public KeyedList(IGrouping<K, V> grouping)
            : base(grouping)
        {
            Key = grouping.Key;
        }

        /// <summary>
        /// Gets whether the key is empty, which is used for empty pseudo groups.
        /// </summary>
        public bool IsEmptyKey
        {
            get
            {
                if (Key == null)
                    return true;
                return string.IsNullOrEmpty(Key.ToString());
            }
        }
    }
}
