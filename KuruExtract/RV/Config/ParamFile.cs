﻿using KuruExtract.RV.IO;

namespace KuruExtract.RV.Config;

internal sealed class ParamFile
{
    public ParamClass? Root { get; private set; }

    public List<KeyValuePair<string, int>>? EnumValues { get; private set; }

    public ParamFile()
    {
        EnumValues = new List<KeyValuePair<string, int>>(10);
    }

    public ParamFile(Stream stream)
    {
        Read(new RVBinaryReader(stream));
    }

    public void Read(RVBinaryReader input)
    {
        var sig = new char[] { '\0', 'r', 'a', 'P' };
        if (!input.ReadBytes(4).SequenceEqual(sig.Select(c => (byte)c)))
            throw new ArgumentException();

        var ofpVersion = input.ReadInt32();
        var version = input.ReadInt32();
        var offsetToEnums = input.ReadInt32();

        Root = new ParamClass(input, "rootClass");

        input.Position = offsetToEnums;
        var nEnumValues = input.ReadInt32();
        EnumValues = Enumerable.Range(0, nEnumValues).Select(_ => new KeyValuePair<string, int>(input.ReadAsciiz(), input.ReadInt32())).ToList();
    }

    public override string? ToString()
    {
        return Root?.ToString(0, true);
    }
}
