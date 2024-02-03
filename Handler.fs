module GitPersona.Handler

open System
open GitPersona.Shared.Types
open GitPersona.Repository.Persona

let printGitPersonaTable () =
    let persona = new Persona("Data Source=git-persona.db;Version=3;")

    let personas = persona.getPersonas ()

    personas
    |> Async.RunSynchronously
    |> List.iter (fun p -> printfn "%d %s %s" p.Id p.Username p.Email)

let addPersonaHandler (name: string) (email: string) =
    let persona = new Persona("Data Source=git-persona.db;Version=3;")

    let personasCount =
        persona.getPersonas ()
        |> Async.RunSynchronously
        |> List.length

    persona.addPersona
        { Id = personasCount + 1
          Username = name
          Email = email }
    |> Async.RunSynchronously
