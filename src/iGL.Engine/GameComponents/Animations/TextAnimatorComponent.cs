using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace iGL.Engine
{
    public class TextAnimatorComponent : AnimationComponent
    {
        public string Text { get; set; }
        public float CharacterInterval { get; set; }
        public float LineDelay { get; set; }

        private TextComponent _textComponent;       
        private int _charIndex;
        private DateTime _tickTime;

        public TextAnimatorComponent(XElement xmlElement) : base(xmlElement) { }

        public TextAnimatorComponent() : base() { }

        protected override void Init()
        {
            base.Init();

            Text = "Booting kernel...";
            CharacterInterval = 0.5f;

        }

        public override void Play()
        {            
            _tickTime = DateTime.UtcNow;
            _charIndex = 0;
            _textComponent.Text = string.Empty;
            _textComponent.Reload();

            base.Play();
        }

        public override void Stop()
        {           
            base.Stop();
        }

        public override void Pause()
        {
            base.Pause();
        }

        public override bool InternalLoad()
        {
            _textComponent = GameObject.Components.FirstOrDefault(c => c is TextComponent) as TextComponent;
            if (_textComponent == null) return false;

            base.InternalLoad();

            return true;
        }

        private void NextCharacter()
        {
            if (AnimationState != State.Playing) return;

            var secondsPassed = (DateTime.UtcNow - _tickTime).TotalSeconds;
            if (secondsPassed > CharacterInterval)
            {
                if (_charIndex < Text.Length)
                {
                    var c = Text[_charIndex];

                    if (c != '\n')
                    {
                        if (c != '\r')
                        {
                            _textComponent.Text += c;
                            _textComponent.Reload();
                        }
                        _charIndex++;

                        _tickTime = DateTime.UtcNow;
                    }
                    else
                    {

                        if (secondsPassed > LineDelay)
                        {
                            _charIndex++;
                            _textComponent.Text = string.Empty;

                            _tickTime = DateTime.UtcNow;
                        }
                    }
                }
                else
                {
                    if (PlayMode == AnimationComponent.Mode.Repeat)
                    {
                        Rewind();
                    }
                    else
                    {
                        Stop();
                    }

                }
            }
        }

        public override void Tick(float timeElapsed)
        {           
            NextCharacter();

            base.Tick(timeElapsed);
        }
    }
}
