Imports System

Namespace Day13VB
    Friend Class Cart
        Public Property X As Integer
        Public Property Y As Integer
        Public Property CurrentDirection As Direction
        Public Property NextTurn As Turn
        Public Property IsDead As Boolean

        Private Function WorkOutNextTurn(ByVal turn As Turn) As Turn
            Select Case turn
                Case Turn.Left
                    Return Turn.Straight
                Case Turn.Straight
                    Return Turn.Right
                Case Turn.Right
                    Return Turn.Left
                Case Else
                    Throw New Exception("Unknown turn")
            End Select
        End Function

        Public Sub TurnCart()
            Select Case NextTurn
                Case Turn.Left

                    Select Case CurrentDirection
                        Case Direction.West
                            CurrentDirection = Direction.South
                        Case Direction.North
                            CurrentDirection = Direction.West
                        Case Direction.East
                            CurrentDirection = Direction.North
                        Case Direction.South
                            CurrentDirection = Direction.East
                        Case Else
                    End Select

                Case Turn.Straight
                Case Turn.Right

                    Select Case CurrentDirection
                        Case Direction.West
                            CurrentDirection = Direction.North
                        Case Direction.North
                            CurrentDirection = Direction.East
                        Case Direction.East
                            CurrentDirection = Direction.South
                        Case Direction.South
                            CurrentDirection = Direction.West
                        Case Else
                    End Select

                Case Else
                    Throw New Exception("Unknown turn")
            End Select

            NextTurn = WorkOutNextTurn(NextTurn)
        End Sub
    End Class
End Namespace
