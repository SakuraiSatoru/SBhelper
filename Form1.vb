'(C) Copyright 2011 by Autodesk, Inc. 

'Permission to use, copy, modify, and distribute this software
'in object code form for any purpose and without fee is hereby
'granted, provided that the above copyright notice appears in
'all copies and that both that copyright notice and the limited
'warranty and restricted rights notice below appear in all
'supporting documentation.

'AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
'AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
'MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK,
'INC. DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL
'BE UNINTERRUPTED OR ERROR FREE.

'Use, duplication, or disclosure by the U.S. Government is
'subject to restrictions set forth in FAR 52.227-19 (Commercial
'Computer Software - Restricted Rights) and DFAR 252.227-7013(c)
'(1)(ii)(Rights in Technical Data and Computer Software), as
'applicable.


Imports System
Imports System.Type
Imports System.Activator
Imports System.Runtime.InteropServices
Imports Inventor

Public Class Form1

    Dim _invApp As Inventor.Application
    Dim _started As Boolean

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Try
            _invApp = Marshal.GetActiveObject("Inventor.Application")

        Catch ex As Exception
            Try
                Dim oInvAppType As Type = _
                  GetTypeFromProgID("Inventor.Application")

                _invApp = CreateInstance(oInvAppType)
                _invApp.Visible = True

                'Note: if you shut down the Inventor session that was started
                'this(way) there is still an Inventor.exe running. We will use
                'this Boolean to test whether or not the Inventor App  will
                'need to be shut down.
                _started = True

            Catch ex2 As Exception
                MsgBox(ex2.ToString())
                MsgBox("Unable to get or start Inventor")
            End Try
        End Try

    End Sub


    Private Sub Form1_FormClosed( _
      ByVal sender As Object, ByVal e As FormClosedEventArgs) _
      Handles Me.FormClosed

        If _started Then
            _invApp.Quit()
        End If

        _invApp = Nothing

    End Sub




    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim bDoc As Inventor.PartDocument
        bDoc = _invApp.ActiveDocument
        For Each iParameter As Inventor.Parameter In bDoc.ComponentDefinition.Parameters
            'MsgBox(iParameter.Expression)
            'MsgBox(iParameter.Value)

        Next
        For Each iParameter As Inventor.Parameter In bDoc.ComponentDefinition.Parameters.ModelParameters
            Dim fstMLetter As String = Mid(iParameter.Name, 1, 1)
            Dim fstULetter As String = Mid(iParameter.Expression, 1, 1)
            Dim uParameter As String
            If fstMLetter = "m" Then
                If fstULetter <> "u" And fstULetter <> "m" Then
                    'MsgBox(iParameter.Name + ":" + iParameter.Expression)
                    uParameter = "u" + Mid(iParameter.Name, 2)
                    'MsgBox(uParameter)
                    bDoc.ComponentDefinition.Parameters.UserParameters.AddByExpression(uParameter, iParameter.Expression, iParameter.Units)
                    iParameter.Expression = uParameter
                End If
            End If
        Next
        MsgBox("enjoy!")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If _invApp.ActiveDocument.DocumentType <> _
     DocumentTypeEnum.kAssemblyDocumentObject Then
            MsgBox("Need to have an Assembly document active")
        Else
            Dim cDoc As Inventor.AssemblyDocument
            cDoc = _invApp.ActiveDocument
            For Each iPartOcc As Inventor.ComponentOccurrence In cDoc.ComponentDefinition.Occurrences    'Find the Part in Assembly
                Dim iPartName As String = Mid(iPartOcc.Name, Len(iPartOcc.Name) - 1)
                If iPartName = ":1" Then
                    iPartOcc.Name = Mid(iPartOcc.Name, 1, Len(iPartOcc.Name) - 2)
                End If
            Next
            MsgBox("enjoy!")
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If _invApp.ActiveDocument.DocumentType <> _
DocumentTypeEnum.kAssemblyDocumentObject Then
            MsgBox("Need to have an Assembly document active")
        Else
            Dim dDoc As Inventor.AssemblyDocument
            dDoc = _invApp.ActiveDocument
            For Each iPartOcc As Inventor.ComponentOccurrence In dDoc.ComponentDefinition.Occurrences    'Find the Part in Assembly
                If LCase(iPartOcc.Name) <> "ground" And LCase(iPartOcc.Name) <> "ground:1" Then
                    iPartOcc.Grounded = False

                End If
            Next
            MsgBox("enjoy!")
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim aDoc As Inventor.PartDocument
        aDoc = _invApp.ActiveDocument
        'For Each iParameter As Inventor.Parameter In aDoc.ComponentDefinition.Parameters.ModelParameters
        'MsgBox(iParameter.Expression + ":" + iParameter.Units)
        'Next
        aDoc.ComponentDefinition.Parameters.UserParameters.AddByExpression("rFloorArea", "0 ul", "ul")
        aDoc.ComponentDefinition.Parameters.UserParameters.AddByExpression("rFootArea", "rFloorArea/uNumOfStories", "ul")
        MsgBox("enjoy!")

    End Sub
End Class