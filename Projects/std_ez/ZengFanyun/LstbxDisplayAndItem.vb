Imports System.ComponentModel
Imports System

Imports System.Windows.Forms

Namespace std_ez

    ''' <summary>
    ''' 用来作为ListControl类的.Add方法中的Item参数的类。通过指定ListControl类的DisplayMember属性，来设置列表框中显示的文本。 
    ''' </summary>
    ''' <remarks>
    ''' 保存数据时：
    '''  With ListBoxWorksheetsName
    '''       .DisplayMember = LstbxDisplayAndItem.DisplayMember
    '''       .ValueMember = LstbxDisplayAndItem.ValueMember
    '''       .DataSource = arrSheetsName   '  Dim arrSheetsName(0 To sheetsCount - 1) As LstbxDisplayAndItem
    '''  End With
    ''' 提取数据时：
    '''  Try
    '''      Me.F_shtMonitorData = DirectCast(Me.ListBoxWorksheetsName.SelectedValue, Worksheet)
    '''  Catch ex As Exception
    '''      Me.F_shtMonitorData = Nothing
    '''  End Try
    ''' 或者是：
    '''  Dim lst As LstbxDisplayAndItem = Me.ComboBoxOpenedWorkbook.SelectedItem
    '''  Try
    '''     Dim Wkbk As Workbook = DirectCast(lst.Value, Workbook)
    '''  Catch ex ...
    ''' </remarks>
    Public Class LstbxDisplayAndItem

        ''' <summary>
        ''' 在列表框中进行显示的文本
        ''' </summary>
        ''' <remarks>此常数的值代表此类中代表要在列表框中显示的文本的属性名，即"DisplayedText"</remarks>
        Public Const DisplayMember As String = "DisplayedText"
        ''' <summary>
        ''' 列表框中每一项对应的值（任何类型的值）
        ''' </summary>
        ''' <remarks>此常数的值代表此类中代表列表框中的每一项绑定的数据的属性名，即"Value"</remarks>
        Public Const ValueMember As String = "Value"

        Private _objValue As Object
        Public ReadOnly Property Value As Object
            Get
                Return _objValue
            End Get
        End Property

        Private _DisplayedText As String
        Public ReadOnly Property DisplayedText As String
            Get
                Return _DisplayedText
            End Get
        End Property

        ''' <summary>
        ''' 构造函数
        ''' </summary>
        ''' <param name="DisplayedText">用来显示在列表的UI界面中的文本</param>
        ''' <param name="Value">列表项对应的值</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal DisplayedText As String, ByVal Value As Object)
            Me._objValue = Value
            Me._DisplayedText = DisplayedText
        End Sub 'New 

        '表示“什么也没有”的枚举值
        ''' <summary>
        ''' 列表框中用来表示“什么也没有”。
        ''' 1、在声明时：listControl控件.Items.Add(New LstbxDisplayAndItem(" 无", NothingInListBox.None))
        ''' 2、在选择列表项时：listControl控件.SelectedValue = NothingInListBox.None
        ''' 3、在读取列表中的数据时，作出判断：If Not LstbxItem.Value.Equals(NothingInListBox.None) Then ...
        ''' </summary>
        ''' <remarks></remarks>
        Enum NothingInListBox
            ''' <summary>
            ''' 什么也没有选择
            ''' </summary>
            ''' <remarks></remarks>
            None
        End Enum

    End Class 'LstbxItem
End Namespace