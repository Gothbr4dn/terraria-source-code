using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using ReLogic.OS;

namespace Terraria.Utilities
{
	public static class CrashDump
	{
		[Flags]
		public enum Options : uint
		{
			Normal = 0u,
			WithDataSegs = 1u,
			WithFullMemory = 2u,
			WithHandleData = 4u,
			FilterMemory = 8u,
			ScanMemory = 0x10u,
			WithUnloadedModules = 0x20u,
			WithIndirectlyReferencedMemory = 0x40u,
			FilterModulePaths = 0x80u,
			WithProcessThreadData = 0x100u,
			WithPrivateReadWriteMemory = 0x200u,
			WithoutOptionalData = 0x400u,
			WithFullMemoryInfo = 0x800u,
			WithThreadInfo = 0x1000u,
			WithCodeSegs = 0x2000u,
			WithoutAuxiliaryState = 0x4000u,
			WithFullAuxiliaryState = 0x8000u,
			WithPrivateWriteCopyMemory = 0x10000u,
			IgnoreInaccessibleMemory = 0x20000u,
			ValidTypeFlags = 0x3FFFFu
		}

		private enum ExceptionInfo
		{
			None,
			Present
		}

		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		private struct MiniDumpExceptionInformation
		{
			public uint ThreadId;

			public IntPtr ExceptionPointers;

			[MarshalAs(UnmanagedType.Bool)]
			public bool ClientPointers;
		}

		public static bool WriteException(Options options, string outputDirectory = ".")
		{
			return Write(options, ExceptionInfo.Present, outputDirectory);
		}

		public static bool Write(Options options, string outputDirectory = ".")
		{
			return Write(options, ExceptionInfo.None, outputDirectory);
		}

		private static string CreateDumpName()
		{
			DateTime dateTime = DateTime.Now.ToLocalTime();
			return string.Format("{0}_{1}_{2}_{3}.dmp", "Terraria", Main.versionNumber, dateTime.ToString("MM-dd-yy_HH-mm-ss-ffff", CultureInfo.InvariantCulture), Thread.CurrentThread.ManagedThreadId);
		}

		private static bool Write(Options options, ExceptionInfo exceptionInfo, string outputDirectory)
		{
			if (!Platform.get_IsWindows())
			{
				return false;
			}
			string path = Path.Combine(outputDirectory, CreateDumpName());
			if (!Utils.TryCreatingDirectory(outputDirectory))
			{
				return false;
			}
			using FileStream fileStream = File.Create(path);
			return Write(fileStream.SafeFileHandle, options, exceptionInfo);
		}

		private static bool Write(SafeHandle fileHandle, Options options, ExceptionInfo exceptionInfo)
		{
			if (!Platform.get_IsWindows())
			{
				return false;
			}
			Process currentProcess = Process.GetCurrentProcess();
			IntPtr handle = currentProcess.Handle;
			uint id = (uint)currentProcess.Id;
			MiniDumpExceptionInformation expParam = default(MiniDumpExceptionInformation);
			expParam.ThreadId = GetCurrentThreadId();
			expParam.ClientPointers = false;
			expParam.ExceptionPointers = IntPtr.Zero;
			if (exceptionInfo == ExceptionInfo.Present)
			{
				expParam.ExceptionPointers = Marshal.GetExceptionPointers();
			}
			bool flag = false;
			if (expParam.ExceptionPointers == IntPtr.Zero)
			{
				return MiniDumpWriteDump(handle, id, fileHandle, (uint)options, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			}
			return MiniDumpWriteDump(handle, id, fileHandle, (uint)options, ref expParam, IntPtr.Zero, IntPtr.Zero);
		}

		[DllImport("dbghelp.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		private static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeHandle hFile, uint dumpType, ref MiniDumpExceptionInformation expParam, IntPtr userStreamParam, IntPtr callbackParam);

		[DllImport("dbghelp.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		private static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeHandle hFile, uint dumpType, IntPtr expParam, IntPtr userStreamParam, IntPtr callbackParam);

		[DllImport("kernel32.dll", ExactSpelling = true)]
		private static extern uint GetCurrentThreadId();
	}
}
