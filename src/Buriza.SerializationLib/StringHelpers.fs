namespace Buriza.SerializationLib.Helpers

open System.Collections.Generic
open System

type StringHelpers =
    static member BytesToHex(bytes: byte[]) =
        BitConverter.ToString(bytes).Replace("-", "").ToLower()

    static member HexToBytes(hex: string) =
        if (hex.Length % 2 = 1) then
            raise (ArgumentException("The binary key cannot have an odd number of digits"))

        let arrLen = hex.Length >>> 1
        let arr = Array.zeroCreate (arrLen)

        for i in 0 .. (arrLen - 1) do
            let v =
                byte (
                    (StringHelpers.GetHexVal(hex[i <<< 1]) <<< 4)
                    + (StringHelpers.GetHexVal(hex[(i <<< 1) + 1]))
                )

            Array.set arr i v

        arr


    static member GetHexVal(hex: Char) : int =
        let v = (int) hex

        let result =
            v
            - if v < 58 then 48
              else if v < 97 then 55
              else 87

        result
