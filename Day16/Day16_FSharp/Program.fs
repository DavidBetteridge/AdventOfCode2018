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

[<EntryPoint>]
let main argv =

    let tests = File.ReadLines("Input.txt") |> Seq.chunkBySize 4 |> Seq.map parseTest 
    
    let testResults = tests |> Seq.map runTest 

    let part1 = testResults 
                    |> Seq.map (fun instructions -> Seq.length instructions) 
                    |> Seq.where (fun a -> a >= 3) 
                    |> Seq.length

    printfn "Part 1 is %d" (part1) 


    let opCodes = tests |> Seq.map (fun ((before, arguments, after)) -> arguments.[0])
    let names = testResults |> Seq.map (fun results -> results |> Seq.map (fun (name,_) -> name))
    let multipleOpCodesAndPossibleInstructions = names |> Seq.zip opCodes

    let opCodesAndPossibleInstructions = multipleOpCodesAndPossibleInstructions 
                                            |> Seq.groupBy (fun (op, _) -> op)
                                            |> Seq.map (fun (opCode, instructions) -> (opCode, instructions |> Seq.map snd ))
                                            |> Seq.map (fun (opCode, instructions) -> (opCode, intersection instructions))

    let opCodes = workoutOpCode (List.ofSeq opCodesAndPossibleInstructions)
    opCodes |> List.iter (fun (opCode, name) -> printfn "%d %s" opCode name)

    0 // return an integer exit code
