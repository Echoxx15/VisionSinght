using System.Collections.Generic;

namespace HardwareCameraNet.IValue;

public interface IFloatVal
{
    double CurValue { get; set; }
    double Max { get; set; }
    double Min { get; set; }
}
public interface IStringVal
{
    string CurEnumEntry { get; set; }
    List<string> SupportEnumEntries { get; set; }
}