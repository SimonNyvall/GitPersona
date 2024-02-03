open System
open GitPersona.Git.Store
open GitPersona.Repository.Persona
open GitPersona.Shared.Types
open GitPersona.Shared.Validate
open GitPersona.Handler

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
    let store = new Store(Environment.GetEnvironmentVariable "GIT_CONFIG_PATH")
    let persona = new Persona("Data Source=git-persona.db;Version=3;")

    if argv.Length > 0 then
        let command = argv.[0]
        let args = argv.[1..]

        match parseCommand command with
        | Some cmd when validateFlags cmd args ->
            match cmd with
            | List -> printGitPersonaTable ()
            | Add ->
                let name = args.[0]
                let email = args.[1]

                addPersonaHandler name email

            0
        | _ ->
            printfn "%s" usage
            1
    else
        printfn "%s" usage
        1
