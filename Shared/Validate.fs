module GitPersona.Shared.Validate

open System
open GitPersona.Shared.Types

let parseCommand (cmd: string) =
    match cmd with
    | "list" -> Some List
    | "add" -> Some Add
    | "remove" -> Some Remove
    | "update" -> Some Update
    | _ -> None

let validateFlags (command: Command) (args: string array) =
    match command with
    | List ->
        args
        |> Array.forall (fun arg ->
            arg = "-A"
            || arg.StartsWith("-e")
            || arg.StartsWith("-n"))
    | Add -> args.Length = 2 // Expecting exactly two arguments: [NAME] [EMAIL]
    | Remove -> args.Length = 1 || args.Length = 2 // Either [ID] or [NAME] [EMAIL]
    | Update ->
        match args with
        | [| "-e"; _ |] -> true
        | [| "-n"; _ |] -> true
        | [| "-ne"; _; _; _ |] -> true
        | [| id |] when id |> String.IsNullOrWhiteSpace |> not -> true
        | _ -> false

let runCommand (cmd: string) (args: string array) =
    match parseCommand cmd with
    | Some command when validateFlags command args ->
        // Execute the command with args
        printfn "Running %s with args: %A" cmd args
    | _ -> printfn "Invalid command or arguments"
