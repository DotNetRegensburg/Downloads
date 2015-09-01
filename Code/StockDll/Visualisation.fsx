


#r @"..\Dependencies\MSDN.FSharp.Charting.dll" 

open MSDN.FSharp.Charting
open System.Windows.Forms

let grid (prices:(System.DateTime * float) seq) =
  let form = new Form(Visible = true, TopMost = true)
  let grid = new DataGridView(Dock = DockStyle.Fill, Visible = true)
  form.Controls.Add(grid)
  grid.DataSource <- prices |> Seq.toArray

let plot (prices:(System.DateTime * float) seq) =
  prices 
  |> Seq.toArray
  |> FSharpChart.Line
  |> FSharpChart.Create
  |> ignore




