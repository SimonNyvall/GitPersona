open GitPersona.Git.Store
open GitPersona.Git.Persona
open GitPersona.Shared.Types
open GitPersona.Shared.Validate

let usage =
    """Usage: persona <command> [<options>] [<args>]
persona list
    -A 
    -e [EMAIL]
    -n [NAME]

persona add [NAME] [EMAIL]

persona remove [ID] OR [NAME] [EMAIL]

persona update [ID]
    -e [NEW_EMAIL]
    -n [NEW_NAME]

Example: persona update -ne [ID] [NEW_NAME] [NEW_EMAIL]

persona use [ID] OR [NAME] [EMAIL]
"""

[<EntryPoint>]
let main argv =
    let store = new Store("./../../../.gitconfig")
    let persona = new Persona()

    if argv.Length > 0 then
        let command = argv.[0]
        let args = argv.[1..]

        match parseCommand command with
        | Some cmd when validateFlags cmd args ->
            match cmd with
            | List ->
                let personas = persona.getPersonas ()

                personas
                |> List.iter (fun p -> printfn "%s : %s" p.Username p.Email)

            0
        | _ ->
            printfn "%s" usage
            1
    else
        printfn "%s" usage
        1
