open System
open System.IO

type machineType = { Registers: int[]; }

let addr (machine:machineType) A B C = 
    machine.Registers.[C] <- machine.Registers.[A] + machine.Registers.[B]

let addi (machine:machineType) A B C = 
    machine.Registers.[C] <- machine.Registers.[A] + B

let mulr (machine:machineType) A B C = 
    machine.Registers.[C] <- machine.Registers.[A] * machine.Registers.[B]

let muli (machine:machineType) A B C = 
    machine.Registers.[C] <- machine.Registers.[A] * B

let banr (machine:machineType) A B C = 
    machine.Registers.[C] <- machine.Registers.[A] &&& machine.Registers.[B]

let bani (machine:machineType) A B C = 
    machine.Registers.[C] <- machine.Registers.[A] &&& B

let borr (machine:machineType) A B C = 
    machine.Registers.[C] <- machine.Registers.[A] ||| machine.Registers.[B]

let bori (machine:machineType) A B C = 
    machine.Registers.[C] <- machine.Registers.[A] ||| B

let setr (machine:machineType) A _ C = 
    machine.Registers.[C] <- machine.Registers.[A]

let seti (machine:machineType) A _ C = 
    machine.Registers.[C] <- A

let gtir (machine:machineType) A B C = 
    machine.Registers.[C] <- if ( A > machine.Registers.[B]) then
                                1
                             else 
                                0

let gtri (machine:machineType) A B C = 
    machine.Registers.[C] <- if ( machine.Registers.[A] > B) then
                                1
                             else 
                                0

let gtrr (machine:machineType) A B C = 
    machine.Registers.[C] <- if ( machine.Registers.[A] > machine.Registers.[B]) then
                                1
                             else 
                                0

let eqir (machine:machineType) A B C = 
    machine.Registers.[C] <- if ( A = machine.Registers.[B]) then
                                1
                             else 
                                0

let eqri (machine:machineType) A B C = 
    machine.Registers.[C] <- if ( machine.Registers.[A] = B) then
                                1
                             else 
                                0

let eqrr (machine:machineType) A B C = 
    machine.Registers.[C] <- if ( machine.Registers.[A] = machine.Registers.[B]) then
                                1
                             else 
                                0

let instructions = [("addr",addr); ("addi",addi); ("mulr",mulr); ("muli",muli); ("banr",banr); ("bani",bani); ("borr",borr); ("bori",bori); 
                    ("setr",setr); ("seti",seti); ("gtir",gtir); ("gtri",gtri); ("gtrr",gtrr); ("eqir",eqir); ("eqri",eqri); ("eqrr",eqrr)]

let checkInstruction (registersBefore:int[]) (registersAfter:int[]) A B C (_,fn) =
    let machine = { Registers = Array.init 4 (fun i -> registersBefore.[i]) }
    fn machine A B C
    machine.Registers = registersAfter

let findPossibleInstructions registersBefore registersAfter A B C instructions =
    let couldBeInstruction = checkInstruction registersBefore registersAfter A B C
    instructions |> Seq.where couldBeInstruction

let parseTest (lines:string[]) =
    let before = lines.[0].Replace(" ","").Split([|"[";",";"]"|], StringSplitOptions.RemoveEmptyEntries) |> Array.skip 1 |> Array.map int
    let arguments = lines.[1].Split(' ') |> Array.map int
    let after = lines.[2].Replace(" ","").Split([|"[";",";"]"|], StringSplitOptions.RemoveEmptyEntries) |> Array.skip 1 |> Array.map int
    (before, arguments, after)

let runTest (registersBefore, (arguments:int[]), registersAfter) = 
    let A = arguments.[1]
    let B = arguments.[2]
    let C = arguments.[3]
    let ins = findPossibleInstructions registersBefore registersAfter A B C instructions
    ins |> Seq.length

[<EntryPoint>]
let main argv =

    let tests = File.ReadLines("Input.txt") |> Seq.chunkBySize 4 |> Seq.map parseTest
    
    let part1 = tests 
                    |> Seq.map runTest 
                    |> Seq.where (fun a -> a >= 3) 
                    //|> Seq.length

    part1 |> Seq.iter (fun i ->  printfn "%o" i)

    //printfn "%o" (part1) 
   // let machine = { Registers = Array.create 4 0}

    printfn "Hello World from F#!"
    0 // return an integer exit code
