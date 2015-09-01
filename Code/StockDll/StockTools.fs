namespace StockDll

module StockTools = 

  open System.Net
  open System.IO
  open Microsoft.FSharp.Control.WebExtensions

  let private getPrices company = 
    async {
      printfn "Get data for %s" company
      let url = "http://ichart.finance.yahoo.com/table.csv?s=" + company + "&d=7&e=7&f=2011&g=d&a=0&b=1&c=2003&ignore=.csv"

      let req = WebRequest.Create(url)
      let! resp = req.AsyncGetResponse()
      let stream = resp.GetResponseStream()
      let reader = new StreamReader(stream)
      let! csv = reader.AsyncReadToEnd()

      let prices = 
        csv.Split('\n')
        |> Seq.skip 1
        |> Seq.map (fun line -> line.Split(','))
        |> Seq.filter (fun items -> items.Length = 7)
        |> Seq.map (fun items -> 
                        System.DateTime.Parse(items.[0]), 
                        float items.[6])

      printfn "Got data for %s" company
      return prices 
    }

  type StockAnalyzer (company, lprices, days) =
    let prices =
      lprices
      |> Seq.map snd
      |> Seq.take days

    static member GetAnalyzers(companies, days) =
      companies
      |> Seq.map (fun c -> 
          async {
            let! prices = getPrices c
            return c, prices
          }
        )
      |> Async.Parallel
      |> Async.RunSynchronously
      |> Seq.map (fun (c, pr) -> StockAnalyzer(c, pr, days))
 
    member SA.Company = company
    member SA.Return = 
      let endPrice = prices |> Seq.nth 0
      let startPrice = prices |> Seq.nth (days - 1)
      endPrice / startPrice - 1.0
    member SA.StdDev =
      let logRet =
        prices
        |> Seq.pairwise
        |> Seq.map (fun (x, y) -> log (x / y))
      let mean = logRet |> Seq.average
      let sqr x = x * x
      let var = logRet |> Seq.averageBy (fun r -> sqr (r - mean))
      sqrt var




