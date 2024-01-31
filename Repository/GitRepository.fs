module GitPersona.Git.Persona

open System
open System.IO
open System.Data.SQLite
open GitPersona.Shared.Types

type Persona() =

    let connection = new SQLiteConnection("Data Source=Repository/git-persona.db;Version=3;")

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
        let command = new SQLiteCommand("SELECT * FROM personas", connection)

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
        let command =
            new SQLiteCommand("INSERT INTO personas (name, email) VALUES (@name, @email)", connection)

        ExecuteNonQuery command persona

    member _.removePersona(persona: GitCredentials) : unit =
        let command =
            new SQLiteCommand("DELETE FROM personas WHERE name = @name AND email = @email", connection)

        ExecuteNonQuery command persona

    member _.updatePersonaEmail(persona: GitCredentials) : unit =
        let command =
            new SQLiteCommand("UPDATE personas SET email = @email WHERE name = @name", connection)

        ExecuteNonQuery command persona

    member _.updatePersonaName(persona: GitCredentials) : unit =
        let command =
            new SQLiteCommand("UPDATE personas SET name = @name WHERE email = @email", connection)

        ExecuteNonQuery command persona
