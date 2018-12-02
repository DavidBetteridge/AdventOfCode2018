Imports System
Imports System.Collections.Generic
Imports System.Linq

Namespace Day13VB
    Friend Enum Direction
        West = 0
        North = 1
        East = 2
        South = 3
    End Enum

    Friend Enum Turn
        Left = 0
        Straight = 1
        Right = 2
    End Enum



    Public Class Program
        Public Shared Sub Main(ByVal args As String())
            Dim parser = New Parser()
            Dim cells As Char(,) = Nothing
            Dim carts As List(Of Cart) = Nothing
            parser.Load(cells, carts)
            Dim tick = 0

            While True
                tick += 1
                Dim orderedCarts = carts.OrderBy(Function(cart) cart.Y).ThenBy(Function(cart) cart.X)

                For Each cart In orderedCarts.Where(Function(c) Not c.IsDead)
                    Dim nextTrack As Char = WorkoutNextTrack(cart.X, cart.Y, cart.CurrentDirection, cells)

                    Select Case nextTrack
                        Case "|"c
                            cart.Y = If(cart.CurrentDirection = Direction.North, cart.Y - 1, cart.Y + 1)
                        Case "-"c
                            cart.X = If(cart.CurrentDirection = Direction.West, cart.X - 1, cart.X + 1)
                        Case "/"c

                            Select Case cart.CurrentDirection
                                Case Direction.West
                                    cart.CurrentDirection = Direction.South
                                    cart.X = cart.X - 1
                                Case Direction.North
                                    cart.CurrentDirection = Direction.East
                                    cart.Y = cart.Y - 1
                                Case Direction.East
                                    cart.CurrentDirection = Direction.North
                                    cart.X = cart.X + 1
                                Case Direction.South
                                    cart.CurrentDirection = Direction.West
                                    cart.Y = cart.Y + 1
                                Case Else
                                    Throw New Exception("Unknown direction")
                            End Select

                        Case "\"c

                            Select Case cart.CurrentDirection
                                Case Direction.West
                                    cart.CurrentDirection = Direction.North
                                    cart.X = cart.X - 1
                                Case Direction.North
                                    cart.CurrentDirection = Direction.West
                                    cart.Y = cart.Y - 1
                                Case Direction.East
                                    cart.CurrentDirection = Direction.South
                                    cart.X = cart.X + 1
                                Case Direction.South
                                    cart.CurrentDirection = Direction.East
                                    cart.Y = cart.Y + 1
                                Case Else
                                    Throw New Exception("Unknown direction")
                            End Select

                        Case "+"c

                            Select Case cart.CurrentDirection
                                Case Direction.West
                                    cart.TurnCart()
                                    cart.X = cart.X - 1
                                Case Direction.North
                                    cart.TurnCart()
                                    cart.Y = cart.Y - 1
                                Case Direction.East
                                    cart.TurnCart()
                                    cart.X = cart.X + 1
                                Case Direction.South
                                    cart.TurnCart()
                                    cart.Y = cart.Y + 1
                                Case Else
                                    Throw New Exception("Unknown direction")
                            End Select

                        Case Else
                            Throw New Exception("Unknown track type")
                    End Select

                    Dim result = carts.Where(Function(c) Not c.IsDead).GroupBy(Function(c) (c.X, c.Y))
                    Dim crashes = result.Where(Function(grp) grp.Count() > 1).ToList()

                    If crashes.Any() Then
                        Console.WriteLine($"Part 1 :: {crashes.First().First().X},{crashes.First().First().Y}")
                    End If

                    For Each crash In crashes

                        For Each c In crash
                            c.IsDead = True
                        Next
                    Next
                Next

                If carts.Where(Function(c) Not c.IsDead).Count() = 1 Then
                    Dim remaining = carts.Single(Function(c) Not c.IsDead)
                    Console.WriteLine($"Part 2 :: {remaining.X},{remaining.Y}")
                    Console.ReadKey()
                End If
            End While

            Console.WriteLine("Hello World!")
        End Sub

        Private Shared Sub Display(ByVal cells As Char(,), ByVal carts As IEnumerable(Of Cart))
            Console.Clear()
            Console.SetCursorPosition(0, 0)

            For y As Integer = 0 To cells.GetUpperBound(1)
                Dim yLocal = y
                For x As Integer = 0 To cells.GetUpperBound(0)
                    Dim xLocal = x
                    Dim cart = carts.FirstOrDefault(Function(c) c.X = xLocal AndAlso c.Y = yLocal AndAlso Not c.IsDead)

                    If cart Is Nothing Then
                        Console.Write(cells(x, y))
                    Else
                        Console.ForegroundColor = ConsoleColor.DarkGreen

                        Select Case cart.CurrentDirection
                            Case Direction.West
                                Console.Write("<")
                            Case Direction.North
                                Console.Write("^")
                            Case Direction.East
                                Console.Write(">")
                            Case Direction.South
                                Console.Write("v")
                            Case Else
                        End Select

                        Console.ResetColor()
                    End If
                Next

                Console.WriteLine()
            Next

            Console.ReadKey()
        End Sub

        Private Shared Function WorkoutNextTrack(ByVal x As Integer, ByVal y As Integer, ByVal currentDirection As Direction, ByVal cells As Char(,)) As Char
            Select Case currentDirection
                Case Direction.West
                    Return cells(x - 1, y)
                Case Direction.North
                    Return cells(x, y - 1)
                Case Direction.East
                    Return cells(x + 1, y)
                Case Direction.South
                    Return cells(x, y + 1)
                Case Else
                    Throw New Exception("Unknown direction")
            End Select
        End Function
    End Class
End Namespace
