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
        private bool _isPlaying;
        private DateTime _tickTime;
        private object _target;

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
                        prop = GameObject.GetType().GetProperties().FirstOrDefault(p => p.Name == property);
                    }
                    else
                    {
                        _target = prop.GetValue(_target, null);
                        if (_target == null) return false;

                        prop = _target.GetType().GetProperties().FirstOrDefault(p => p.Name == property);
                    }

                    if (prop == null) return false;
                }

                _propertyInfo = prop;
            }
            else
            {
                _target = GameObject;
                _propertyInfo = GameObject.GetType().GetProperties().FirstOrDefault(p => p.Name == Property);
            }

            return _propertyInfo != null;
        }



        private void Step()
        {
            float percentage = (float)(DateTime.UtcNow - _tickTime).TotalSeconds / (DurationSeconds);

            if (percentage > 1) percentage = 1;

            try
            {
                if (_propertyInfo.PropertyType == typeof(float))
                {
                    float startValue = float.Parse(StartValue);
                    float stopValue = float.Parse(StopValue);

                    float value = stopValue * percentage;
                    _propertyInfo.SetValue(_target, value, null);

                }
                else if (_propertyInfo.PropertyType == typeof(bool))
                {
                    if (percentage < 1)
                    {
                        bool value = bool.Parse(StartValue);
                        _propertyInfo.SetValue(_target, value, null);
                    }
                    else
                    {
                        bool value = bool.Parse(StopValue);
                        _propertyInfo.SetValue(_target, value, null);
                    }
                }
                else if (_propertyInfo.PropertyType == typeof(int))
                {
                    int startValue = int.Parse(StartValue);
                    int stopValue = int.Parse(StopValue);

                    int value = (int)((stopValue * 1.0f) * percentage);
                    _propertyInfo.SetValue(_target, value, null);
                }
                else if (_propertyInfo.PropertyType == typeof(Vector3))
                {
                    var startValues = StartValue.Replace("(",string.Empty).Replace(")", string.Empty).Split(',').Select(s => float.Parse(s)).ToArray();
                    var stopValues = StopValue.Replace("(", string.Empty).Replace(")", string.Empty).Split(',').Select(s => float.Parse(s)).ToArray();

                    if (startValues.Length == 3 && stopValues.Length == 3)
                    {
                        var startVec = new Vector3(startValues[0], startValues[1], startValues[2]);
                        var stopVec = new Vector3(stopValues[0], stopValues[1], stopValues[2]);

                        var value = Vector3.Lerp(startVec, stopVec, percentage);
                        _propertyInfo.SetValue(_target, value, null);
                    }
                }
                else if (_propertyInfo.PropertyType == typeof(Vector4))
                {
                    var startValues = StartValue.Split(',').Select(s => float.Parse(s)).ToArray();
                    var stopValues = StopValue.Split(',').Select(s => float.Parse(s)).ToArray();

                    if (startValues.Length == 4 && stopValues.Length == 4)
                    {
                        var startVec = new Vector4(startValues[0], startValues[1], startValues[2], startValues[3]);
                        var stopVec = new Vector4(stopValues[0], stopValues[1], stopValues[2], stopValues[3]);

                        var value = Vector4.Lerp(startVec, stopVec, percentage);
                        _propertyInfo.SetValue(_target, value, null);
                    }
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
