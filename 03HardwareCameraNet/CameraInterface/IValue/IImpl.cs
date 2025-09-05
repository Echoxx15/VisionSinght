using System.Collections.Generic;

namespace HardwareCameraNet.IValue;

/// <summary>
/// IFloatVal接口的具体实现类（用于承载CurValue/Max/Min的实际值）
/// </summary>
public class FloatValImpl : IFloatVal
{
    // 实现接口的三个属性（直接用自动属性即可，无需额外逻辑）
    public double CurValue { get; set; }
    public double Max { get; set; }
    public double Min { get; set; }

    // （可选）添加构造函数，方便快速赋值
    public FloatValImpl(double curValue, double max, double min)
    {
        CurValue = curValue;
        Max = max;
        Min = min;
    }

    // （可选）无参构造函数（序列化或默认实例化用）
    public FloatValImpl() { }
}
/// <summary>
/// IFloatVal接口的具体实现类（用于承载CurValue/Max/Min的实际值）
/// </summary>
public class StringValImpl : IStringVal
{
    public string CurEnumEntry { get; set; }
    public List<string> SupportEnumEntries { get; set; }
    
    public StringValImpl(string curEnumEntry, List<string> supportEnumEntries)
    {
        CurEnumEntry = curEnumEntry;
        SupportEnumEntries = supportEnumEntries;
    }

    public StringValImpl() { }
}