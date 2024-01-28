using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NvidiaGameManager.DisplayManagement.Native
{
    // totally stolen from: https://github.com/falahati/WindowsDisplay
    [StructLayout(LayoutKind.Sequential)]
    internal struct LRect : IEquatable<LRect>
    {
        [MarshalAs(UnmanagedType.U4)] public readonly int Left;
        [MarshalAs(UnmanagedType.U4)] public readonly int Top;
        [MarshalAs(UnmanagedType.U4)] public readonly int Right;
        [MarshalAs(UnmanagedType.U4)] public readonly int Bottom;

        public Rectangle ToRectangle()
        {
            return new Rectangle(Left, Top, Right - Left, Bottom - Top);
        }

        public LRect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public LRect(Rectangle rectangle) : this(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom)
        {
        }

        public bool Equals(LRect other)
        {
            return Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is LRect rectangle && Equals(rectangle);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Left;
                hashCode = hashCode * 397 ^ Top;
                hashCode = hashCode * 397 ^ Right;
                hashCode = hashCode * 397 ^ Bottom;

                return hashCode;
            }
        }

        public static bool operator ==(LRect left, LRect right)
        {
            return Equals(left, right) || left.Equals(right);
        }

        public static bool operator !=(LRect left, LRect right)
        {
            return !(left == right);
        }
    }
}
