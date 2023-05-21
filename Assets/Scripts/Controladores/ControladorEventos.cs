using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControladorEventos
{
    //Controlador de eventos necesario para el patron Observer
    
    static readonly Dictionary<Type, List<object>> Callbacks = new Dictionary<Type, List<object>>();

    //Agrega observadores al evento
    public static void AddListener<T>(Action<T> callback)
    {
        if (!Callbacks.TryGetValue(typeof(T), out var l))
            Callbacks.Add(typeof(T), l = new List<object>());
#if UNITY_EDITOR
        if (l.Contains(callback))
            throw new Exception("Callback added twice");
#endif
        l.Add(callback);
    }

    public static void RemoveListener<T>(Action<T> callback)
    {
        if (Callbacks.TryGetValue(typeof(T), out var l))
            l.Remove(callback);
    }

    //Disparador de eventos
    public static void TriggerEvent<T>(T evt)
    {
        if (Callbacks.TryGetValue(typeof(T), out var l))
        {
                var copy = l.ToList();
                foreach (Action<T> cb in copy)
                    cb(evt);
        }
    }
}