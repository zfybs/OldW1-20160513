Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports System.IO

Namespace OldW.GlobalSettings


    ''' <summary>
    ''' OldW程序中所用到的一些全局性的常数
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Constants

        ''' <summary>
        ''' 整个程序的标志性名称
        ''' </summary>
        ''' <remarks></remarks>
        Public Const AppName As String = "OldW"

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

#Region "   ---   程序中的各种族样板文件的名称"

        ''' <summary>
        ''' 基坑中的土体族的族样板文件，用来创建基坑中的土体，以及用来模拟开挖的分块小土体。
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FamilyTemplateName_Soil As String = "公制常规模型.rft"

#End Region

#Region "   ---   共享参数系列"
        '  对应的共享参数文档中的内容如下：
        '  # This is a Revit shared parameter file.
        '  # Do not edit manually.
        '  *META	VERSION	MINVERSION
        '  META	2	1
        '  *GROUP	ID	NAME
        '  GROUP	1	OldW
        '  *PARAM	GUID	NAME	DATATYPE	DATACATEGORY	GROUP	VISIBLE	DESCRIPTION	USERMODIFIABLE
        '  PARAM	0948fd00-11d6-4ee1-beb7-66ee43fecf75	开挖完成	TEXT		1	1		1
        '  PARAM	2284a656-0770-481e-b251-496cde4e7f6d	OldW_Project	TEXT		1	1		0
        '  PARAM	c3d04d9e-aa78-4328-90c5-cf58167d1f09	监测数据	TEXT		1	1		0

        ''' <summary>
        ''' 共享参数组的名称
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SP_GroupName As String = "OldW"

        ''' <summary>
        ''' 监测数据点的Element中，用一个共享参数来存储此测点的监测数据。此共享参数的Guid值。
        ''' </summary>
        ''' <returns>测点元素中表示监测数据的共享参数的Guid值。</returns>
        ''' <remarks>如果要用扩展方法，请加上标签：System.Runtime.CompilerServices.Extension() </remarks>
        Public Shared ReadOnly Property SP_Guid_Monitor As Guid
            Get
                Return New Guid("c3d04d9e-aa78-4328-90c5-cf58167d1f09")
            End Get
        End Property

        ''' <summary>
        ''' 作为基坑模型的标志判断的参数的GUID值。
        ''' Revit中与基坑开挖相关的Document对象的标志性特征在于：此Document对象所对应的项目，在其“管理-项目信息”中，有一个参数：OldW_Project。
        ''' 在此OldWDocument中，可以在Revit的Document中进行与基坑相关的操作，比如搜索基坑开挖土体，记录测点信息等。
        ''' </summary>
        ''' <returns>标识OldWDocument对象的项目信息（共享参数）OldW_Project的Guid值。</returns>
        Public Shared ReadOnly Property SP_Guid_OldWProjectInfo As Guid
            Get
                Return New Guid("2284a656-0770-481e-b251-496cde4e7f6d")
            End Get
        End Property

        ''' <summary>
        ''' 作为基坑模型的标志判断的参数的名称。
        '''  Revit中与基坑开挖相关的Document对象的标志性特征在于：此Document对象所对应的项目，在其“管理-项目信息”中，有一个参数：OldW_Project。
        ''' 在此OldWDocument中，可以在Revit的Document中进行与基坑相关的操作，比如搜索基坑开挖土体，记录测点信息等。
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SP_OldWProjectInfo As String = "OldW_Project"


        ''' <summary>
        ''' 每一个开挖土体单元，都有一个对应的开挖完成的日期数据。
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property SP_Guid_ExcavationCompleted As Guid
            Get
                Return New Guid("0948fd00-11d6-4ee1-beb7-66ee43fecf75")
            End Get
        End Property

        ''' <summary>
        ''' 每一个开挖土体单元，都有一个对应的开挖完成的日期数据。
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SP_ExcavationCompleted As String = "开挖完成"

#End Region

    End Class

End Namespace