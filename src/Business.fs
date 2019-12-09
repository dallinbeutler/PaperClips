module Business
open Elmish

// MODEL
type Model = 
  {
    funds : float
    sold : int
    unsold : int
    clipPrice : float
    demand: float
    marketingLvl : int
    lvlUpCost:float
    timerTicking:bool
  }

type Msg =
| LowerPrice
| RaisePrice
| ImproveMarketing
//| ClipsSold of int
| TimeTick //int is number of clips
| ClipMade of int
| PurchaseMade of int

let buy (state:Model) cost = 
  {state with funds = state.funds - cost }

let init() : Model = 
  {
    funds = 30.0
    sold = 0
    unsold = 0
    clipPrice = 0.25
    demand = 0.32
    marketingLvl = 1
    lvlUpCost = 100.00
    timerTicking = false
    }

let clamp min max value = 
  if value < min then min
  else if value > max then max 
  else value

open System
let r = Random()

let tickUpdate state =  
  if state.unsold = 0 
  then state
  else
    let newsold = int <| (r.NextDouble() * ( state.demand / state.clipPrice ))
    let unsold = state.unsold - newsold
    let sold = state.sold + newsold
    {state with unsold = unsold; sold = sold; funds = state.funds + (state.clipPrice * (float newsold) )}
// UPDATE
let update (msg:Msg) (state:Model) =  
  match msg with
  | LowerPrice -> {state with clipPrice = (state.clipPrice - 0.01) }
  | RaisePrice -> {state with clipPrice = (state.clipPrice + 0.01) }
  | ImproveMarketing -> {state with marketingLvl = state.marketingLvl + 1}
  | TimeTick-> tickUpdate state  
  | ClipMade count -> {state with unsold = state.unsold + count} 
  | PurchaseMade amount-> {state with funds= state.funds - float amount}
open Fable.React
open Fable.React.Props

// VIEW (rendered with React)
let view state dispatch =
  div [Style [
              MarginBottom 0
              MarginTop 0
              PaddingTop 0
              MarginTop 0
              ]]
      [ 
        h3 [] [ str ("Business ") ]
        hr []
        div [] [str (sprintf "Available Funds: $ %.2f" state.funds)]
        div [] [str (sprintf "Unsold Inventory: %i" state.unsold)]
        div [][
        button [ OnClick (fun ev -> dispatch LowerPrice) ] [ str "lower" ] 
        button [ OnClick (fun _ -> dispatch RaisePrice) ] [ str "raise" ] 
        label [] [str (sprintf " Price per Clip: $ %.2f" state.clipPrice)]
        ]
        div [] [str (sprintf "Public Demand: %.0f%%" (state.demand * 100.0))]
        br[]
        button 
          [OnClick (fun _ -> dispatch ImproveMarketing) ; Disabled true] 
          [ str "marketing" ] 
        label [] [str (sprintf " Level: %i" state.marketingLvl)]
        div[][str (sprintf "Cost: $ %.2f" state.lvlUpCost )]
        br[]
        ]