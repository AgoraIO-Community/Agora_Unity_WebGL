using System;
using System.Runtime.InteropServices;
using AOT;

#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX 
namespace agora_gaming_rtc
{
	/** The IScreenCaptureManager class, which manages the sources for screen sharing. */
	public abstract class IScreenCaptureManager : IRtcEngineNative
	{ 

		public abstract void CreateScreenCaptureManager(int thumbHeight, int thumbWidth, int iconHeight, int iconWidth, bool includeScreen);

	 	public abstract int GetScreenCaptureSourcesCount();

		public abstract ScreenCaptureSourceType GetScreenCaptureSourceType(uint index);

        public abstract string GetScreenCaptureSourceName(uint index);

        public abstract string GetScreenCaptureSourceProcessPath(uint index);

        public abstract string GetScreenCaptureSourceTitle(uint index);

        public abstract IntPtr GetScreenCaptureSourceId(uint index);

		public abstract bool GetScreenCaptureIsPrimaryMonitor(uint index);

		public abstract ThumbImageBuffer GetScreenCaptureThumbImage(uint index);

		public abstract ThumbImageBuffer GetScreenCaptureIconImage(uint index);
	}

    /** The definition of ScreenCaptureManager. */
	public sealed class ScreenCaptureManager : IScreenCaptureManager
    {
		
		private IRtcEngine mEngine = null;
		private static ScreenCaptureManager _screenCaptureManagerInstance;

		private ScreenCaptureManager(IRtcEngine rtcEngine)
		{
			mEngine = rtcEngine;
		}

		~ScreenCaptureManager() 
		{

		}

		public static ScreenCaptureManager GetInstance(IRtcEngine rtcEngine)
		{
			if (_screenCaptureManagerInstance == null)
			{
				_screenCaptureManagerInstance = new ScreenCaptureManager(rtcEngine);
			}
			return _screenCaptureManagerInstance;
		}

     	public static void ReleaseInstance()
		{
			_screenCaptureManagerInstance = null;
		}

		// used internally
		public void SetEngine (IRtcEngine engine)
		{
			mEngine = engine;
		}
		/** Creates a ScreenCaptureManager class, which includes a list of shareable screens and windows.
		*
		* You can call this method before sharing a screen or window to get a list of shareable screens and windows, 
		* which enables a user to use thumbnails in the list to easily choose a particular screen or window to share. 
		* 
		* This list also contains important information such as window ID and screen ID, with which you can call
		* `StartScreenCaptureByWindowId` or `StartScreenCaptureByDisplayId` to start the sharing.
		* 
		* @note This method applies to macOS and Windows only.
		* 
		* @param thumbHeight The target height of the screen or window thumbnail in pixels. 
		* @param thumbWidth The target width of the screen or window thumbnail in pixels.
		* @param iconHeight The target height of the app icon in pixels.
		* @param iconWidth The target width of the app icon in pixels.
		* @param includeScreen Whether the SDK returns screen information in addition to window information:
		* - true: Yes. The SDK returns both the screen and window information.
		* - false: No. The SDK returns the window information only.
		*/
		public override void CreateScreenCaptureManager(int thumbHeight, int thumbWidth, int iconHeight, int iconWidth, bool includeScreen)
		{
			if (mEngine == null) return;

			IRtcEngineNative.getScreenCaptureSources(thumbHeight, thumbWidth, iconHeight, iconWidth, includeScreen);
		}
        /**
		* Gets the number of the shareable screens or windows.
		* @return The number of shareable screens or windows.
		*/
        public override int GetScreenCaptureSourcesCount()
        {
            if (mEngine == null)
				return -1;

			return IRtcEngineNative.getScreenCaptureSourcesCount();
        }
        /**
		* Gets the type of the shared target. 
		* @param index The index number of the shared target.
		* @return The screen capture source type: ScreenCaptureSourceType.
		*/
		public override ScreenCaptureSourceType GetScreenCaptureSourceType(uint index)
		{
			if (mEngine == null)
				return ScreenCaptureSourceType.ScreenCaptureSourceType_Unknown;
			if (index >= 0 && index < GetScreenCaptureSourcesCount())
			{
				return (ScreenCaptureSourceType) IRtcEngineNative.getScreenCaptureSourceType(index);
			}
			return ScreenCaptureSourceType.ScreenCaptureSourceType_Unknown;
		}
        /**
		* Gets the name of the window or screen.
		* @param index The index number of the shared window or screen.
		* @return The name of the shared window or screen.
		*/
        public override string GetScreenCaptureSourceName(uint index)
		{
			if (mEngine == null)
				return "Engine is null";
			if (index >= 0 && index < GetScreenCaptureSourcesCount())
			{
				return Marshal.PtrToStringAnsi(IRtcEngineNative.getScreenCaptureSourceName(index));
			}
			return "Invalid Argument";
		}
        /**
		* Gets the process to which the window or screen belongs.
		* @param index The index number of the shared window or screen.
		* @return The process to which the window or screen belongs.
		*/
        public override string GetScreenCaptureSourceProcessPath(uint index)
		{
			if (mEngine == null)
				return "Engine is null";
			if (index >= 0 && index < GetScreenCaptureSourcesCount())
			{
				return Marshal.PtrToStringAnsi(IRtcEngineNative.getScreenCaptureSourceProcessPath(index));
			}
			return "Invalid Argument";
		}
        /**
		* Gets the title of the window or screen.
		* @param index The index number of the shared window or screen.
		* @return The title of the window or screen.
		*/
        public override string GetScreenCaptureSourceTitle(uint index)
		{
			if (mEngine == null)
				return "Engine is null";
			if (index >= 0 && index < GetScreenCaptureSourcesCount())
			{
				return Marshal.PtrToStringAnsi(IRtcEngineNative.getScreenCaptureSourceTitle(index));
			}
			return "Invalid Argument";
		}
        /**
		* Gets the source ID of the window or screen.
		* @param index The index number of the shared window or screen.
		* @return The source ID of the window or screen.
		*/
        public override IntPtr GetScreenCaptureSourceId(uint index)
		{
			if (mEngine == null)
				return IntPtr.Zero;
			if (index >= 0 && index < GetScreenCaptureSourcesCount())
			{
				return IRtcEngineNative.getScreenCaptureSourceId(index);
			}
			return IntPtr.Zero;
		}
        /**
		* Checks whether the screen is the primary display.
		* @param index The index number of the shared screen.
		* @return Whether the screen or window is the primary display:
		* - true: Yes.
		* - false: No.
		*/
		public override bool GetScreenCaptureIsPrimaryMonitor(uint index)
		{
			if (mEngine == null)
				return false;
			if (index >= 0 && index < GetScreenCaptureSourcesCount())
			{
				return IRtcEngineNative.getScreenCaptureIsPrimaryMonitor(index);
			}
			return false;
		}
        /** Gets the image content of the thumbnail.
		* @param index The index number of the shared window or screen.
		* @return The image buffer of the thumbnail: ThumbImageBuffer.
		*/
		public override ThumbImageBuffer GetScreenCaptureThumbImage(uint index)
		{
			if (mEngine == null)
				return new ThumbImageBuffer();
			if (index >= 0 && index < GetScreenCaptureSourcesCount())
			{
				ThumbImageBuffer buffer = new ThumbImageBuffer();
				IRtcEngineNative.getScreenCaptureThumbImage(index, ref buffer);
				return buffer;
			}
			return new ThumbImageBuffer();
		}
        /** Gets the image content of the icon.
		* @param index The index number of the shared window or screen.
		* @return The image buffer of the icon: ThumbImageBuffer.
		*/
		public override ThumbImageBuffer GetScreenCaptureIconImage(uint index)
		{
			if (mEngine == null)
				return new ThumbImageBuffer();
			if (index >= 0 && index < GetScreenCaptureSourcesCount())
			{
				ThumbImageBuffer buffer = new ThumbImageBuffer();
				IRtcEngineNative.getScreenCaptureIconImage(index, ref buffer);
				return buffer;
			}
			return new ThumbImageBuffer();
		}
	}
}
#endif