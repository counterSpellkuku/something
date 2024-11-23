using System;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    /*
     * Usage:
     * public class SerializeDictionary_ :  SerializeDictionary<key_datatype, value_datatype> {}
     * public SerializeDictionary_ dic = new SerializeDictionary_(); 
     */

    [Serializable]
    public class SerializableDictionary <K,V> : Dictionary<K,V>, ISerializationCallbackReceiver where K : class
    {
        [SerializeField] private List<SerializableKeyValue<K, V>> _keyValueList;

        public void OnBeforeSerialize() 
        {
            if (this.Count < _keyValueList.Count)
            {
                return;
            }

            _keyValueList.Clear();
        
            foreach (var kv in this)
            {
                _keyValueList.Add(new SerializableKeyValue<K, V>()
                {
                    Key = kv.Key,
                    Value = kv.Value
                });
            }        
        }

        public void OnAfterDeserialize() 
        {
            this.Clear();
            foreach (var kv in _keyValueList)
            {
                if(!this.TryAdd(kv.Key, kv.Value))
                {
                    Debug.LogError($"List has duplicate Key");
                }
            }
        }
    }
}