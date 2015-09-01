
let a = 1
let b = "Hallo"
let now = System.DateTime.Now

let tuple = a, b, "Hallo"
let data = [ 2.3; 2.4; 4.3; ]

let sqr x = x * x

let sumOfSquaresP nums =
  let mutable acc = 0.0
  for x in nums do
    acc <- acc + sqr x
  acc

sumOfSquaresP data

let rec sumOfSquaresF nums =
  match nums with
  | [] -> 0.0
  | head::tail -> sqr head + sumOfSquaresF tail

sumOfSquaresP data

let sumOfSquares nums =
  nums
  |> Seq.map sqr
  |> Seq.sum

sumOfSquares data

// PSeq benötigt den FSharpPowerpack von 
// http://fsharppowerpack.codeplex.com/
#r "../Dependencies/FSharp.Powerpack.Parallel.Seq.dll"
open Microsoft.FSharp.Collections

let sumOfSquaresParallel nums =
  nums
  |> PSeq.map (fun x -> x * x)
  |> PSeq.sum

sumOfSquaresParallel data




let map f list =
  let rec recMap source result =
    match source with
    | head::tail -> 
      let result = (f head)::result
      recMap tail result
    | [] -> result |> List.rev
  recMap list []

map sqr data
data |> map sqr
data |> map (fun nr -> nr.ToString())
[ now ] |> map (fun t -> System.DateTime.Now - t)
