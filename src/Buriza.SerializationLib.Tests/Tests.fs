module Tests

open System
open Xunit
open Buriza.SerializationLib
open System.Collections.Generic

[<Fact>]
let ``My test`` () =
    Assert.True(
        (TransactionBody.New.GetCbor = { Inputs = List<Input>()
                                         Outputs = List<Output>()
                                         Fee = 0UL }
            .GetCbor)
    )
