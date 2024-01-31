module GitPersona.Stream

open System
open System.IO

type Stream () =
    member _.write (path: string) f =
        let writer = new StreamWriter(path)
        f writer
        writer.Close()

    member _.read (path: string) f =
        let stream = new StreamReader(path)
        let data = stream.ReadToEnd()
        stream.Close()
        f data
