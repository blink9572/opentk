#region License
//
// The Open Toolkit Library License
//
// Copyright (c) 2006 - 2010 the Open Toolkit library.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
//
#endregion

using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace OpenTK.Platform.MacOS.Carbon
{
    using CGDirectDisplayID = System.IntPtr;

    // Quartz Display services used here are available in MacOS X 10.3 and later.

    enum CGDisplayErr
    {

    }

    enum CGError
    {
        Success = 0,
        Failure = 1000,
        IllegalArgument = 1001,
        InvalidConnection = 1002,
        InvalidContext = 1003,
        CannotComplete = 1004,
        NotImplemented = 1006,
        RangeCheck = 1007,
        TypeCheck = 1008,
        InvalidOperation = 1010,
        NoneAvailable = 1011,
    }

    internal static class CG
    {
        const string appServices = "/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices";

        // CGPoint -> HIPoint
        // CGSize -> HISize
        // CGRect -> HIRect

        [DllImport(appServices,EntryPoint="CGGetActiveDisplayList")]
        internal unsafe static extern CGDisplayErr GetActiveDisplayList(int maxDisplays, IntPtr* activeDspys, out int dspyCnt);

        [DllImport(appServices,EntryPoint="CGMainDisplayID")]
        internal static extern IntPtr MainDisplayID();

        // Note: sizeof(HIRect) == 16, which is larger than 8 bytes.
        // The x86 and x64 Mac ABIs pass such structs as pointers in the
        // first parameter slot. This is normally handled automatically
        // by gcc/clang, but here we have to do it ourselves.
        // See "Listing 4" on https://developer.apple.com/library/mac/documentation/DeveloperTools/Conceptual/LowLevelABI/130-IA-32_Function_Calling_Conventions/IA32.html#//apple_ref/doc/uid/TP40002492-SW3
        internal unsafe static HIRect DisplayBounds(IntPtr display)
        {
            HIRect rect;
            DisplayBounds(out rect, display);
            return rect;
        }

        [DllImport(appServices, EntryPoint = "CGDisplayBounds")]
        unsafe static extern void DisplayBounds(out HIRect rect, IntPtr display);

        [DllImport(appServices,EntryPoint="CGDisplayPixelsWide")]
        internal static extern int DisplayPixelsWide(IntPtr display);

        [DllImport(appServices,EntryPoint="CGDisplayPixelsHigh")]
        internal static extern int DisplayPixelsHigh(IntPtr display);

        [DllImport(appServices,EntryPoint="CGDisplayCurrentMode")]
        internal static extern IntPtr DisplayCurrentMode(IntPtr display);

        [DllImport(appServices,EntryPoint="CGDisplayCapture")]
        internal static extern CGDisplayErr DisplayCapture(IntPtr display);

        [DllImport(appServices,EntryPoint="CGCaptureAllDisplays")]
        internal static extern CGDisplayErr CaptureAllDisplays();

        [DllImport(appServices,EntryPoint="CGShieldingWindowLevel")]
        internal static extern uint ShieldingWindowLevel();

        [DllImport(appServices,EntryPoint="CGDisplayRelease")]
        internal static extern CGDisplayErr DisplayRelease(IntPtr display);

        [DllImport(appServices,EntryPoint="CGReleaseAllDisplays")]
        internal static extern CGDisplayErr DisplayReleaseAll();

        [DllImport(appServices, EntryPoint = "CGDisplayAvailableModes")]
        internal static extern IntPtr DisplayAvailableModes(IntPtr display);

        [DllImport(appServices, EntryPoint = "CGDisplaySwitchToMode")]
        internal static extern IntPtr DisplaySwitchToMode(IntPtr display, IntPtr displayMode);

        [DllImport(appServices, EntryPoint = "CGWarpMouseCursorPosition")]
        internal static extern CGError WarpMouseCursorPosition(HIPoint newCursorPosition);

        [DllImport(appServices, EntryPoint = "CGCursorIsVisible")]
        internal static extern bool CursorIsVisible();

        [DllImport(appServices, EntryPoint = "CGDisplayShowCursor")]
        internal static extern CGError DisplayShowCursor(CGDirectDisplayID display);

        [DllImport(appServices, EntryPoint = "CGDisplayHideCursor")]
        internal static extern CGError DisplayHideCursor(CGDirectDisplayID display);

         [DllImport(appServices, EntryPoint = "CGAssociateMouseAndMouseCursorPosition")]
        internal static extern CGError AssociateMouseAndMouseCursorPosition(bool connected);

        [DllImport(appServices, EntryPoint="CGSetLocalEventsSuppressionInterval")]
        internal static extern CGError SetLocalEventsSuppressionInterval(double seconds);
    }
}
