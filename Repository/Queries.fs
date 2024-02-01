module GitPersona.Repository.Queries

let getPersonasQuery =
    "SELECT * FROM personas"


let addPersonaQuery =
    "INSERT INTO personas (name, email) VALUES (@name, @email)"

let removePersonaByIdQuery =
    "DELETE FROM personas WHERE id = @id"

let removePersonaByNameAndEmailQuery =
    "DELETE FROM personas WHERE name = @name AND email = @email"

let updatePersonaQuery =
    "UPDATE personas SET name = @name, email = @email WHERE id = @id"

