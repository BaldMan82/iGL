using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using iGL.Engine;

namespace iGL.Designer
{
    public class EngineAssets
    {
        private static EngineAssets _assets;

        private List<Type> _components;
        private List<Type> _gameObjects;

        public IEnumerable<Type> Components
        {
            get { return _components.AsEnumerable(); }
        }

        public IEnumerable<Type> GameObjects
        {
            get { return _gameObjects.AsEnumerable(); }
        }

        public static EngineAssets Instance
        {
            get
            {
                lock (typeof(EngineAssets))
                {
                    if (_assets == null) _assets = new EngineAssets();
                }

                return _assets;
            }
        }

        private EngineAssets()
        {
            var engineAssembly = Assembly.GetAssembly(typeof(Game));
            var testGame = Assembly.GetAssembly(typeof(TestGame.TestGame));

            _gameObjects = GetGameObjectTypes(engineAssembly);
            _gameObjects.AddRange(GetGameObjectTypes(testGame));

            _components = GetGameComponentTypes(engineAssembly);
            _components.AddRange(GetGameComponentTypes(testGame));
                    
        }

        private List<Type> GetGameObjectTypes(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => HasGameObjectType(t) && !t.IsAbstract).ToList();
        }

        private List<Type> GetGameComponentTypes(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => HasGameComponentType(t) && !t.IsAbstract).ToList();
        }

        private bool HasGameComponentType(Type type)
        {
            if (type.BaseType == null) return false;

            if (type.BaseType == typeof(GameComponent)) return true;
            if (type.BaseType == typeof(Object)) return false;

            return HasGameComponentType(type.BaseType);
        }

        private bool HasGameObjectType(Type type)
        {
            if (type.IsAbstract) return false;

            if (type.BaseType == typeof(GameObject)) return true;
            if (type.BaseType == typeof(Object)) return false;

            return HasGameObjectType(type.BaseType);
        }
    }
}
