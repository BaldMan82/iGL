﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using iGL.Engine.Math;

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
        public float DurationSeconds { get; set; }

        private PropertyInfo _propertyInfo;
        private bool _isPlaying;
        private DateTime _tickTime;

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
            _propertyInfo = GameObject.GetType().GetProperties().FirstOrDefault(p => p.Name == Property);

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
                    _propertyInfo.SetValue(this.GameObject, value, null);

                }
                else if (_propertyInfo.PropertyType == typeof(bool))
                {
                    if (percentage < 1)
                    {
                        bool value = bool.Parse(StartValue);
                        _propertyInfo.SetValue(this.GameObject, value, null);
                    }
                    else
                    {
                        bool value = bool.Parse(StopValue);
                        _propertyInfo.SetValue(this.GameObject, value, null);
                    }
                }
                else if (_propertyInfo.PropertyType == typeof(int))
                {
                    int startValue = int.Parse(StartValue);
                    int stopValue = int.Parse(StopValue);

                    int value = (int)((stopValue*1.0f) * percentage);
                    _propertyInfo.SetValue(this.GameObject, value, null);
                }
                else if (_propertyInfo.PropertyType == typeof(Vector3))
                {
                    var startValues = StartValue.Split(',').Select(s => float.Parse(s)).ToArray();
                    var stopValues = StopValue.Split(',').Select(s => float.Parse(s)).ToArray();

                    if (startValues.Length == 3 && stopValues.Length == 3)
                    {
                        var startVec = new Vector3(startValues[0], startValues[1], startValues[2]);
                        var stopVec = new Vector3(stopValues[0], stopValues[1], stopValues[2]);

                        var value = Vector3.Lerp(startVec, stopVec, percentage);
                        _propertyInfo.SetValue(this.GameObject, value, null);
                    }
                }
            }
            catch { }

            if (percentage >= 1)
            {
                Stop();
            }
        }

        public override void Tick(float timeElapsed)
        {
            if (!_isPlaying) return;

            Step();
        }
    }
}
