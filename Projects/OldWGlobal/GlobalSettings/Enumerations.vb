Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports System.IO

Namespace OldW.GlobalSettings


    ''' <summary>
    ''' 监测仪器的族名称（也是族文件的名称），同时也作为监测仪器的类型判断
    ''' </summary>
    ''' <remarks>从枚举值返回对应的枚举字符的方法：GlobalSettings.InstrumentationType.沉降测点.ToString</remarks>
    Public Enum InstrumentationType
        其他

        ''' <summary> 比如地下连续墙的水平位移 </summary>
        墙体测斜
        ''' <summary> 比如基坑外地表的垂直位移 </summary>
        地表隆沉
        ''' <summary> 比如基坑中支撑的轴力 </summary>
        支撑轴力
        ''' <summary> 比如基坑中立柱的垂直位移 </summary>
        立柱隆沉
    End Enum



End Namespace