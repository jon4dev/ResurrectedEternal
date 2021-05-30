// Copyright (C) 2013-2015 aevitas
// See the file LICENSE for copying permission.

using System;

// These custom exception types are provided to enable framework implementers to catch operations specific to RedRain.
// When writing extensions, you should always try to throw these whenever possible.

namespace RedRain.Common
{
	/// <summary>
	///     Base exception type thrown by RedRain.
	/// </summary>
	public class RedRainException : Exception
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="RedRainException" /> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public RedRainException(string message)
			: base(message)
		{
		}
	}

	/// <summary>
	///     Exception thrown when a reading operation fails.
	/// </summary>
	public class RedRainReadException : RedRainException
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="RedRainReadException" /> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public RedRainReadException(string message)
			: base(message)
		{
		}


		/// <summary>
		///     Initializes a new instance of the <see cref="RedRainReadException" /> class.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="count">The count.</param>
		public RedRainReadException(IntPtr address, int count)
			: this(string.Format("ReadProcessMemory failed! Could not read {0} bytes from {1}!", count, address.ToString("X"))
				)
		{
		}
	}

	/// <summary>
	///     Exception thrown when a writing operation fails.
	/// </summary>
	public class RedRainWriteException : RedRainException
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="RedRainWriteException" /> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public RedRainWriteException(string message)
			: base(message)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="RedRainWriteException" /> class.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="count">The count.</param>
		public RedRainWriteException(IntPtr address, int count)
			: this(string.Format("WriteProcessMemory failed! Could not write {0} bytes at {1}!", count, address.ToString("X")))
		{
		}
	}

	/// <summary>
	///     Exception thrown when an operation related to injection fails.
	/// </summary>
	public class RedRainInjectionException : RedRainException
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="RedRainInjectionException" /> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public RedRainInjectionException(string message) : base(message)
		{
		}
	}
}