using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Timberborn.Common;

/*
 * Credit goes to @TobbyTheBobby
 * https://github.com/TobbyTheBobby/TimberbornModsUnity/blob/master/Assets/ChooChoo/Scripts/Core/ChooChooCore.cs
 */

namespace FrostyMods.Common
{
    public class ReflectionHelpers
    {
        private readonly Dictionary<string, FieldInfo> _fieldInfos = new();
        private readonly Dictionary<string, MethodInfo> _methodInfos = new();
        private readonly Dictionary<string, PropertyInfo> _propertyInfos = new();

        public void SetPrivateProperty(object instance, string fieldName, object newValue)
        {
            var propertyInfo = _propertyInfos.GetOrAdd(fieldName, () => AccessTools.TypeByName(instance.GetType().Name).GetProperty(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));

            propertyInfo.SetValue(instance, newValue);
        }

        public object InvokePublicMethod(object instance, string methodName, object[] args)
        {
            if (!_methodInfos.ContainsKey(methodName))
            {
                _methodInfos.Add(methodName, AccessTools.TypeByName(instance.GetType().Name).GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance));
            }

            return _methodInfos[methodName].Invoke(instance, args);
        }

        public object InvokePrivateMethod(object instance, string methodName, object[] args)
        {
            if (!_methodInfos.ContainsKey(methodName))
            {
                _methodInfos.Add(methodName, AccessTools.TypeByName(instance.GetType().Name).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance));
            }

            return _methodInfos[methodName].Invoke(instance, args);
        }

        public void ChangePrivateField(object instance, string fieldName, object newValue)
        {
            if (!_fieldInfos.ContainsKey(fieldName))
            {
                _fieldInfos.Add(fieldName, AccessTools.TypeByName(instance.GetType().Name).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance));
            }

            _fieldInfos[fieldName].SetValue(instance, newValue);
        }

        public object GetPrivateField(object instance, string fieldName)
        {
            if (!_fieldInfos.ContainsKey(fieldName))
            {
                _fieldInfos.Add(fieldName, AccessTools.TypeByName(instance.GetType().Name).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance));
            }

            return _fieldInfos[fieldName].GetValue(instance);
        }
    }
}