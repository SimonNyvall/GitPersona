module GitPersona.Repository.Persona

open System
open System.IO
open System.Data
open System.Data.SQLite
open System.Data.Common
open Microsoft.FSharp.Control
open GitPersona.Shared.Types
open GitPersona.Repository.Queries

type Persona(connectionString: string) =
    let executeNonQueryAsync (query: string) (gitCredentials: GitModel) : Async<unit> =
        async {
            use connection = new SQLiteConnection(connectionString)

            do!
                connection.OpenAsync()
                |> Async.AwaitTask
                |> Async.Ignore

            let command = new SQLiteCommand(query, connection)

            command.Parameters.Add(new SQLiteParameter("@id", gitCredentials.Id))
            |> ignore

            command.Parameters.Add(new SQLiteParameter("@name", gitCredentials.Username))
            |> ignore

            command.Parameters.Add(new SQLiteParameter("@email", gitCredentials.Email))
            |> ignore

            do!
                command.ExecuteNonQueryAsync()
                |> Async.AwaitTask
                |> Async.Ignore
        }


    member _.getPersonas() : Async<GitModel list> =
        async {
            use connection = new SQLiteConnection(connectionString)
            let! _ = Async.AwaitIAsyncResult(connection.OpenAsync())

            let command = new SQLiteCommand(getPersonasQuery, connection)

            use! reader =
                Async.FromContinuations (fun (cont, econt, ccont) ->
                    try
                        cont (command.ExecuteReader(CommandBehavior.CloseConnection))
                    with
                    | ex -> econt ex)

            let rec readAsync (reader: SQLiteDataReader) : Async<GitModel list> =
                async {
                    if reader.Read() then
                        let id = reader.GetInt32(0)
                        let name = reader.GetString(1)
                        let email = reader.GetString(2)

                        let model =
                            { Id = id
                              Username = name
                              Email = email }

                        let! rest = readAsync reader
                        return model :: rest
                    else
                        return []
                }

            let! models = readAsync reader
            return models
        }


    member _.addPersona(persona: GitModel) : Async<unit> =
        async {
            let query = addPersonaQuery
            do! executeNonQueryAsync query persona
        }


    member _.removePersonaWithId(id: int) : Async<unit> =
        async {
            let query = removePersonaByIdQuery
            do! executeNonQueryAsync query { Id = id; Username = ""; Email = "" }
        }


    member _.removePersonaWithNameAndEmail(name: string, email: string) : Async<unit> =
        async {
            let query = removePersonaByNameAndEmailQuery

            do!
                executeNonQueryAsync
                    query
                    { Id = 0
                      Username = name
                      Email = email }
        }


    member _.updatePersonaWithId(id: int, newUsername: string, newEmail: string) : Async<unit> =
        async {
            let query = updatePersonaQuery

            do!
                executeNonQueryAsync
                    query
                    { Id = id
                      Username = newUsername
                      Email = newEmail }
        }
