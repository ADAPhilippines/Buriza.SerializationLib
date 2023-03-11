namespace Buriza.SerializationLib

module TransactionBuilder =

    type Transaction = { Fee: int }

    let NewTx = { Fee = 100 }
