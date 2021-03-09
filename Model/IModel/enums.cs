﻿

namespace DbModel.Tools
{
    //对接方的设备分类
    //public enum Abutment_DevTypes { 无=0,固定设备 = 100, 生产设备, 摄像头, 枪机 = 1021, 球机, 半球, 门禁 = 103, 单向 = 1031, 双向, 消防设备 = 104, 危化品, 定位基站, 巡检点, 停车位, 移动设备 = 200, 一卡通, 人员定位终端, 移动终端 }
    public enum Abutment_DevTypes { 无 = 0, 固定设备 = 100, 生产设备=101, 摄像头=102, 枪机 = 1021, 球机=1022, 半球=1023, 门禁 = 103, 单向 = 1031, 双向=1032, 消防设备 = 104, 烟感 = 1041, 温感, 普通温感模块, 感温电缆, 控制, 监视火警, 监管, 手报, 手动报警按钮, 危化品 = 105, 定位基站=106, 巡检点=107, 停车位=108, 移动设备 = 200, 一卡通=201, 人员定位终端=202, 移动终端=203 }

    //对接方设备状态
    public enum Abutment_Status { 正常, 维修中, 已报失, 已作废 = 9 }

    //对接方设备运行状态
    public enum Abutment_RunStatus { 正常, 离线, 报警,未知 }

    //对接方设备告警级别
    public enum Abutment_DevAlarmLevel { 无, 低, 中, 高, 未定 }

    //对接方设备告警级别
    public enum Abutment_DevAlarmSrc { 未知, 视频监控, 门禁, 消防, SIS = 11, 人员定位 }

    //区域分类
    //范围类型不要添加子节点，范围只能是叶子节点
    public enum AreaTypes { 区域, 园区, 分组, 大楼, 楼层, 机房, 设备, 部件, 范围, CAD,SwitchArea }

    //性别
    public enum Sexs { 未知, 男, 女 }

    //权限类型
    public enum TimeSettingType { 时间长度, 时间点范围 }

    //定位告警类型
    public enum LocationAlarmType { 区域告警, 消失告警, 低电告警, 传感器告警, 重启告警, 非法拆卸, 求救信号, 晕倒告警 }

    //定位告警等级
    public enum LocationAlarmLevel { 正常, 一级告警, 二级告警, 三级告警, 四级告警 }

    //定位告警处理类型
    public enum LocationAlarmHandleType { 未处理,误报, 忽略, 确认 }

    //基站类型
    public enum ArchorTypes { 副基站, 主基站 }

    //是否启动
    public enum IsStart { 否, 是 }

    public enum DepartType { 本厂, 外委单位 }

    //巡检状态
    public enum InspectionStatus
    {
        NewBuild,        //新建
        AlreadyIssued,   //已下达
        Completed,       //已完成
        Cancelled,       //已取消
        InExecution,     //执行中
        Expired          //已过期
    }
}
