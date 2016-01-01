﻿using System;
using VLCLibraryImport;
using System.Runtime.InteropServices;
using System.Threading;

namespace VLCLibrary
{

	public class INewFrameEventArgs : EventArgs
	{
		public INewFrameEventArgs (Gdk.Pixbuf frame)
		{
			this.frame = frame;
		}
		public Gdk.Pixbuf frame=null;
	}

	public delegate void NewFrameEventHandler(object sender, INewFrameEventArgs e);

	public class VLCMediaPlayer : VLCBase
	{
		public  event NewFrameEventHandler NewFrameEvent=null;

		private LibVLC _core=null;
		private VLCMedia _media=null;

		private IntPtr _instance;
	
		private VLCVideoBuffer videoBuffer=null;

		private uint _width = 1280;
		private uint _height= 720;
		private Gtk.Image drawObject;
		private Gdk.Pixbuf buffer;
		GCHandle _this;

		public IntPtr Handler {
			get { return _instance;  } 		
			set {} 
		}


		public uint width {
			get { return _width;  } 		
			set { _width = value;  } 
		}

		public uint height {
			get { return _height;  } 		
			set { _height = value; } 
		}

		public VLCMediaPlayer (LibVLC core,VLCMedia media)
		{
			_this = GCHandle.Alloc(this);

			_core = core;
			_media = media;
	
			_instance = NativeVLC.libvlc_media_player_new_from_media(_media.Handler);
	
			NativeVLC.libvlc_video_set_callbacks (_instance,new NativeVLC.Lock_Callback(LockCalback),new NativeVLC.Unlock_Callback(UnlockCalback),new NativeVLC.Display_Callback(DisplayCalback),(IntPtr)_this); 

			drawObject = new Gtk.Image ();
		}

		~VLCMediaPlayer ()
		{
			Console.WriteLine ("DESTRUCTOR VLCMediaPlayer ");
		

		}


		protected override void Dispose(bool disposing)
		{
			Console.WriteLine ("DISPOSE2 ???? "+(disposing?"yes":"no"));

			if (disposing) {
				_core.Dispose ();
				_media.Dispose ();
			}

			if (_instance == IntPtr.Zero) return;
			//@TODO native instance remove
			_instance = IntPtr.Zero;
		}

		public void SetDrawable(Gtk.Image obj)
		{
			drawObject = obj;
		}

		private void CreateBuffer(uint width,uint height,uint bytes)
		{
			Console.WriteLine ("CREATE BUFFER");
			videoBuffer = new VLCVideoBuffer(width, height,bytes);

			NativeVLC.libvlc_video_set_format(_instance, "RGBA", width, height, bytes * width);
			Console.WriteLine ("CREATE BUFFER DONE");

		}

		public  int setVideoFormat(ref IntPtr opaque, string chroma, ref UInt32 width, ref UInt32 height, ref UInt32 pitches,ref UInt32 lines)
		{
			return 0;
		}


		public  void videoFormatClean(ref IntPtr opaque) {

		}

		public static IntPtr LockCalback( IntPtr opaque, ref IntPtr planes)
		{
			GCHandle handle2 = (GCHandle) opaque;
			VLCMediaPlayer _this = (handle2.Target as VLCMediaPlayer);

			Console.WriteLine ("Lock "+_this.GetType());
		
			planes = _this.videoBuffer.Lock ();
		
			return IntPtr.Zero;
		}

		public static void UnlockCalback( IntPtr opaque,ref IntPtr picture, ref IntPtr planes)
		{
			GCHandle handle2 = (GCHandle) opaque;
			VLCMediaPlayer _this = (handle2.Target as VLCMediaPlayer);

			Console.WriteLine ("Unlock "+_this.GetType());
			_this.videoBuffer.Unlock ();

		}

		public void NewFrame()
		{
			Gtk.Application.Invoke (delegate {

				if (NewFrameEvent != null) {
					Gdk.Pixbuf frame = new Gdk.Pixbuf (videoBuffer.FrameBuffer, Gdk.Colorspace.Rgb, true, 8, (int)videoBuffer.Width, (int)videoBuffer.Height,(int) videoBuffer.Stride);

					NewFrameEvent (this, new INewFrameEventArgs (frame));

					frame.Dispose();
					frame = null;
				}
			});
		}

		public static  void DisplayCalback( IntPtr opaque, ref IntPtr picture)
		{
			Console.WriteLine (">--------------------");
				
				
				GCHandle handle2 = (GCHandle) opaque;
				VLCMediaPlayer _this = (handle2.Target as VLCMediaPlayer);
				Console.WriteLine ("DISPLAY "+_this.GetType());
				_this.NewFrame();
				Console.WriteLine ("System.GC.Collect()");

				System.GC.Collect();
			
			Console.WriteLine (">--------------------");

		}

		public void Play()
		{
			CreateBuffer (_width,_height,4);
			 
			NativeVLC.libvlc_media_player_play(_instance);

	
		}
	}
}

