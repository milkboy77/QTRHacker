using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum EHClauseType : uint
	{
		EHFault,
		EHFinally,
		EHFilter,
		EHTyped,
		EHUnknown,
	}
}