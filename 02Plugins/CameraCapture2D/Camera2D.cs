using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionCore.PluginBase;

namespace CameraCapture2D;

[Category("图像采集")]
[DisplayName("面阵相机")]
[Serializable]
public class Camera2D : IToolPlugin
{
    public string PluginId { get; }
    public Version Version { get; }
    public string Description { get; }
    public void AddToToolbar(object toolbar)
    {
        throw new NotImplementedException();
    }
}

