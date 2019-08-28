namespace Secs4Net
{
	/// <summary>
	/// <para xml:lang="en">The SECS item format code (Bit: 876543).</para>
	/// </summary>
	public enum SecsFormat : byte
	{
		/// <summary>
		/// <para xml:lang="en">A list of items. (Format Code: Octal: 00 | Binary: 000 000)</para>
		/// </summary>
		List = 0b_000_000_00,

		/// <summary>
		/// <para xml:lang="en">Binary values (<see cref="byte"/>). (Format Code: Octal: 10 | Binary: 001 000)</para>
		/// </summary>
		Binary = 0b_001_000_00,

		/// <summary>
		/// <para xml:lang="en">Boolean values (<see cref="bool"/>). (Format Code: Octal: 11 | Binary: 001 001)</para>
		/// </summary>
		Boolean = 0b_001_001_00,

		/// <summary>
		/// <para xml:lang="en">ASCII characters. (Format Code: Octal: 20 | Binary: 010 000)</para>
		/// </summary>
		/// <remarks><para xml:lang="en">Non-printing characters are equipment-specific.</para></remarks>
		ASCII = 0b_010_000_00,

		/// <summary>
		/// <para xml:lang="en">JIS-8 characters. (Format Code: Octal: 21 | Binary: 010 001)</para>
		/// </summary>
		JIS8 = 0b_010_001_00,

		///// <summary>
		///// <para xml:lang="en">2 byte character. (Format Code: Octal: 22 | Binary: 010 010)</para>
		///// </summary>
		///// <remarks><para xml:lang="en">Most significant byte sent first. The code for Multi-byte character must be specified in the data in the first 2 bytes of the TEXT item.</para></remarks>
		//Unicode = 0b_010_010_00,

		/// <summary>
		/// <para xml:lang="en">8 byte signed integer (<see cref="long"/>). (Format Code: Octal: 30 | Binary: 011 000)</para>
		/// </summary>
		/// <remarks><para xml:lang="en">Most significant byte sent first.</para></remarks>
		I8 = 0b_011_000_00,

		/// <summary>
		/// <para xml:lang="en">1 byte signed integer (<see cref="sbyte"/>). (Format Code: Octal: 31 | Binary: 011 001)</para>
		/// </summary>
		I1 = 0b_011_001_00,

		/// <summary>
		/// <para xml:lang="en">2 byte signed integer (<see cref="short"/>). (Format Code: Octal: 32 | Binary: 011 010)</para>
		/// </summary>
		/// <remarks><para xml:lang="en">Most significant byte sent first.</para></remarks>
		I2 = 0b_011_010_00,

		/// <summary>
		/// <para xml:lang="en">4 byte signed integer (<see cref="int"/>). (Format Code: Octal: 33 | Binary: 011 100)</para>
		/// </summary>
		/// <remarks><para xml:lang="en">Most significant byte sent first.</para></remarks>
		I4 = 0b_011_100_00,

		/// <summary>
		/// <para xml:lang="en">8 byte floating point (<see cref="double"/>). (Format Code: Octal: 40 | Binary: 100 000)</para>
		/// </summary>
		/// <remarks><para xml:lang="en">IEEE 754. The byte containing the sign bit is sent first.</para></remarks>
		F8 = 0b_100_000_00,

		/// <summary>
		/// <para xml:lang="en">4 byte floating point (<see cref="float"/>). (Format Code: Octal: 41 | Binary: 100 100)</para>
		/// </summary>
		/// <remarks><para xml:lang="en">IEEE 754. The byte containing the sign bit is sent first.</para></remarks>
		F4 = 0b_100_100_00,

		/// <summary>
		/// <para xml:lang="en">8 byte unsigned integer (<see cref="ulong"/>). (Format Code: Octal: 50 | Binary: 101 000)</para>
		/// </summary>
		/// <remarks><para xml:lang="en">Most significant byte sent first.</para></remarks>
		U8 = 0b_101_000_00,

		/// <summary>
		/// <para xml:lang="en">1 byte unsigned integer (<see cref="byte"/>). (Format Code: Octal: 51 | Binary: 101 001)</para>
		/// </summary>
		U1 = 0b_101_001_00,

		/// <summary>
		/// <para xml:lang="en">2 byte unsigned integer (<see cref="ushort"/>). (Format Code: Octal: 52 | Binary: 101 010)</para>
		/// </summary>
		/// <remarks><para xml:lang="en">Most significant byte sent first.</para></remarks>
		U2 = 0b_101_010_00,

		/// <summary>
		/// <para xml:lang="en">4 byte unsigned integer (<see cref="uint"/>). (Format Code: Octal: 53 | Binary: 101 100)</para>
		/// </summary>
		/// <remarks><para xml:lang="en">Most significant byte sent first.</para></remarks>
		U4 = 0b_101_100_00
	}
}