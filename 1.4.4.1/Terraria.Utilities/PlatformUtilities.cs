using System;
using System.IO;
using System.Runtime.InteropServices;
using SDL2;

namespace Terraria.Utilities
{
	public static class PlatformUtilities
	{
		private unsafe static IntPtr INTERNAL_getScaledSurface(byte[] data, int srcW, int srcH, int dstW, int dstH)
		{
			IntPtr intPtr = SDL.SDL_CreateRGBSurface(0u, srcW, srcH, 32, 255u, 65280u, 16711680u, 4278190080u);
			SDL.SDL_LockSurface(intPtr);
			SDL.SDL_Surface* ptr = (SDL.SDL_Surface*)(void*)intPtr;
			Marshal.Copy(data, 0, ptr->pixels, srcW * srcH * 4);
			SDL.SDL_UnlockSurface(intPtr);
			if (srcW != dstW || srcH != dstH)
			{
				IntPtr intPtr2 = SDL.SDL_CreateRGBSurface(0u, dstW, dstH, 32, 255u, 65280u, 16711680u, 4278190080u);
				SDL.SDL_SetSurfaceBlendMode(intPtr, SDL.SDL_BlendMode.SDL_BLENDMODE_NONE);
				SDL.SDL_BlitScaled(intPtr, IntPtr.Zero, intPtr2, IntPtr.Zero);
				SDL.SDL_FreeSurface(intPtr);
				intPtr = intPtr2;
			}
			return intPtr;
		}

		public static void SavePng(Stream stream, int width, int height, int imgWidth, int imgHeight, byte[] data)
		{
			IntPtr surface = INTERNAL_getScaledSurface(data, width, height, imgWidth, imgHeight);
			byte[] array = new byte[width * height * 4 + 41 + 57 + 256];
			IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
			IntPtr dst = SDL.SDL_RWFromMem(intPtr, array.Length);
			SDL_image.IMG_SavePNG_RW(surface, dst, 1);
			SDL.SDL_FreeSurface(surface);
			Marshal.Copy(intPtr, array, 0, array.Length);
			Marshal.FreeHGlobal(intPtr);
			_ = array[33];
			_ = array[34];
			_ = array[35];
			_ = array[36];
			stream.Write(array, 0, array.Length);
		}
	}
}
