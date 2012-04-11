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

namespace iGL.iPhone
{
	using iGL.TestGame;
	[Register ("EAGLView")]
	public class EAGLView : iPhoneOSGameView
	{
		private iOSGL _iOSGL = new iOSGL();
		private TestGame _game;
		private DateTime _lastRender = DateTime.MinValue;
		
		[Export("initWithCoder:")]
		public EAGLView (NSCoder coder) : base (coder)
		{
			LayerRetainsBacking = true;
			LayerColorFormat = EAGLColorFormat.RGBA8;
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
			
			/* tick at constant interval for debugging purpose */
			_game.Tick(1f / 100f);
						
			_game.Render();
			
			SwapBuffers ();
			
			_lastRender = DateTime.UtcNow;
		}
		
	}
}
