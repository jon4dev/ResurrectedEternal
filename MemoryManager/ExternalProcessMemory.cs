// Copyright (C) 2013-2015 aevitas
// See the file LICENSE for copying permission.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using RedRain.Common;

namespace RedRain
{
    /// <summary>
    ///     Provides functionality for reading from, writing to, and allocating memory in external processes.
    ///     This class relies on a handle to the remote process and to its main thread to perform memory reading and writing.
    /// </summary>
    public sealed class ExternalProcessMemory : NativeMemory
    {
        public static event Action OnProcessInvalid;
        //private readonly SafeMemoryHandle _mainThreadHandle;
        internal SafeMemoryHandle ProcessHandle;

        private System.Threading.Timer _recreateHandle;

        private int _numCount = 0;

        private bool IsProcesStillActive()
        {
            return System.Diagnostics.Process.GetProcessesByName("csgo").Length > 0;
        }

        private void OnRecreate(object o)
        {
            //_numCount++;
            //if(_numCount > 10)
            //         {

            //check if process is still valid.
            if (!IsProcesStillActive())
            {
                //Console.WriteLine("Ayylmao");
                OnProcessInvalid?.Invoke();
                _recreateHandle.Dispose();
                ProcessHandle.Dispose();
                Environment.Exit(0);
                //this.Dispose();
                return;
                //
            }
            _numCount++;
            //do we even have to do that.
            if(_numCount > 100)
            {
                var _copy = ProcessHandle;
                ProcessHandle = OpenProcess(ProcessAccess.CreateThread | ProcessAccess.QueryInformation | ProcessAccess.VMRead |
                ProcessAccess.VMWrite | ProcessAccess.VMOperation | ProcessAccess.SetInformation, false, _processId);
                _copy.Dispose();
                _numCount = 0;
            }

            //if (ProcessHandle.IsInvalid)
            //{
                
            //    _recreateHandle.Dispose();
            //    this.Dispose();
            //    //	}
            //    //}

            //}
        }


        private int _processId;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExternalProcessMemory" /> class.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <param name="access">The access flags.</param>
        /// <param name="createInjector">if set to <c>true</c> creates an injector for module loading support.</param>
        /// <exception cref="PlatformNotSupportedException">
        ///     The platform is Windows 98 or Windows Millennium Edition (Windows Me);
        ///     set the <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> property to false to access this
        ///     property on Windows 98 and Windows Me.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The process's <see cref="P:System.Diagnostics.Process.Id" /> property has
        ///     not been set.-or- There is no process associated with this <see cref="T:System.Diagnostics.Process" /> object.
        /// </exception>
        public ExternalProcessMemory(Process process,
            ProcessAccess access, bool createInjector = false) : base(process, createInjector)
        {
            ProcessHandle = OpenProcess(access, false, process.Id);
            _processId = process.Id;
            process.Dispose();
            _recreateHandle = new System.Threading.Timer(OnRecreate, null, 0, 100);
            // Obtain a handle to the process' main thread so we can suspend/resume it whenever we need to.0
            //_mainThreadHandle = OpenThread(ThreadAccess.ALL, false, (uint) process.Threads[0].Id);

            //Process.EnterDebugMode();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExternalProcessMemory" /> class.
        ///     This constructor opens a handle to the specified process using the following flags:
        ///     ProcessAccess.CreateThread | ProcessAccess.QueryInformation | ProcessAccess.VMRead | ProcessAccess.VMWrite |
        ///     ProcessAccess.VMOperation | ProcessAccess.SetInformation
        /// </summary>
        /// <param name="process">The process.</param>
        /// <param name="createInjector">if set to <c>true</c> creates an injector for module loading support.</param>
        /// <exception cref="PlatformNotSupportedException">
        ///     The platform is Windows 98 or Windows Millennium Edition (Windows Me);
        ///     set the <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> property to false to access this
        ///     property on Windows 98 and Windows Me.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The process's <see cref="P:System.Diagnostics.Process.Id" /> property has
        ///     not been set.-or- There is no process associated with this <see cref="T:System.Diagnostics.Process" /> object.
        /// </exception>
        public ExternalProcessMemory(Process process, bool createInjector = false) : this(process,
            ProcessAccess.CreateThread | ProcessAccess.QueryInformation | ProcessAccess.VMRead |
            ProcessAccess.VMWrite | ProcessAccess.VMOperation | ProcessAccess.SetInformation, createInjector)
        {
        }

        // VirtualAllocEx specifies size as a size_t - we'll use the .NET equivalent.
        private IntPtr AllocateMemory(UIntPtr size)
        {
            return VirtualAllocEx(ProcessHandle, IntPtr.Zero, size, AllocationType.Commit, MemoryProtection.ExecuteReadWrite);
        }

        /// <summary>
        ///     Allocates a chunk of memory of the specified size in the remote process.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        /// <exception cref="RedRainException"></exception>
        public override AllocatedMemory Allocate(UIntPtr size)
        {
            var chunk = AllocateMemory(size);

            if (chunk != IntPtr.Zero)
            {
                return new AllocatedMemory(chunk, size.ToUInt32(), this);
            }

            //CHelper.Log(new RedRainException($"Couldn't allocate {size.ToUInt32()} sized chunk!").ToString(), "Error.log");
            return new AllocatedMemory(IntPtr.Zero, 0, this);
        }

        #region Implementation of IDisposable

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            if (IsDisposed)
                return;

            ProcessHandle?.Dispose();
            //_mainThreadHandle?.Dispose();

            Process.LeaveDebugMode();

            base.Dispose();
        }

        #endregion

        #region P/Invokes
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetExitCodeProcess(IntPtr hProcess, out uint ExitCode);

        [DllImport("kernel32", EntryPoint = "OpenThread", SetLastError = true)]
        public static extern IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll")]
        private static extern SafeMemoryHandle OpenProcess(
            ProcessAccess dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern unsafe bool ReadProcessMemory(
            SafeMemoryHandle hProcess, IntPtr lpBaseAddress, byte* lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(
            SafeMemoryHandle hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        private static extern bool VirtualProtectEx(
            SafeMemoryHandle hProcess, IntPtr lpAddress, IntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr VirtualAllocEx(SafeMemoryHandle hProcess, IntPtr lpAddress,
            UIntPtr dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        #endregion

        #region Overrides of NativeMemory

        /// <summary>
        ///     Reads the specified amount of bytes from the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="count">The count.</param>
        /// <param name="isRelative">if set to <c>true</c> [is relative].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Address may not be zero, and count may not be zero.</exception>
        /// <exception cref="RedRainReadException">
        ///     Thrown if the ReadProcessMemory operation fails, or doesn't return the
        ///     specified amount of bytes.
        /// </exception>
        public override unsafe byte[] ReadBytes(IntPtr address, int count, bool isRelative = false)
        {
            Requires.NotEqual(count, 0, nameof(count));
            Requires.NotEqual(address, IntPtr.Zero, nameof(address));

            if (isRelative)
                address = ToAbsolute(address);

            // We should consider not throwing if the amount of bytes read doesn't match the expected amount of bytes.
            // It is perfectly possible for user code to handle these scenarios - we just have to think of a proper way of notifying them.

            var buffer = new byte[count];
            fixed (byte* b = buffer)
            {
                int numRead;
                if (ReadProcessMemory(ProcessHandle, address, b, count, out numRead) && numRead == count)
                    return buffer;
            }
            //CHelper.Log(new RedRainReadException(address, count).ToString(), "Error.log");
            return new byte[0];

        }

        /// <summary>
        ///     Writes the specified bytes at the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="isRelative">if set to <c>true</c> [is relative].</param>
        /// <returns></returns>
        /// <exception cref="RedRain.Common.RedRainWriteException"></exception>
        /// <exception cref="OverflowException">
        ///     The array is multidimensional and contains more than
        ///     <see cref="F:System.Int32.MaxValue" /> elements.
        /// </exception>
        public override void WriteBytes(IntPtr address, byte[] bytes, bool isRelative = false)
        {
            Requires.NotEqual(address, IntPtr.Zero, nameof(address));
            Requires.NotEqual(bytes.Length, 0, nameof(bytes));

            uint oldProtect;
            int numWritten;

            // dwSize is a size_t, meaning it *may* differ depending architecture. Though very unlikely to cause trouble if 
            // defined as uint, we're passing it as IntPtr here as it is the closest .NET equivalent.
            //MemoryProtection.
            VirtualProtectEx(ProcessHandle, address, (IntPtr)bytes.Length, (uint)MemoryProtection.ExecuteReadWrite,
                out oldProtect);
            WriteProcessMemory(ProcessHandle, address, bytes, bytes.Length, out numWritten);
            VirtualProtectEx(ProcessHandle, address, (IntPtr)bytes.Length, oldProtect, out oldProtect);

            // All we need to check - if WriteProcessMemory fails numWriten will be 0.
            var success = numWritten == bytes.Length;
            //if (!success)
            //             CHelper.Log(new RedRainWriteException(address, bytes.Length).ToString(), "Error.log");
        }

        /// <summary>
        ///     Reads a value of the specified type at the specified address.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address">The address.</param>
        /// <param name="isRelative">if set to <c>true</c> [is relative].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Address may not be zero, and count may not be zero.</exception>
        /// <exception cref="RedRainReadException">
        ///     Thrown if the ReadProcessMemory operation fails, or doesn't return the
        ///     specified amount of bytes.
        /// </exception>
        /// <exception cref="MissingMethodException">
        ///     The class specified by <paramref name="T" /> does not have an accessible
        ///     default constructor.
        /// </exception>
        public override unsafe T Read<T>(IntPtr address, bool isRelative = false)
        {
            Requires.NotEqual(address, IntPtr.Zero, nameof(address));

            var size = MarshalCache<T>.Size;
            // Unsafe context doesn't allow for the use of await - run the read synchronously.
            var buffer = ReadBytes(address, size, isRelative);
            if (buffer.Length == 0)
                return default(T);
            try
            {
                fixed (byte* b = buffer)
                    return Marshal.PtrToStructure<T>((IntPtr)b);
            }
            catch
            {
                return default(T); //Marshal.PtrToStructure<T>(IntPtr.Zero);
            }

        }

        /// <summary>
        ///     Reads the specified amount of values of the specified type at the specified address.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address">The address.</param>
        /// <param name="count">The count.</param>
        /// <param name="isRelative">if set to <c>true</c> [is relative].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Address may not be zero, and count may not be zero.</exception>
        /// <exception cref="RedRainReadException">
        ///     Thrown if the ReadProcessMemory operation fails, or doesn't return the
        ///     specified amount of bytes.
        /// </exception>
        /// <exception cref="MissingMethodException">
        ///     The class specified by <paramref name="T" /> does not have an accessible
        ///     default constructor.
        /// </exception>
        public override T[] Read<T>(IntPtr address, int count, bool isRelative = false)
        {
            Requires.NotEqual(address, IntPtr.Zero, nameof(address));

            var size = MarshalCache<T>.Size;

            var ret = new T[count];

            // Read = add + n * size
            for (var i = 0; i < count; i++)
                ret[i] = Read<T>(address + (i * size), isRelative);

            return ret;
        }

        /// <summary>
        ///     Writes the specified value at the specfied address.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address">The address.</param>
        /// <param name="value">The value.</param>
        /// <param name="isRelative">if set to <c>true</c> [is relative].</param>
        /// <returns></returns>
        /// <exception cref="OverflowException">
        ///     The array is multidimensional and contains more than
        ///     <see cref="F:System.Int32.MaxValue" /> elements.
        /// </exception>
        /// <exception cref="RedRainWriteException">WriteProcessMemory failed.</exception>
        /// <exception cref="ArgumentException"><paramref name="T" /> is a reference type that is not a formatted class. </exception>
        public override unsafe void Write<T>(IntPtr address, T value, bool isRelative = false)
        {
            Requires.NotEqual(address, IntPtr.Zero, nameof(address));

            // TODO: Optimize this method to take marshalling requirements into account

            var size = MarshalCache<T>.Size;
            var buffer = new byte[size];
            //Console.WriteLine(size);
            fixed (byte* b = buffer)
                Marshal.StructureToPtr(value, (IntPtr)b, true);

            WriteBytes(address, buffer, isRelative);
        }

        #endregion
    }
    [Flags]
    public enum ScanFlags
    {
        NONE,
        READ,
        SUBSTRACT_BASE,
    }
}