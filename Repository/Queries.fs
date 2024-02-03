module GitPersona.Repository.Queries

let getPersonasQuery = "SELECT * FROM personas"


let addPersonaQuery = "INSERT INTO personas (id, name, email) VALUES (@id, @name, @email)"

let removePersonaByIdQuery = "DELETE FROM personas WHERE id = @id"

let removePersonaByNameAndEmailQuery =
    "DELETE FROM personas WHERE name = @name AND email = @email"

let updatePersonaQuery =
    "UPDATE personas SET name = @name, email = @email WHERE id = @id"

let checkIfTablePersonasExistsQuery =
    "SELECT name FROM sqlite_master WHERE type='table' AND name='personas'"

let createTablePersonasQuery =
    "CREATE TABLE personas (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL, email TEXT NOT NULL UNIQUE)"
