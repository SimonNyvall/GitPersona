module GitPersona.Git.Persona

open System
open System.IO
open System.Data.SQLite
open GitPersona.Shared.Types
open GitPersona.Repository.Queries

type Persona() =

    let connection =
        new SQLiteConnection("Data Source=Repository/git-persona.db;Version=3;")

    let ExecuteNonQuery (command: SQLiteCommand) (gitCredentials: GitCredentials) : unit =
        do connection.Open()

        command.Parameters.Add(new SQLiteParameter("@name", gitCredentials.Username))
        |> ignore

        command.Parameters.Add(new SQLiteParameter("@email", gitCredentials.Email))
        |> ignore

        command.Connection <- connection
        command.ExecuteNonQuery() |> ignore

        connection.Close()

    member _.getPersonas() : GitCredentials list =
        let command = new SQLiteCommand(getPersonasQuery, connection)

        do connection.Open()

        let reader = command.ExecuteReader()

        let rec read (reader: SQLiteDataReader) =
            if reader.Read() then
                let name = reader.GetString(0)
                let email = reader.GetString(1)
                { Username = name; Email = email } :: read reader
            else
                []

        read reader

    member _.addPersona(persona: GitCredentials) : unit =
        let command = new SQLiteCommand(addPersonaQuery, connection)

        ExecuteNonQuery command persona

    member _.removePersonaByNameAndEmail(persona: GitCredentials) : unit =
        let command = new SQLiteCommand(removePersonaByNameAndEmailQuery, connection)

        ExecuteNonQuery command persona

    member _.removePersonaId(persona: GitCredentials) : unit =
        let command = new SQLiteCommand(removePersonaByIdQuery, connection)

        ExecuteNonQuery command persona

    member _.updatePersonaEmail(persona: GitCredentials) : unit =
        let command = new SQLiteCommand(updatePersonaQuery, connection)

        ExecuteNonQuery command persona

    member _.updatePersonaName(persona: GitCredentials) : unit =
        let command =
            new SQLiteCommand("UPDATE personas SET name = @name WHERE email = @email", connection)

        ExecuteNonQuery command persona
