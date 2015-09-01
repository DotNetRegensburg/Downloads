open System.Net
open System.IO
open Microsoft.FSharp.Control.WebExtensions

let getPrices company = 
    let url = "http://ichart.finance.yahoo.com/table.csv?s=" + company + "&d=7&e=7&f=2011&g=d&a=0&b=1&c=1900&ignore=.csv"

    let req = WebRequest.Create(url)
    let resp = req.GetResponse()
    let stream = resp.GetResponseStream()
    let reader = new StreamReader(stream)
    let csv = reader.ReadToEnd()

    let prices = 
      csv.Split('\n')
      |> Seq.skip 1
      |> Seq.map (fun line -> line.Split(','))
      |> Seq.filter (fun items -> items.Length = 7)
      |> Seq.map (fun items -> 
                      System.DateTime.Parse(items.[0]), 
                      float items.[6])

    prices 


#load "Visualisation.fsx"

getPrices "MSFT" 
|> Visualisation.plot

getPrices "AAPL"
|> Visualisation.plot

