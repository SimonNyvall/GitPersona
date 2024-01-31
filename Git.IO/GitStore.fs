module GitPersona.Git.Store

open System
open System.IO

open GitPersona.Shared.Types
open GitPersona.Stream

type Store(storeLocation: string) =

    let stream = new Stream()

    member _.getGitInfo() : GitCredentials =
        let parse (data: string) =
            let lines = data.Split "\n"

            let extract (key: string) =
                lines
                |> Array.filter (fun line -> line.Contains key)
                |> Array.head
                |> fun line -> line.Split "="
                |> Array.rev
                |> Array.head
                |> fun line -> line.Trim()

            let username = extract "name"
            let email = extract "email"

            { Username = username; Email = email }

        stream.read storeLocation parse

    member _.writeGitInfo(gitInfo: GitCredentials) =
        let readAllLines (path: string) = File.ReadAllLines(path)

        let updateLine (line: string) (key: string) (newValue: string) =
            if line.Contains(key) then
                sprintf "        %s = %s" key newValue
            else
                line

        let updateLines (lines: string array) =
            lines
            |> Array.map (fun line ->
                let lineWithEmail = updateLine line "email" gitInfo.Email
                updateLine lineWithEmail "name" gitInfo.Username)

        let lines = readAllLines storeLocation
        let updatedLines = updateLines lines

        let writeToStream (writer: StreamWriter) =
            Array.iter (fun (line: string) -> writer.WriteLine(line)) updatedLines

        let stream = new Stream()
        stream.write storeLocation writeToStream
