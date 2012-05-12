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
        private bool _isPlaying;
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
            if (!IsLoaded) return;

            _isPlaying = true;
            _tickTime = DateTime.UtcNow;
            _textComponent.Text = string.Empty;

            base.Play();
        }

        public override void Stop()
        {
            _isPlaying = false;

            base.Stop();
        }

        public override void Pause()
        {
            base.Pause();
        }

        public override bool InternalLoad()
        {
            _textComponent = GameObject.Components.FirstOrDefault(c => c is TextComponent) as TextComponent;

            /* loaded if textcomponent is found */
            return _textComponent != null;
        }

        private void NextCharacter()
        {
            var secondsPassed = (DateTime.UtcNow - _tickTime).TotalSeconds;
            if (secondsPassed > CharacterInterval)
            {
                if (_charIndex < Text.Length)
                {
                    var c = Text[_charIndex];

                    if (c != '\n')
                    {
                        _textComponent.Text += c;
                        _textComponent.Reload();
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
                    Stop();
                }
            }
        }

        public override void Tick(float timeElapsed)
        {
            if (!_isPlaying) return;

            NextCharacter();
        }
    }
}
