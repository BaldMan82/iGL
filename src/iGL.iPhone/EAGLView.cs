using System;

using OpenTK;
using OpenTK.Graphics.ES20;
using GL1 = OpenTK.Graphics.ES11.GL;
using All1 = OpenTK.Graphics.ES11.All;
using OpenTK.Platform.iPhoneOS;

using MonoTouch.Foundation;
using MonoTouch.CoreAnimation;
using MonoTouch.ObjCRuntime;
using MonoTouch.OpenGLES;
using MonoTouch.UIKit;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;

namespace iGL.iPhone
{
	using iGL.TestGame;
	[Register ("EAGLView")]
	public class EAGLView : iPhoneOSGameView
	{
		private iOSGL _iOSGL = new iOSGL();
		private TestGame _game;
		private DateTime _lastRender = DateTime.MinValue;
		
		private float _pinchDistance = 0f;
		
		private List<UITouch> _uiTouches = new List<UITouch>();
		
		[Export("initWithCoder:")]
		public EAGLView (NSCoder coder) : base (coder)
		{
			LayerRetainsBacking = true;
			LayerColorFormat = EAGLColorFormat.RGBA8;
			
			MultipleTouchEnabled = true;
				
		}
		
		[Export ("layerClass")]
		public static new Class GetLayerClass ()
		{
			return iPhoneOSGameView.GetLayerClass ();
		}
		
		protected override void ConfigureLayer (CAEAGLLayer eaglLayer)
		{
			eaglLayer.Opaque = true;
		}
		
		protected override void CreateFrameBuffer ()
		{			
		
			ContextRenderingApi = EAGLRenderingAPI.OpenGLES2;
			base.CreateFrameBuffer ();		
			
			uint _depthRenderbuffer ;
			
			GL.GenRenderbuffers(1, out _depthRenderbuffer);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _depthRenderbuffer);
            
			GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferInternalFormat.DepthComponent16, 320, 460);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferSlot.DepthAttachment, RenderbufferTarget.Renderbuffer, _depthRenderbuffer);			
					
		}
		
		protected override void DestroyFrameBuffer ()
		{
			base.DestroyFrameBuffer ();
	
		}
		
		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);
			
			_uiTouches.AddRange(touches.ToArray<UITouch>());
		
			if (_uiTouches.Count == 1) 
			{
				var touch = touches.AnyObject as UITouch;
				var loc = touch.LocationInView(this);
				
				_game.MouseButton(iGL.Engine.Events.MouseButton.Button1, true, (int)loc.X, (int)loc.Y);
			}
			else if (_uiTouches.Count == 2) 
			{				
				
				var p1 = _uiTouches[0].LocationInView(this);
				var p2 = _uiTouches[1].LocationInView(this);
				
				var vec1 = new Vector2(p1.X, p1.Y);
				var vec2 = new Vector2(p2.X, p2.Y);
				
				var direction = vec2 - vec1;
				_pinchDistance = direction.Length;				
				
				_game.MouseButton(iGL.Engine.Events.MouseButton.Button1, false, (int)((p1.X + p2.X) / 2.0f), (int)((p1.Y + p2.Y) / 2.0f));
					
			}
		}
		
		public override void TouchesMoved (NSSet touches, UIEvent evt)
		{
			base.TouchesMoved (touches, evt);
			
			if (_uiTouches.Count == 2)
			{							
				/* pinch zoom */
				
				var p1 = _uiTouches[0].LocationInView(this);
				var p2 = _uiTouches[1].LocationInView(this);
				
				var vec1 = new Vector2(p1.X, p1.Y);
				var vec2 = new Vector2(p2.X, p2.Y);
				
				var direction = vec2 - vec1;
				var newpinchDistance = direction.Length;		
						
				var amount = _pinchDistance - newpinchDistance;
				
				_game.MouseZoom(-(int)amount*2);
				
				_pinchDistance = newpinchDistance;
			}
			else if (_uiTouches.Count == 1)
			{
				/* mouse move */
				
				var loc = _uiTouches[0].LocationInView(this);
				
				_game.MouseMove((int)loc.X, (int)loc.Y);
				
			}
		}
		
		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			base.TouchesEnded(touches, evt);
						
			if (_uiTouches.Count == 1)
			{
				var loc = _uiTouches[0].LocationInView(this);
				
				_game.MouseButton(iGL.Engine.Events.MouseButton.Button1, false, (int)loc.X, (int)loc.Y);
			}
			
			foreach( var uiTouch in touches.ToArray<UITouch>()) _uiTouches.Remove(uiTouch);
						
		}
		
		public override void TouchesCancelled (NSSet touches, UIEvent evt)
		{
			base.TouchesCancelled (touches, evt);
			
			foreach( var uiTouch in touches.ToArray<UITouch>()) _uiTouches.Remove(uiTouch);
		}
		
		#region DisplayLink support
		
		int frameInterval;
		CADisplayLink displayLink;
		
		public bool IsAnimating { get; private set; }
		
		// How many display frames must pass between each time the display link fires.
		public int FrameInterval {
			get {
				return frameInterval;
			}
			set {
				if (value <= 0)
					throw new ArgumentException ();
				frameInterval = value;
				if (IsAnimating) {
					StopAnimating ();
					StartAnimating ();
				}
			}
		}
		
		public void StartAnimating ()
		{
			if (IsAnimating)
				return;
			
			CreateFrameBuffer ();
			CADisplayLink displayLink = UIScreen.MainScreen.CreateDisplayLink (this, new Selector ("drawFrame"));
			displayLink.FrameInterval = frameInterval;
			displayLink.AddToRunLoop (NSRunLoop.Current, NSRunLoop.NSDefaultRunLoopMode);
			this.displayLink = displayLink;
			
			
			
			IsAnimating = true;
			
			_game = new TestGame(_iOSGL);
			_game.Resize(this.Size.Width, this.Size.Height);
			_game.Load();	
			
			GL.Enable(EnableCap.DepthTest);
			
			
		}
		
		public void StopAnimating ()
		{
			if (!IsAnimating)
				return;
			displayLink.Invalidate ();
			displayLink = null;
			DestroyFrameBuffer ();
			IsAnimating = false;
		}
		
		[Export ("drawFrame")]
		void DrawFrame ()
		{		
			OnRenderFrame (new FrameEventArgs ());
		}
		
		#endregion
		
		protected override void OnRenderFrame (FrameEventArgs e)
		{							
			base.OnRenderFrame (e);
			
			MakeCurrent ();
		
			if (_lastRender == DateTime.MinValue) _lastRender = DateTime.UtcNow;
			
			float ticks = (float)(DateTime.UtcNow -_lastRender).TotalSeconds;			
			
			_lastRender = DateTime.UtcNow;
			
			if (ticks > 0.02f) ticks = 0.02f;
			
			_game.Tick(ticks);
			
			_game.Render();
			
			SwapBuffers ();
			
		}
		
	}
}
