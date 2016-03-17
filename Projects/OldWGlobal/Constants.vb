Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports System.IO

Namespace OldW.GlobalSettings


    ''' <summary>
    ''' OldW程序中所用到的一些全局性的常数
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Constants

#Region "   ---   程序中的各种族文件的名称"
        ''' <summary>
        ''' 基坑中的土体族文件，此土体族在整个模型中只有一个。
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FamilyName_Soil As String = "基坑土体"
        ''' <summary>
        ''' 用来模拟开挖的分块土体的族
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FamilyName_SoilRemove As String = "开挖土体"

#End Region

        Public Const AppName As String = "OldW"

        ''' <summary>
        ''' 监测数据点的Element中，用一个共享参数来存储此测点的监测数据。此共享参数的Guid值。
        ''' </summary>
        ''' <returns>测点元素中表示监测数据的共享参数的Guid值。</returns>
        ''' <remarks>如果要用扩展方法，请加上标签：System.Runtime.CompilerServices.Extension() </remarks>
        Public Shared ReadOnly Property Guid_Monitor As Guid
            Get
                Return New Guid("c3d04d9e-aa78-4328-90c5-cf58167d1f09")
            End Get
        End Property

        ''' <summary>
        ''' Revit中与基坑开挖相关的Document对象的标志性特征在于：此Document对象所对应的项目，在其“管理-项目信息”中，有一个参数：OldW_Project。
        ''' 在此OldWDocument中，可以在Revit的Document中进行与基坑相关的操作，比如搜索基坑开挖土体，记录测点信息等。
        ''' </summary>
        ''' <returns>标识OldWDocument对象的项目信息（共享参数）OldW_Project的Guid值。</returns>
        Public Shared ReadOnly Property Guid_OldWProject As Guid
            Get
                Return New Guid("2284a656-0770-481e-b251-496cde4e7f6d")
            End Get
        End Property

    End Class

End Namespace