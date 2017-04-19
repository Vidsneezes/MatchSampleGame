using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStore {

    private Dictionary<string, float> _floatDictionary;
    private Dictionary<string, string> _stringDictionary;

    public StateStore()
    {
        _floatDictionary = new Dictionary<string, float>();
        _stringDictionary = new Dictionary<string, string>();
    }

    public void SetFloat(string key, float value)
    {
        if (_floatDictionary.ContainsKey(key))
        {
            _floatDictionary[key] = value;
        }
        else
        {
            _floatDictionary.Add(key, value);
        }
    }

    public bool GetFloat(string key, out float saveTo)
    {
        return _floatDictionary.TryGetValue(key, out saveTo);
    }

    public void SetString(string key, string value)
    {
        if (_stringDictionary.ContainsKey(key))
        {
            _stringDictionary[key] = value;
        }
        else
        {
            _stringDictionary.Add(key, value);
        }
    }

    public bool GetString(string key, out string saveTo)
    {
        return _stringDictionary.TryGetValue(key, out saveTo);
    }

}
