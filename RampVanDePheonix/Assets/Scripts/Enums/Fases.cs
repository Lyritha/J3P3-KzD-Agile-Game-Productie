using System;

[Flags]
public enum Fases
{
    None = 0,
    Achterhoek = 1 << 0,
    Pheonix = 1 << 1,
    Amerika = 1 << 2,
    EndScreen = 1 << 3,
    All = ~0
}