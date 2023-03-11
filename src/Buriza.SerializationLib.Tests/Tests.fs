module Tests

open System
open Xunit
open Buriza.SerializationLib

[<Fact>]
let ``My test`` () =
    Assert.True((TransactionBuilder.NewTx = { Fee = 100 }))
