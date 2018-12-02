open System
open System.IO

type registerState = int[]

type machineType = { Registers: registerState; }

type programLine = { opCode: int;  operandA:int;  operandB:int;  operandC: int}


let updateRegisters machine registerNumber newValue =
    let newRegisters = 
        match registerNumber with
        | 0 -> [|newValue;machine.Registers.[1];machine.Registers.[2];machine.Registers.[3];|]
        | 1 -> [|machine.Registers.[0];newValue;machine.Registers.[2];machine.Registers.[3];|]
        | 2 -> [|machine.Registers.[0];machine.Registers.[1];newValue;machine.Registers.[3];|]
        | 3 -> [|machine.Registers.[0];machine.Registers.[1];machine.Registers.[2];newValue;|]
        | _ -> failwith "Unknown register number"
    {Registers = newRegisters}

let addr (machine:machineType) A B C = 
    updateRegisters machine C (machine.Registers.[A] + machine.Registers.[B])

let addi (machine:machineType) A B C = 
    updateRegisters machine C (machine.Registers.[A] + B)

let mulr (machine:machineType) A B C = 
    updateRegisters machine C (machine.Registers.[A] * machine.Registers.[B])

let muli (machine:machineType) A B C = 
    updateRegisters machine C (machine.Registers.[A] * B)

let banr (machine:machineType) A B C = 
    updateRegisters machine C (machine.Registers.[A] &&& machine.Registers.[B])

let bani (machine:machineType) A B C = 
    updateRegisters machine C (machine.Registers.[A] &&& B)

let borr (machine:machineType) A B C = 
    updateRegisters machine C (machine.Registers.[A] ||| machine.Registers.[B])

let bori (machine:machineType) A B C = 
    updateRegisters machine C (machine.Registers.[A] ||| B)

let setr (machine:machineType) A _ C = 
    updateRegisters machine C (machine.Registers.[A])

let seti (machine:machineType) A _ C = 
    updateRegisters machine C A

let gtir (machine:machineType) A B C = 
    updateRegisters machine C ( if ( A > machine.Registers.[B]) then
                                    1
                                else 
                                    0 )
    
let gtri (machine:machineType) A B C = 
    updateRegisters machine C ( if ( machine.Registers.[A] > B) then
                                    1
                                 else 
                                    0)

let gtrr (machine:machineType) A B C = 
    updateRegisters machine C ( if ( machine.Registers.[A] > machine.Registers.[B]) then
                                    1
                                 else 
                                    0)

let eqir (machine:machineType) A B C = 
    updateRegisters machine C ( if ( A = machine.Registers.[B]) then
                                    1
                                 else 
                                    0)

let eqri (machine:machineType) A B C = 
    updateRegisters machine C ( if ( machine.Registers.[A] = B) then
                                    1
                                 else 
                                    0)

let eqrr (machine:machineType) A B C = 
    updateRegisters machine C ( if ( machine.Registers.[A] = machine.Registers.[B]) then
                                    1
                                 else 
                                    0 )

let instructions = [("addr",addr); ("addi",addi); ("mulr",mulr); ("muli",muli); ("banr",banr); ("bani",bani); ("borr",borr); ("bori",bori); 
                    ("setr",setr); ("seti",seti); ("gtir",gtir); ("gtri",gtri); ("gtrr",gtrr); ("eqir",eqir); ("eqri",eqri); ("eqrr",eqrr)]

let checkInstruction (registersBefore:int[]) (registersAfter:int[]) A B C (_,fn) =
    let machine = { Registers = Array.init 4 (fun i -> registersBefore.[i]) }
    let updatedMachine = fn machine A B C
    updatedMachine.Registers = registersAfter

let findPossibleInstructions registersBefore registersAfter A B C instructions =
    let couldBeInstruction = checkInstruction registersBefore registersAfter A B C
    instructions |> Seq.where couldBeInstruction

let parseInstruction (line:string) : programLine =
    let parts = line.Split(' ') |> Array.map int
    match parts with
    | [|opCode;A;B;C|] -> { opCode = opCode; operandA=A; operandB=B; operandC=C }
    | _ -> failwith "Instruction in the wrong format" 

let parseTest (lines:string[]) =
    let before:registerState = lines.[0].Replace(" ","").Split([|"[";",";"]"|], StringSplitOptions.RemoveEmptyEntries) |> Array.skip 1 |> Array.map int
    let arguments = parseInstruction lines.[1] 
    let after:registerState = lines.[2].Replace(" ","").Split([|"[";",";"]"|], StringSplitOptions.RemoveEmptyEntries) |> Array.skip 1 |> Array.map int
    (before, arguments, after)

let runTest (registersBefore, programLine, registersAfter) = 
    let A = programLine.operandA
    let B = programLine.operandB
    let C = programLine.operandC
    findPossibleInstructions registersBefore registersAfter A B C instructions

let rec intersection (listOfinstructions:seq<seq<string>>) : seq<string> =
    match List.ofSeq listOfinstructions with
    | [] -> Seq.empty
    | (x1::[]) -> x1
    | (x1::x2::[]) -> Set.intersect (Set.ofSeq x1) (Set.ofSeq x2) |> Set.toSeq
    | (x1::x2::xs) -> let x = Set.intersect (Set.ofSeq x1) (Set.ofSeq x2) |> Set.toSeq
                      intersection (x::xs)

let removeOpCode (fullList:list<int * seq<string>>) (opCode:int) : list<int * seq<string>> = 
    fullList |> List.where (fun (op, _) -> op <> opCode)

let removeInstruction (fullList:list<int * seq<string>>) (instruction:string) : list<int * seq<string>> = 
    fullList |> List.map (fun (op, instructions) -> (op, instructions |> Seq.where (fun instr -> instr <> instruction)  ))

let rec workoutOpCode (opCodesAndPossibleInstructions:list<int * seq<string>>) : list<int * string> =
    match opCodesAndPossibleInstructions with
    | [] -> List.Empty
    | _ ->
            let (opCode, instructions) = opCodesAndPossibleInstructions 
                                            |> Seq.find (fun (_, possible) -> possible |> Seq.length = 1)
            let instruction = instructions |> Seq.head         
            let reducedCodes = removeOpCode opCodesAndPossibleInstructions opCode
            let reducedCodes' = removeInstruction reducedCodes instruction
            (opCode, instruction) :: workoutOpCode reducedCodes'

let runProgram opCodes machine instruction =
    let (_, instructionName) = opCodes |> List.find ( fun (op, _) -> op = instruction.opCode)
    let (_, fn) = instructions |> List.find (fun (name, fn) -> name = instructionName)
    fn machine instruction.operandA instruction.operandB instruction.operandC

let workOutOpCodes (tests:seq<(registerState*programLine*registerState)>) testResults = 
    let opCodes = tests |> Seq.map (fun ((_, line, _)) -> line.opCode)
    let names = testResults |> Seq.map (Seq.map fst)
    let multipleOpCodesAndPossibleInstructions = names |> Seq.zip opCodes

    let opCodesAndPossibleInstructions = multipleOpCodesAndPossibleInstructions 
                                            |> Seq.groupBy fst
                                            |> Seq.map (fun (opCode, instructions) -> (opCode, instructions |> Seq.map snd ))
                                            |> Seq.map (fun (opCode, instructions) -> (opCode, intersection instructions))

    workoutOpCode (List.ofSeq opCodesAndPossibleInstructions)

[<EntryPoint>]
let main argv =

    let tests = File.ReadLines("Input.txt") |> Seq.chunkBySize 4 |> Seq.map parseTest 
    
    let testResults = tests |> Seq.map runTest 

    let part1 = testResults 
                    |> Seq.map Seq.length
                    |> Seq.where (fun a -> a >= 3) 
                    |> Seq.length

    printfn "Part 1 is %d" (part1) 

    let opCodes = workOutOpCodes tests testResults

    let program = File.ReadLines("Input Part 2.txt") |> Seq.map parseInstruction
    let machine = { Registers = Array.create 4 0}
    let runner = runProgram opCodes
    let finalMachine = program |> Seq.fold runner machine

    printfn "Part 2 is %d" finalMachine.Registers.[0]
    0 
