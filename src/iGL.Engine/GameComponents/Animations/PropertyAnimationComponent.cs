using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using iGL.Engine.Math;
using System.Xml.Linq;

namespace iGL.Engine
{
    public class PropertyAnimationComponent : AnimationComponent
    {
        public enum Type
        {
            Linear
        }

        public string Property { get; set; }
        public string StartValue { get; set; }
        public string StopValue { get; set; }

        private PropertyInfo _propertyInfo;
        private MethodInfo _setMethod;
        private bool _isPlaying;
        private DateTime _tickTime;
        private object _target;
        private static Dictionary<System.Type, IEnumerable<PropertyInfo>> _typePropertyCache = new Dictionary<System.Type, IEnumerable<PropertyInfo>>();

        private float _startValueFloat;
        private float _stopValueFloat;
        private bool _startValueBool;
        private bool _stopValueBool;
        private int _startValueInt;
        private int _stopValueInt;
        private Vector3 _startValueVector3;
        private Vector3 _stopValueVector3;
        private Vector4 _startValueVector4;
        private Vector4 _stopValueVector4;

        private Action<Vector3> _vec3SetAction;
        private Action<Vector4> _vec4SetAction;
        private Action<bool> _boolSetAction;
        private Action<int> _intSetAction;
        private Action<float> _floatSetAction;

        public PropertyAnimationComponent(XElement xmlElement) : base(xmlElement) { }

        public PropertyAnimationComponent() : base() { }

        public override void Play()
        {
            base.Play();

            _tickTime = DateTime.UtcNow;
            _isPlaying = true;
        }

        public override void Stop()
        {
            base.Stop();

            _isPlaying = false;
        }

        public override void Pause()
        {
            base.Pause();
        }

        public override bool InternalLoad()
        {
            base.InternalLoad();

            if (Property == null) return false;

            var properties = Property.Split('.');
            if (properties.Length > 0)
            {
                PropertyInfo prop = null;
                _target = GameObject;
                foreach (var property in properties)
                {
                    if (prop == null)
                    {
                        prop = GameObject.GetProperties().FirstOrDefault(p => p.Name == property);
                    }
                    else
                    {
                        _target = prop.GetValue(_target, null);
                        if (_target == null) return false;

                        prop = GetTypeProperties(_target.GetType()).FirstOrDefault(p => p.Name == property);
                    }

                    if (prop == null) return false;
                }

                _propertyInfo = prop;
                _setMethod = _propertyInfo.GetSetMethod();                
            }
            else
            {
                _target = GameObject;
                _propertyInfo = GameObject.GetType().GetProperties().FirstOrDefault(p => p.Name == Property);
                _setMethod = _propertyInfo.GetSetMethod();

            }

            if (_propertyInfo.PropertyType == typeof(float))
            {
                _startValueFloat = float.Parse(StartValue);
                _stopValueFloat = float.Parse(StopValue);

                _floatSetAction = (Action<float>)Delegate.CreateDelegate(typeof(Action<float>), _target, _setMethod);
            }
            else if (_propertyInfo.PropertyType == typeof(bool))
            {
                _startValueBool = bool.Parse(StartValue);
                _stopValueBool = bool.Parse(StopValue);

                _boolSetAction = (Action<bool>)Delegate.CreateDelegate(typeof(Action<bool>), _target, _setMethod);
            }
            else if (_propertyInfo.PropertyType == typeof(int))
            {
                _startValueInt = int.Parse(StartValue);
                _stopValueInt = int.Parse(StopValue);

                _intSetAction = (Action<int>)Delegate.CreateDelegate(typeof(Action<int>), _target, _setMethod);
            }
            else if (_propertyInfo.PropertyType == typeof(Vector3))
            {
                var startValues = StartValue.Replace("(", string.Empty).Replace(")", string.Empty).Split(',').Select(s => float.Parse(s)).ToArray();
                var stopValues = StopValue.Replace("(", string.Empty).Replace(")", string.Empty).Split(',').Select(s => float.Parse(s)).ToArray();

                _startValueVector3 = new Vector3(startValues[0], startValues[1], startValues[2]);
                _stopValueVector3 = new Vector3(stopValues[0], stopValues[1], stopValues[2]); ;

                _vec3SetAction = (Action<Vector3>)Delegate.CreateDelegate(typeof(Action<Vector3>), _target, _setMethod);

            }
            else if (_propertyInfo.PropertyType == typeof(Vector4))
            {
                var startValues = StartValue.Split(',').Select(s => float.Parse(s)).ToArray();
                var stopValues = StopValue.Split(',').Select(s => float.Parse(s)).ToArray();

                _startValueVector4 = new Vector4(startValues[0], startValues[1], startValues[2], startValues[3]);
                _stopValueVector4 = new Vector4(stopValues[0], stopValues[1], stopValues[2], stopValues[3]);

                _vec4SetAction = (Action<Vector4>)Delegate.CreateDelegate(typeof(Action<Vector4>), _target, _setMethod);
            }

            return _propertyInfo != null;
        }

        private IEnumerable<PropertyInfo> GetTypeProperties(System.Type type)
        {
            IEnumerable<PropertyInfo> props;
            if (!_typePropertyCache.TryGetValue(type, out props))
            {
                props = type.GetProperties()
                            .Where(p => p.GetSetMethod() != null);

                _typePropertyCache.Add(type, props);
            }

            return props;
        }

        private void Step()
        {
            if (GameObject.Scene.IsDisposing) return;

            float percentage = (float)(DateTime.UtcNow - _tickTime).TotalSeconds / (DurationSeconds);

            if (percentage > 1) percentage = 1;

            try
            {
                if (_propertyInfo.PropertyType == typeof(float))
                {
                    float value = _startValueFloat + (_stopValueFloat - _startValueFloat) * percentage;
                    _floatSetAction(value);
                }
                else if (_propertyInfo.PropertyType == typeof(bool))
                {
                    if (percentage < 1)
                    {
                        _boolSetAction(_startValueBool);
                    }
                    else
                    {
                        _boolSetAction(_stopValueBool);
                    }
                }
                else if (_propertyInfo.PropertyType == typeof(int))
                {
                    int value = (int)(_startValueInt + (_stopValueInt - _startValueInt) * percentage);
                    _intSetAction(value);
                }
                else if (_propertyInfo.PropertyType == typeof(Vector3))
                {
                    var value = Vector3.Lerp(_startValueVector3, _stopValueVector3, percentage);                
                    _vec3SetAction(value);
                }
                else if (_propertyInfo.PropertyType == typeof(Vector4))
                {
                    var value = Vector4.Lerp(_startValueVector4, _stopValueVector4, percentage);
                    _vec4SetAction(value);
                }
            }
            catch { }

            if (percentage >= 1)
            {
                if (PlayMode == AnimationComponent.Mode.RepeatInverted)
                {
                    var start = StartValue;
                    StartValue = StopValue;
                    StopValue = start;

                    Play();
                }
                else if (PlayMode == AnimationComponent.Mode.Repeat)
                {
                    Play();
                }
                else
                {
                    Stop();
                }
            }
        }

        public override void Tick(float timeElapsed)
        {
            if (!_isPlaying) return;

            Step();
        }
    }
}
