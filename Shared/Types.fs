module GitPersona.Shared.Types

type username = string
type email = string

type GitCredentials = { Username: username; Email: email }

type GitModel = { Id: int; Username: username; Email: email }

type Command =
    | List
    | Add
    | Remove
    | Update
