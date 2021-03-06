﻿using System;

namespace NuBot
{
    public struct ColorPair
    {
        public ConsoleColor? ForegroundColor { get; private set; }
        public ConsoleColor? BackgroundColor { get; private set; }

        public ColorPair(ConsoleColor? foreground, ConsoleColor? background)
            : this()
        {
            ForegroundColor = foreground;
            BackgroundColor = background;
        }

        public static ColorPair ForegroundOnly(ConsoleColor color)
        {
            return new ColorPair(color, null);
        }
    }
}
