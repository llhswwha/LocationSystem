using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


[XmlType(TypeName = "SystemSetting")]
public class SystemSetting
{
    /// <summary>
    /// 是否调试模式
    /// 调试模式时：
    ///     1.登陆时显示登陆信息调试界面。
    ///     2.在屏幕上显示帧率。
    ///     3.显示电厂CAD。
    ///     4.对焦一个人员时，高亮当前定位信息关联的基站设备。
    ///     5.历史轨迹时显示实际定位点（放个小球在定位点上）。
    /// 这几点都可以在调试模式参数DebugSetting中再细化，当前相当于默认都是true.
    /// IsDebug是true时，【开发调试页面】才显示出来。
    /// </summary>
    [XmlElement]
    public bool IsDebug = false;
    /// <summary>
    /// 屏幕分辨率
    /// </summary>
    [XmlElement]
    public  ResolutionSetting ResolutionSetting; 
    /*todo:
     *     1.加一个页面，显示日志列表，IsDebug是true时显示。
     *     2.加一个位置显示当前是访客模式还是管理模式
     */

    /// <summary>
    /// 版本信息，就一个版本号
    /// </summary>
    [XmlElement]
    public VersionSetting VersionSetting;

    /// <summary>
    /// 是否强制全屏
    /// </summary>
    [XmlElement]
    public bool IsFullScreen ;

    /// <summary>
    /// 是否显示首页
    /// </summary>
    [XmlElement]
    public bool IsShowHomePage=false ;

    /// <summary>
    /// 是否显示左侧拓补图
    /// </summary>
    [XmlElement]
    public bool IsShowLeftTopo=true;

    /// <summary>
    /// 是否显示右下角告警推送
    /// </summary>
    [XmlElement]
    public bool IsShowRightDownAlarm=true;

    /// <summary>
    /// 是否显示右上角统计信息
    /// </summary>
    [XmlElement]
    public bool IsShowRightTopInfo=true;

    /// <summary>
    /// 摄像头跟随相关参数
    /// 【后续随着功能调整会没用】
    /// </summary>
    [XmlElement]
    public CinemachineSetting CinemachineSetting;

    /// <summary>
    /// 通信相关参数设置
    /// </summary>
    [XmlElement]
    public CommunicationSetting CommunicationSetting;

    /// <summary>
    /// 刷新间隔时间
    /// 1.实时定位信息
    /// 2.区域统计数据
    /// 3.人员树节点
    /// 4.部门树节点刷新
    /// 5.附近摄像头界面刷新
    /// 6.顶视图截图保存【没什么用，先放着，可以设置成】
    /// </summary>
    [XmlElement]
    public RefreshSetting RefreshSetting;

    /// <summary>
    /// 霍尼维尔视频设置
    /// </summary>
    [XmlElement]
    public HoneyWellSetting HoneyWellSetting;

    /// <summary>
    /// 模型动态加载相关设置
    /// </summary>
    [XmlElement]
    public AssetLoadSetting AssetLoadSetting;

    /// <summary>
    /// 历史轨迹相关设置
    /// </summary>
    [XmlElement]
    public HistoryPathSetting HistoryPathSetting; 

    /// <summary>
    /// 定位相关设置
    /// </summary>
    [XmlElement]
    public LocationSetting LocationSetting;

    /// <summary>
    /// 调试模式相关设置
    /// </summary>
    [XmlElement]
    public DebugSetting DebugSetting;

    /// <summary>
    /// 设备加载相关设置
    /// 【临时调试用，不显示】
    /// </summary>
    [XmlElement]
    public DeviceSetting DeviceSetting;
    /// <summary>
    /// 是否显示告警
    /// </summary>
    [XmlElement]
    public AlarmSetting AlarmSetting;

    public SystemSetting()
    {
        ResolutionSetting = new ResolutionSetting();
        CinemachineSetting = new CinemachineSetting();
        CommunicationSetting = new CommunicationSetting();
        VersionSetting = new VersionSetting();
        RefreshSetting = new RefreshSetting();
        AssetLoadSetting = new AssetLoadSetting();
        HoneyWellSetting = new HoneyWellSetting();
        DeviceSetting = new DeviceSetting();
        HistoryPathSetting = new HistoryPathSetting();
        LocationSetting = new LocationSetting();
        DebugSetting = new DebugSetting();
        AlarmSetting = new AlarmSetting();
    }
}

