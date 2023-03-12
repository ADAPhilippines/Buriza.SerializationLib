namespace Buriza.SerializationLib

open PeterO.Cbor
open System.Collections.Generic

type Coin = uint64

type Input = { Id: byte[]; Index: uint }

type Address = byte[]

type CurrencySymbol = byte[]

type TokenName = byte[]

type MultiAsset =
    { Lovelace: Coin
      Tokens: Dictionary<CurrencySymbol, Dictionary<TokenName, Coin>> }

type Value =
    | MultiAsset of MultiAsset
    | Lovelace of Coin

    member value.GetCbor =
        match value with
        | Lovelace lovelace -> CBORObject.FromObject(lovelace)
        | MultiAsset ma -> CBORObject.FromObject(ma.Lovelace)

type Input with

    member input.GetCbor = CBORObject.NewArray().Add(input.Id).Add(input.Index)

type Output =
    { Address: Address
      Value: Value }

    member output.GetCbor =
        CBORObject.NewArray().Add(output.Address).Add(output.Value.GetCbor)

type Key = { Key: byte[]; ChainCode: byte[] }

type VKeyWitness =
    { VKey: Key
      Skey: Key
      Signature: byte[] }

type WitnessSet =
    { VKeyWitnesses: ICollection<VKeyWitness> }

type Body =
    { Inputs: IList<Input>
      Outputs: IList<Output>
      Fee: Coin }

    static member New =
        { Inputs = List<Input>()
          Outputs = List<Output>()
          Fee = 0UL }

    member txBody.GetCbor =
        let txBodyCbor = CBORObject.NewMap()
        let inputsCbor = CBORObject.NewArray()
        let outputsCbor = CBORObject.NewArray()

        for input in txBody.Inputs do
            inputsCbor.Add(input.GetCbor) |> ignore

        for output in txBody.Outputs do
            outputsCbor.Add(output.GetCbor) |> ignore

        txBodyCbor.Add(0, inputsCbor).Add(1, outputsCbor).Add(2, txBody.Fee)


// transaction =
//   [ transaction_body
//   , transaction_witness_set
//   , bool
//   , auxiliary_data / null
//   ]

type Transaction =
    { Body: Body
      IsValid: bool }

    member tx.Serialize =
        CBORObject
            .NewArray()
            .Add(tx.Body.GetCbor)
            .Add(CBORObject.NewMap())
            .Add(CBORObject.FromObject(tx.IsValid))
            .Add(null)
            .EncodeToBytes()
