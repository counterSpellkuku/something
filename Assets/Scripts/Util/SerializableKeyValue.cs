using System;

namespace Util
{
    [Serializable]
    public class SerializableKeyValue<K,V>
    {
        public K Key;
        public V Value;
    }
}