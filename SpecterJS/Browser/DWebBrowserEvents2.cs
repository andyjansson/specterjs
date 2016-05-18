using System;
using System.Runtime.InteropServices;

namespace SpecterJS.Browser
{
	[ComImport, Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D"),
	InterfaceType(ComInterfaceType.InterfaceIsIDispatch),
	TypeLibType(TypeLibTypeFlags.FHidden)]
	public interface DWebBrowserEvents2
	{
		[DispId(271)]
		void NavigateError(
			[In, MarshalAs(UnmanagedType.IDispatch)] object pDisp,
			[In] ref object URL, [In] ref object frame,
			[In] ref object statusCode, [In, Out] ref bool cancel);

		[DispId(250)]
		void BeforeNavigate2(
			[In, MarshalAs(UnmanagedType.IDispatch)] object pDisp,
			[In] ref object URL,
			[In] ref object flags,
			[In] ref object targetFrameName,
			[In] ref object postData,
			[In] ref object headers,
			[In, Out] ref bool cancel);

		[DispId(252)]
		void NavigateComplete2(
			[In, MarshalAs(UnmanagedType.IDispatch)] object pDisp,
			[In] ref object URL);

		[DispId(0x111)]
		void NewWindow3(
			[In, Out, MarshalAs(UnmanagedType.IDispatch)] ref object ppDisp,
			[In, Out] ref bool Cancel,
			[In] uint dwFlags,
			[In, MarshalAs(UnmanagedType.BStr)] string bstrUrlContext,
			[In, MarshalAs(UnmanagedType.BStr)] string bstrUrl);

		[DispId(253)]
		void OnQuit();
	}
}
