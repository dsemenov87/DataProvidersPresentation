open FSharp.Data.Sql
open System

let [<Literal>] connString  = "Host=localhost;Database=contactdb;Username=postgres;Password=1"
let [<Literal>] dbVendor    = Common.DatabaseProviderTypes.POSTGRESQL
let [<Literal>] resPath     = @"./bin/Debug/"
let [<Literal>] indivAmount = 1000
let [<Literal>] useOptTypes = true

type dbSchema = SqlDataProvider<dbVendor, connString, "", resPath, indivAmount, useOptTypes>

let ctx = dbSchema.GetDataContext()

[<EntryPoint>]
let main argv =     

    let q1 = query {
        for c in ctx.Public.Companies do
        where (c.Registrationstate = Some "None")
        //where (c.Created > Some DateTime.Now)
        //where (c.Companytype = Some "ooo")
        sortByDescending c.Created
        select (c.Name, c.Address, c.Companytype, c.Registrationstate)
        take 10
    }

    let q2 = query {
        for c in ctx.Public.Config do
        select (c.Name, c.Value)
    }

    q1 |> Seq.toArray |> Array.map (printfn "%A") |> ignore

    Console.ReadKey() |> ignore

    0 // return an integer exit code
