Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.InteropServices

Namespace Day13VB
    Friend Class Parser
        Public Function Convert(ByVal matrix As Char()()) As Char(,)
            Dim w As Integer = matrix.Count()
            Dim h As Integer = matrix(0).Length
            Dim result = New Char(h - 1, w - 1) {}

            For i As Integer = 0 To w - 1

                For j As Integer = 0 To h - 1
                    result(j, i) = matrix(i)(j)
                Next
            Next

            Return result
        End Function

        Public Sub Load(ByRef cells As Char(,), ByRef carts As List(Of Cart))
            Dim file = System.IO.File.ReadAllLines("Input.txt").[Select](Function(line) line.ToCharArray()).ToArray()
            cells = Convert(file)
            carts = New List(Of Cart)()

            For y As Integer = 0 To cells.GetUpperBound(1)

                For x As Integer = 0 To cells.GetUpperBound(0)

                    Select Case cells(x, y)
                        Case "<"c
                            carts.Add(New Cart() With {
                                .X = x,
                                .Y = y,
                                .CurrentDirection = Direction.West,
                                .NextTurn = Turn.Left
                            })
                            cells(x, y) = "-"c
                        Case ">"c
                            carts.Add(New Cart() With {
                                .X = x,
                                .Y = y,
                                .CurrentDirection = Direction.East,
                                .NextTurn = Turn.Left
                            })
                            cells(x, y) = "-"c
                        Case "^"c
                            carts.Add(New Cart() With {
                                .X = x,
                                .Y = y,
                                .CurrentDirection = Direction.North,
                                .NextTurn = Turn.Left
                            })
                            cells(x, y) = "|"c
                        Case "v"c
                            carts.Add(New Cart() With {
                                .X = x,
                                .Y = y,
                                .CurrentDirection = Direction.South,
                                .NextTurn = Turn.Left
                            })
                            cells(x, y) = "|"c
                        Case Else
                    End Select
                Next
            Next
        End Sub
    End Class
End Namespace
